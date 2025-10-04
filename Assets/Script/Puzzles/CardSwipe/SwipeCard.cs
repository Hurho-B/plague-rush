using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour,IPointerDownHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectMovement;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    Vector2 startLocation;
    public bool drop;
    private bool dragging;
    public float timer;
    public float maxOffsets;
    public float minOffsets;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectMovement = GetComponent<RectTransform>();

        startLocation = rectMovement.anchoredPosition;

        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //begin drag 
        canvasGroup.blocksRaycasts = false;
    }
    private void Update()
    {
        if (dragging)
        {
            timer = timer + Time.deltaTime;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas)
        {
            dragging = true;

            rectMovement.anchoredPosition += new Vector2(eventData.delta.x / canvas.scaleFactor, 0f);

            // Clamp X movement
            float clampedX = Mathf.Clamp(rectMovement.anchoredPosition.x, minOffsets, maxOffsets);
            rectMovement.anchoredPosition = new Vector2(clampedX, rectMovement.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        dragging = false;
        timer = 0;
        if (!drop)
        {
            rectMovement.anchoredPosition = startLocation;
        }
    }
}
