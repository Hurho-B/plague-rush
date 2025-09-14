using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWires : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private Transform startParent;
    [SerializeField] private Canvas canvas;

    public bool dragInSlot; // change if drop in right slot 
    private bool isDragging = false;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        startParent = transform.parent;

        canvasGroup.alpha = 0.8f;   
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
    }

    //Drag with the mouse 
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragInSlot)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

           
            rectTransform.position = startPosition;
            transform.SetParent(startParent);
        }
        isDragging = false;
    }

}
