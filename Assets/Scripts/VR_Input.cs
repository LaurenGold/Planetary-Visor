using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class VR_Input : MonoBehaviour {

    public bool debug;
    public InputDevice lController, rController;
    void Start()
    {
        SetupControllers(debug);
    }

    /*
     * Assign controller references and optionally log statements.
     */ 
    public void SetupControllers(bool debug=false) {
        var vr_devices = new List<InputDevice>();
        InputDevices.GetDevicesWithRole(InputDeviceRole.LeftHanded, vr_devices);
        if (vr_devices.Count > 0)
            lController = vr_devices[0];

        InputDevices.GetDevicesWithRole(InputDeviceRole.RightHanded, vr_devices);
        if (vr_devices.Count > 0)
            rController = vr_devices[0];

        if (debug) {
            foreach (var device in vr_devices) {
                Debug.Log(string.Format("Device name '{0}' has role '{1}'",
                    device.name, device.role.ToString()));
            }
        }
    }

    /*
     * NOTE: This does not provide StateUp/StateDown for trigger.
     */ 
    public bool GetTrigger(InputDeviceRole hand) {
        if (hand.Equals(InputDeviceRole.LeftHanded))
            return (lController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue);
        else if (hand.Equals(InputDeviceRole.RightHanded))
            return (rController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue);
        else
            return false;
    }

    /*
     * NOTE: This does not provide StateUp/StateDown for grip.
     */
    public bool GetGrip(InputDeviceRole hand) {
        if (hand.Equals(InputDeviceRole.LeftHanded))
            return (lController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) && gripValue);
        else if (hand.Equals(InputDeviceRole.RightHanded))
            return (rController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) && gripValue);
        else
            return false;
    }

    /*
     * Invoke haptic feedback for controllers via amplitude and timing.
     */ 
    public void ProvideHaptic(InputDeviceRole hand, float amplitude, float duration) {
        HapticCapabilities capabilities;
        uint channel = 0;

        if (hand.Equals(InputDeviceRole.LeftHanded) &&
            lController.TryGetHapticCapabilities(out capabilities)) {
            if (capabilities.supportsImpulse) {
                lController.SendHapticImpulse(channel, amplitude, duration);
            }
        } else if (hand.Equals(InputDeviceRole.RightHanded) &&
            rController.TryGetHapticCapabilities(out capabilities)) {
            if (capabilities.supportsImpulse) {
                rController.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

}
