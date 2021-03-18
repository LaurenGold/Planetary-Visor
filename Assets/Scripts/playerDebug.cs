using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDebug : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= 1) {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }
    }
}
