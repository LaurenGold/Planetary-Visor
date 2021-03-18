using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    private UserStudyController userStudy;
    // Start is called before the first frame update
    void Start()
    {
        userStudy = GameObject.FindGameObjectWithTag("Player").GetComponent<UserStudyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
