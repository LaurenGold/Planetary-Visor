using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace Buttons.VR {
    public class ButtonController : MonoBehaviour {
        private static bool started = false;

        // TODO: Auto-populate list of button actions rather manual assign (grip, trigger, etc).
        static readonly Dictionary<string, InputFeatureUsage<bool>> availableButtons = new Dictionary<string, InputFeatureUsage<bool>>
        {
            {"triggerButton", CommonUsages.triggerButton },
            {"thumbrest", CommonUsages.thumbrest },
            {"primary2DAxisClick", CommonUsages.primary2DAxisClick },
            {"primary2DAxisTouch", CommonUsages.primary2DAxisTouch },
            {"menuButton", CommonUsages.menuButton },
            {"gripButton", CommonUsages.gripButton },
            {"secondaryButton", CommonUsages.secondaryButton },
            {"secondaryTouch", CommonUsages.secondaryTouch },
            {"primaryButton", CommonUsages.primaryButton },
            {"primaryTouch", CommonUsages.primaryTouch },

        };

        static readonly Dictionary<string, InputFeatureUsage<Vector2>> availableVector2s = new Dictionary<string, InputFeatureUsage<Vector2>>
        {
            {"dPad", CommonUsages.dPad },
            {"primmary2DAxis", CommonUsages.primary2DAxis },
            {"secondary2DAxis", CommonUsages.secondary2DAxis },

        };


        public enum ButtonOption {
            triggerButton,
            thumbrest,
            primary2DAxisClick,
            primary2DAxisTouch,
            menuButton,
            gripButton,
            secondaryButton,
            secondaryTouch,
            primaryButton,
            primaryTouch,
            dPad
        };

        [Tooltip("Input device role (left or right controller)")]
        public InputDeviceRole deviceRole;

        [Tooltip("Select the button")]
        public ButtonOption button;

        [Tooltip("Event when the button starts being pressed")]
        public UnityEvent OnPress;

        [Tooltip("Event when the button starts being pressed")]
        public UnityEvent OnHold;

        [Tooltip("Event when the button is released")]
        public UnityEvent OnRelease;

        // to check whether pressed
        public bool IsPressed { get; private set; }

        // to obtain input devices
        List<InputDevice> inputDevices;
        bool inputValue;

        InputFeatureUsage<bool> m_inputFeature;

        void Awake() {
            // Get label selected by user from list of enumerates.
            string featureLabel = Enum.GetName(typeof(ButtonOption), button);
            // find dictionary entry assign it to object reference.
            availableButtons.TryGetValue(featureLabel, out m_inputFeature);
            // init list
            inputDevices = new List<InputDevice>();
        }

        /// <summary>
        /// If OnPress fired, user just pressed button. If IsPressed is true, user is holding, 
        /// if OnRelease fired, user just released button.
        /// Every frame check if user just pressed, is holding, or just released the specified button
        /// on the specified input device (controller, tracker, etc).        
        /// </summary>
        void Update() {
            // Gets list of active input devices available that match specified role.
            InputDevices.GetDevicesWithRole(deviceRole, inputDevices);            
            for (int i = 0; i < inputDevices.Count; i++) { 
                // if we haven't pressed the start button in the UI yet, don't process any button events
                if (!started) {
                    continue;
                }
                
                if (inputDevices[i].TryGetFeatureValue(m_inputFeature,
                    out inputValue) && inputValue) {    // Access current feature state, write to inputValue.
                    if (!IsPressed) {               // if start pressing, trigger event
                        IsPressed = true;
                        OnPress.Invoke();           // fire callback for all objects listening to this input config.
                    } else if (IsPressed)
                    {
                        OnHold.Invoke();
                    }
                } else if (IsPressed) {             // check for button release
                    IsPressed = false;
                    OnRelease.Invoke();             // fire callback for all objects listening to release event.
                }
            }
        }

        /// <summary>
        /// enable buttons that haven't been activated yet after pressing start on the start menu
        /// </summary>
        public void EnableAllButtons() {
            ButtonController.started = true;
        }
    }
}