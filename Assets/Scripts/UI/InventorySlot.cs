using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemIndex;
    private DragDrop dragDrop;

    private void Start()
    {
        dragDrop = FindObjectOfType<DragDrop>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragDrop.SetItem(Inventory.instance.GetItemAtIndex(itemIndex));
        dragDrop.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragDrop.GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragDrop.ResetItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Show tooltip
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Hide tooltip 
    }
}

