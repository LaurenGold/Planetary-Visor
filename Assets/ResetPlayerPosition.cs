using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reset the player's position to the specified location
/// </summary>
public class ResetPlayerPosition : MonoBehaviour {
    public Vector3 Position;
    
    /// <summary>
    /// Reset the player's position to the specified location
    /// </summary>
    public void ResetPosition() {
        transform.position = new Vector3(Position.x, Position.y, Position.z);
    }
}
