using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideObstacle : MonoBehaviour
{
    private playerMovement playerMovementScript;
    private Menu menuScript;

    private void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            menuScript = GameObject.Find("Canvas").GetComponent<Menu>();
            playerMovementScript = collision.gameObject.GetComponent<playerMovement>();
            //nothing much just reset.-- will need to add more 
            // SceneManager.LoadScene(0);
            playerMovementScript.enabled = false;
            menuScript.OpenLosePage();
        }
    }
}
