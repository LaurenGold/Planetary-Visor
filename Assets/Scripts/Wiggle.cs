using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    private float wiggleDistance = 0.5f;

    private float wiggleSpeed = 1f;

    private float origY;

    // Start is called before the first frame update
    void Start()
    {
        origY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = origY + Mathf.Sin(Time.time * wiggleSpeed) * wiggleDistance;
        transform.position = pos;
    }
}
