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
    private Transform playerLocation;
    public GameObject timeline;

    private void Start() //activating whichever puzzle
    {
        puzzleManagerScript = canvas.GetComponent<PuzzleManager>();
        int childCount = gameObject.transform.childCount;
        Transform[] children = new Transform[childCount];
        spawnLocation = children;

        for (int i = 0; i < childCount; i++)
        {
            if(i == 3)
            {
                playerLocation = transform.GetChild(i).gameObject.transform;
                break;
            }
            spawnLocation[i] = transform.GetChild(i).gameObject.transform;//get child without assigning
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            // let puzzle manager handle it 
            StartCutscene();
        }
    }

    void StartCutscene()
    {
        playerScript = player.GetComponent<playerMovement>();
        playerScript.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        timeline.SetActive(true);
        StartCoroutine(PuzzleStarting());
    }

    IEnumerator PuzzleStarting()
    {
        yield return new WaitForSeconds(3f);
        player.transform.position = playerLocation.position;
        puzzleManagerScript.player = player;
        puzzleManagerScript.puzzlePosition = spawnLocation;
        puzzleManagerScript.startPuzzle();
        this.gameObject.SetActive(false);
    }
}
