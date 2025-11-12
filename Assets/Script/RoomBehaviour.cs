using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject turnCorner;
    public GameObject[] walls;
    public GameObject[] obstaclePack;

    // These are used to determine when to delete a room. Turnip    
    private bool visited = false;
    private bool occupied = false;

    // direction { x, z}
    // North     { 0, 1}
    // South     { 0,-1}
    // East      { 1, 0}
    // West      {-1, 0}

    // GameObject[] doorsLeft = GameObject.FindGameObjectsWithTag(c_doorLeft).Where(o => o.transform.parent.name == "Wall_Door_Long_01");

    public void Start()
    {
        obstaclePack = GameObject.FindGameObjectsWithTag("ObstaclePack");
    }

    // Checks to make per 60 frames
    public void FixedUpdate()
    {
        if (visited && !occupied)
        {
            Destroy(transform);
        }
    }

    // Status list oudated, update parameters
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < 4; i++)
        {
            walls[i].SetActive(!status[i]);
        }
        turnCorner.SetActive(status[4]);
    }

}
