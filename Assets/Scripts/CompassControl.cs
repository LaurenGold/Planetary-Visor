using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassControl : MonoBehaviour {

    public GameObject azimuth;
    public GameObject North, East, South, West;
    private RectTransform azimithUI;
    private float azimuthWidth;
    public GameObject userGaze;

	void Start () {

        if (gameObject.name.Contains("Azimuth"))
            azimuth = gameObject;
        else
            Debug.Log("Need to assign Azmith property to Azimuth game object.");

        if (azimuth) {  // If azimuth is assigned.
            North = gameObject.transform.GetChild(0).gameObject;
            East = gameObject.transform.GetChild(1).gameObject;
            South = gameObject.transform.GetChild(2).gameObject;
            West = gameObject.transform.GetChild(3).gameObject;
        }
        azimithUI = azimuth.GetComponent<RectTransform>();
        azimuthWidth = azimithUI.sizeDelta.x;
        // Azimith indicators get offsetted for some reason, thus resetting to 0.
        ResetAzimuthIndicator(0, ref North);
        ResetAzimuthIndicator(0, ref East);
        ResetAzimuthIndicator(0, ref South);
        ResetAzimuthIndicator(0, ref West);
    }

    public void ResetAzimuthIndicator(int sign, ref GameObject AzimuthIndicator) {
        Vector3 resetLocation = new Vector3(0, -60, 0);
        AzimuthIndicator.gameObject.transform.localPosition = resetLocation;
    }
    	
	void Update () {
        float userGazeVal = userGaze.transform.eulerAngles.y;

        // Checking to see if left or right sign shows.
        bool showLeft = (userGazeVal % 90) <= 45;
        bool showRight = 90 - (userGazeVal % 90) <= 45;

        // Determine which sign, -1 means not showing.
        int left_sign = -1;
        int right_sign = -1;
        int leftPixel = -1;
        int rightPixel = -1;

        if (showLeft) {
            left_sign = LeftCompassSign(userGazeVal);
            // Determine pixel location to draw.
            leftPixel = LeftPixelLocation(userGazeVal, left_sign);
            RenderLeftAzimuth(left_sign, leftPixel);    // Object knows location
            TurnOffAzimuthIndicators(left_sign);
        }
        if (showRight) {    // Calculate what to show for right
            right_sign = RightCompassSign(userGazeVal);
            // Determine pixel location to draw.
            rightPixel = RightPixelLocation(userGazeVal, right_sign);
            RenderRightAzimuth(right_sign, rightPixel);
            TurnOffAzimuthIndicators(right_sign);
        }
    }

    public void TurnOnLeftAzimuthIndicator(int sign) {
        switch (sign) {
            case 0:
                ResetAzimuthIndicator(0, ref North);
                azimuth.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 90:
                ResetAzimuthIndicator(0, ref East);
                azimuth.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 180:
                ResetAzimuthIndicator(0, ref South);
                azimuth.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 270:
                ResetAzimuthIndicator(0, ref West);
                azimuth.transform.GetChild(3).gameObject.SetActive(true);
                break;
        } 
    }

    public void TurnOnRightAzimuthIndicator(int sign) {
        switch (sign) {
            case 0:
                azimuth.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 90:
                azimuth.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 180:
                azimuth.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 270:
                azimuth.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    public void TurnOffAzimuthIndicators(int sign) {
        switch (sign) {
            case 0:
                azimuth.transform.GetChild(1).gameObject.SetActive(false);
                azimuth.transform.GetChild(2).gameObject.SetActive(false);
                azimuth.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 90:
                azimuth.transform.GetChild(0).gameObject.SetActive(false);
                azimuth.transform.GetChild(2).gameObject.SetActive(false);
                azimuth.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 180:
                azimuth.transform.GetChild(0).gameObject.SetActive(false);
                azimuth.transform.GetChild(1).gameObject.SetActive(false);
                azimuth.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 270:
                azimuth.transform.GetChild(0).gameObject.SetActive(false);
                azimuth.transform.GetChild(1).gameObject.SetActive(false);
                azimuth.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    public void RenderLeftAzimuth(int left_sign, int leftPixel) {
        if (left_sign == 0) {
            Vector3 temp = North.gameObject.transform.localPosition;
            temp.x = leftPixel;
            North.gameObject.transform.localPosition = temp;
        } else if (left_sign == 90) {
            Vector3 temp = East.gameObject.transform.localPosition;
            temp.x = leftPixel;
            East.gameObject.transform.localPosition = temp;
        } else if (left_sign == 180) {
            Vector3 temp = South.gameObject.transform.localPosition;
            temp.x = leftPixel;
            South.gameObject.transform.localPosition = temp;
        } else if (left_sign == 270) {
            Vector3 temp = West.gameObject.transform.localPosition;
            temp.x = leftPixel;
            West.gameObject.transform.localPosition = temp;
        }
    }

    public void RenderRightAzimuth(int right_sign, int rightPixel) {
        if (right_sign == 0) {
            Vector3 temp = North.gameObject.transform.localPosition;
            temp.x = rightPixel;
            North.gameObject.transform.localPosition = temp;
        } else if (right_sign == 90) {
            Vector3 temp = East.gameObject.transform.localPosition;
            temp.x = rightPixel;
            East.gameObject.transform.localPosition = temp;
        } else if (right_sign == 180) {
            Vector3 temp = South.gameObject.transform.localPosition;
            temp.x = rightPixel;
            South.gameObject.transform.localPosition = temp;
        } else if (right_sign == 270) {
            Vector3 temp = West.gameObject.transform.localPosition;
            temp.x = rightPixel;
            West.gameObject.transform.localPosition = temp;
        }
    }

    /*
     * N = 0, E=90, S=180, W=270;
     */
    public int LeftPixelLocation(float userGaze, int left_sign) {
        TurnOnLeftAzimuthIndicator(left_sign);
        int angleDistance = (int)userGaze - left_sign;
        int pixelDistance = angleDistance * -5;
        return pixelDistance;
    }

    /*
     * N = 0, E=90, S=180, W=270;
     */
    public int RightPixelLocation(float userGaze, int right_sign) {
        TurnOnRightAzimuthIndicator(right_sign);
        if (right_sign == 0) {
            right_sign = 360;
        }
        int angleDistance = right_sign - (int)userGaze;
        int pixelDistance = angleDistance * 5;
        return pixelDistance;
    }

    /// <summary>
    /// Returns degrees from compass direction
    /// </summary>
    /// <param name="userGaze">Int denotes compass direction, 1, 2, 3, 4 => N E S W</param>
    /// <returns>Degree given direction</returns>
    public int LeftCompassSign(float userGaze) {
        if (userGaze <= 45) {
            return 0;   // north
        } else if (Mathf.Abs(90-userGaze) <= 45) {
            return 90;
        } else if (Mathf.Abs(180-userGaze) <= 45) {
            return 180;
        } else {
            return 270;
        }
    }

    /// <summary>
    /// Returns degrees from compass direction
    /// </summary>
    /// <param name="userGaze">Int denotes compass direction, 1, 2, 3, 4 => N E S W</param>
    /// <returns>Degree given direction</returns>
    public int RightCompassSign(float userGaze) {
        if (Mathf.Abs(userGaze-360) <= 45) { // last 45 degrees of circle
            return 0;   // north
        } else if (Mathf.Abs(userGaze-90) <= 45) {
            return 90;   // East
        } else if (Mathf.Abs(userGaze-180) <= 45) {
            return 180;   // South
        } else {
            return 270;
        }
    }
}
