using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightPuzzle : MonoBehaviour
{
    public Image[] lightColor;
    public int[] orderNb;
    private int maxNb = 5;
    private int nbTimedone;
    public GameObject blockClick;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowColor());
    }


    IEnumerator ShowColor()
    {
        int i = Random.Range(0, lightColor.Length);
        orderNb[nbTimedone] = i;
        nbTimedone++;
        Color currentColor = lightColor[i].color;
        Color startColor = currentColor;
        currentColor.a = 1f;
        lightColor[i].color = currentColor;

        //Wait a bit 
        yield return new WaitForSeconds(0.7f);
        lightColor[i].color = startColor;
        if (nbTimedone == maxNb)
        {
            Debug.Log("Stop loop");
            yield return new WaitForSeconds(0.2f);
            blockClick.SetActive(false);
            nbTimedone = 0;
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(ShowColor());
        }
    }

    public void ClickLightButton(int whichButtton)
    {
        if (whichButtton == orderNb[nbTimedone])
        {
            nbTimedone++;
            //if all correct you did it 
            if (nbTimedone == maxNb)
            {
                Debug.Log("You Win");
                nbTimedone = 0;
                
            }
        }
        else
        {
            //restart the loop 
            nbTimedone = 0;
            blockClick.SetActive(true);
            StartCoroutine(ShowColor());
        }
    }
}

