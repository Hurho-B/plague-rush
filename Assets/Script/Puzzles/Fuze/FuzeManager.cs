using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuzeManager : MonoBehaviour
{
    private DragFuze[] dragScripts;
    private FixDropZone[] dropZones;
    private RectTransform brokenFuzePos;
    public GameObject parentGameobject;
    [SerializeField] int ypos = 122;
    trashDropZone trashScript;
    public PuzzleManager puzzleManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        trashScript = GetComponentInChildren<trashDropZone>();
        dragScripts = GetComponentsInChildren<DragFuze>();
        RectTransform rect = GameObject.Find("BrokenFuze").GetComponent<RectTransform>();
        brokenFuzePos = rect;
        ChooseWhichIsBroken();
    }

    //start puzzle which will be in puzzle manager

    void ChooseWhichIsBroken()
    {
        int i = Random.Range(0, dragScripts.Length - 1);
        dragScripts[i].canDrag = true;
        brokenFuzePos.anchoredPosition = dragScripts[i].GetRectTransform().anchoredPosition;
        Vector2 pos = brokenFuzePos.anchoredPosition;
        pos.y = ypos;
        brokenFuzePos.anchoredPosition = pos;
    }
    public void FixMove()
    {
        int i = dragScripts.Length - 1;
        dragScripts[i].canDrag = true;
    }
    public void FinishMiniGame()
    {
        //reset game 
        trashScript.gameObject.SetActive(true);
        //set everything to there start postion 
        foreach (DragFuze script in dragScripts)
        {
            script.ResetPosition(transform); // Replace with your actual method or logic
        }
        puzzleManagerScript.FinishActivePuzzle();
    }
}

