using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the end quartz in the MiniGame
/// </summary>
public class MMQuartz : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        MiniGame.instance.EndMiniGame();
    }
}
