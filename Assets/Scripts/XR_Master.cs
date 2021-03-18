using UnityEngine;

/// <summary>
/// Class XR_Master handles the PixelBlock movement-flow via Trigger control
/// and manages Multi-PixelBlock selection through Touchpad controls.
/// </summary>

public class XR_Master : MonoBehaviour {
    public PixelBlockManager pixelBlockManager;
    public XR_Cursor xrcursor;

    private Vector2 touchpadLocAtStart;
    private Vector2 touchpadLoc;
    private bool swipeLeft;
    private bool swipeRight;

    /// <summary>
    /// Initializes a PixelBlock at the Unity Origin
    /// </summary>
    void Start() {
        //pixelBlockManager.MovePixelBlock(0, 0);
    }

    /// <summary>
    /// Constantly call the methods to check if a user switches the ActivePixelBlock
    /// </summary>
    void Update() {
        ChangePixelSelection();
        ChangePixelSelectionDebug();
    }

    /// <summary>
    /// Called when user presses right hand trigger on controller. 
    /// Gets the unity (X,Z) position of the XR_Cursor
    /// Moves pixel block to specified location
    /// </summary>
    public void GetCursorLocation() {
        float pixBlockX = xrcursor.cursor.transform.position.x;
        float pixBlockZ = xrcursor.cursor.transform.position.z;

        

        // only set pixel on terrain if we hit a valid target
        if (xrcursor.validTarget) {
            pixelBlockManager.MovePixelBlock(pixBlockX, pixBlockZ);
        }
    }

    /// <summary>
    /// Handles keyboard controls for swithcing between pixel blocks
    /// </summary>
    public void ChangePixelSelectionDebug() {
        if (Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Switching pixel blocks..");
            pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock + 1) % pixelBlockManager.pixelBlocks.Count;
        }
        if (Input.GetKeyDown(KeyCode.B))
            pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock - 1) % pixelBlockManager.pixelBlocks.Count;
    }

    /// <summary>
    /// DEPRECATED AFTER SWITCH TO XR 
    /// Handles VR controls for switching between pixel blocks
    /// </summary>
    public void ChangePixelSelection() {
        if (touchpadLocAtStart.x > touchpadLoc.x)
            swipeLeft = true;
        else if (touchpadLocAtStart.x < touchpadLoc.x)
            swipeRight = true;

        if (swipeLeft) {
            pixelBlockManager.ActivePixelBlock += 1 % pixelBlockManager.pixelBlocks.Count;
            swipeLeft = false;
        }
        if (swipeRight) {
            pixelBlockManager.ActivePixelBlock -= 1 % pixelBlockManager.pixelBlocks.Count;
            swipeRight = false;
        }
    }


    /// <summary>
    /// Removes all of the average pixel blocks from the terrain, and clears the data. 
    /// The data from the most recent averaged selection will remain on the spectral graph
    /// until the user starts a new average selection
    /// </summary>
    /// <remarks>
    /// This method is called when the user pressed the right controller grip button (this is temporary)
    /// </remarks>
    public void resetAvgPixBlockSet() {
        if (pixelBlockManager.pixelBlocks[pixelBlockManager.ActivePixelBlock].averagePixelBlock) {
            foreach (GameObject pb in pixelBlockManager.averagePixelBlocks) {
                Destroy(pb);
            }
            pixelBlockManager.averagePixelBlocks.Clear();
            pixelBlockManager.rowcolSet.Clear();
        }
    }
}
