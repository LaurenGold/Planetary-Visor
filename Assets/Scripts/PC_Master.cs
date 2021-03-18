using UnityEngine;
using System.Collections;

/// <summary>
/// PC_Master handles all keyboard controls
/// </summary>

public class PC_Master : MonoBehaviour
{
    public PixelBlockManager pixelBlockManager;
    public XR_Cursor xrcursor;
    public PC_ScalePlayer pc_scalePlayer;

    private Vector2 touchpadLocAtStart;
    private Vector2 touchpadLoc;
    private Vector3 moveDirection;
    private bool swipeLeft;
    private bool swipeRight;
    public bool audioPlaying;

    public float moveSpeed = 45.0f;

    /// <summary>
    /// Initializes a PixelBlock at the Unity Origin
    /// </summary>
    void Start()
    {
        audioPlaying = false;
        PixelBlockManager.manager.enabled = true;
        Screen.lockCursor = true;
    }

    /// <summary>
    /// Constantly call the methods to check if a user switches the ActivePixelBlock
    /// </summary>
    void Update()
    {
        GetKeyBoardInput();
        if (Input.GetKey(KeyCode.Escape))
            Screen.lockCursor = false;
        else
            Screen.lockCursor = true;
    }

    public void GetKeyBoardInput()
    {
        if (Input.GetMouseButtonDown(0)) { MovePixelBlock(); }
        if (Input.GetKeyDown(KeyCode.G)) pixelBlockManager.ChangePixelSelection(-1);
        if (Input.GetKeyDown(KeyCode.F)) pixelBlockManager.ChangePixelSelection(1);
        if (Input.GetKeyDown(KeyCode.R)) pixelBlockManager.resetAvgPixBlockSet();
        if (Input.GetKeyDown(KeyCode.Space)) PlayAudio();
    }

    /// <summary>
    /// Called when user LEFT CLICKS
    /// Moves pixel block to specified location
    /// </summary>
    public void MovePixelBlock() {
        float pixBlockX = xrcursor.cursor.transform.position.x;
        float pixBlockZ = xrcursor.cursor.transform.position.z;
        Debug.Log("Moved Pixel Block to " + pixBlockX + "," + pixBlockZ);

        pixelBlockManager.MovePixelBlock(pixBlockX, pixBlockZ);
    }

    // THIS FUNCTION IS DEPRECATED: Functionality handled in PixelBlockManager.
    /// <summary>
    /// Handles keyboard controls for swithcing between pixel blocks
    /// </summary>
    public void ChangePixelSelection(int dir) {
        if (dir == 2) pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock + 1) % pixelBlockManager.pixelBlocks.Count;
        
        if (dir == 1) pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock - 1) % pixelBlockManager.pixelBlocks.Count;
    }

    //THIS FUNCTION IS DEPRECATED: Functionality handled in PixelBlockManager.
    /// <summary>
    /// Called when user pressed R
    /// Removes all of the average pixel blocks from the terrain, and clears the data. 
    /// The data from the most recent averaged selection will remain on the spectral graph
    /// until the user starts a new average selection
    /// </summary>
    public void resetAvgPixBlockSet()
    {
        if (pixelBlockManager.pixelBlocks[pixelBlockManager.ActivePixelBlock].averagePixelBlock)
        {
            foreach (GameObject pb in pixelBlockManager.averagePixelBlocks)
            {
                Destroy(pb);
            }
            pixelBlockManager.averagePixelBlocks.Clear();
            pixelBlockManager.rowcolSet.Clear();
        }
    }

    public void PlayAudio() {
        AudioSource audio = GetComponent<AudioSource>();
        if (audioPlaying == false) audio.Play();
        if(audioPlaying == true) audio.Pause();
        audioPlaying = !audioPlaying;
    }
    
}
