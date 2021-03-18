using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AssignMeshCollider : MonoBehaviour
{
    public TeleportationArea teleportationArea;
    // TODO: Load/instantiate prefab from resources.
    public GameObject highlightFence;   // Children: left, right, top, bott
    // Start is called before the first frame update
    void Start()
    {
        if (teleportationArea == null)
            teleportationArea = gameObject.GetComponent<TeleportationArea>();

        if (teleportationArea.colliders.Count <= 1) {
            teleportationArea.colliders.Add(gameObject.GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    { 
        //if (teleportationArea.colliders.Count >= 1) {
        //    Debug.Log(string.Format("Bounds:\t{0}",teleportationArea.colliders[0].bounds.ToString()));
        //    highlightFence.transform.position = teleportationArea.colliders[0].bounds.center;
        //    highlightFence.transform.GetChild(0).transform.position = (teleportationArea.colliders[0].bounds.center + teleportationArea.colliders[0].bounds.extents);
        //    highlightFence.transform.GetChild(1).transform.position = (teleportationArea.colliders[0].bounds.center - teleportationArea.colliders[0].bounds.extents);
        //    // TODO: Add remaining bounds/fencing.
        //}
    }
}
