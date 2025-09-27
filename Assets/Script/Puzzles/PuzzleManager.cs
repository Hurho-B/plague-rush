using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject wirePage;
    [SerializeField] GameObject timerPage;
    [SerializeField] GameObject puzzle2;// not done the other puzzles 
    [SerializeField] GameObject puzzle3;
    [SerializeField] GameObject puzzle4;

    public GameObject player; //leave empty in engine
    private Timer timerScript;

    //puzzle count
    private int howManyPuzzleFinish;
    public int puzzleNeededFinish;

    private void Start()
    {
        timerScript = gameObject.GetComponent<Timer>();
    }

    public void startPuzzle()
    {
        //start timer
        timerScript.startTimer();
        SpawnPuzzle();
    }
    public void SpawnPuzzle()
    {
        //need an if to check if we spawn another
        if(puzzleNeededFinish != howManyPuzzleFinish)
        {
            //need to randomize which spawn or activated
            //for now
            wirePage.SetActive(true);
            timerPage.SetActive(true);
        }
        else
        {
            Debug.Log("Stop all puzzles");
            AllPuzzleFinish();
        }
    }

    public void FinishActivePuzzle()
    {
        howManyPuzzleFinish++;
        SpawnPuzzle();
    }
     private void AllPuzzleFinish()
    {
        timerScript.StopTimer(false);
        ClosePages();
        //make player move
        playerMovement plaScript = player.GetComponent<playerMovement>();
        plaScript.enabled = true;
        plaScript.forwardSpeed += 3;
        Cursor.visible = false;
        //make wall move
        GameObject wall = GameObject.FindWithTag("EndWALL");
        Destroy(wall);
        //reset values
        howManyPuzzleFinish = 0;
        //2 generator for different region 
    }

    public  void ClosePages()
    {
        wirePage.SetActive(false);
        timerPage.SetActive(false);
    }

}
