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

    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private InventorySlot[] inventorySlots;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        inventorySlots = FindObjectsOfType<InventorySlot>();
    }


    public void AddItem(InventoryItem item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems[inventoryItems.IndexOf(item)].Add(item.GetAmount());
            return;
        }

        inventoryItems.Add(item);
        AddItemToUI(inventoryItems.Count - 1);

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
            if (slot.itemIndex != -1)
                continue;

            slot.itemIndex = index;
            slot.SetIcon();
            return;
        }
    }
}
