using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the "Horse Race" Mini game
/// </summary>
public class HRMiniGame : MiniGame
{
    [Header("Horse Race")]
    [SerializeField] private HRPlayer player;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
    }

}
