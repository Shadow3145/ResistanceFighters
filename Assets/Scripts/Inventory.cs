using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryItem : IEquatable<InventoryItem>
{
    private int index;
    private ItemType itemType;
    private int amount;

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
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems[inventoryItems.IndexOf(item)].Add(item.GetAmount());
            return;
        }

        inventoryItems.Add(item);
    }

    public void ConsumeItem(InventoryItem item)
    {
        if (!inventoryItems.Contains(item))
            return;
        
        int index = inventoryItems.IndexOf(item);

        if (inventoryItems[index].GetAmount() <= item.GetAmount())
            inventoryItems.Remove(item);
        else
            inventoryItems[index].Consume(item.GetAmount());        
    }

    public InventoryItem GetItemAtIndex(int index)
    {
        if (index < 0 || index >= inventoryItems.Count)
            return null;
        
        return inventoryItems[index];
    }
}
