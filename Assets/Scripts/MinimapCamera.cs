using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        //camera rotates with player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
