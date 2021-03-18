using System.IO;
using UnityEngine;

/// <summary>
/// Class DataCube reads in the Lat/Long/Spectral Data from bytes file
/// Contains access methods for other classes to access certain data
/// </summary>

public class TemporalDataCube {
    public static Vector2[,] coordCube;
    public static float[,,] dataCube;

    public static int dataWidth = 600;
    public static int dataHeight = 448;
    public static int spectralBands = 246;
    public static int coordBands = 14;
    public static float lowerSSAbound = 0.6f;
    public static float upperSSAbound = 0.9f;

    /// <summary>
    /// Reads in the bytes file and processes its data
    /// </summary>
    public static void LoadData() {
        if (dataCube != null) {
            return;
        }
        TextAsset dataAsset = Resources.Load("ssa") as TextAsset;
        BinaryReader br = new BinaryReader(new MemoryStream(dataAsset.bytes));
        dataCube = new float[dataWidth, dataHeight, spectralBands];
        // Read bands in reverse order. Each band is sequenced per its respective row/col.
        // So spectral band 249 is iterated dHeight*dWidth times to find all readings for band 249/
        // Same principle is applied for remaining bands till 1.
        for (int band = spectralBands - 1; band >= 0; band--) {
            for (int row = 0; row < dataHeight; row++) {
                for (int col = 0; col < dataWidth; col++) {
                    // dWidth is not 0 indexed, hence we subtract 1 as well for indexing.
                    dataCube[dataWidth - col - 1, row, band] = br.ReadSingle();
                }
            }
        }
        // Open new stream for reading coordinates. This prevents false readings from our stream due to
        // ReadSingle() calls.
        TextAsset dataAsset2 = Resources.Load("ddr") as TextAsset;
        BinaryReader br2 = new BinaryReader(new MemoryStream(dataAsset2.bytes));

        float long_min = 137.4333f;
        float long_max = 0;
        float lat_min = 0;
        float lat_max = -4.709409f;

        coordCube = new Vector2[dataWidth, dataHeight];
        // Only 14 coordinate bands, we iterate through each of them with respect to the row and column.
        // Each band is indexed for it's respective reading per region of pixel from satellite. 
        // So band 0 has dHeight*dWidth samples. 
        for (int band = 0; band < coordBands; band++) {
            for (int row = 0; row < dataHeight; row++) {
                for (int colTemp = 0; colTemp < dataWidth; colTemp++) {
                    int col = dataWidth - colTemp - 1;//reverse order
                    //coordCube[dataWidth - sample-1, lin, band] = br2.ReadSingle();
                    if (band == 3) {            // latitude                   
                        coordCube[col, row] = new Vector2(br2.ReadSingle(), 0);
                        lat_min = lat_min < coordCube[col, row].x ? lat_min : coordCube[col, row].x;
                        lat_max = lat_max > coordCube[col, row].x ? lat_max : coordCube[col, row].x;
                    } else if (band == 4) {     // longitude
                        coordCube[col, row].y = br2.ReadSingle();
                        long_min = long_min < coordCube[col, row].y ? long_min : coordCube[col, row].y;
                        long_max = long_max > coordCube[col, row].y ? long_max : coordCube[col, row].y;
                    } else {
                        br2.ReadSingle();       // Must call to continue sequence read of stream.
                    }
                }
            }
        }
    }

    /// <summary>
    /// Converts Unity (X, Z) coords to DataCube col row
    /// </summary>
    /// <param name="originLatLong"> Origin of current terrain </param>
    /// <param name="x"> Unity X coordinate to convert </param>
    /// <param name="z"> Unity Z coordinate to convert </param>
    /// <returns> DataCube col row </returns>
    public static Vector2 getColRowFromUnityXZ(Vector2 originLatLong, float x, float z) {
        Vector2 latlong = CoordinateEquations.unityCoordsToLatLong(x, z);
        latlong += new Vector2(originLatLong.x, originLatLong.y);
        Vector2 colrow = FindClosestRowCol(latlong);
        return colrow;
    }

    /// <summary>
    /// Find closest point representing lat/long on satellite pixel render.
    /// Converts lat/long to col/row
    /// </summary>
    /// <param name="desiredLatLong"> Lat/Long to be converted </param>
    /// <returns> col row Vector2 </returns>
    /// <remarks> Used to linearly scale for terrain representation. </remarks>
    public static Vector2 FindClosestRowCol(Vector2 desiredLatLong) {
        Vector2 closestPoint = new Vector2();
        float minDistance = Mathf.Infinity;
        for (int row = 0; row < dataHeight; row++) {
            for (int col = 0; col < dataWidth; col++) {
                float distToDesiredLatLong = Vector2.Distance(desiredLatLong, coordCube[col, row]);
                if (distToDesiredLatLong < minDistance) {
                    minDistance = distToDesiredLatLong;
                    closestPoint = new Vector2(col, row);
                }
            }
        }
        return closestPoint;
    }

    /// <summary>
    /// Finds the latitude and longitude from the given row col
    /// </summary>
    /// <param name="xRow"> Row to be converted to lat </param>
    /// <param name="yCol"> Col to be converted to long </param>
    /// <returns> Closest lat/long to input col row </returns>
    public static Vector2 FindLatLongFromRowCol(float xRow, float yCol) {
        int xRounded = (int)Mathf.Round(xRow);
        int yRounded = (int)Mathf.Round(yCol);

        Vector2 closestPoint = coordCube[xRounded, yRounded];
        return closestPoint;
    }
}
