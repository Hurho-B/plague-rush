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
    [Tooltip("The chance a turn should happen when legal.")]
    public int turnWeight;
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
        { board.Add(new Cell()); }

        for (int i = 1; i < pathLength; i++)
        {
            var pCell = board[i - 1];

            board[i].visited = true;
            board[i].direction = PickDirection(i);
            board[i].grid = new int[] { pCell.grid[0] + pCell.direction[0],
                pCell.grid[1] + pCell.direction[1] };


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
        for (int i = 0; i < cell; i++)
        {
            // direction { x, z}
            // North     { 0, 1}
            // South     { 0,-1}
            // East      { 1, 0}
            // West      {-1, 0}
            var pastCell = board[i].grid;
            var currCell = board[cell].grid;
            
            if ((pastCell[1] - 1) == currCell[1] && pastCell[0] == currCell[0])
            { neighbors.Remove("North"); }
            if ((pastCell[0] - 1) == currCell[0] && pastCell[1] == currCell[1])
            { neighbors.Remove("East"); }
            if ((pastCell[1] + 1) == currCell[1] && pastCell[0] == currCell[0])
            { neighbors.Remove("South"); }
            if ((pastCell[0] + 1) == currCell[0] && pastCell[1] == currCell[1])
            { neighbors.Remove("West"); }
        }

        // Returns a valid 
        return neighbors;
    }

    int[] PickDirection(int cell)
    {
        Hashtable validDir = CheckNeighbors(cell);
        int [] direction = new int[] { 0, 0 };
        int[] pastCellDir = board[cell - 1].direction;
        int doTurn = Random.Range(0, 11);

        string directions = "";
        foreach (DictionaryEntry key in validDir)
            directions += key.Key+" ";
        print(directions);

        // Checking if the path is going North/South already
        if (pastCellDir[1] != 0)
        {
            print("We're going North/South!");
            // Checking if we're facing North, else South
            if (pastCellDir[1] == 1)
            {
                if (validDir.ContainsKey("North") && doTurn < turnWeight)
                { direction = new int[] { 0, 1 }; }
                else
                {
                    if (validDir.ContainsKey("East") && validDir.ContainsKey("West"))
                    {
                        if (Random.Range(0, 2) == 0)
                            direction = new int[] { 1, 0 };
                        else
                            direction = new int[] { -1, 0 };
                    }
                    else if (validDir.ContainsKey("East"))
                    { direction = new int[] { 1, 0 }; }
                    else
                    { direction = new int[] { -1, 0 }; }
                }
            }
            else
            {
                if (validDir.ContainsKey("South") && doTurn < turnWeight)
                { direction = new int[] { 0, -1 }; }
                else
                {
                    if (validDir.ContainsKey("East") && validDir.ContainsKey("West"))
                    {
                        if (Random.Range(0, 2) == 0)
                            direction = new int[] { 1, 0 };
                        else
                            direction = new int[] { -1, 0 };
                    }
                    else if (validDir.ContainsKey("East"))
                    { direction = new int[] { 1, 0 }; }
                    else
                    { direction = new int[] { -1, 0 }; }
                }
            }
        }
        else if (pastCellDir[0] != 0)
        {
            print("We're going East/West!");
            // Checking if we're facing East, else West
            if (pastCellDir[0] == 1)
            {
                if (validDir.ContainsKey("East") && doTurn < turnWeight)
                { direction = new int[] { 1, 0 }; }
                else
                {
                    if (validDir.ContainsKey("North") && validDir.ContainsKey("South"))
                    {
                        if (Random.Range(0, 2) == 0)
                            direction = new int[] { 0, 1 };
                        else
                            direction = new int[] { 0, -1 };
                    }
                    else if (validDir.ContainsKey("North"))
                    { direction = new int[] { 0, 1 }; }
                    else
                    { direction = new int[] { 0, -1 }; }
                }
            }
            else
            {
                if (validDir.ContainsKey("West") && doTurn < turnWeight)
                { direction = new int[] { -1, 0 }; }
                else
                {
                    if (validDir.ContainsKey("North") && validDir.ContainsKey("South"))
                    {
                        if (Random.Range(0, 2) == 0)
                            direction = new int[] { 0, 1 };
                        else
                            direction = new int[] { 0, -1 };
                    }
                    else if (validDir.ContainsKey("North"))
                    { direction = new int[] { 0, 1 }; }
                    else
                    { direction = new int[] { 0, -1 }; }
                }
            }
        }
        // If neither, then this is the first cell and
        // a direction must be initialized, default North
        else
        {
            print("Hit the failsafe assignment!");
            direction = new int[] { 0, 1 };
        }

        return direction;
    }
}

