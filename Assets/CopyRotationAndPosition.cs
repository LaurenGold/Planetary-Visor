using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotationAndPosition : MonoBehaviour {
    public GameObject Origin;
    public Vector3 OriginalScale;

    // Update is called once per frame
    void Update() {
        transform.position = Origin.gameObject.transform.position;
        transform.rotation = Origin.gameObject.transform.rotation;
        transform.localScale = OriginalScale * Camera.main.transform.lossyScale.y;
    }

    /// <summary>
    /// set the origin for our script
    /// </summary>
    public void SetOrigin(GameObject origin) {
        Origin = origin;
    }
}
