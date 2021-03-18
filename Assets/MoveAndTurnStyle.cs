using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Provider;
using UnityEngine.XR.Management;
public class MoveAndTurnStyle : MonoBehaviour
{
    DeviceBasedContinuousTurnProvider SmoothTurn;
    DeviceBasedSnapTurnProvider SnapTurn;
    DeviceBasedContinuousMoveProvider continuousMove;
    TeleportationProvider teleport;

    public GameObject leftTeleportController;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        SmoothTurn = player.GetComponent<DeviceBasedContinuousTurnProvider>();
        SnapTurn = player.GetComponent<DeviceBasedSnapTurnProvider>();
        continuousMove = player.GetComponent<DeviceBasedContinuousMoveProvider>();
        teleport = player.GetComponent<TeleportationProvider>();
    }

    public void ActivateSmoothTurn()
    {
        SnapTurn.enabled = false;
        SmoothTurn.enabled = true;
    }

    public void ActivateSnapTurn()
    {
        SmoothTurn.enabled = false;
        SnapTurn.enabled = true;
    }

    public void ActivateTeleport()
    {
        continuousMove.enabled = false;
        leftTeleportController.SetActive(true);
        teleport.enabled = true;
    }

    public void ActivateContinuous()
    {
        leftTeleportController.SetActive(false);
        teleport.enabled = false;
        continuousMove.enabled = true;
    }


}
