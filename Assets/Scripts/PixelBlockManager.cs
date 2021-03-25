using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class PixelBlockManager manages the current Pixel Block
/// Calls methods to plot, move, and shape the selected Pixel Block
/// </summary>

public class PixelBlockManager : MonoBehaviour {
    public static PixelBlockManager manager;
    public TextAsset textAsset1, textAsset2;
    public HashSet<GameObject> averagePixelBlocks;
    public HashSet<Vector2> rowcolSet;
    public List<PixelBlock> pixelBlocks = new List<PixelBlock>();
    public SpectralPlotter spectralPlotter;
    public GameObject mro;
    public XR_Cursor laserpointer;
    public int ActivePixelBlock { get; set; }
    public GameObject blueSquare, greenSquare, redSquare;
    public GameObject arrow;

    public MoveMRO moveMROmgr;
    //the lineIndex in MRO vector results chart
    public int lineIndex = 17;
    public int numLines = 450;
    public float t;
    /// <summary>
    /// Adds each pixel block to the list at runtime
    /// </summary>
    private void Awake() {
        // Sets the Singleton
        manager = this;

        // TODO: Add pragma pre-compiler commands to use correct LoadData function call based on platform.
        DataCube.LoadData();
        
        //Find pixelblocks within the scene
        foreach (PixelBlock pb in FindObjectsOfType<PixelBlock>()) {
            pb.Start(); //Note: Have to do this to ensure the Pixelblocks are ready before we move them!
            pixelBlocks.Add(pb);
            pb.gameObject.SetActive(false);
        }
        ActivePixelBlock = 0;
        NextPixelBlock();
        rowcolSet = new HashSet<Vector2>();
        averagePixelBlocks = new HashSet<GameObject>();
    }

    /// <summary>
    /// Initializes laserPointer color
    /// </summary>
    public void Start() {
        laserpointer.setColor(pixelBlocks[ActivePixelBlock].pb_color);
    }

    /// <summary>
    /// Display the correct color of pixelblock
    /// </summary>
    public void Update() {
        ShowActive();
        t = lineIndex / (numLines - 1);

        if (moveMROmgr != null)
        {
            moveMROmgr.MoveBetween(t);
        }
    }

    /// <summary>
    /// Moves, shapes, and plots the current pixel block after user presses trigger
    /// </summary>
    /// <param name="x"> The specified unity X coordinate to move the pixel block to </param>
    /// <param name="z"> The specified unity Z coordinate to move the pixel block to </param>
    public void MovePixelBlock(float x, float z) {
        // Check if user has average pixelBlock selected
        if (pixelBlocks[ActivePixelBlock].averagePixelBlock)
            AveragePixelBlocks(x, z);

        else
        {
            

            pixelBlocks[ActivePixelBlock].gameObject.SetActive(true);
            bool newColRow = pixelBlocks[ActivePixelBlock].MoveShapeDrawFromXZ(x, z);

            //Reduce computation if user selected the same spot
            if (!newColRow)
                return;

            //Graph the active pixel blocks' spectral data
            spectralPlotter.ResetPixels();
            for (int i = 0; i < pixelBlocks.Count; i++)
            {
                if (!(pixelBlocks[i].averagePixelBlock) && pixelBlocks[i].isActiveAndEnabled)
                {
                    Vector2 lastColRow = pixelBlocks[i].lastKnownColRow;
                    spectralPlotter.PlotData(Mathf.RoundToInt(lastColRow.x), Mathf.RoundToInt(lastColRow.y), pixelBlocks[i].pb_color);
                }
            }
            spectralPlotter.ApplyPixels();
        }
    }

    /// <summary>
    /// Computes the average data point of specified pixel block locations and plots spectral data.
    /// </summary>
    /// <param name="x"> The specified unity X coordinate used to retrieve DataCube row </param>
    /// <param name="z"> The specified unity Z coordinate used to retrieve DataCube col</param>
    public void AveragePixelBlocks(float x, float z) {
        Vector2 selectedXZ = new Vector2(x, z);

        if (rowcolSet == null) rowcolSet = new HashSet<Vector2>();

        Vector2 currentRowCol = DataCube.getColRowFromUnityXZ(x, z);

        //adds currentRowcol to the rowcol set if it is unique
        if (rowcolSet.Add(currentRowCol)) {
            InstantiateNewPixelBlock(selectedXZ);

            //Compute average of all the vector rowcols in the list:
            float colSum = 0f;
            float rowSum = 0f;
            foreach (Vector2 rowcol in rowcolSet) {
                colSum += rowcol.x;
                rowSum += rowcol.y;
            }
            Vector2 averageRowCol = new Vector2(colSum / rowcolSet.Count, rowSum / rowcolSet.Count);
                
            // Clear graph and plot the result
            spectralPlotter.ResetPixels();
            spectralPlotter.PlotData(Mathf.RoundToInt(averageRowCol.x), Mathf.RoundToInt(averageRowCol.y), Color.white);
            spectralPlotter.ApplyPixels();
        }
    }

    /// <summary>
    /// Creates a new average PixelBlock GameObject where the user points to.
    /// </summary>
    /// <param name="currentRowCol"> The rowcol needed to move and shape this pixelblock </param>
    private void InstantiateNewPixelBlock(Vector2 selectedXZ) {
        GameObject newPBGameObj = Instantiate(Resources.Load("Prefabs/AveragePixelBlock"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        PixelBlock newPB = newPBGameObj.GetComponent<PixelBlock>();

        newPB.gameObject.SetActive(true);
        newPB.Start(); //Note: Have to do this to ensure the lineRenderer is created and the PB is shaped correctly

        averagePixelBlocks.Add(newPBGameObj);
        newPB.MoveShapeDrawFromXZ(selectedXZ.x, selectedXZ.y);
    }

    /// <summary>
    /// Handles switching between pixel blocks. dir = +1 to increase, dir = -1 to decrease
    /// </summary>
    public void ChangePixelSelection(int dir)
    {
        ActivePixelBlock = (ActivePixelBlock + pixelBlocks.Count + dir) % pixelBlocks.Count;
        laserpointer.setColor(pixelBlocks[ActivePixelBlock].pb_color);
    }

    public void NextPixelBlock() {
        ActivePixelBlock = (ActivePixelBlock + pixelBlocks.Count + 1) % pixelBlocks.Count;
        laserpointer.setColor(pixelBlocks[ActivePixelBlock].pb_color);

        // skip average pixel blocks when just changing the colors
        if (pixelBlocks[ActivePixelBlock].averagePixelBlock) {
            NextPixelBlock();
        }
    }

    /// <summary>
    /// Removes all of the average pixel blocks from the terrain, and clears the data. 
    /// The data from the most recent averaged selection will remain on the spectral graph
    /// until the user starts a new average selection
    /// </summary>
    public void resetAvgPixBlockSet()
    {
        foreach (GameObject pb in averagePixelBlocks)
        {
           Destroy(pb);
        }
        averagePixelBlocks.Clear();
        rowcolSet.Clear();  
    }

    /// <summary>
    /// Turn off all pixelblocks in the scene
    /// </summary>
    public void resetPixelBlocks()
    {
        foreach (PixelBlock pb in pixelBlocks)
        {
            pb.gameObject.SetActive(false);
        }
        spectralPlotter.ResetPixels();
    }

    /// <summary>
    /// visual representation on the side that shows which color pixelblock is selected
    /// </summary>
    public void ShowActive() {
        switch (ActivePixelBlock % pixelBlocks.Count) {
            case 1:
                //move arrow along x axis
                arrow.transform.position = new Vector3(blueSquare.transform.position.x + 100f, blueSquare.transform.position.y, 0f);

                break;
            case 2:
                //move arrow along x axis
                arrow.transform.position = new Vector3(greenSquare.transform.position.x + 100f, greenSquare.transform.position.y, 0f);

                break;
            case 0:
                //move arrow along x axis
                arrow.transform.position = new Vector3(redSquare.transform.position.x + 100f, redSquare.transform.position.y, 0f);

                break;

        }
    }
    public void ShowActiveVR() {
        switch (ActivePixelBlock % pixelBlocks.Count) {

            case 1:
                arrow.transform.position = new Vector3(greenSquare.transform.position.x + 5f, greenSquare.transform.position.y, blueSquare.transform.position.z);
                break;

            case 2:
                arrow.transform.position = new Vector3(blueSquare.transform.position.x + 5f, blueSquare.transform.position.y, blueSquare.transform.position.z);
                break;

            case 0:
                arrow.transform.position = new Vector3(redSquare.transform.position.x + 5f, redSquare.transform.position.y, blueSquare.transform.position.z);
                break;

        }
    }
}

