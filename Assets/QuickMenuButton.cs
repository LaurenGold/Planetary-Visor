using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Handles image changes when a button is hovered and clicked.
/// </summary>
public class QuickMenuButton : MonoBehaviour {
    [Tooltip("Inactive button sprite")]
    public Sprite normalSprite;

    [Tooltip("Hovered button sprite")]
    public Sprite hoverSprite;

    [Tooltip("Active button sprite")]
    public Sprite activeSprite;

    [Tooltip("Select the button")]
    public Button button;

    bool isHovering = false;
    bool isPressed = false;

    /// <summary>
    /// called when the button is no longer being hovered over by cursor
    /// </summary>
    public void OnStopHover() {
        isHovering = false;
        if (isPressed == false) {
            button.image.sprite = normalSprite;
        }
    }

    /// <summary>
    /// called when the button is being hovered over by cursor
    /// </summary>
    public void OnStartHover() {
        isHovering = true;
        button.image.sprite = hoverSprite;
    }

    /// <summary>
    /// called when the button is no longer being pressed by the cursor
    /// </summary>
    public void OnStopPress() {
        isPressed = false;
        if (isHovering) {
            button.image.sprite = hoverSprite;
        }
        else {
            button.image.sprite = normalSprite;
        }
    }

    /// <summary>
    /// called when the button is being pressed by the cursor
    /// </summary>
    public void OnStartPress() {
        isPressed = true;
        button.image.sprite = activeSprite;
    }
    
    // Start is called before the first frame update
    void Start() {
        gameObject.active = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

    // called when the main UI is disabled
    void OnDisable() {
        OnStopPress();
        OnStopHover();
    }
}