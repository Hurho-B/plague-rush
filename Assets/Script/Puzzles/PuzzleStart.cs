using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStart : MonoBehaviour
{
    private GameObject player;
    public GameObject canvas;
    private PuzzleManager puzzleManagerScript;
    private playerMovement playerScript;
    private Transform[] spawnLocation;

    private void Start() //activating whichever puzzle
    {
        puzzleManagerScript = canvas.GetComponent<PuzzleManager>();
        int childCount = gameObject.transform.childCount;
        Transform[] children = new Transform[childCount];
        spawnLocation = children;

        for (int i = 0; i < childCount; i++)
        {
            spawnLocation[i] = transform.GetChild(i).gameObject.transform;//get child without assigning
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            playerScript = player.GetComponent<playerMovement>();
            playerScript.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // let puzzle manager handle it 
            puzzleManagerScript.player = player;
            puzzleManagerScript.puzzlePosition = spawnLocation;
            puzzleManagerScript.startPuzzle();          
        }
    }
}
