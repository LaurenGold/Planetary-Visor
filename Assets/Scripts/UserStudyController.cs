using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserStudyController : MonoBehaviour
{
    public int phase;

    public RawImage textHolder;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI phaseDescription;

    public bool canAdvance = false;
    public bool initialized = false; // initialize each phase once
    private bool scaleReached = false; // for task 5, if height of scale reached
    private bool inWaypoint = false;
    private float timer;
    bool avgPixelsPlaced = true;

    public GameObject waypoint;
    public GameObject distanceSphere;

    public PixelBlockManager pixelBlockManager;
    //public CameraSnap cameraSnap;
    public GameObject player;

    void Start()
    {
        phase = 1;
    }

    void Update()
    {
        if (!initialized)
        {
            StateInitialize();
        }
        if (StateChangeReady())
        {
            textHolder.color = new Color(0f, 255f, 0f, 110f);
            canAdvance = true;
        }
        else
        {
            canAdvance = false;
        }
    }
    // initialize anything needed for task
    public void StateInitialize()
    {
        textHolder.color = new Color(255f, 255f, 255f, 110f);

        pixelBlockManager.resetPixelBlocks();

        switch (phase)
        {
            case 1:
                Instantiate(waypoint, new Vector3(0.12f, 0f, 2.6f), Quaternion.identity);
                phaseText.text = "1:";
                phaseDescription.text = "Push L joystick forward to move towards the yellow waypoint.";
                break;
            case 2:
                phaseText.text = "2:";
                phaseDescription.text = "Pull R trigger to drag CRISM pixel around. Press top button on R hand to switch colors.";
                break;
            case 3:
                avgPixelsPlaced = false;
                phaseText.text = "3:";
                phaseDescription.text = "Change to white color pixel and place multiple to get the average spectral reading. Clear the pixels by pressing the top button on L hand.";
                break;
            case 4:
                phaseText.text = "4:";
                phaseDescription.text = "Hold both grip buttons down and move controllers inwards to scale up. Once at the top, move controllers outwards to scale back down. You may release the grip buttons to stop scaling at any time.";
                break;
            case 5:
                timer = 15f;
                Instantiate(waypoint, new Vector3(14f, 3.97f, 17.4f), Quaternion.identity);
                Instantiate(distanceSphere, new Vector3(-23.2f, 5.36f, 7f), Quaternion.identity);
                phaseText.text = "5:";
                phaseDescription.text = "Navigate to yellow waypoint and look for floating yellow ball. Estimate distance in meters from you to the ball (Hint: Curiosity is about 3 meters long).";
                break;
            case 6:
                phaseText.text = "6:";
                phaseDescription.text = "The VR tasks are now complete. Feel free to roam around and play with the software.";
                break;
        }
        initialized = true;
    }

    /// <summary>
    /// Indiciate whether task has been completed or not
    /// </summary>
    /// <returns></returns>
    public bool StateChangeReady()
    {
        switch (phase)
        {
            case 1:
                if (inWaypoint) return true;
                break;
            case 2:
                int activeBlocks = 0;
                foreach (PixelBlock p in pixelBlockManager.pixelBlocks)
                {
                    if (p.gameObject.activeSelf) activeBlocks++;
                }
                if (activeBlocks > 1) return true;
                break;
            case 3:
                if(pixelBlockManager.averagePixelBlocks.Count > 2) {
                    avgPixelsPlaced = true;
                }
                if (avgPixelsPlaced) return true;
                break;
                break;
            case 4:
                if (player.transform.lossyScale.x > 200f) scaleReached = true;

                if (scaleReached)
                {
                    if (player.transform.lossyScale.x < 1.5f) return true;
                }
                break;
            case 5:
                timer -= Time.deltaTime;
                if (timer <= 0) return true;
                break;
        }
        return false;
    }

    /// <summary>
    /// Change state and conduct any cleanup that needs to be done
    /// </summary>
    public void StateChange()
    {
        if (canAdvance)
        {
            switch (phase)
            {
                case 1:
                    Destroy(GameObject.FindGameObjectWithTag("Waypoint"));
                    inWaypoint = false;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    Destroy(GameObject.FindGameObjectWithTag("Waypoint"));
                    Destroy(GameObject.Find("DistanceSphere(Clone)"));
                    break;
            }
            phase += 1;
            initialized = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint"))
        {
            if (phase == 1)
            {
                inWaypoint = true;
            }
        }
    }
}
