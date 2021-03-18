using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiriseReposition : MonoBehaviour
{
    Vector2 latlong = new Vector2(-4.67572f,137.3755f); //Latitude and Longitude of center    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 unityXZ = CoordinateEquations.latlongToUnity(latlong);
        transform.position = new Vector3(unityXZ.x, transform.position.y, unityXZ.y);
    }
}
