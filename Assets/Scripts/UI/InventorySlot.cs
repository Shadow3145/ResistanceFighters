using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemIndex = -1;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private GameObject amount;
    [SerializeField] private ItemType type;
    
    private TextMeshProUGUI amountText;

    private DragDrop dragDrop;

    private void Start()
    {
        dragDrop = FindObjectOfType<DragDrop>();
        SetIcon();
        SetAmount();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemIndex == -1 || Inventory.instance.GetInventoryItem(itemIndex).GetItemType() != ItemType.Ingredient)
        {
            eventData = null;
            return;
        }
        SoundManager.instance.PlaySFX(2);
        dragDrop.SetItem(Inventory.instance.GetInventoryItem(itemIndex));
        dragDrop.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemIndex == -1 || Inventory.instance.GetInventoryItem(itemIndex).GetItemType() != ItemType.Ingredient)
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
        SoundManager.instance.PlaySFX(3);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.HideItemTooltip();
    }

    private void SetIcon()
    {
        Item item = Inventory.instance.GetItem(itemIndex);
        itemIcon.sprite = item == null
            ? defaultIcon
            : item.GetIcon();
    }

    public void SetAmount()
    {
        if (itemIndex == -1)
        {
            amount.SetActive(false);
            return;
        }
        amount.SetActive(true);
        amountText = amount.GetComponentInChildren<TextMeshProUGUI>();
        amountText.text = GetItem().GetAmount().ToString();
    }

    public InventoryItem GetItem()
    {
        return Inventory.instance.GetInventoryItem(itemIndex);
    }

    public void AddItem()
    {
        SetIcon();
        SetAmount();
    }

    public void RemoveItem()
    {
        itemIndex = -1;
        SetIcon();
        amount.SetActive(false);
    }

    public ItemType GetSlotType()
    {
        return type;
    }
}

