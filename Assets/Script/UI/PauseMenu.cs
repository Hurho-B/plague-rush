using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePage;
    bool pauseOpen = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseOpen)
            {
                //close
                ClosePage();
            }
            else
            {
                //pause 
                OpenPage();
            }
        }
    }

    void OpenPage()
    {
        pauseOpen = true;
        Time.timeScale = 0f;
        PausePage.SetActive(true);
    }
    void ClosePage()
    {
        pauseOpen = false;
        Time.timeScale = 1f;
        PausePage.SetActive(false);
    }
}
