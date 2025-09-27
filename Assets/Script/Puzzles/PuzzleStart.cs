using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStart : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;
    private PuzzleManager puzzleManagerScript;
    private playerMovement playerScript;

    private void Start() //activating whichever puzzle
    {
        playerScript = player.GetComponent<playerMovement>();
        puzzleManagerScript = canvas.GetComponent<PuzzleManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerScript.enabled = false;
            Cursor.visible = true;
            // let puzzle manager handle it 
            puzzleManagerScript.startPuzzle();
            puzzleManagerScript.player = player;
        }
    }
}
