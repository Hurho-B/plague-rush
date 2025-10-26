using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] obstacle;
    public GameObject[] floor;
    public GameObject[] walls;
    public GameObject corner;

    private bool visited = false;
    private bool occupied = false;

    // direction { x, z}
    // North     { 0, 1}
    // South     { 0,-1}
    // East      { 1, 0}
    // West      {-1, 0}

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
        corner.SetActive(status[4]);
    }

}
