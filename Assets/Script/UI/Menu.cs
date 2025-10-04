using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public playerMovement playerScript;
    public GameObject playerCam;//set active for these 2
    public GameObject startCam;

    bool pageOpen = false;

    [Header("Pause Menu")]
    public GameObject pausePage;

    [Header("Lose Page")]
    public GameObject losePage;

    [Header("Start Page")]
    public GameObject startPage;

    private void Start()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && playerScript.enabled == true)
        {
            if (pageOpen)
            {
                //close
                ClosePausePage();
            }
            else
            {
                //pause 
                OpenPausePage();
            }
        }
    }

    void OpenPausePage()
    {
        pageOpen = true;
        Time.timeScale = 0f;
        pausePage.SetActive(true);
    }
    void ClosePausePage()
    {
        pageOpen = false;
        Time.timeScale = 1f;
        pausePage.SetActive(false);
    }

    public void OpenLosePage()
    {
        pageOpen = false;
        losePage.SetActive(true);
        Cursor.visible = true;
    }
    public void OnClickRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    //start page 
    public void OnClickStart()
    {
        //make delay to make it look better 
        playerScript.enabled = true;
        startPage.SetActive(false);
        //Hide Cursor
        Cursor.visible = false;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.4f);
        playerCam.SetActive(true);
        startCam.SetActive(false);
    }
}
