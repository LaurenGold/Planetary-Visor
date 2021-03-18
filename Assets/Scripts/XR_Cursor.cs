using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// from GitHub/StoneFox/ViveCursor.cs
//Alis' Edits

public class XR_Cursor : MonoBehaviour {
    public enum AxisType {
        XAxis,
        ZAxis
    }

    public bool showCursor = true;
    public bool validTarget = true;
    public Color color;
    public AxisType facingAxis = AxisType.XAxis;
    public float thickness = 0.002f;
    public float length = 1000f;     // May hard code set distance for shooting data cube.
    protected float contactDistance = 0f;
    public RaycastHit hitObject;
    protected Transform contactTarget = null;

    public GameObject cursor;
    private GameObject holder, pointer;
    private Material myMaterial;
    public Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);

    public void setColor(Color col) {
        color = col;
        if (myMaterial != null)
            myMaterial.SetColor("_Color", color);
    }

    /// <summary>
    /// Initialize class variables
    /// </summary>
    void Awake() {
        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity; //Fixes rotation to be consistent with glove. 

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localRotation = Quaternion.identity;

        myMaterial = new Material(pointer.GetComponent<MeshRenderer>().material.shader);
        myMaterial.SetColor("_Color", color);
        pointer.GetComponent<MeshRenderer>().material = myMaterial;

        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<Rigidbody>().isKinematic = true;
        pointer.layer = 2;

        if (showCursor) {
            cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.transform.parent = transform;
            cursor.GetComponent<MeshRenderer>().material = myMaterial;
            cursor.transform.localScale = cursorScale* cursor.transform.localScale.x/ cursor.transform.lossyScale.x;


            cursor.GetComponent<SphereCollider>().isTrigger = true;
            cursor.AddComponent<Rigidbody>().isKinematic = true;
            cursor.layer = 2;
        }

        SetPointerTransform(length, thickness);
    }

    float GetBeamLength(bool bHit, RaycastHit hit) {
        float actualLength = length;
        //reset if beam not hitting or hitting new target
        if (!bHit || (contactTarget && contactTarget != hit.transform)) {
            contactDistance = 0f;
            contactTarget = null;
        }
        //check if beam has hit a new target
        if (bHit) {
            contactDistance = hit.distance;
            contactTarget = hit.transform;
        }
        //adjust beam length if something is blocking it
        if (bHit && contactDistance < length) { actualLength = contactDistance; }
        if (actualLength <= 0) {    actualLength = length;  }
        return actualLength;
    }

   

    void Update() {
        int PlaneLayer = 1 << 31;

        // get world space coordinates for controller in up direction.
        Vector3 v = transform.TransformDirection(Vector3.forward);   
        
        Ray raycast = new Ray(transform.position, v);
        bool rayHit = Physics.Raycast(raycast, out hitObject, Mathf.Infinity, PlaneLayer);
        // Beam length equals distance between vectors a and b in the same coordinate space (local to this object).
        // Length of two vectors is a^2+b^2=length^2

        if (rayHit) {
            cursor.transform.position = hitObject.point;
        }
        float beamLength = Vector3.Magnitude(cursor.transform.localPosition);
        SetPointerTransform(beamLength, thickness);
        cursor.transform.localScale = cursorScale * cursor.transform.localScale.x / cursor.transform.lossyScale.x;

        // test against UI layer. we don't want any XR interaction to occur with mars terrain/etc if our cursor
        // is hitting a UI
        List<RaycastResult> results = new List<RaycastResult>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(
            // we know that the UI will always be in a constant distance relative to the camera, so put our world
            // coordinates at that constant distance
            transform.position + v * 3f * Camera.main.transform.lossyScale.z
        );
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPoint;
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0) {
            validTarget = false;
        }
        else {
            validTarget = true;
        }
    }

    void SetPointerTransform(float setLength, float setThickness) {
        //if the additional decimal isn't added then the beam position glitches
        float beamPosition = setLength / (2 + 0.00001f);        
        // position of primitives is in center, so moving position to desired location/2 suffices.
        // Z is scaled to reflect the beam length, position is placed halfway which gives proper delta between controller and collision point.
        pointer.transform.localScale = new Vector3(setThickness, setThickness, setLength);
        pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);
    }
}