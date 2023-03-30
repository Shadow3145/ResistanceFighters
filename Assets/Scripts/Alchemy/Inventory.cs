using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InventoryItem : IEquatable<InventoryItem>
{
    [SerializeField]private int index;
    [SerializeField] private ItemType itemType;
    [SerializeField] private int amount;

    public InventoryItem(int index, ItemType itemType, int amount = 1)
    {
        this.index = index;
        this.itemType = itemType;
        this.amount = amount;
    }

    public int GetIndex()
    {
        return index;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public int GetAmount()
    {
        return amount;
    }

    public void Add(int amount)
    {
        this.amount += amount;
    }

    public void Consume(int amount)
    {
        this.amount -= amount;
    }

    public bool Equals(InventoryItem other)
    {
        return (index == other.index && itemType == other.itemType);
    }

    public Item GetItem()
    {
       return ItemManager.instance.GetItem(this);
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private List<InventoryItem> inventoryItems;

    private InventorySlot[] inventorySlots;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        inventorySlots = FindObjectsOfType<InventorySlot>();
        Array.Reverse(inventorySlots);
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < inventorySlots.Length; i++)
            inventoryItems.Add(null);
        for (int i = 0; i < ItemManager.instance.GetIngredients().Count; i++)
        {
            AddItem(new InventoryItem(i, ItemType.Ingredient, 20));
        }
    }


    public void AddItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
        {
            int index = inventoryItems.IndexOf(item);
            inventoryItems[index].Add(item.GetAmount());
            ChangeAmountInUI(index);
            return;
        }

        int i = FindFreeIndex();
        inventoryItems[i] = item;
        AddItemToUI(i);
    }

    public void ConsumeItem(InventoryItem item)
    {
        if (!inventoryItems.Contains(item))
            return;
        
        int index = inventoryItems.IndexOf(item);

        if (inventoryItems[index].GetAmount() <= item.GetAmount())
        {
            inventoryItems[index] = null;
            RemoveItemFromUI(index);
        }
        else
        {
            inventoryItems[index].Consume(item.GetAmount());
            ChangeAmountInUI(index);
        }
    }

    public InventoryItem GetInventoryItem(int index)
    {
        if (index < 0 || index >= inventoryItems.Count)
            return null;
        
        return inventoryItems[index];
    }

    public Item GetItem(int index)
    {
        InventoryItem invItem = GetInventoryItem(index);
        if (invItem == null)
            return null;

        return invItem.GetItem();
    }

    private void AddItemToUI(int index)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemIndex != -1 || slot.GetSlotType() != inventoryItems[index].GetItemType())
                continue;

            slot.itemIndex = index;
            slot.AddItem();
            return;
        }
    }

    private void ChangeAmountInUI(int index)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemIndex == index)
            {
                slot.SetAmount();
                return;
            }
        }
    }

    private void RemoveItemFromUI(int index)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemIndex == index)
            {
                slot.RemoveItem();
                return;
            }
        }
    }

    private int FindFreeIndex()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] == null)
                return i;
        }

        return -1;
    }
}
