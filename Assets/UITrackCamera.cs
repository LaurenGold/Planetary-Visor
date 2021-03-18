using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Component meant to be attached to a Canvas object, allows the Canvas to position itself relative to the camera
/// when the player teleports/opens the UI. Automatically scales up/down when the player scales, and has terrain
// avoidance so it does not clip the UI with the terrain.
/// </summary>
public class UITrackCamera : MonoBehaviour {
    public Vector3 originalScale;
    public bool trackAtStart;

    private Vector3 rotation; // current rotation of the UI
    private Vector3 lastPosition = new Vector3(0.0f, 0.0f, -100.0f); // the last position the UI was at
    private float updateDistance = 1.5f; // amount of distance the UI must travel for its position to be updated
    private float lastScale = 0.0f;
    private float startTime;
    
    // Start is called before the first frame update
    void Start() {
        SetRotation();
        ResetPosition();

        if (trackAtStart) {
            startTime = Time.time;
        }
    }

    void Update() {
        SetPosition();

        // track the camera for a seccond at the start so the main menu is positioned correctly
        if (Time.time - startTime < 1) {
            ResetPosition();
        }
    }

    // called when gameobject is enabled
    void OnEnable() {
        SetRotation();
        ResetPosition();
    }

    // called when gameobject is enabled
    void Awake() {
        SetRotation();
        ResetPosition();
    }

    /// <summary>
    /// resets the cached position of the UI
    /// </summary>
    public void ResetPosition() {
        lastPosition = new Vector3(0.0f, 0.0f, -100.0f);
    }

    /// <summary>
    /// sets the rotation of the UI
    /// </summary>
    public void SetRotation() {
        rotation = Vector3.Normalize(new Vector3(
            Camera.main.transform.forward.x,
            0,
            Camera.main.transform.forward.z
        ));
    }

    /// <summary>
    /// set the position of the UI relative to the camera. position it in front, and account for the terrain
    /// so it doesn't get stuck.
    /// </summary>
    public void SetPosition() {
        Vector3 newPosition = rotation * 3f * Camera.main.transform.lossyScale.z
            + Camera.main.transform.position - Vector3.up * 0.5f * Camera.main.transform.lossyScale.z;
        
        transform.localScale = originalScale * Camera.main.transform.lossyScale.y;
        transform.rotation = Quaternion.LookRotation(rotation, Vector3.up);

        // calculate how big the UI is in the world
        var rectTransform = GetComponent<RectTransform>();
        float worldWidth = rectTransform.sizeDelta.x * originalScale.y * Camera.main.transform.lossyScale.y;
        float worldHeight = rectTransform.sizeDelta.y * originalScale.y * Camera.main.transform.lossyScale.y;

        // step along the UI's width to test if the UI overlaps with the ground
        float maxDistance = 0f;
        for (int i = -1; i <= 1; i++) {
            // set up some objects for the raycast
            RaycastHit hitObject;
            int terrainLayer = 1 << 31;

            // we're shooting a ray from the top of the UI straight down to the bottom. if we get an intersection,
            // then we should adjust the UI's position accordingly
            Vector3 start = newPosition + Vector3.up * worldHeight / 2f
                + transform.right * (float)(i) * worldWidth / 2f;
            Ray raycast = new Ray(start, Vector3.up * -1f);
            bool rayHit = Physics.Raycast(raycast, out hitObject, worldHeight, terrainLayer);
            if (rayHit) {
                // calculate the distance between the hit position and the bottom of the UI
                float distance = worldHeight - Vector3.Distance(hitObject.point, start);
                if (distance > maxDistance) {
                    maxDistance = distance;
                }
            }
        }

        // adjust the UI accordingly based on the distance between the hit position and the bottom of the UI
        if (maxDistance != 0f) {
            newPosition = rotation * 3f * Camera.main.transform.lossyScale.z
                + Camera.main.transform.position - Vector3.up * 0.5f * Camera.main.transform.lossyScale.z
                + Vector3.up * (maxDistance + 0.05f);
        }

        // set the position only if we've moved significantly
        if (
            Vector3.Distance(newPosition, lastPosition) > (updateDistance * Camera.main.transform.lossyScale.z)
            || Math.Abs(lastScale - Camera.main.transform.lossyScale.z) > 0.0001f // if they're close to equal
        ) { 
            transform.position = newPosition;

            lastPosition = transform.position;
            lastScale = Camera.main.transform.lossyScale.z;
        }
    }
}
