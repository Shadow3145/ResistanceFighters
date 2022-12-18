using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon;

    private Ingredient ingredient;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (ingredient != null)
            AlchemyTable.instance.RemoveIngredient(ingredient);

        SetItem(eventData.pointerDrag.GetComponent<InventorySlot>().GetItem());
        AlchemyTable.instance.AddIngredient(ingredient);
        icon.sprite = ingredient.GetIcon();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AlchemyTable.instance.RemoveIngredient(ingredient);
        ingredient = null;
        icon.sprite = defaultIcon;
    }

    private void SetItem(InventoryItem item)
    {
        ingredient = item.GetItem() as Ingredient;
    }
}
