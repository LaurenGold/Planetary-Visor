using System.Collections.Generic;
using UnityEngine;

public class PC_ScalePlayer : MonoBehaviour
{
    public float scaleFactor;
    private Vector3 originalScale;

    public void ScalePlayerUp()
    {
        originalScale = transform.localScale;

        float scaleX = originalScale.x * scaleFactor;
        float scaleY = originalScale.y * scaleFactor;
        float scaleZ = originalScale.z * scaleFactor;

        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        //Max / Min scale if too big or too small
        scaleX = Mathf.Clamp(scaleX, 1, 250);
        scaleY = Mathf.Clamp(scaleY, 1, 250);
        scaleZ = Mathf.Clamp(scaleZ, 1, 250);
        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        Debug.Log("BIGGER: new scale" + originalScale);
        transform.localScale = originalScale;
    }

    public void ScalePlayerDown()
    {
        originalScale = transform.localScale;

        float scaleX = originalScale.x / scaleFactor;
        float scaleY = originalScale.y / scaleFactor;
        float scaleZ = originalScale.z / scaleFactor;

        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        //Max / Min scale if too big or too small
        scaleX = Mathf.Clamp(scaleX, 1, 250);
        scaleY = Mathf.Clamp(scaleY, 1, 250);
        scaleZ = Mathf.Clamp(scaleZ, 1, 250);
        originalScale = new Vector3(scaleX, scaleY, scaleZ);

        Debug.Log("SMALLER: new scale" + originalScale);
        transform.localScale = originalScale;
    }

}
