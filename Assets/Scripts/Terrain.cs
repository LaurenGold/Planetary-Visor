using UnityEngine;

/// <summary>
/// Class terrain keeps track of each terrains information
/// </summary>
public class Terrain : MonoBehaviour {
    public GameObject terrainMesh;
    public Vector2 originLatLong;
    public string terrainName;
    
    // Initializes class members using attached gameobject
    void Awake() {
        terrainMesh = gameObject;
        terrainName = gameObject.name;    
    }
}
