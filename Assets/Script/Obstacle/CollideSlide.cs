using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideSlide : MonoBehaviour
{
    private Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //nothing much just reset.-- will need to add more 
            SceneManager.LoadScene(0);
        }
    }

    public void SetColliderActive(bool active)
    {
        col.enabled = active;
    }
}
