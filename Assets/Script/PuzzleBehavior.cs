using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBehavior : MonoBehaviour
{
    private bool visited = false;
    private bool occupied = false;
    
    private void FixedUpdate() {
        if (visited && !occupied) Destroy(gameObject, 1);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            visited = true;
            occupied = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            occupied = false;
        }
    }
}
