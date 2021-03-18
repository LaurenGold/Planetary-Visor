using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine;

/// <summary>
/// Component attached to individual interactable elements in a Canvas UI. Automatically attached via
/// UIHapticsMain. Adds haptic/sound support to hover/click events for the interactable element.
/// </summary>
public class UIHaptics : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {
    [Tooltip("Right VR controller")]
    public GameObject RightController;
    [Tooltip("Left VR controller")]
    public GameObject LeftController;
    [Tooltip("Controller for this instance")]
    public UIHapticsMain Parent;

    private List<GameObject> controllerArray = new List<GameObject>();
    private List<InputDevice> controllerDeviceArray = new List<InputDevice>();

    void Start() {
        // put the controllers into an array
        controllerArray.Add(RightController);
        controllerArray.Add(LeftController);

        var vrDevices = new List<InputDevice>(); // temp array
        // get right hand
        InputDevices.GetDevicesWithRole(InputDeviceRole.RightHanded, vrDevices);
        if (vrDevices.Count > 0) {
            controllerDeviceArray.Add(vrDevices[0]);
        }

        // get left hand
        InputDevices.GetDevicesWithRole(InputDeviceRole.LeftHanded, vrDevices);
        if (vrDevices.Count > 0) {
            controllerDeviceArray.Add(vrDevices[0]);
        }
    }
    
    // implements from IPointerDownHandler
    public void OnPointerDown(PointerEventData pointerEventData) {
        VibrateController(pointerEventData);
        Parent.PressSound(this);
    }

    // implements from IPointerEnterHandler
    public void OnPointerEnter(PointerEventData pointerEventData) {
        VibrateController(pointerEventData);
        Parent.HoverSound(this);
    }

    /// <summary>
    /// vibrate a specific controller if it hovers/clicks a button
    /// <summary>
    private void VibrateController(PointerEventData pointerEventData) {
        float smallestDelta = 2;
        int interactedController = -1;
        for (int i = 0; i < controllerArray.Count; i++) {
            GameObject controller = controllerArray[i];
            Vector3 rightPosition = controller.transform.position;
            Vector3 rightDirection = controller.transform.forward;
            Vector3 directionToHit = Vector3.Normalize(pointerEventData.pointerCurrentRaycast.worldPosition - rightPosition);
            float dot = Vector3.Dot(rightDirection, directionToHit);

            // pick the controller that is closest to the raycast's direction
            if (1 - dot < smallestDelta) {
                smallestDelta = 1 - dot;
                interactedController = i;
            }
        }

        // if we found a controller, make it vibrate
        if (interactedController != -1) {
            controllerDeviceArray[interactedController].SendHapticImpulse(0, (float)0.5, (float)0.15);
        }
    }
}
