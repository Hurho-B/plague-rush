using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideSlide : MonoBehaviour
{
    private Collider col;
    private Menu menuScript;
    private playerMovement playerMovementScript;
    private void Start()
    {
        col = GetComponent<Collider>();
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            menuScript = GameObject.Find("Canvas").GetComponent<Menu>();
            //nothing much just reset.-- will need to add more 
            playerMovementScript = collision.gameObject.GetComponent<playerMovement>();
            playerMovementScript.enabled = false;
            menuScript.OpenLosePage();
        }
    }

    public void SetColliderActive(bool active)
    {
        col.enabled = active;
    }
}
