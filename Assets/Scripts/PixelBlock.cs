using System;
using UnityEngine;

/// <summary>
/// Class PixelBlock moves and shapes the current pixel block
/// </summary>
/// <remarks>
/// Longitude being the x (W/E), latitude being the z assignment (N/S)
/// </remarks>

public class PixelBlock : MonoBehaviour {
    public Color32 pb_color;
    public Vector2 lastKnownColRow;
    public bool averagePixelBlock;
    public bool temporalPB; //for future implementation
    private float extradist = 6;

    private Vector3[] linePositions;
    private LineRenderer lineRenderer;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private int layer_mask;


    //Note: This must be public to ensure PixelBlockManager can access Start()
    // Otherwise the PBs will not be ready the first time they are called
    public void Start() {
        if(gameObject.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.2f;
            lineRenderer.positionCount = 80;
            linePositions = new Vector3[80];
            lineRenderer.useWorldSpace = true;
        }
        layer_mask = LayerMask.GetMask("Terrain");
    }

    /// <summary>
    /// Gets the column and row from the DataCube required to calculate vectors
    /// </summary>
    /// <param name="x"> Specified Unity X coordinate of pixelblock location </param>
    /// <param name="z"> Specified Unity Z coordinate of pixelblock location </param>
    /// <returns>true if new colrow</returns>
    public bool MoveShapeDrawFromXZ(float x, float z)
    {
        Vector2 colrow = DataCube.getColRowFromUnityXZ(x, z);   //get colrow from datacube
        if (lastKnownColRow == colrow)
            return false;
        
        Vector2[] corners = CalculateCorners(Mathf.RoundToInt(colrow.x), Mathf.RoundToInt(colrow.y));
        MoveShapeAndDrawPixelBlock(corners);
        lastKnownColRow = colrow;

        return true;
    }

    /// <summary>
    /// Calculates 4 vectors required to draw the pixel block on terrain
    /// </summary>
    /// <param name = "col" > Column retrieved from DataCube</param>
    /// <param name = "row" > Row retrieved from DataCube</param>
    /// <remarks>
    /// Returns corners in Unity coordinates
    /// </remarks>
    Vector2[] CalculateCorners(float col, float row)
    {
        Vector2[,] coordCube = DataCube.coordCube;
        int dataHeight = coordCube.GetLength(1);
        int dataWidth = coordCube.GetLength(0);
        // Pixel indexing is addressed between indices, so 0-1 refers to pixel 1.
        // Last pixel can be indexed by addressing second to last indice.
        int selectedRow = Mathf.RoundToInt(Mathf.Clamp(row, 1, dataHeight - 2));
        int selectedCol = Mathf.RoundToInt(Mathf.Clamp(col, 1, dataWidth - 2));
        // Find nearest point from user selection corresponding to 4 square regions from
        // center point. Think with respect to satellite long/lat, 
        // which pixel are we looking at with respect to user selection?
        // Each region is averaged out to encompass full surrounding pixels to which
        // values are passed onto raycasting based on where the pixel block will be located
        // due to change in region because of origin difference per terrain.
        Vector2[] corners = new Vector2[4];

        corners[0] = (coordCube[selectedCol, selectedRow] + coordCube[selectedCol - 1, selectedRow - 1]
              + coordCube[selectedCol - 1, selectedRow] + coordCube[selectedCol, selectedRow - 1]) / 4;
        corners[1] = (coordCube[selectedCol, selectedRow] + coordCube[selectedCol + 1, selectedRow - 1]
          + coordCube[selectedCol + 1, selectedRow] + coordCube[selectedCol, selectedRow - 1]) / 4;
        corners[2] = (coordCube[selectedCol, selectedRow] + coordCube[selectedCol + 1, selectedRow + 1]
         + coordCube[selectedCol + 1, selectedRow] + coordCube[selectedCol, selectedRow + 1]) / 4;
        corners[3] = (coordCube[selectedCol, selectedRow] + coordCube[selectedCol - 1, selectedRow + 1]
         + coordCube[selectedCol - 1, selectedRow] + coordCube[selectedCol, selectedRow + 1]) / 4;
       
        for (int i = 0; i < 4; i++) {
            corners[i] = CoordinateEquations.latlongToUnity(corners[i]);
        }
        return corners;
    }

    /// <summary>
    /// Creates the final shape of the pixel block and draws mesh
    /// </summary>
    /// <param name="v1"> Corners of the pixel block in x,z Unity coordinates</param>
    public void MoveShapeAndDrawPixelBlock(Vector2[] corners)//Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        Vector2 center = (corners[0]+corners[1]+corners[2]+corners[3]) / 4;
        Ray ray_p = new Ray(new Vector3(center.x, 100, center.y), Vector3.down);
        
        RaycastHit hit;
        if (!Physics.Raycast(ray_p, out hit, Mathf.Infinity, layer_mask))
            return;
        
        transform.position = hit.point;
        //World coordinates of MRO;
        Vector3 mro_pos = PixelBlockManager.manager.mro.transform.position;

        Vector3 v1 = new Vector3(corners[0].x, transform.position.y,corners[0].y);
        Vector3 v2 = new Vector3(corners[1].x, transform.position.y, corners[1].y);
        Vector3 v3 = new Vector3(corners[2].x, transform.position.y, corners[2].y);
        Vector3 v4 = new Vector3(corners[3].x, transform.position.y, corners[3].y);

        Vector3 topA = Vector3.MoveTowards(v1, mro_pos, extradist) - transform.position;
        Vector3 topB = Vector3.MoveTowards(v2, mro_pos, extradist) - transform.position;
        Vector3 topC = Vector3.MoveTowards(v3, mro_pos, extradist) - transform.position;
        Vector3 topD = Vector3.MoveTowards(v4, mro_pos, extradist) - transform.position;
        //Debug.Log(topA +"," +topB + ","+topC+","+topD);
        Vector3 botA = Vector3.MoveTowards(v1, mro_pos, -extradist) - transform.position;
        Vector3 botB = Vector3.MoveTowards(v2, mro_pos, -extradist) - transform.position;
        Vector3 botC = Vector3.MoveTowards(v3, mro_pos, -extradist) - transform.position;
        Vector3 botD = Vector3.MoveTowards(v4, mro_pos, -extradist) - transform.position;
        //Debug.Log(botA + "," + botB + "," + botC + "," + botD);

        Vector3[] vertices = { botD, topA, topB, topC, topD, botA, botB, botC };
        int[] triangles = {
            1, 2, 3, //top
            4, 1, 3,
            3, 2, 6, //front
            7, 3, 6,
            2, 1, 5, //
            6, 2, 5,
            1, 4, 0, //
            5, 1, 0,
            3, 7, 0, //
            4, 3, 0,
            7, 6, 5, //
            0, 7, 5,
            //inside triangles
            3, 2, 1, //top
            3, 1, 4,
            6, 2, 3, //front
            6, 3, 7,
            5, 1, 2, //
            5, 2, 6,
            0, 4, 1, //
            0, 1, 5,
            0, 7, 3, //
            0, 3, 4,
            5, 6, 7, //
            5, 7, 0
        };

        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        DrawIntersectionLine(topA,botA,topB,botB,20,0);
        DrawIntersectionLine(topB,botB,topC,botC,20,20);   
        DrawIntersectionLine(topC,botC,topD,botD,20,40);
        DrawIntersectionLine(topD,botD,topA,botA,20,60);   

        lineRenderer.SetPositions(linePositions);
    }

    void DrawIntersectionLine(Vector3 p1t, Vector3 p1b, Vector3 p2t, Vector3 p2b, int length, int offset) {
        for (int i = 0; i < length; i++) {
            Vector3 pointTop = Vector3.Lerp(p1t + transform.position, p2t + transform.position, i / (length - 1f));
            Vector3 pointBot = Vector3.Lerp(p1b + transform.position, p2b + transform.position, i / (length - 1f));
            Ray ray_p = new Ray(pointTop, pointBot - pointTop);
            RaycastHit hit;
            if (Physics.Raycast(ray_p, out hit, Vector3.Distance(pointTop, pointBot) * 2, layer_mask)) {
                linePositions[offset + i] = hit.point;
            }
            else {
                linePositions[offset + i] = pointBot;
            }
        }
    }
}
