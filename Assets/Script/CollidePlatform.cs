using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidePlatform : MonoBehaviour
{
    playerMovement playerScript;
    public string direction;
    public Transform centerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<playerMovement>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //give direction to player movement
            playerScript.TurnPlayerTemple( direction,centerTransform);
        }
    }


}
