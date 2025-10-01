using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresComplete : MonoBehaviour
{
    public int howManyWiresCompleted;
    private PuzzleManager puzzleManagerScript;

    private void Start()
    {
        puzzleManagerScript = GetComponentInParent<PuzzleManager>();
    }

    public void WireDone() // all 4 wires completed 
    {
        howManyWiresCompleted++;
        if(howManyWiresCompleted == 4)
        {
            //tell puzzle manager that  it his completed 
            puzzleManagerScript.FinishActivePuzzle();
        }
    }
}
