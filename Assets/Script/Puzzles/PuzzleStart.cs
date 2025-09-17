using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStart : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;
    private playerMovement playerScript;

    private void Start()
    {
        playerScript = player.GetComponent<playerMovement>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerScript.enabled = false;
            Cursor.visible = true;
            canvas.SetActive(true);
        }
    }
}
