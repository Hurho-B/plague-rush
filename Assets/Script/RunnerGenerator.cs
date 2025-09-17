using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public int[] direction = { 0, 0 };
    }

    [Header("Debug")]
    [Tooltip("The length of the generated segment.")]
    public Vector2 size; // Working on converting this into an int
    [Tooltip("The # of units each room's center is seperated by.")]
    public int offset;
    [Header("Prefabs")]
    [Tooltip("Used for standard path segments.")]
    public GameObject room;
    [Tooltip("Used for puzzle rooms at the end of each path.")]
    public GameObject endPiece;


    List<Cell> board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PathGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     // Deletes the existing list + gameObjects
        //     // Generates a new dungeon
        //     board.Clear(); 
        //     for (int i = (transform.childCount - 1); i >= 0; i--)
        //     {
        //         Transform child = transform.GetChild(i);
        //         Destroy(child.gameObject);
        //     }
        //     PathGenerator();
        // }
    }

    void GeneratePath()
    {
        // After PathGenerator(), new path list is brought in
        // and used to make the actual environment/gameObjects
        //
        //
        // To-do:
        // - Make the generator place parts according to prior cells
        // - A change in directions should create a new generator so
        //   existing generators may be culled
        for (int i = 0; i < size.x; i++)
        {
            Cell currentCell = board[i];
            int[] grid = { 0, 0 };

            if (i != 0)
            {
            }

            for (int j = 0; j < size.y; j++)
                {
                    
                    RoomBehaviour newRoom = Instantiate(room, new Vector3(currentCell.Item1 * offset, 0, currentCell.Item2 * offset), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.direction);

                    newRoom.name += " " + i;
                }
            grid += board.direction[i];
        }
    }

    void PathGenerator()
    {
        // Generating the workable area
        board = new List<Cell>();
        for (int i = 0; i < (size.x * size.y); i++)
        {
            board.Add(new Cell());
        }

        board[0].direction = new { 0, 1 };

        for (int i = 0; i < size.x; i++)
        {
            int currentCell = 0;
            Stack<int> path = new Stack<int>();
            int k = 0;
            while (k < 1000)
            {
                k++;
                if (currentCell == board.Count - 1 || path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
        }
        GeneratePath();
    }
}
