using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the "Mines Maze" Minigame
/// </summary>
public class MMMiniGame : MiniGame
{
    [Header("MinesMaze")]
    [SerializeField] private MMCell3D cellPrefab;
    [SerializeField] private int mazeSize;
    [SerializeField] private Rigidbody player;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        Generate3DMaze(GenerateMaze());

    }

    protected override void MiniGameUpdate()
    {
        player.velocity = player.transform.forward * speed * Input.GetAxis("Vertical");
        player.angularVelocity = new Vector3(0, 1, 0) * turnSpeed * Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Generates an empty maze
    /// </summary>
    /// <returns>An empty maze</returns>
    MMCell[][] GenerateEmptyMaze()
    {
        MMCell[][] maze = new MMCell[mazeSize][];
        for (int y = 0; y < mazeSize; y++)
        {
            maze[y] = new MMCell[mazeSize];
            for (int x = 0; x < mazeSize; x++)
            {
                maze[y][x] = new MMCell();
            }
        }
        return maze;
    }

    /// <summary>
    /// Generates the maze's data
    /// </summary>
    /// <returns>The maze's data</returns>
    MMCell[][] GenerateMaze()
    {
        MMCell[][] maze = GenerateEmptyMaze();

        List<int[]> ways;
        int maxTunnel = 5 * mazeSize;
        int maxSizeHallway = 5;
        int randomSizeHallway = Random.Range(1, maxSizeHallway + 1);
        int currentSizeHallway = 0;
        int x = 0;
        int y = 0;

        int[] down = new int[] { 0, 1 };
        int[] up = new int[] { 0, -1 };
        int[] left = new int[] { -1, 0 };
        int[] right = new int[] { 1, 0 };

        while (maxTunnel > 0)
        {
            ways = new List<int[]>
            {
                up,
                left,
                down,
                right
            };
            if (x <= 0)
            {
                ways.Remove(left);
            }
            if (x >= mazeSize - 1)
            {
                ways.Remove(right);
            }
            if (y <= 0)
            {
                ways.Remove(up);
            }
            if (y >= mazeSize - 1)
            {
                ways.Remove(down);
            }

            int[] move = ways[Random.Range(0, ways.Count)];
            while (currentSizeHallway < randomSizeHallway)
            {
                if (x + move[0] > mazeSize - 1 || y + move[1] > mazeSize - 1 || x + move[0] < 0 || y + move[1] < 0)
                {
                    break;
                }

                maze[y][x].OpenWall(move, false);
                x += move[0];
                y += move[1];
                maze[y][x].OpenWall(move, true);
                currentSizeHallway++;
            }

            currentSizeHallway = 0;
            randomSizeHallway = Random.Range(1, maxSizeHallway + 1);
            maxTunnel--;

        }

        maze[y][x].isExit = true;

        return maze;
    }


    /// <summary>
    /// Generates the 3D maze
    /// </summary>
    /// <param name="maze">The maze's data</param>
    void Generate3DMaze(MMCell[][] maze)
    {
        for (int y = 0; y < mazeSize; y++)
        {
            for (int x = 0; x < mazeSize; x++)
            {
                if (maze[y][x].IsEmpty()) continue;
                Instantiate(cellPrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity, transform).Initialize(maze[y][x]);
            }
        }
    }

}
