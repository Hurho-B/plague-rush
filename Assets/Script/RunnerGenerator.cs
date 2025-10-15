using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        public int[] direction = { 0, 0 };
        public int[] grid = { 0, 0 };
    }

    [Header("Debug")]
    [Tooltip("The length of the path.")]
    public int pathLength;
    [Tooltip("The length of a segment.")]
    public int segmentLength;
    [Tooltip("The # of units each room's center is seperated by.")]
    public int offset;
    [Tooltip("The chance a turn should happen when legal.")]
    public int turnWeight;
    [Header("Prefabs")]
    [Tooltip("Used for standard path segments.")]
    public GameObject room;
    [Tooltip("Used for puzzle rooms at the end of each path.")]
    public GameObject endPiece;

    List<Cell> board;
    int iteration = 0;
    bool lastTurnRight = true;
    int numOfSameTurns = 0;
    int[] grid = { 0, 0 };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PathGenerator();
        if (iteration > 0)
        {
            // Later on, use this to transfer grid and
            // direction information into the future
            // generator iterations.
            // 
            // Is this neccessary?
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GeneratePath()
    {
        int[] gridSpace = grid;
        for (int i = 0; i < pathLength; i++)
        {
            // Build room segments
            Cell currentCell = board[i];
            RoomBehaviour newRoom = Instantiate(room, new Vector3(gridSpace[0] * offset, 0, gridSpace[1] * offset), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
            newRoom.UpdateRoom(currentCell.status);
            newRoom.name += " " + i;

            // Update grid for GameManager script
            gridSpace[0] += currentCell.direction[0];
            gridSpace[1] += currentCell.direction[1];
        }
    }

    void PathGenerator()
    {
        // Generating the workable area, check against the board
        // before adding a Cell to the path.
        board = new List<Cell>();
        for (int i = 0; i < pathLength; i++)
        { board.Add(new Cell()); }
        board[0].grid = new int[] { 0, 0 };
        board[0].direction = new int[] { 0, 1 };

        for (int i = 1; i < pathLength; i++)
        {
            var pCell = board[i - 1];
            int[] newDir;

            if (i % pathLength == 0)
            { newDir = MakeTurnCheck(i); }
            else
            { newDir = pCell.direction; }

            board[i].visited = false;
            board[i].direction = newDir;
            board[i].grid = new int[] { pCell.grid[0] + pCell.direction[0],
                pCell.grid[1] + pCell.direction[1] };
            // board[i].status;
        }
        GeneratePath();
    }

    int[] MakeTurnCheck(int cell)
    {
        int[] direction = { 0, 0 };
        int doTurn = Random.Range(0, 11);

        // This if/else block acts as a fail-safe, checking for straight
        // paths and if a turn needs to happen in the other direction.
        if (doTurn > turnWeight)
        {
            return board[cell - 1].direction;
        }
        else if (lastTurnRight && numOfSameTurns >= 2)
        {
            lastTurnRight = false;
            numOfSameTurns = 1;
            return GrabDir(cell);
        }
        else if (!lastTurnRight && numOfSameTurns >= 2)
        {
            lastTurnRight = true;
            numOfSameTurns = 1;
            return GrabDir(cell);
        }

        // This block is now doing coin flips to see which turn to make
        // and modify the appropriate variables for tracking.
        // 0 = Turn Left
        // 1 = Turn Right
        doTurn = Random.Range(0, 2);
        if (doTurn == 0)
        {
            if (lastTurnRight == false)
            {
                numOfSameTurns += 1;
            }
            else
            {
                lastTurnRight = false;
                numOfSameTurns = 1;
            }
            return GrabDir(cell);
        }
        else if (doTurn == 1)
        {
            if (lastTurnRight == true)
            {
                numOfSameTurns += 1;
            }
            else
            {
                lastTurnRight = true;
                numOfSameTurns = 1;
            }
            return GrabDir(cell);
        }

        return direction;
    }
    
    int[] GrabDir(int cell)
    {
        int[] direction = { 0, 0 };

        return direction;
    }
}

