using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Get the audio controller and call the death sound method directly
            PlayerAudioController audioController = collision.gameObject.GetComponent<PlayerAudioController>();
            if (audioController != null)
            {
                audioController.PlayDeathSound();
            }

            // Get other components and disable movement
            playerMovement playerMovementScript = collision.gameObject.GetComponent<playerMovement>();
            Menu menuScript = GameObject.Find("Canvas_").GetComponent<Menu>();

            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;
            }

            if (menuScript != null)
            {
                menuScript.OpenLosePage();
            }
        }
    }
}