using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private List<Item> inventoryItems = new List<Item>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddItem(Item item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems[inventoryItems.IndexOf(item)].Add(item.GetAmount());
            return;
        }

        inventoryItems.Add(item);
    }

    public void RemoveItem(Item item)
    {
        if (!inventoryItems.Contains(item))
            return;
        
        int index = inventoryItems.IndexOf(item);

        if (inventoryItems[index].GetAmount() <= item.GetAmount())
            inventoryItems.Remove(item);
        else
            inventoryItems[index].Consume(item.GetAmount());        
    }
}
