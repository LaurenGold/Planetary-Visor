using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Component that can be attached to a Canvas UI that adds haptic/sound feedback to all buttons, sliders
/// toggles, and dropdowns in the Canvas UI.
/// </summary>
public class UIHapticsMain : MonoBehaviour {
    [Tooltip("Right VR controller")]
    public GameObject RightController;
    [Tooltip("Left VR controller")]
    public GameObject LeftController;
    [Tooltip("Sound on hover")]
    public AudioClip HoverAudio;
    [Tooltip("Sound on click")]
    public AudioClip PressAudio;

    private AudioSource hoverAudioSource;
    private AudioSource pressAudioSource;
    private UIHaptics lastHoveredHaptics;
    private float lastHoverPlayTime;
    
    // Start is called before the first frame update
    void Start() {
        // look for interactable UI elements
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Button>(true).Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Slider>(true).Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Dropdown>(true).Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Toggle>(true).Select(x => x.gameObject));

        // create the hover audio source from the audio clip
        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.clip = HoverAudio;
        hoverAudioSource.playOnAwake = false;
        
        // create the press audio source from the audio clip
        pressAudioSource = gameObject.AddComponent<AudioSource>();
        pressAudioSource.clip = PressAudio;
        pressAudioSource.playOnAwake = false;

        // add the UIHaptics component to all buttons
        foreach (GameObject item in gameObjects) {
            UIHaptics haptics = item.AddComponent<UIHaptics>() as UIHaptics;
            haptics.Parent = this;
            haptics.RightController = RightController;
            haptics.LeftController = LeftController;
        }
    }

    /// <summary>
    /// play the hover sound with a quota limit so we don't spam hover sounds on the same button
    /// </summary>
    public void HoverSound(UIHaptics haptics) {
        // prevent rapid sound playing
        if (
            Time.time - lastHoverPlayTime > 0.3f
            || lastHoveredHaptics != haptics
        ) {
            hoverAudioSource.Play();
            lastHoverPlayTime = Time.time;
            lastHoveredHaptics = haptics;
        }
    }

    /// <summary>
    /// play the press sound when we click
    /// </summary>
    public void PressSound(UIHaptics haptics) {
        pressAudioSource.Play();
    }
}
