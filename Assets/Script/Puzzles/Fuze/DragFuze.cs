using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFuze : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Vector2 startTransform;
    public Canvas canvas;
    public bool canDrag;
    public bool fix;
    public bool dropInDropZone;
    private CanvasGroup canvasGp;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGp = GetComponent<CanvasGroup>();
        startTransform = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Optional: Bring to front or highlight
        canvasGp.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null && canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGp.blocksRaycasts = true;
        if (!dropInDropZone)
        {
            // Optional: Snap to grid or finalize position
            rectTransform.anchoredPosition = startTransform;
        }
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }
    public void ResetPosition(Transform parent)
    {
        transform.SetParent(parent);
        rectTransform.anchoredPosition = startTransform;
        canDrag = false;
    }
}

