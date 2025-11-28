using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool inTurnRange = false;
        public bool[] status = new bool[6];
        public int[] direction = { 0, 0 };
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
    [Tooltip("The number of rooms that will be made empty from a turn.")]
    public int turnBuffer;
    [Header("Prefabs")]
    [Tooltip("Used for standard path segments.")]
    public GameObject room;
    [Tooltip("Used for puzzle rooms at the end of each path.")]
    public GameObject endPiece;

    private List<Cell> board = new List<Cell>();
    private bool lastTurnRight = true;
    private int numOfSameTurns = 1;
    private int[] grid = { 0, 0 };
    private int pathLength;

    private int areaStartPoint;
    private int areaEndPoint;

    // Initializing variables
    private void Awake()
    {
        pathLength = numOfSegments * segmentLength;
    }
    
    // Generates a new work area on List<Cell> board
    public void Start()
    {
        areaStartPoint = board.Count;
        areaEndPoint = board.Count + pathLength;
        for (int i = areaStartPoint; i < areaEndPoint; i++) { board.Add(new Cell()); }
        PathGenerator();
    }
    
    private void GeneratePath() {
        for (int i = areaStartPoint; i < areaEndPoint; i++) {
            // Build room segments
            Cell currentCell = board[i];
            RoomBehaviour newRoom = Instantiate(room, new Vector3(grid[0] * offset, 0, grid[1] * offset), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
            newRoom.UpdateRoom(currentCell.status);
            newRoom.name += " " + i;

            // Update grid for future cell placements
            grid[0] += currentCell.direction[0];
            grid[1] += currentCell.direction[1];
        }
    }

    private void PathGenerator() {
        for (int i = areaStartPoint; i < areaEndPoint; i++) {
            if (i == 0)
            {
                board[i].visited = false;
                board[i].direction = new int[] { 0, 1 };
                board[i].status[0] = true;
                continue;
            }
            var pCell = board[i - 1];
            board[i].visited = false;
            if (i % segmentLength == 0)
            {
                board[i].direction = MakeTurnCheck(i);
            }
            else
            {
                board[i].direction = GrabDir(i, -1);
            }
        }
		board[board.Count - 1].status[5] = true;
        GeneratePath();
    }

    private int[] MakeTurnCheck(int cell) {
        int[] direction = { 0, 0 };
        int[] pCell = board[cell - 1].direction;
        int doTurn = Random.Range(0, 11);
        Debug.Log("Cell " + cell + " | " +numOfSameTurns);

        // This if/else block acts as a fail-safe, checking for straight
        // paths and if a turn needs to happen in the other direction.
        if (doTurn < turnWeight) {
            if (pCell[0] == 0) {
                board[cell].status[0] = true;
                board[cell].status[2] = true;
            }
            else if (pCell[1] == 0) {
                board[cell].status[1] = true;
                board[cell].status[3] = true;
            }
            return pCell;
        }
        board[cell].inTurnRange = true;
        if (lastTurnRight && numOfSameTurns >= 2) {
            lastTurnRight = false;
            numOfSameTurns = 1;
            return GrabDir(cell, 0);
        }
        else if (!lastTurnRight && numOfSameTurns >= 2) {
            lastTurnRight = true;
            numOfSameTurns = 1;
            return GrabDir(cell, 1);
        }

        // 0 = Turn Left | 1 = Turn Right
        doTurn = Random.Range(0, 2);
        if (doTurn == 0) {
            if (lastTurnRight == false) {
                numOfSameTurns += 1;
            }
            else {
                lastTurnRight = false;
                numOfSameTurns = 1;
            }
            return GrabDir(cell, 0);
        }
        else if (doTurn == 1) {
            if (lastTurnRight == true) {
                numOfSameTurns += 1;
            }
            else {
                lastTurnRight = true;
                numOfSameTurns = 1;
            }
            return GrabDir(cell, 1);
        }

        return direction;
    }
    
    private int[] GrabDir(int cell, int turnDir)
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

    private void DeclareObiStatus()
    {
        for (int i = 0; i < board.Count; i++) {
            if (board[i].inTurnRange == true) {
                continue;
            }
            
        }
        // Check local cells relative to turnBuffer to determine if an
        // obstacle pack is allowed to spawn in that room.
    }
}

