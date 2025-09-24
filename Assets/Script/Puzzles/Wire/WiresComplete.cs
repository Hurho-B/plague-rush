using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresComplete : MonoBehaviour
{
    public int howManyWiresCompleted;
    public GameObject wirePage;

    public void WireDone()
    {
        howManyWiresCompleted++;
        if(howManyWiresCompleted == 4)
        {
            wirePage.SetActive(false);
        }
    }
}
