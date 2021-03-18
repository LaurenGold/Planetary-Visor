using UnityEngine;
using UnityEngine.UI;

public class SpectralMap : MonoBehaviour
{
    private Color[] mapCols;
    private Texture2D texMap;
    public Texture2D crosshairs;
    public RawImage mapImage, mapImageOverlay;
    private int rband = 5, gband = 60, bband = 120;

    /// <summary>
    /// Initialize variables and call MapData()
    /// </summary>
    void Start()
    {
        texMap = new Texture2D(DataCube.dataWidth, DataCube.dataHeight);
        mapCols = new Color[DataCube.dataWidth * DataCube.dataHeight];
        mapImageOverlay.texture = crosshairs;
        mapImage.texture = texMap;
        MapData();
    }

    /// <summary>
    /// Determine map pixels via DataCube and apply the result to the map canvas
    /// </summary>
    public void MapData() {
        for (int col = 0; col < DataCube.dataWidth; col++) {
            for (int row = 0; row < DataCube.dataHeight; row++) {
                //need to change col and row each time you teleport and use that location to update player marker
                float R = Mathf.InverseLerp(DataCube.lowerSSAbound, DataCube.upperSSAbound, DataCube.dataCube[col, row, rband]);
                float G = Mathf.InverseLerp(DataCube.lowerSSAbound, DataCube.upperSSAbound, DataCube.dataCube[col, row, gband]);
                float B = Mathf.InverseLerp(DataCube.lowerSSAbound, DataCube.upperSSAbound, DataCube.dataCube[col, row, bband]);
                mapCols[row * DataCube.dataWidth + col] = new Color(R, G, B);
            }
        }
        texMap.SetPixels(mapCols);
        texMap.Apply();
    }
}
