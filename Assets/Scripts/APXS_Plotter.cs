using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine.Animations;
using TMPro;

public class APXS_Plotter : MonoBehaviour
{
    float metersPerDegree = 59288.8889f;    // marsCirc/360
    public int terrain;                     //this should always be layer #31
    public Texture2D plotBackground;
    public TextAsset apxsData;
    public GameObject apxsLabel;

    class ApxsReading
    {
        public float longitude;
        public float latitude;
        public string sol;
        public string target;

        public float[] oxides;              //32
        public float[] errors;
        public ApxsReading()
        {
            oxides = new float[16];
            errors = new float[16];
        }
        public GameObject gameObject;
    }

    float northingToZmeters = 0.1f;
    float eastingToXmeters = 0.1f;
    List<ApxsReading> readings;
    private Color[] emptyColors;
    private Color[] colors;
    Color black = new Color(0, 0, 0, 0);

    int numPixelsX = 458;
    int numPixelsY = 449;
    int pixelOffsetX = 47;
    int pixelOffsetY = 52;
    private int lastIndex;

    private List<string> columnNames = new List<string>();
    private Dictionary<string, List<float>> csvDataTable = new Dictionary<string, List<float>>();

    // how many graphs are intersecting at the parent index
    private Dictionary<int, List<int>> intersectingGraphs = new Dictionary<int, List<int>>();

    // the positions of an intersecting graph. first position is the parent's position, second position is the corrected position
    private Dictionary<int, (Vector3, Vector3)> intersectingGraphsPositions = new Dictionary<int, (Vector3, Vector3)>();

    // the list of graphs that we're close to and want to animate/show
    private Dictionary<int, float> animatedGraphs = new Dictionary<int, float>();

    private Vector2 originLatLong;
   
    void Update()
    {
        // go through all intersecting graphs and see if we need to expand any of them
        foreach (KeyValuePair<int, List<int>> entry in intersectingGraphs)
        {
            bool close = false;
            // compare parent position to camera position
            if (Vector3.Distance(readings[entry.Key].gameObject.transform.position, Camera.main.transform.position) < 10.0f)
            {
                // if we're close to this graph, then expand it out if we haven't already
                if (!animatedGraphs.ContainsKey(entry.Key))
                {
                    animatedGraphs.Add(entry.Key, 0.0f);
                }
                close = true;
            }
            else // if we weren't close enough to the parent graph, check all other graphs in the parent's intersecting list
            {
                foreach (int element in entry.Value)
                {
                    Vector3 position = intersectingGraphsPositions[element].Item2;
                    if (Vector3.Distance(position, Camera.main.transform.position) < 10.0f)
                    {
                        // if we're close to this graph, then expand it out if we haven't already
                        if (!animatedGraphs.ContainsKey(entry.Key))
                        {
                            animatedGraphs.Add(entry.Key, 0.0f);
                        }
                        close = true;
                        break;
                    }
                }
            }

            // if we're no longer close to an already expanded graph, reset the positions of the graph's intersecting children
            if (!close && animatedGraphs.ContainsKey(entry.Key))
            {
                animatedGraphs.Remove(entry.Key);

                // hide all the intersecting graphs
                foreach (int element in entry.Value)
                {
                    readings[element].gameObject.transform.position = new Vector3(0, 0, -1000);
                }
            }
        }

        // process the expansion operation
        foreach (int key in animatedGraphs.Keys.ToList())
        {
            // calculate the percent for the cosine interpolation, animation lasts 0.7 seconds
            float percent = animatedGraphs[key] / 0.7f;
            if (percent < 1)
            {
                foreach (int element in intersectingGraphs[key])
                {
                    // turn the linear interpolation into a cosine interpolation (its smoother)
                    float smoothScale = -(float)Math.Cos((float)Math.PI * percent) / 2.0f + 0.5f;
                    readings[element].gameObject.transform.position = Vector3.Lerp(
                        intersectingGraphsPositions[element].Item1,
                        intersectingGraphsPositions[element].Item2,
                        smoothScale
                    );
                }
            }
            
            // increment deltaTime so we actually animate
            animatedGraphs[key] += Time.deltaTime;
        }
    }

    //Monitor changes to originLatLong, move readings as needed. 
    void RepositionPlots()
    {
        intersectingGraphs = new Dictionary<int, List<int>>();
        intersectingGraphsPositions = new Dictionary<int, (Vector3, Vector3)>();
        
        if (originLatLong != TerrainManager.activeTerrain.originLatLong)
        {
            originLatLong = TerrainManager.activeTerrain.originLatLong;

            for (int i = 0; i < readings.Count; i++)
            {
                float x = (readings[i].longitude - originLatLong.y) * metersPerDegree;
                float z = (readings[i].latitude - originLatLong.x) * metersPerDegree;
                Vector2 unityCoords = CoordinateEquations.latlongToUnity(readings[i].latitude, readings[i].longitude);

                Ray r = new Ray(unityCoords.x * Vector3.right + Vector3.up * 1000 + unityCoords.y * Vector3.forward, Vector3.down);
                RaycastHit hitinfo;
                
                int terrainLayerMask = 1 << terrain;    //cast rays only against colliders in layer "terrain" 
                if (Physics.Raycast(r, out hitinfo, Mathf.Infinity, terrainLayerMask))
                {
                    readings[i].gameObject.transform.position = hitinfo.point + Vector3.up;
                }
                else
                {
                    readings[i].gameObject.transform.position = new Vector3(unityCoords.x, 0, unityCoords.y);
                }
            }
            
            // once we're done calculating the positions of all the graphs, see if any are intersecting
            // note: runs in O(n^2) time, implement a quadtree if you want this to run faster (shouldn't be needed though, there
            // aren't like thousands of APXS graphs to worry about and this set position method is only run once anyways)
            for (int i = 0; i < readings.Count; i++)
            {
                // don't test the graph if there's already other graphs intersecting with it (treat this graph as the parent
                // of all other intersecting graphs)
                if (!intersectingGraphs.ContainsKey(i))
                {
                    // test the graph against all other graphs
                    for(int j = 0; j < readings.Count; j++)
                    {
                        // only test for intersection if:
                        // 1. the graph we're testing (i) isn't the one we just located (j)
                        // 2. the graph we just located (j) isn't already intersecting with another graph
                        if (j != i && !intersectingGraphsPositions.ContainsKey(j))
                        {
                            // if we're close enough, then the graphs are probably intersecting
                            if (Vector3.Distance(readings[i].gameObject.transform.position, readings[j].gameObject.transform.position) < 2.0f)
                            {
                                // treat graph j as our parent graph. if our parent graph doesn't already have a list of intersecting graphs,
                                // then create one so we can add the graph we're testing (i) to it
                                if (!intersectingGraphs.ContainsKey(j))
                                {
                                    intersectingGraphs[j] = new List<int>();
                                }

                                intersectingGraphs[j].Add(i);
                                
                                // if the graph we're testing doesn't have its positions set, then set them
                                if (!intersectingGraphsPositions.ContainsKey(i)) {
                                    intersectingGraphsPositions.Add(i, (
                                        // parent's position
                                        readings[j].gameObject.transform.position,
                                        // parent's position but off to the side so the graph we're testing doesn't intersect with it anymore
                                        readings[j].gameObject.transform.position + new Vector3(2 * intersectingGraphs[j].Count, 0, 0)
                                    ));
                                }
                                // hide the graph at a super secret location
                                readings[i].gameObject.transform.position = new Vector3(0, 0, -1000);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    // Awake is called before start
    void Awake()
    {
        
        //rawImage.texture = apxsTex;
        colors = new Color[512 * 512];
        emptyColors = plotBackground.GetPixels();
        emptyColors.CopyTo(colors, 0);  //make graph black

        readCsv();

        for (int i = 0; i < readings.Count; i++)
        {
            PlotData(i);
            GameObject newPlotQuad = CreatePlotObject();
            GameObject newapxsLabel = Instantiate(apxsLabel, newPlotQuad.transform);
            //APXS: Sol - TargetName
            newapxsLabel.GetComponent<TextMeshPro>().text = "APXS: Sol " +
                readings[i].sol+" - "+readings[i].target;
            readings[i].gameObject = newPlotQuad;
            readings[i].gameObject.name = readings[i].sol + " - " + readings[i].target;
        }

        RepositionPlots();
        
    }

    public GameObject CreatePlotObject()
    {
        GameObject newPlotObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newPlotObject.AddComponent<PointToCamera>();
        newPlotObject.transform.parent = transform;

        Texture2D texture = new Texture2D(512, 512);
        texture.SetPixels(colors);  //apply colors array to canvas
        texture.Apply();

        Shader shader = Shader.Find("Standard");
        Material mat = new Material(shader);
        mat.SetTexture("_MainTex", texture);

        newPlotObject.GetComponent<Renderer>().material = mat;
        newPlotObject.transform.localScale = new Vector3(0.001f, 1,1);
        return newPlotObject;
    }

    public void readCsv()
    {
        List<string> whiteSpaceList = new List<string>();
        using (var reader = new StreamReader(new MemoryStream((Resources.Load("APXS", typeof(TextAsset)) as TextAsset).bytes)))    
        {
            //reads header
            var firstLine = reader.ReadLine();
            var names = firstLine.Split(',');
            
            readings = new List<ApxsReading>();
            while (!reader.EndOfStream)
            {
                try
                {
                    var line = reader.ReadLine();
                    string[] values = line.Split(',');
                    ApxsReading reading = new ApxsReading();
                    reading.sol = values[0];
                    reading.target = values[1];
                    reading.longitude = float.Parse(values[5]);
                    reading.latitude = float.Parse(values[6]);
                    for (int i = 0; i < 16; i++)
                    {
                        reading.oxides[i] = float.Parse(values[14 + i * 2]);
                        reading.errors[i] = float.Parse(values[14 + i * 2 + 1]);
                    }
                    readings.Add(reading);
                }
                catch
                {

                }

            }
        }

        //removes entries with white space
        for (int i = 0; i < whiteSpaceList.Count; i++)
        {
            csvDataTable.Remove(whiteSpaceList[i]);
            columnNames.Remove(whiteSpaceList[i]);
        }
    }

    public void readCsv(TextAsset dataAsset) {
        List<string> whiteSpaceList = new List<string>();
        using (var reader = new StreamReader(new MemoryStream((dataAsset).bytes)))
        {
            //reads header
            var firstLine = reader.ReadLine();
            var names = firstLine.Split(',');

            readings = new List<ApxsReading>();
            while (!reader.EndOfStream) {
                try {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    ApxsReading reading = new ApxsReading();

                    reading.longitude = float.Parse(values[6]);
                    reading.latitude = float.Parse(values[5]);
                    for (int i = 0; i < 16; i++) {
                        reading.oxides[i] = float.Parse(values[14 + i * 2]);
                        reading.errors[i] = float.Parse(values[14 + i * 2 + 1]);
                    }
                    readings.Add(reading);
                } catch {

                }
            }
        }

        //removes entries with white space
        for (int i = 0; i < whiteSpaceList.Count; i++) {
            csvDataTable.Remove(whiteSpaceList[i]);
            columnNames.Remove(whiteSpaceList[i]);
        }
    }

    public void drawCircle(int xcoor, int ycoor, int radius)
    {
        for (int xi = xcoor - radius; xi <= xcoor + radius; xi++)
        {
            for (int yi = ycoor - radius; yi <= ycoor + radius; yi++)
            {
                try
                {
                    colors[yi * numPixelsX + xi] = new Color(1.0f, 1.0f, 1.0f);
                }
                catch (System.IndexOutOfRangeException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
    }

    public void PlotData(int row)
    {
        //ResetPixels();
        emptyColors.CopyTo(colors, 0);  //make graph black


        float apxsMaxReading = 1.5f;
        float spacingX = numPixelsX / (16 + 1f);
        float yScale = numPixelsY / apxsMaxReading;
        int barWidth = 2;
        for (int i = 0; i < 16; i++)
        {
            int x = (int)(spacingX * (i + 1));
            int y = (int)(yScale * Mathf.Min(readings[row].oxides[i]/readings[0].oxides[i],2)/2);   //Max it out at 2

            for (int yy = 0; yy < y; yy++)
            {
                for (int bar_x = -barWidth; bar_x < barWidth; bar_x++)
                {
                    int xx = x + bar_x;
                    int yyy = yy + pixelOffsetY;
                    int xxx = xx + pixelOffsetX;
                    if (yyy * 512 + xxx < colors.Length) { 
                        colors[yyy * 512 + xxx] = Color.white;
                    }
                }
            }
        }
    }

    public void ResetPixels()
    {
        emptyColors.CopyTo(colors, 0);  //make graph black
    }
}