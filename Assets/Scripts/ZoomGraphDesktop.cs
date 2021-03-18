using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ZoomGraphDesktop : MonoBehaviour
{
    public GameObject[] canvasElements;
    public Canvas canvas;

    private bool isZoomed = false;

    private RectTransform graph;
    private Vector3 origGraphPos;
    private Vector3 origGraphScale;

    void Start()
    {
        // get and store graph and its original position/scale
        graph = gameObject.GetComponent<RectTransform>();
        origGraphPos = graph.localPosition;
        origGraphScale = graph.localScale;
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleZoom();
        }
       if (Input.GetKeyDown(KeyCode.X))
        {
            StartScreenshot();
        } 
    }
    // toggle zoom spectral graph
    private void ToggleZoom()
    {
        Debug.Log("zoom test");

        // if not zoomed in:
        if(!isZoomed)
        {
            isZoomed = true;
            // iterate thru all canvas elements and hide
            foreach (GameObject g in canvasElements)
            {
                g.GetComponent<CanvasGroup>().alpha = 0;
            }
            // increase graph size and place in center of screen
            //graph.sizeDelta = new Vector2(Screen.height, Screen.height);
            graph.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            graph.localPosition = new Vector3(0, 0, 0);
        }
        // if zoomed in already, set back to normal:
        else
        {
            isZoomed = false;
            // iterate thru all canvas elements and unhide
            foreach (GameObject g in canvasElements)
            {
                g.GetComponent<CanvasGroup>().alpha = 1;
            }
            // reset scale and position
            graph.localScale = origGraphScale;
            graph.localPosition = origGraphPos;
        }
    }
    // start the screenshot corountine via ui button
    public void StartScreenshot()
    {
        Debug.Log("screenshot button pressed");
        StartCoroutine(TakeScreenshot());
    }
    // take screenshot of graph
    private IEnumerator TakeScreenshot()
    {
        // check if graph is zoomed in, if not then zoom in first
        if (!isZoomed)
        {
            ToggleZoom();
        }
        // wait until end of frame
        yield return new WaitForEndOfFrame();

        // get width and height of graph
        int width = System.Convert.ToInt32(graph.rect.width*2.75);
        int height = System.Convert.ToInt32(graph.rect.height*2.75);

        Debug.Log("rect w=" + width + " h=" + height);

        Vector2 temp = graph.transform.position;
        var startX = temp.x - width / 2;
        var startY = temp.y - height / 2;
        Debug.Log("graph pos x=" + temp.x + " y=" + temp.y);

        // create texture over graph area
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        string uniqueID = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        File.WriteAllBytes("Screenshot-"+uniqueID+".png", bytes);
    }
}
