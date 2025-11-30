using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject turnCorner;
    public GameObject[] floorPattern;
    public GameObject[] walls;
    public GameObject[] obstaclePack;

    // These are used to determine when to delete a room.   
    private bool visited = false;
    private bool occupied = false;
    private bool lastCellInArea = false;

    // direction { x, z}
    // North     { 0, 1}
    // South     { 0,-1}
    // East      { 1, 0}
    // West      {-1, 0}

    /*
    Status Key
    0-3 = Walls
    4 = Corner
    5 = Final Cell
    */
    public void UpdateRoom(bool[] status, int[] direction)
    {
        if (status[5]) lastCellInArea = true;
        for (int i = 0; i < 4; i++) walls[i].SetActive(!status[i]);
        turnCorner.SetActive(status[4]);
        if (!status[6]) GenerateObstacles(direction);
        SetFloor(status, direction);
    }

    private void FixedUpdate() {
        if (visited && !occupied) Destroy(gameObject, 1);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            visited = true;
            occupied = true;
        }
        if (lastCellInArea) {
            GameObject.Find("/Generator").GetComponent<RunnerGenerator>().Start();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            occupied = false;
        }
    }

    private void SetFloor(bool[] status, int[] direction)
    {
        int index = 0;
        if (status[4]) index = 1;
        
        Quaternion orientation = Quaternion.Euler(0, 0, 0);
        if (index == 0) {
            if (direction[0] == 0) orientation = Quaternion.Euler(0, 90, 0);
        }
        else if (status[0] || status[3]) {
            if (status[0] && status[3]) orientation = Quaternion.Euler(0, 180, 0);
            if (status[0]) orientation = Quaternion.Euler(0, 270, 0);
            if (status[3]) orientation = Quaternion.Euler(0, 90, 0);
        }
        Instantiate(floorPattern[index], transform.position, orientation, transform);
    }
    
    private void GenerateObstacles(int[] direction)
    {
        int drawOfTheCard = Random.Range(0, obstaclePack.Length);
        Quaternion orientation = Quaternion.Euler(0, 0, 0);
        // obj.transform.SetParent(parent, false);
        // Vector3 parentVector = new Vector3(trans)
        if (direction[0] == 0) {
            if (direction[0] == 1) orientation = Quaternion.Euler(0,  90, 0);
            if (direction[0] != 1) orientation = Quaternion.Euler(0, 270, 0);
        }
        else {
            if (direction[1] != 1) orientation = Quaternion.Euler(0, 180, 0);
        }
        Instantiate(obstaclePack[drawOfTheCard], transform.position, orientation, transform);
    }
}
