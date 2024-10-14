using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the 3D representation of a cell
/// </summary>
public class MMCell3D : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject upWall;
    [SerializeField] private GameObject downWall;
    [SerializeField] private GameObject exit;

    /// <summary>
    /// Initialize the cell
    /// </summary>
    /// <param name="room">The linked data</param>
    public void Initialize(MMCell room)
    {
        leftWall.SetActive(room.leftWall);
        rightWall.SetActive(room.rightWall);
        upWall.SetActive(room.upWall);
        downWall.SetActive(room.downWall);

        if (!room.isExit) Destroy(exit);
    }
}
