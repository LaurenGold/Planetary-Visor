using UnityEngine;

/** Rotates the star icon on the sat map **/
public class ForwardIndicator : MonoBehaviour {

    private RectTransform forwardArrowTransform;
    private GameObject MainCameraSight;

	void Start () {
        forwardArrowTransform = gameObject.GetComponent<RectTransform>();
        MainCameraSight = Camera.main.gameObject;
	}

	void Update () {
        Vector3 newRotation = forwardArrowTransform.localEulerAngles;
        // Inverse and assign to z prop because of rect transform variation from 0-360 on y.
        newRotation.z = MainCameraSight.transform.localEulerAngles.y;
        newRotation.z *= -1;
        forwardArrowTransform.localEulerAngles = newRotation;
	}
}
