using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public bool North_South;
    public GameObject[] obstacle;
    public GameObject[] floor;
    private bool beenVisited = false;

    // North-South  East-West
    // 1            0

    // Checks to make per 60 frames
    public void FixedUpdate()
    {
        if (beenVisited)
        {
            // Delete room after it's been visited
        }
    }

    public void UpdateRoom(bool[] status)
    {
        // 3 types of obstacles need to be implemented:
        // - Objects to jump over
        // - Objects to slide under
        // - Objects to go around
        //
        // 
    }

}
