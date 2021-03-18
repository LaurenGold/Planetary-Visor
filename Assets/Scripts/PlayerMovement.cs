using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    [Tooltip("Speed for W,A,S,D movement.")]
    public float moveSpeed = 4.5f;
    [Tooltip("Speed for dragging object around.")]
    public float speedH = 2.0f, speedV = 2.0f;
    // Every object instantiated with this class has a reference to UI manager
    // so that they may assign themselves as "Selected."
    [Tooltip("Root/parent object containing UI canvas.")]
    //public UIManager m_UI;
    private Vector3 moveDirection;
    private float yaw, pitch;
    private Ray ray;
    private RaycastHit hit;

    public void Awake() {
        //if (m_UI == null) m_UI = GameObject.Find("UI").GetComponent<Canvas>();
    }

    public void Start() {
        //m_UI.ToggleButtons(false);
    }

    void FixedUpdate() {
        //if user left clicks
        if (Input.GetMouseButton(0)) {
            Debug.Log("User left clicked!");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // If clicking object, display UI for step creation/editing.
            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject()) {
                Debug.Log("Hit object");
            }
            if (Input.GetMouseButton(1)) {
                Debug.Log("User right clicked");
                yaw += speedH * Input.GetAxis("Mouse X");
                pitch -= speedV * Input.GetAxis("Mouse Y");
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }
            if (!EventSystem.current.IsPointerOverGameObject())
                CheckControls();
        }

        void CheckControls() {
            //if (m_UI.Typing) return;
            moveDirection = Vector3.zero;

            // Separate conditionals so multiple buttons can be held simultaneously.
            if (Input.GetKeyDown(KeyCode.W)) moveDirection += transform.forward;
            if (Input.GetKeyDown(KeyCode.S)) moveDirection += -transform.forward;
            if (Input.GetKeyDown(KeyCode.A)) moveDirection += -transform.right;
            if (Input.GetKeyDown(KeyCode.D)) moveDirection += transform.right;
            transform.position += moveDirection.normalized * Time.deltaTime;
        }

    }
}
