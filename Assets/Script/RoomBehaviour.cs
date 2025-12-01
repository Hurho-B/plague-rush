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
    0-3 = Walls     status[0] - status[3] = Walls
    4 = Corner                  status[4] = Corner
    5 = finalCell               status[5] = Final Cell
    6 = turnRight               status[6] = Turn Right
    7 = before turn             status[7] = Before Turn
                                status[8] = Spawn Obstacles
    */
    public void UpdateRoom(bool[] status, int[] direction)
    {
        if (status[5]) lastCellInArea = true;
        turnCorner.SetActive(status[4]);
        SetWalls(status, direction);
        SetFloor(status, direction);
        if (!status[8]) GenerateObstacles(direction);
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

    private void SetWalls(bool[] status, int[] direction)
    {
        List<GameObject> activeWalls = new List<GameObject>();
        for (int i = 0; i < 4; i++) {
            if (status[i]) {
                walls[i].SetActive(!status[i]);
                continue;
            }
			activeWalls.Add(walls[i]);
        }
        
        bool firstWall = true;
        foreach (GameObject wall in activeWalls) {
            if (status[4]) {
				StraightRoomWall(wall);
				// something something, corners	
			}
            else if (!status[8]) StraightRoomWall(wall);
            else PlaceWalls(status, direction, wall, firstWall);
            firstWall = false;
        }
    }

    /*
Status Key
0-3 = Walls     status[0] - status[3] = Walls
4 = Corner                  status[4] = Corner
5 = finalCell               status[5] = Final Cell
6 = turnRight               status[6] = Turn Right
7 = before turn             status[7] = Before Turn
                            status[8] = Spawn Obstacles
*/
    private void StraightRoomWall(GameObject wall) {
        wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

	private void PlaceWalls(bool[] status, int[] direction, GameObject wall, bool firstWall) { 
		if (status[6] == status[7]) {
    		if ((status[6] && (direction[1] ==  1 || direction[0] == 1)) || (!status[6] && (direction[1] ==  -1 || direction[0] == 1))) {
	    		if (firstWall) {
        			wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
	    		else {
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
    		}
    		else if ((!status[6] && (direction[1] ==  1 || direction[0] == -1)) || (status[6] && (direction[1] ==  -1 || direction[0] == -1))) {
	    		if (firstWall) {
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
	    		else {
        			wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
    		}
		}
		else {
    		if ((!status[7] && (direction[1] ==  -1 || direction[0] == 1)) || (status[7] && (direction[1] ==  1 || direction[0] == -1))) {
	    		if (firstWall) {
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
	    		else {
        			wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
	    		}
    		}
    		else if ((status[7] && (direction[1] ==  1 || direction[0] == -1)) || (!status[7] && (direction[1] ==  -1 || direction[0] == 1))) {
	    		if (firstWall) {
        			wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
	    		}
	    		else {
        			wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        			wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
	    		}
    		}
		}
	}

	private void BeforeTurnRoomWall(bool[] status, int[] direction, bool firstWall, GameObject wall) {
		if (direction[1] ==  1 &&  status[7]) {
        	if (firstWall) {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
		}
		else if (direction[1] == -1 &&  status[7]) {
        	if (firstWall) {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
		}
		else if (direction[0] ==  1 &&  status[7]) {
        	if (firstWall) {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
		}
	    else if (direction[0] == -1 &&  status[7]) { if (firstWall) {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
		}
	}

	private void AfterTurnRoomWall(bool[] status, int[] direction, bool firstWall, GameObject wall) {
		if (direction[1] ==  1 && !status[7]) {
        	if (firstWall) {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
    	}
		else if (direction[1] == -1 && !status[7]) {
        	if (firstWall) {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        	}
    	}
		else if (direction[0] ==  1 && !status[7]) {
	        if (firstWall) {
    	        wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        	    wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        	}
		}
		else if (direction[0] == -1 && !status[7]) {
			if (firstWall) {
            	wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        	}
        	else {
            	wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            	wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        	}
		}
	}
    
    private void TurnRightRoomWall(GameObject wall, bool firstWall)
    {
        if (firstWall) {
            wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        else {
            wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    
    private void TurnLeftRoomWall(GameObject wall, bool firstWall)
    {
        if (firstWall) {
            wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else {
            wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            wall.gameObject.transform.GetChild(2).gameObject.SetActive(false);
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
            else if (status[0]) orientation = Quaternion.Euler(0, 270, 0);
            else if (status[3]) orientation = Quaternion.Euler(0, 90, 0);
        }
        Instantiate(floorPattern[index], transform.position, orientation, transform);
    }
    
    private void GenerateObstacles(int[] direction)
    {
        int drawOfTheCard = Random.Range(0, obstaclePack.Length);
        Quaternion orientation = Quaternion.Euler(0, 0, 0);
        if (direction[0] == 0) {
            if (direction[0] == 1) orientation = Quaternion.Euler(0, 270, 0);
            else if (direction[0] != 1) orientation = Quaternion.Euler(0,  90, 0);
        }
        else {
            if (direction[1] != 1) orientation = Quaternion.Euler(0, 180, 0);
        }
        Instantiate(obstaclePack[drawOfTheCard], transform.position, orientation, transform);
    }
}
