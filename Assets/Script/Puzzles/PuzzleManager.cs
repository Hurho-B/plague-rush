using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject[] puzzlePage;
    [SerializeField] GameObject timerPage;
    private GameObject puzzleChosen;
    int i = 0;//for the array

    public GameObject player; //leave empty in engine
    private Timer timerScript;

    //puzzle count
    private int howManyPuzzleFinish;
    public int puzzleNeededFinish;

    private void Start()
    {
        timerScript = gameObject.GetComponent<Timer>();
        MixedPuzzles(puzzlePage);
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
            
            puzzlePage[i].SetActive(true);
            puzzleChosen = puzzlePage[i];

            timerPage.SetActive(true);
            i++;
        }
        else
        {
            AllPuzzleFinish();
        }
    }

    public void FinishActivePuzzle()
    {
        puzzlePage[i - 1].SetActive(false);
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
        puzzleChosen.SetActive(false);
        timerPage.SetActive(false);
    }

    void MixedPuzzles(GameObject[] array) //Will need to definitively change this. need to randomize which one are activated yes which spawn out of all the 5 then they are the one to do
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            //swapping
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

}
