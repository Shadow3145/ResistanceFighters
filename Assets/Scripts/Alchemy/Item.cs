using System;
using UnityEngine;

public class Item : ScriptableObject, IEquatable<Item>
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected float price;

    [TextArea(3, 10)]
    [SerializeField] protected string description;
    [SerializeField] protected float dropProbability;

    public Item()
    {

    }
    public Item(string name, float price, Sprite icon)
    {
        itemName = name;
        this.price = price;
        this.icon = icon;
    }

    public Item(Item item)
    {
        itemName = item.name;
        icon = item.icon;
        price = item.price;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetName()
    {
        return itemName;
    }
 
    public bool Equals(Item other)
    {
        if (other == null)
            return false;

        return this.itemName == other.itemName;        
    }

    public virtual string GetDescription()
    {
        return description;
    }


}
