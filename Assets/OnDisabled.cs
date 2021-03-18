using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Settable UnityEvent callback for when the game object/component is disabled.
/// </summary>
public class OnDisabled : MonoBehaviour {
    [Tooltip("Called when we're disabled")]
    public UnityEvent WhenDisabled;
    
    void OnDisable() {
        WhenDisabled.Invoke();
    }
}
