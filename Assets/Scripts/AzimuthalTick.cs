using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzimuthalTick : MonoBehaviour {

    private const float AZIMUTHAL_WIDTH = 450f; // 450 + 3 offset for tick thickness.
    private Camera userGaze;
    private int userGazeAngle, azimuthal_line_offset, priorOffset;
    private RectTransform objectTransform;
    private Vector3 tickPosition;

	void Start () {
        userGaze = Camera.main;
        azimuthal_line_offset = 3;     // Each tick has line thickness of 6, so half is 3.
        objectTransform = gameObject.GetComponent<RectTransform>();
        // Store current tick location.
        priorOffset = CheckTickOffset();
	}


    /// <summary>
    /// Calculate tick number, and offset by indicator value * 5
    /// </summary>
    /// <returns></returns>
    int CheckTickOffset() {
        int tickIndicator = int.Parse(gameObject.name.Substring(5));
        return tickIndicator * 56;      // Displacement value between ticks.
    }
	
	void Update () {
        tickPosition = objectTransform.gameObject.transform.localPosition;
        userGazeAngle = (int)userGaze.transform.localEulerAngles.y;
        // Get user gaze based on 90 degree segments
        userGazeAngle = userGazeAngle % 90;
        // Azimuth is scaled by 5 for widthspan.
        int pixelLocation = priorOffset + userGazeAngle * 5;
        //Debug.Log("Pixel location:\t" + pixelLocation);
        tickPosition.x = AZIMUTHAL_WIDTH - (pixelLocation % AZIMUTHAL_WIDTH);   // reset after 450"
        objectTransform.gameObject.transform.localPosition = tickPosition;
    }
}
