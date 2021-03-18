using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuClick : MonoBehaviour
{
    public GameObject helpPanel;
    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray();
        RaycastHit hitInfo = new RaycastHit();
        
    }

    // Update is called once per frame
    void Update()
    {
        //mouse Input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	// mouse is pointing at the screen, get the mouse input
        RaycastHit hitInfo;

        //get icon by layer name
        int layerMask = LayerMask.GetMask("help");
	    // look up code to create dropdown

        //check if mouse hit icon
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)) {
            Debug.Log("hit");
            //toggle on help menu
            helpPanel.SetActive(true);
        }
    } 
}
