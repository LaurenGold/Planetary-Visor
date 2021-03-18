using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Script for changing the color of the pause menu settings button on hover.
/// </summary>
public class SettingsButtonHover : MonoBehaviour
{
    /// <summary>
    /// called when the button is being hovered over by the cursor
    /// </summary>
    public void OnHover() {
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        textmeshPro.color = new Color32(236, 146, 80, 255);
    }

    /// <summary>
    /// called when the button is no longer being hovered over by the cursor
    /// </summary>
    public void OnNormal() {
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        textmeshPro.color = new Color32(255, 255, 255, 255);
    }

    void OnDisable() {
        OnNormal();
    }
}
