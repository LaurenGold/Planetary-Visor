using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMRO : MonoBehaviour
{
    public PixelBlock pixelBlock;                   
    public float initial_latitude = -8.0429f;
    public float initial_longitude = 136.7477f;
    public float initial_altitudeKm = 2.48237f;
    public float final_latitude = 135.96586485f;   
    public float final_longitude = -1.63527329f;  
    public float final_altitudeKm = 3658.8f; 
    public Vector3 initial_position;
    public Vector3 final_position;
    public float final_altitude;

    float marsRadKm = 3389;
   
    void Update() {
        //going from unity mars latlon to unity coords
        Vector2 init_unitycoords = CoordinateEquations.latlongToUnity(initial_longitude, initial_latitude); 
        Vector2 final_unitycoords = CoordinateEquations.latlongToUnity(final_longitude, final_latitude);
        float y = (initial_altitudeKm - marsRadKm) * 1000;
        initial_position = new Vector3(init_unitycoords.x, y, init_unitycoords.y);
        final_position = new Vector3(final_unitycoords.x, y, final_unitycoords.y);
        
    }
    public void MoveBetween(float t) {

        //interpolate between mro start poition and end position
        transform.position = Vector3.Lerp(initial_position, final_position, t);
    }



}
