using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresComplete : MonoBehaviour
{
    [Header("Wire Settings")]
    public int howManyWiresCompleted;
    private PuzzleManager puzzleManagerScript;

    [Header("Audio Settings")]
    [SerializeField] AudioSource wirePickupSound;
    [SerializeField] AudioSource wireConnectSound;
    [SerializeField] AudioSource wireDisconnectSound;
    [SerializeField] AudioSource puzzleCompleteSound;

    private void Start()
    {
        puzzleManagerScript = GetComponentInParent<PuzzleManager>();
    }

    public void OnWirePickup()
    {
        if (wirePickupSound != null)
        {
            wirePickupSound.Play();
        }
    }

    public void OnWireConnect()
    {
        if (wireConnectSound != null)
        {
            wireConnectSound.Play();
        }
    }

    public void OnWireDisconnect()
    {
        if (wireDisconnectSound != null)
        {
            wireDisconnectSound.Play();
        }
    }

    public void WireDone() // all 4 wires completed 
    {
        howManyWiresCompleted++;

        if (howManyWiresCompleted == 4)
        {
            // Play completion sound
            if (puzzleCompleteSound != null)
            {
                puzzleCompleteSound.Play();
            }

            //tell puzzle manager that it is completed 
            puzzleManagerScript.FinishActivePuzzle();
        }
    }
}