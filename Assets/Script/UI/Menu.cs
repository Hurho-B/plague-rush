using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public playerMovement playerScript;
    public GameObject playerCam;//set active for these 2
    public GameObject startCam;
    public PlayerAudioController audioController;

    [Header("Menu Audio")]
    public AudioSource menuMusic; // Menu background music that stops when game starts
    public AudioSource loseScreenMusic; // Music that plays only on lose screen
    public AudioSource pauseMusic; // Music that plays only when paused
    public AudioSource cutsceneMusic; // Music that plays only during cutscene
    private bool gameStarted = false;

    public void OnStartButtonClicked()
    {
        // Stop all menu music when game starts
        StopAllMenuAudio();
        gameStarted = true;

        // Start the audio controller
        if (audioController != null)
        {
            audioController.StartGame();
        }
    }

    bool pageOpen = false;

    [Header("Pause Menu")]
    public GameObject pausePage;

    [Header("Lose Page")]
    public GameObject losePage;

    [Header("Start Page")]
    public GameObject startPage;

    [Header("Timeline")]
    public GameObject cutscene;

    private void Start()
    {
        Cursor.visible = true;

        // Start playing menu music if not already playing (only on main menu)
        if (menuMusic != null && !menuMusic.isPlaying && !gameStarted)
        {
            menuMusic.Play();
        }

        // Make sure other music is stopped at start
        StopNonMenuAudio();
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

        // Play pause music only when pause menu is activated
        if (pauseMusic != null && !pauseMusic.isPlaying)
        {
            pauseMusic.Play();
        }
    }

    void ClosePausePage()
    {
        pageOpen = false;
        Time.timeScale = 1f;
        pausePage.SetActive(false);
        Cursor.visible = false;

        // Stop pause music when unpausing
        if (pauseMusic != null && pauseMusic.isPlaying)
        {
            pauseMusic.Stop();
        }
    }

    public void OpenLosePage()
    {
        pageOpen = false;
        losePage.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Stop other audio and play lose screen music only when lose screen is activated
        StopAllMenuAudio();
        if (loseScreenMusic != null && !loseScreenMusic.isPlaying)
        {
            loseScreenMusic.Play();
        }
    }

    public void OnClickRestart()
    {
        // Stop all audio when restarting
        StopAllMenuAudio();
        SceneManager.LoadScene(0);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    //start page 
    public void OnClickStart()
    {
        // Stop menu music when starting game
        StopAllMenuAudio();
        gameStarted = true;

        // Start the audio controller
        if (audioController != null)
        {
            audioController.StartGame();
        }

        // Play cutscene music when cutscene starts
        if (cutsceneMusic != null && !cutsceneMusic.isPlaying)
        {
            cutsceneMusic.Play();
        }

        //make delay to make it look better 
        cutscene.SetActive(true);
        startPage.SetActive(false);
        //Hide Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5.4f);
        cutscene.SetActive(false);
        playerScript.enabled = true;
        playerCam.SetActive(true);
        startCam.SetActive(false);

        // Stop cutscene music when cutscene ends
        if (cutsceneMusic != null && cutsceneMusic.isPlaying)
        {
            cutsceneMusic.Stop();
        }
    }

    // Helper methods to manage audio
    void StopAllMenuAudio()
    {
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();
        if (loseScreenMusic != null && loseScreenMusic.isPlaying)
            loseScreenMusic.Stop();
        if (pauseMusic != null && pauseMusic.isPlaying)
            pauseMusic.Stop();
        if (cutsceneMusic != null && cutsceneMusic.isPlaying)
            cutsceneMusic.Stop();
    }

    void StopNonMenuAudio()
    {
        if (loseScreenMusic != null && loseScreenMusic.isPlaying)
            loseScreenMusic.Stop();
        if (pauseMusic != null && pauseMusic.isPlaying)
            pauseMusic.Stop();
        if (cutsceneMusic != null && cutsceneMusic.isPlaying)
            cutsceneMusic.Stop();
    }
}