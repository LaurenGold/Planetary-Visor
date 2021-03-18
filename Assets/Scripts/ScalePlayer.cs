using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Buttons.VR;

public class ScalePlayer : MonoBehaviour {
    private float originalDistance;
    private Vector3 originalScale;
    [SerializeField]
    public ButtonController lHandController, rHandController;
    private Hand lHand, rHand;
    public GameObject leftHand, rightHand;

    /// <summary>
    /// Sets up class variables to default values
    /// </summary>
    void Start() {
        originalDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Assign controllers to class variables
    /// </summary>
    public void AssignControllers() {
        var controllers = gameObject.GetComponents<ButtonController>();
        // iterate over button controllers, find left and right controller and assign reference for button mappings.
        foreach (ButtonController controller in controllers) {
            Debug.Log("Controller obj:\t" + controller.deviceRole);
            if (controller.deviceRole.Equals(InputDeviceRole.LeftHanded)) {
                lHandController = controller;
            } else if (controller.deviceRole.Equals(InputDeviceRole.RightHanded)) {
                rHandController = controller;
            }
        }
    }

    /// <summary>
    /// Ensure controllers are always connected
    /// </summary>
    public void Update() {
        if (lHandController == null || rHandController == null)
            AssignControllers();
    }

    /// <summary>
    /// Get the updated distance between the controllers
    /// </summary>
    public void GripDown() {
        originalDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Scale based on the current distance between the controllers
    /// </summary>
    public void GripPressing() {
        //While both grips are held down
        float currentDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
        float distRatio = originalDistance / currentDistance;

        float scaleX = originalScale.x * distRatio;
        float scaleY = originalScale.y * distRatio;
        float scaleZ = originalScale.z * distRatio;
        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        //Max / Min scale if too big or too small
        scaleX = Mathf.Clamp(scaleX, 1, 250);
        scaleY = Mathf.Clamp(scaleY, 1, 250);
        scaleZ = Mathf.Clamp(scaleZ, 1, 250);
        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        transform.localScale = originalScale;
    }
}
