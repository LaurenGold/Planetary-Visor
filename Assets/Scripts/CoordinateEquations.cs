using UnityEngine;

public static class CoordinateEquations {
    public static float metersPerLatDegree = 59288.8889f; // marsCirc/360
    public static float metersPerLongDegree = 59288.8889f; // marsCirc/360

    /// <summary>
    /// Converts unity (x,z) coordinates to Mars latitude and longitude
    /// </summary>
    /// <param name="unityX">Unity X coordinate</param>
    /// <param name="unityZ">Unity Z coordinate</param>
    /// <returns>Latitude and Longitude pair</returns>
    public static Vector2 unityCoordsToLatLong(float unityX, float unityZ) {
        Vector2 latlong = new Vector2(unityZ / metersPerLatDegree, unityX / metersPerLongDegree);
        latlong += TerrainManager.activeTerrain.originLatLong;
        return latlong;
    }

    /// <summary>
    /// Converts Mars latitude and longitude to unity (x,z) coordinates
    /// </summary>
    /// <param name="lat">Mars Latitude</param>
    /// <param name="longit">Mars Longitude</param>
    /// <returns>Unity x,y coordinates</returns>
    public static Vector2 latlongToUnity(float lat, float longit)
    {
        float adjustedLat = lat - TerrainManager.activeTerrain.originLatLong.x;
        float adjustedLong = longit - TerrainManager.activeTerrain.originLatLong.y;
        Vector2 unityCoords = new Vector2(adjustedLong * metersPerLongDegree, adjustedLat * metersPerLatDegree);
        return unityCoords;
    }

    /// <summary>
    /// Shadows latlongToUnity(float lat, float longit), with a Vector2 input.
    /// </summary>
    /// <param name="coord">Lat/Long</param>
    /// <returns>Unity x,z coordinates</returns>
    public static Vector2 latlongToUnity(Vector2 coord)
    {
        return latlongToUnity(coord.x, coord.y);
    }
}
