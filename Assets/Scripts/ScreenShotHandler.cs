using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    public void ScreenShot()
    {
        Canvas graph = gameObject.GetComponent<Canvas>();
        graph.scaleFactor = 3f;
        ScreenCapture.CaptureScreenshot("output.png");
    }
}
