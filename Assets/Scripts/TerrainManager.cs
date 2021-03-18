using UnityEngine;

/// <summary>
/// Class TerrainManager handles the terrain selection and terrain menu state
/// </summary>

public class TerrainManager : MonoBehaviour {
    public static TerrainManager manager;
    public Terrain[] terrains;
    public static Terrain activeTerrain;
    public GameObject mainMenu, xrCursor;
    public PixelBlock pixelBlock;
    private GameObject player;
    private bool stateOfMenu, menuPressed;
    public static int terrainLayer = 31;


    /// <summary>
    /// Initializes class members and sets default terrain
    /// </summary>
    public void Awake() {
        manager = this;
        stateOfMenu = false;
        player = GameObject.Find("Player");
        terrains = FindObjectsOfType<Terrain>();

        activeTerrain = terrains[0];
        // NOTE: All terrain objects must be active so they are assigned at runtime.
        foreach (Terrain terrain in terrains) {
            if (!terrain.name.Equals("mariaspass")) {
                terrain.gameObject.SetActive(false);
            } else {
                activeTerrain = terrain;
            }
        }
    }

    /// <summary>
    /// Toggles the menu state when user presses menu button on controller
    /// </summary>
    public void ToggleMenu() {
        stateOfMenu = !stateOfMenu;
        mainMenu.SetActive(stateOfMenu);
    }

    /// <summary>
    /// User pulls trigger to select terrain
    /// </summary>
    public void SelectTerrain() {
        if (stateOfMenu == true) {
            Debug.Log("switching");
            int layer_mask = LayerMask.GetMask("UI");
            Ray ray = new Ray(xrCursor.transform.position, xrCursor.transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
                return;

            string selectedTerrain = hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.IndexOf("Menu"));
            SwitchTerrain(selectedTerrain);

            player.transform.position = new Vector3(0, 0, 0);
            pixelBlock.transform.position = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Activates the selected terrain and mesh
    /// </summary>
    /// <param name="selectedTerrain"> Specified terrain to switch to </param>
    public void SwitchTerrain(string selectedTerrain) {
        foreach (Terrain terrain in terrains) {
            if (terrain.name.Equals(selectedTerrain)) {
                terrain.terrainMesh.SetActive(true);
                activeTerrain = terrain;
                return;
            } else {
                terrain.terrainMesh.SetActive(false);
            }
        }
    }
}