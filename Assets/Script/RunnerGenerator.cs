using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public int[] direction = { 0, 0 };
        public int[] grid = { 0, 0 };
    }

    [Header("Debug")]
    [Tooltip("The size of the workable space.")]
    public Vector2 size; // Working on converting this into an int
    [Tooltip("The length of the path.")]
    public int pathLength;
    [Tooltip("The # of units each room's center is seperated by.")]
    public int offset;
    [Header("Prefabs")]
    [Tooltip("Used for standard path segments.")]
    public GameObject room;
    [Tooltip("Used for puzzle rooms at the end of each path.")]
    public GameObject endPiece;

    List<Cell> board;
    int iteration = 0;
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
            newRoom.UpdateRoom(currentCell.direction);
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
        board.Add(new Cell());
        board[0].direction = new int[] { 0, 1 };
        for (int i = 1; i < pathLength; i++)
        {
            board.Add(new Cell());
            board[i].direction = PickDirection(i);
        }
        GeneratePath();
    }

    Hashtable CheckNeighbors(int cell)
    {
        Hashtable neighbors = new Hashtable();
        neighbors.Add("North", new int[] { 0, 1 });
        neighbors.Add("East",  new int[] { 1, 0 });
        neighbors.Add("South", new int[] { 0,-1 });
        neighbors.Add("West",  new int[] {-1, 0 }); 
        // Checking all prior cells to see if any are adjacent
        for (int i = 1; i < cell; i++)
        {
            if ((board[i].grid[1] - 1) == board[cell].grid[1])
            { neighbors.Remove("North"); }
            if ((board[i].grid[0] - 1) == board[cell].grid[0])
            { neighbors.Remove("East"); }
            if ((board[i].grid[1] + 1) == board[cell].grid[1])
            { neighbors.Remove("South"); }
            if ((board[i].grid[0] + 1) == board[cell].grid[0])
            { neighbors.Remove("West"); }
        }

        // Returns a valid 
        return neighbors;
    }

    int[] PickDirection(int cell)
    {
        int[] direction = new int[] { 0, 0 };
        int turnWeight = 10;

        board[cell].grid = new int[] { (board[cell-1].direction[0] + grid[0]), (board[cell-1].direction[1] + grid[1]) };
        // bool doTurn = false;
        // int turnVal = Random.Range(1, 100);
        // if (turnWeight > turnVal)
        // { doTurn = true; }

        // If statement is checking if the direction is
        // going along the X-axis
        Hashtable validDir = CheckNeighbors(cell);
        if (board[cell].direction[0] != 0)
        {
            int pastCellDir = board[cell - 1].direction[0];
            // Checking if we're facing East, else West
            if (pastCellDir == 1)
            {
                if (!validDir.ContainsKey("East"))
                { direction = new int[] { 0, 1 }; }
            }
            else
            {
                if (!validDir.ContainsKey("West"))
                { direction = new int[] { 0, 1 }; }
            }
        }
        // If not along the X-axis, check it against the
        // Zed-Axis
        else if (board[cell].direction[1] != 0)
        {
            int pastCellDir = board[cell - 1].direction[1];
            // Checking if we're facing North, else South
            if (pastCellDir == 1)
            {
                if (!validDir.ContainsKey("North"))
                { direction = new int[] { 1, 0 }; }
            }
            else
            {
                if (!validDir.ContainsKey("South"))
                { direction = new int[] { 1, 0 }; }
            }
        }
        // If neither, then this is the first cell and
        // a direction must be initialized, default North
        else { direction = new int[] { 0, 1 }; }

        return direction;
    }
}

