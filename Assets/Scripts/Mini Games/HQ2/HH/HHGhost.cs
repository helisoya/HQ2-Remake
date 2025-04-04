using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the ghost in the "Haunted House" Minigame
/// </summary>
public class HHGhost : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float appearanceTime = 5f;
    private MMCell[][] mazeData;
    private float lastTP;


    void Update()
    {
        if (!MiniGame.instance.started) return;
        if (Time.time - lastTP >= appearanceTime) FindNewSpot();


    }

    /// <summary>
    /// Setup the ghost
    /// </summary>
    private void FindNewSpot()
    {
        lastTP = Time.time;
        mazeData = ((MMMiniGame)MiniGame.instance).mazeData;

        int mazeSize = mazeData.Length;
        int x = 0, y = 0;
        while (mazeData[y][x].IsEmpty() || (x + y <= 2))
        {
            x = Random.Range(0, mazeSize);
            y = Random.Range(0, mazeSize);
        }

        transform.position = new Vector3((x * 10) + 5, 0, (y * 10) + 5);
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

    }
}
