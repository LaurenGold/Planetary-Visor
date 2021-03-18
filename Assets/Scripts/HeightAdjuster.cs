using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightAdjuster : MonoBehaviour
{
    public GameObject cameraRig;
    public GameObject user;
    public int terrainLayer;
    public float cameraHeight = 1.5f;
    public float scale = 1.0f;
    public Text scaleText;

    void Start()
    {
        terrainLayer = 31;
    }


    void Update()
    {
        scale *= (1+Input.GetAxis("Mouse ScrollWheel"));
        scale = Mathf.Clamp(scale, 1f, 250);
        RecalculateHeight(terrainLayer);
        if (scaleText != null)
            scaleText.text = "Scale (Mouse Wheel): " + scale+"X";
    }

    void RecalculateHeight(int terrain)
    {
        Ray r = new Ray(user.transform.position+Vector3.up*1000,Vector3.down);
        RaycastHit hitinfo;
        int terrainLayerMask = 1 << terrain;
        if (Physics.Raycast(r, out hitinfo, Mathf.Infinity, terrainLayerMask)) { 
            cameraRig.transform.position = new Vector3(cameraRig.transform.position.x,
                                                hitinfo.point.y + cameraHeight*scale,
                                                cameraRig.transform.position.z);
        }
    }
}
