using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorControllerManager : MonoBehaviour {
    public GameObject pbody, lHand, rHand;
    [SerializeField]
    public Vector3 pbodyPos, lHandPos, rHandPos;
    [SerializeField]
    public Quaternion pbodyRot, lHandRot, rHandRot;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // left
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count == 1) {
            UnityEngine.XR.InputDevice device = leftHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
            //lHand.transform.position = device.
            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out lHandPos);
            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out lHandRot);
            lHand.transform.rotation = lHandRot;
            //lHand.transform.position = lHand.transform.InverseTransformVector(pbody.transform.position);
            lHand.transform.localPosition = lHandPos;
            //lHand.transform.position *= gameObject.transform.localScale.x;
        } else if (leftHandDevices.Count > 1) {
            Debug.Log("Found more than one left hand!");
        }
        // right
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        if (rightHandDevices.Count == 1) {
            UnityEngine.XR.InputDevice device = rightHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
            //lHand.transform.position = device.
            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out rHandPos);

            Debug.Log("rHandPos:" + rHandPos);

            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rHandRot);
            rHand.transform.rotation = rHandRot;
            rHand.transform.localPosition = rHandPos;
            //rHand.transform.position *= gameObject.transform.localScale.x;
        } else if (rightHandDevices.Count > 1) {
            Debug.Log("Found more than one right hand!");
        }

    }
}
