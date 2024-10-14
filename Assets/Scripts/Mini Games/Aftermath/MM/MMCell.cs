using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a cell
/// </summary>
public class MMCell
{
    public bool leftWall;
    public bool rightWall;
    public bool upWall;
    public bool downWall;
    public bool isExit;


    public MMCell()
    {
        leftWall = true;
        rightWall = true;
        upWall = true;
        downWall = true;
        isExit = false;
    }

    /// <summary>
    /// Is the cell empty
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        if (!leftWall) return false;
        if (!rightWall) return false;
        if (!upWall) return false;
        if (!downWall) return false;
        return true;
    }

    /// <summary>
    /// Opens a wall in the cell
    /// </summary>
    /// <param name="move">The move</param>
    /// <param name="opposite">Should the move be reversed ?</param>
    public void OpenWall(int[] move, bool opposite)
    {
        if (opposite)
        {
            move = new int[] { -move[0], -move[1] };
        }
        if (move[0] == -1)
        {
            leftWall = false;
        }
        else if (move[0] == 1)
        {
            rightWall = false;
        }
        else if (move[1] == 1)
        {
            downWall = false;
        }
        else if (move[1] == -1)
        {
            upWall = false;
        }
    }
}
