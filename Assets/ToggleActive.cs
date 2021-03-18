using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggle the activeness of the game object
/// </summary>
public class ToggleActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// toggle the active state of the gameobject
    /// </summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.active);
    }
}
