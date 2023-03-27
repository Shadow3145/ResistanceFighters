using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Item,
    Potion,
    Ingredient
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private List<Item> regularItems;
    [SerializeField] private List<Potion> potions;
    [SerializeField] private List<Ingredient> ingredients;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Item GetItem(InventoryItem inventoryItem)
    {
        switch (inventoryItem.GetItemType())
        {
            case ItemType.Item:
                return GetRegularItem(inventoryItem.GetIndex());
            case ItemType.Potion:
                return GetPotion(inventoryItem.GetIndex());
            case ItemType.Ingredient:
                return GetIngredient(inventoryItem.GetIndex());
            default:
                return null;
        }
    }

    public Item GetRegularItem(int index)
    {
        if (index < 0 || index >= regularItems.Count)
            return null;

        return regularItems[index];
    }

    public Potion GetPotion(int index)
    {
        if (index < 0 || index >= potions.Count)
            return null;

        return potions[index];
    }

    public Ingredient GetIngredient(int index)
    {
        if (index < 0 || index >= ingredients.Count)
            return null;
        
        return ingredients[index];
    }

    public int GetIndex(Potion potion)
    {
        return potions.IndexOf(potion);
    }

    public int GetIndex(Ingredient ingredient)
    {
        return ingredients.IndexOf(ingredient);
    }
}
