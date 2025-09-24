using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropWires : MonoBehaviour, IDropHandler
{
    private DragWires dragWiresScript;
    private WiresComplete wirePageScript;
    public string gameObjectName;

    private void Start()
    {
        wirePageScript = GetComponentInParent<WiresComplete>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        

        if (eventData.pointerDrag != null && eventData.pointerDrag.gameObject.name == gameObjectName)
        {
            
            dragWiresScript = eventData.pointerDrag.gameObject.GetComponent<DragWires>();
            dragWiresScript.dragInSlot = true;
            // Snap to this drop zone
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.position;
            wirePageScript.WireDone();
        }
    }
}
