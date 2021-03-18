using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public GameObject helpPanel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }
    }
}
