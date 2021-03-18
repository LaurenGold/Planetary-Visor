using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, 0f);
    }
}
