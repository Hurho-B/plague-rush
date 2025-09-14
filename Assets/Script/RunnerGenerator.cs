using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public Vector2 size;
    public Vector2 offset;
    public int startPos = 0;
    public GameObject room;

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
            // New rooms need to be placed in accordance to "NorthSouth"
            // bool relative of the previous room. "If statement"?
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                RoomBehaviour newRoom = Instantiate(room, new Vector3(1 * offset.x, 0, i * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateRoom(currentCell.status);

                newRoom.name += " " + i + "-" + j;
            }

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

        for (int i = 0; i < size.x; i++)
        {
            int currentCell = startPos;
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
