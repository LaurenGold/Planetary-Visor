using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

public class RoverTraversal : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    private void Start()
    {
        /** Code from unity documentation to set properties of line renderer, not important **/
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        LoadData();
    }

    /// <summary>
    /// Reads in the lat long, converts to unity x z and draws resulting path
    /// </summary>
    private void LoadData()
    {
        //Read in the latitude and longitude coords of rover traversal points
        TextAsset traversalLong = Resources.Load("traversal_long") as TextAsset;
        TextAsset traversalLat = Resources.Load("traversal_lat") as TextAsset;

        //turn the text of each into an array- each element is a string lat/long
        string[] longLines = traversalLong.text.Split('\n');
        string[] latLines = traversalLat.text.Split('\n');

        //number of lat/long coords
        int dataLen = longLines.Length;
        lineRenderer.positionCount = dataLen;

        //will hold float values- this step converts the string lat long to floats
        float[] longFloats = new float[dataLen];
        float[] latFloats= new float[dataLen];
        for(int i=0; i< dataLen; i++)
        {
            longFloats[i] = float.Parse(longLines[i]);
            latFloats[i] = float.Parse(latLines[i]);
        }

        //convert lat/long to unity coords
        float[] unityXCoords = new float[dataLen];
        float[] unityZCoords = new float[dataLen];
        for(int i = 0; i < dataLen; i++)
        {
            Vector2 unityCoords = CoordinateEquations.latlongToUnity(latFloats[i], longFloats[i]);  //TODO- need to make sure this method is correct!
            unityXCoords[i] = unityCoords.x;
            unityZCoords[i] = unityCoords.y;
        }

        // interpolate coordinate data by multiplier
        int multiplier = 15;
        Vector2[] interpUnityCoords = new Vector2[dataLen * multiplier];
        for (int i = 0; i < dataLen-1; i++)
        {
            interpUnityCoords[i * multiplier] = new Vector2(unityXCoords[i], unityZCoords[i]);

            for (int k = 1; k < multiplier; k++)
            {
                interpUnityCoords[i * multiplier + k] = Vector2.Lerp(interpUnityCoords[i * multiplier], new Vector2(unityXCoords[i + 1], unityZCoords[i + 1]), k / (float)multiplier);
            }
        }
        interpUnityCoords[(dataLen - 1) * multiplier] = new Vector2(unityXCoords[dataLen - 1], unityZCoords[dataLen - 1]);

        //draw lines based on unity coords
        int interpDataLen = dataLen * multiplier;
        lineRenderer.SetVertexCount(interpDataLen);
        Vector3[] points = new Vector3[interpDataLen];
        for (int i = 0; i < interpDataLen; i++)
        {

            Ray r = new Ray(interpUnityCoords[i].x * Vector3.right + Vector3.up * 1000 + interpUnityCoords[i].y * Vector3.forward, Vector3.down);
            RaycastHit hitinfo;

            int terrainLayerMask = 1 << TerrainManager.terrainLayer;    //cast rays only against colliders in layer "terrain" 
            if (Physics.Raycast(r, out hitinfo, Mathf.Infinity, terrainLayerMask))
            {
                points[i] = hitinfo.point + Vector3.up * .1f;
            }
            else
            {
                if (i > 0)
                {
                    points[i] = new Vector3(interpUnityCoords[i].x, points[i - 1].y, interpUnityCoords[i].y);
                }
                else
                {
                    points[i] = new Vector3(interpUnityCoords[i].x, -5f, interpUnityCoords[i].y);
                }

            }
        }
        lineRenderer.SetPositions(points);
    }
}
