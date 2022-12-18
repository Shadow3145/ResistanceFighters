using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemIndex = -1;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Sprite defaultIcon;

    private DragDrop dragDrop;

    private void Start()
    {
        dragDrop = FindObjectOfType<DragDrop>();
        SetIcon();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemIndex == -1)
        {
            eventData = null;
            return;
        }
        dragDrop.SetItem(Inventory.instance.GetInventoryItem(itemIndex));
        dragDrop.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemIndex == -1)
        {
            eventData = null;
            return;
        }
        dragDrop.GetComponent<RectTransform>().position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragDrop.ResetItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemIndex == -1)
            return;

        Tooltip.instance.ShowItemTooltip(Inventory.instance.GetInventoryItem(itemIndex));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.HideItemTooltip();
    }

    public void SetIcon()
    {
        Item item = Inventory.instance.GetItem(itemIndex);
        itemIcon.sprite = item == null
            ? defaultIcon
            : item.GetIcon();
    }

    public InventoryItem GetItem()
    {
        return Inventory.instance.GetInventoryItem(itemIndex);
    }
}

