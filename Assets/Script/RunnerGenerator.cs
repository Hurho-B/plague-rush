using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[5];
        public int[] direction = { 0, 0 };
        public int[] grid = { 0, 0 };
    }

    [Header("Debug")]
    [Tooltip("The number of segments being generated per area.")]
    public int numOfSegments;
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
    bool lastTurnRight = true;
    int numOfSameTurns = 1;
    int[] grid = { 0, 0 };
    int pathLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PathGenerator();
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

            // Update grid for future cell placements
            gridSpace[0] += currentCell.direction[0];
            gridSpace[1] += currentCell.direction[1];
        }
    }

    void PathGenerator()
    {
        // Generating the workable area, check against the board
        // before adding a Cell to the path.
        pathLength = numOfSegments * segmentLength;
        board = new List<Cell>();
        for (int i = 0; i < pathLength; i++)
        { board.Add(new Cell()); }
        board[0].grid = new int[] { 0, 0 };
        board[0].direction = new int[] { 0, 1 };
        board[0].status[0] = true;

        for (int i = 1; i < pathLength; i++)
        {
            var pCell = board[i - 1];
            int[] newDir;

            if (i % segmentLength == 0 && i != segmentLength)
            { newDir = MakeTurnCheck(i); }
            else
            { newDir = GrabDir(i, -1); }

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
        int[] pCell = board[cell - 1].direction;
        int doTurn = Random.Range(0, 11);
        Debug.Log("Cell " + cell + " | " +numOfSameTurns);


        // This if/else block acts as a fail-safe, checking for straight
        // paths and if a turn needs to happen in the other direction.
        if (doTurn < turnWeight)
        {
            if (pCell[0] == 0)
            {
                board[cell].status[0] = true;
                board[cell].status[2] = true;
            }
            else if (pCell[1] == 0)
            {
                board[cell].status[1] = true;
                board[cell].status[3] = true;
            }
            return pCell;
        }
        else if (lastTurnRight && numOfSameTurns >= 2)
        {
            lastTurnRight = false;
            numOfSameTurns = 1;
            return GrabDir(cell, 0);
        }
        else if (!lastTurnRight && numOfSameTurns >= 2)
        {
            lastTurnRight = true;
            numOfSameTurns = 1;
            return GrabDir(cell, 1);
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
            return GrabDir(cell, 0);
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
            return GrabDir(cell, 1);
        }

        return direction;
    }
    
    int[] GrabDir(int cell, int turnDir)
    {
        int[] newDir = { 0, 0 };
        int[] pDir = board[cell - 1].direction;

        // First check if the cell is going along the North/South
        // axis, then check again to see if it's going North or South.
        // Repeat again along the East/West axis.
        //
        // Refer to RoomBehaviour.cs for cardinal key. These must be numerical
        // to modify the grid later.
        // -1 = Same Direction
        //  0 = Turn Left
        //  1 = Turn Right
        if (turnDir == -1)
        {
            if (pDir[0] == 0)
            {
                newDir = pDir;
                board[cell].status[0] = true;
                board[cell].status[2] = true;
            }
            else if (pDir[1] == 0)
            {
                newDir = pDir;
                board[cell].status[1] = true;
                board[cell].status[3] = true;
            }
        }
        else if (pDir[0] == 0)
        {
            board[cell].status[4] = true;
            if (pDir[1] == 1)
            {
                if (turnDir == 0)
                {
                    newDir = new int[] { -1, 0 };
                    board[cell].status[2] = true;
                    board[cell].status[3] = true;
                }
                else if (turnDir == 1)
                {
                    newDir = new int[] { 1, 0 };
                    board[cell].status[2] = true;
                    board[cell].status[1] = true;
                }
            }
            else if (pDir[1] == -1)
            {
                if (turnDir == 0)
                {
                    newDir = new int[] { 1, 0 };
                    board[cell].status[0] = true;
                    board[cell].status[1] = true;
                }
                else if (turnDir == 1)
                {
                    newDir = new int[] { -1, 0 };
                    board[cell].status[0] = true;
                    board[cell].status[3] = true;
                }
            }
        }
        else if (pDir[1] == 0)
        {
            board[cell].status[4] = true;
            if (pDir[0] == 1)
            {
                if (turnDir == 0)
                {
                    newDir = new int[] { 0, 1 };
                    board[cell].status[3] = true;
                    board[cell].status[0] = true;
                }
                else if (turnDir == 1)
                {
                    newDir = new int[] { 0, -1 };
                    board[cell].status[3] = true;
                    board[cell].status[2] = true;
                }
            }
            else if (pDir[0] == -1)
            {
                if (turnDir == 0)
                {
                    newDir = new int[] { 0, -1 };
                    board[cell].status[1] = true;
                    board[cell].status[2] = true;
                }
                else if (turnDir == 1)
                {
                    newDir = new int[] { 0, 1 };
                    board[cell].status[1] = true;
                    board[cell].status[0] = true;
                }
            }
        }
        return newDir;
    }
}

