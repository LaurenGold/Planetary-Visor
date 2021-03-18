using UnityEngine;

/// <summary>
/// Class MapPositionUpdater updates satellite map according to user's current location
/// </summary>

public class MapPositionUpdater : MonoBehaviour {
    public GameObject positionIndicator;
    public GameObject imageMap;
 
    /// <summary>
    /// Check for user's position and move player mark to equivalent location on map
    /// </summary>
    void Update() {
        float unityX = transform.position.x;
        float unityZ = transform.position.z;
  
        Vector2 latlong = CoordinateEquations.unityCoordsToLatLong(unityX, unityZ);
        Vector2 rowcol = DataCube.FindClosestRowCol(latlong);
        float row = rowcol.x/600f; //Scaled between 0 and 1 (600 is the width of the datacube)
        float col = rowcol.y/448f; //Scaled between 0 and 1 (448 is the height of the datacube)

        //Scale row and col to fit the image map
        //Image map dimensions: 213 x 188 | DataCube dimensions: 600 x 448
        float satelliteRow = row*213;
        float satelliteCol = col*188;
        Vector3 newSatCoords = new Vector3(satelliteRow, satelliteCol, 0);

        positionIndicator.transform.localPosition = newSatCoords;
    }
}
