using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the bell in the BC MiniGame
/// </summary>
public class BCBell : MonoBehaviour
{
    void OnMouseDown()
    {
        BCMiniGame miniGame = (BCMiniGame)MiniGame.instance;

        if (miniGame.canInterract)
        {
            miniGame.SendPizza();
        }
    }
}
