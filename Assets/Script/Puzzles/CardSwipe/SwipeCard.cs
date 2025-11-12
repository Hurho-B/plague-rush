using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Settings")]
    private RectTransform rectMovement;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    Vector2 startLocation;
    public bool drop;
    private bool dragging;
    public float timer;
    public float maxOffsets;
    public float minOffsets;

    [Header("Audio Settings")]
    [SerializeField] AudioSource cardSwipeSound;
    [SerializeField] AudioSource cardAcceptedSound;
    [SerializeField] AudioSource cardRejectedSound;

    private bool hasPlayedSwipeSound = false;

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
        hasPlayedSwipeSound = false;
    }

    private void Update()
    {
        if (dragging)
        {
            timer = timer + Time.deltaTime;
            //  Debug.Log(timer);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas)
        {
            dragging = true;
            float newx = rectMovement.anchoredPosition.x + eventData.delta.x / canvas.scaleFactor;
            if (newx > rectMovement.anchoredPosition.x)
            {
                // Clamp X movement
                float clampedX = Mathf.Clamp(newx, minOffsets, maxOffsets);
                rectMovement.anchoredPosition = new Vector2(clampedX, rectMovement.anchoredPosition.y);

                // Play swipe sound once when card reaches a certain threshold
                if (!hasPlayedSwipeSound && clampedX > (maxOffsets * 0.5f))
                {
                    if (cardSwipeSound != null)
                    {
                        cardSwipeSound.Play();
                    }
                    hasPlayedSwipeSound = true;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        dragging = false;
        timer = 0;

        if (!drop)
        {
            // Play rejected sound (card returns to start)
            if (cardRejectedSound != null)
            {
                cardRejectedSound.Play();
            }

            rectMovement.anchoredPosition = startLocation;
        }
        else
        {
            // Play accepted sound (card was successfully swiped)
            if (cardAcceptedSound != null)
            {
                cardAcceptedSound.Play();
            }
        }
    }
}