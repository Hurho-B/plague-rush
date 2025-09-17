using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] obstacle;
    public GameObject[] floor;
    public GameObject[] wall;

    private int[] direction = {0,0};
    private bool visited = false;

    // direction { x, z}
    // North     { 0, 1}
    // South     { 0,-1}
    // East      { 1, 0}
    // West      {-1, 0}

    // Checks to make per 60 frames
    public void FixedUpdate()
    {
        if (visited)
        {
            // Delete room after it's been visited
        }
    }

    public int getDirection()
    {
        return direction;
    }

    // Status list oudated, update parameters
    public void UpdateRoom(int[] dir)
    {
        direction = dir;
        // 3 types of obstacles need to be implemented:
        // - Objects to jump over
        // - Objects to slide under
        // - Objects to go around
        //
        // 
    }

}
