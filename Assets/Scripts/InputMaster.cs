/*
using UnityEngine;

public class InputMaster : MonoBehaviour
{
    public PixelBlockManager pixelBlockManager;
    public XR_Cursor xr_cursor;

    
    private Vector2 touchpadLocAtStart;
    private Vector2 touchpadLoc;
    private bool swipeLeft;
    private bool swipeRight;


    /// <summary>
    /// Move pixel block on command
    /// </summary>
    void Update() {
        // Check for when user presses trigger to move pixel block
        bool trigger = true;
        if (trigger || Input.GetKeyDown(KeyCode.Space)) {
            float pixBlockX = xr_cursor.cursor.transform.position.x;// * (1/gameObject.transform.localScale.x);   //get x z location of where the ray cast hit (vivecursor)
            float pixBlockZ = xr_cursor.cursor.transform.position.z;// * (1/gameObject.transform.localScale.z);
            pixelBlockManager.MovePixelBlock(pixBlockX, pixBlockZ);
        }
        ChangePixelSelection();
        ChangePixelSelectionDebug();
    }

    public void ChangePixelSelectionDebug() {
        if (Input.GetKeyDown(KeyCode.N))
            pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock + 1) % pixelBlockManager.pixelBlocks.Count;
        if (Input.GetKeyDown(KeyCode.B))
            pixelBlockManager.ActivePixelBlock = (pixelBlockManager.ActivePixelBlock - 1) % pixelBlockManager.pixelBlocks.Count;

    }

    public void ChangePixelSelection() {
        //swipe left on hand DPAD to switch between controlling pixel blocks.

        //determine direction of swipe (negative or positive)
        if (touchpadLocAtStart.x > touchpadLoc.x) swipeLeft = true;
        else if (touchpadLocAtStart.x < touchpadLoc.x) swipeRight = true;

        if (swipeLeft) {
            pixelBlockManager.ActivePixelBlock += 1 % pixelBlockManager.pixelBlocks.Count;
            swipeLeft = false;
        }
        if (swipeRight) {
            pixelBlockManager.ActivePixelBlock -= 1 % pixelBlockManager.pixelBlocks.Count;
            swipeRight = false;
        }
    }
} */
