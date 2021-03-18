using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Speed for W,A,S,D movement.")]
    public float speed = 8;
    [Tooltip("Speed for looking around.")]
    public float mouseSensitivity = 8.5f;

    private float yaw, pitch;
    private Vector3 moveDirection;

    public GameObject playerBody;
    public Transform arm;
    public float cameraHeight;
    public Transform cameraTransform;
    void Start()
    {
        if (playerBody == null)
            playerBody = GameObject.Find("Player");
    }

    void Update()
    {
        yaw += mouseSensitivity * Input.GetAxis("Mouse X");
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
        if (pitch >= 90)
        {
            pitch = 90;
        }
        if (pitch <= -90)
        {
            pitch = -90;
        }
        playerBody.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Checks simultaneously for w,a,s,d
        playerBody.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0f, 
                            Input.GetAxis("Vertical") * Time.deltaTime * speed);
    }

    void CheckControls() {
        moveDirection = Vector3.zero;
        // Separate conditionals so multiple buttons can be held simultaneously.
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection += -transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection += -transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
        playerBody.transform.position += speed * moveDirection.normalized * Time.deltaTime;
    }

}
