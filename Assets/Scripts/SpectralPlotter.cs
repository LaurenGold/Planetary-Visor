using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class SpectralPlotter plots the input data onto the canvas on the user's left hand
/// </summary>

public class SpectralPlotter : MonoBehaviour {
    public RawImage rawImage;
    public Text textMin;
    public Text textMax;
    public Text temporalText;
    [SerializeField]
    public int numPixels;
    [SerializeField]
    public int numPixelsY;

    private Texture2D spectrumTex;
    private Color[] emptyColors;
    private Color[] colors;
    private int numSpectralBands = 246;
    
    /// <summary>
    /// Initializes all class members
    /// </summary>
    void Awake() {
        spectrumTex = new Texture2D(numPixels, numPixelsY);
        rawImage.texture = spectrumTex;
        colors = new Color[numPixels * numPixelsY];
        emptyColors = new Color[numPixels * numPixelsY];
        emptyColors.CopyTo(colors, 0);
    }

    /// <summary>
    /// Populates the colors array with the correct data
    /// </summary>
    /// <param name="col"> Used to retrieve spectral data </param>
    /// <param name="row"> Used to retrieve spectral data </param>
    /// <param name="pb_color"> Color to graph data in </param>
    public void PlotData(int col, int row, Color pb_color)
    {
        /*   For the future- might be nice to have a label on the graph for temporal data.
         *   Not sure if we want to plot the temporal data together or separate from the original dataset.
        if(pb_color == temporalColor) temporalText.text = "Temporal Data";
        else temporalText.text = "";
        */
        
        //bounds.x holds minBound, bounds.Y holds maxBound
        Vector2 bounds = calculateNewMinMaxBounds(col, row);
        Vector2 prev = new Vector2(0, 0);
        Vector2 coordinate = new Vector2(0, 0);

        for (int i = 0; i < numPixels; i++) {
            float ind = i * numSpectralBands / numPixels;
            int lower = Mathf.FloorToInt(ind);
            int higher = lower < numSpectralBands - 2 ? lower + 1 : lower;
            float val1 = ind - lower;
            float val2 = 1 - val1;
            float lowerData = DataCube.dataCube[col, row, lower];
            float higherData = DataCube.dataCube[col, row, higher];
            float dat1 = val1 * lowerData + val2 * higherData;
            //InverseLerp finds how far dat1 is along between lowerSSAbound and upperSSAbound.
            //dat = 0 if dat1 == lowerSSAbound. dat = 1 if dat1 == upperSSAbound.
            float dat = Mathf.InverseLerp(bounds.x, bounds.y, dat1);
            //dat = the height value of the data point, normalized from 0.0f to 1.0f.
            int jj = Mathf.FloorToInt(dat * numPixelsY);
            if(coordinate.y != jj) {
                coordinate = new Vector2(i, jj);
            }
            for (int j = Mathf.Clamp(jj - 2, 0, numPixelsY - 1); j < Mathf.Clamp(jj + 2, 0, numPixelsY - 1); j++) {
                colors[j * numPixels + i] = colors[j * numPixels + i] == colors[0] ? pb_color : colors[j * numPixels + i];
            }
        
            if(prev.y != coordinate.y) {
                ConnectPoints(prev, coordinate, pb_color);
                prev = new Vector2(coordinate.x, coordinate.y);
            }
        }

       
    }
    /// <summary>
    /// Connnects the previous point to the next point on the graph to plot a continuous line.
    /// </summary>
    private void ConnectPoints(Vector2 pointA, Vector2 pointB, Color pb_color) {
        //Debug.Log("Diff:\t" + (pointB.x - pointA.x));
        // Find slope between two points
        float xS = (pointB.x - pointA.x);
        float yS = (pointB.y - pointA.y);
        float slope = yS / xS;
     
        //from pointa to pointb for k++
        for (int k = (int)pointA.x - 1; k < (int)pointB.x; k++) {
            //pix = slope(x) + b
          
            int pix = (int)(slope * k - slope * pointA.x + pointA.y);
            for (int j = Mathf.Clamp(pix - 2, 0, numPixelsY - 1); j < Mathf.Clamp(pix + 5, 0, numPixelsY - 1); j++) {
                if (k >= 1 && k < numPixels && pix >= 0)
                    colors[j * numPixels + k] = pb_color;
            }
            //if inside bounds of the graph
            //k is column pix is row
            if (k >= 1 && k < numPixels && pix >= 0) {
                //Debug.Log("Plotting");
                colors[k + pix * numPixels] = pb_color;
            }
        }
    }

    /// <summary>
    /// Called by PixelBlockManager to reset graph to black
    /// </summary>
    public void ResetPixels() {
        emptyColors.CopyTo(colors, 0);
    }

    /// <summary>
    /// Applies the colors array to the canvas
    /// </summary>
    public void ApplyPixels() {
        spectrumTex.SetPixels(colors);
        spectrumTex.Apply();
    }

    /// <summary>
    /// Returns min/max bounds and sets text min/max
    /// </summary>
    /// <param name="col"> Used to retrieve spectral data from DataCube </param>
    /// <param name="row"> Used to retrieve spectral data from DataCube </param>
    /// <returns> Vector2 with min/max of input data </returns>
    private Vector2 calculateNewMinMaxBounds(int col, int row) {
        float minBound = 1.0f;
        float maxBound = 0.0f;
        for (int i = 0; i < numSpectralBands; i++) {
            float bound = DataCube.dataCube[col, row, i];
            if (bound < minBound) {
                minBound = bound;
                minBound = minBound * 100;
                minBound = Mathf.FloorToInt(minBound);
                minBound = minBound / 100;
            }
            if (bound > maxBound) {
                maxBound = bound;
                maxBound = maxBound * 100;
                maxBound = Mathf.CeilToInt(maxBound);
                maxBound = maxBound / 100;
            }
        }
        
        //These values control the smoothness of the drawn line
        minBound = minBound - 0.05f;
        maxBound = maxBound + 0.05f;
        
        double newMin = Math.Round(minBound, 2, MidpointRounding.AwayFromZero);
        double newMax = Math.Round(maxBound, 2, MidpointRounding.AwayFromZero);
        textMin.text = "" + newMin;
        textMax.text = "" + newMax;
        return new Vector2(minBound, maxBound);
    }

    /// <summary>
    /// DEPRECATED: used to Zoom in on SpectralPlotter for screen shot capability
    /// </summary>
    public void ZoomIn()
    {
        Canvas graph = gameObject.GetComponent<Canvas>();
        graph.scaleFactor = 3f;   
    }
}
