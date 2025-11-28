using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject turnCorner;
    public GameObject[] walls;

    // These are used to determine when to delete a room.   
    private bool visited = false;
    private bool occupied = false;
    private bool lastCellInArea = false;
    private RunnerGenerator runnerGenerator;
    private GameObject[] obstaclePack;

    // direction { x, z}
    // North     { 0, 1}
    // South     { 0,-1}
    // East      { 1, 0}
    // West      {-1, 0}

    // GameObject[] doorsLeft = GameObject.FindGameObjectsWithTag(c_doorLeft).Where(o => o.transform.parent.name == "Wall_Door_Long_01");

    public void Start()
    {
        
        // obstaclePack = GameObject.FindGameObjectsWithTag("ObstaclePack");
    }
    
    /*
    Status Key
    0-3 = Walls
    4 = Corner
    5 = Final Cell
    */
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < 4; i++) { walls[i].SetActive(!status[i]); }
        turnCorner.SetActive(status[4]);
        if (status[5])
        {
            lastCellInArea = true;
            runnerGenerator = GameObject.Find("/Generator").GetComponent<RunnerGenerator>();
        }
    }

    private void FixedUpdate() {
        if (visited && !occupied) {
            Destroy(gameObject, 1);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            visited = true;
            occupied = true;
        }
        if (lastCellInArea) {
            runnerGenerator.Start();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            occupied = false;
        }
    }

    public void CityTurns()
    {
        // status[0] = true;
        // status[1] = true;
        // status[2] = false;
        // status[3] = false;
    }
}
