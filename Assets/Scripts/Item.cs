using System;
using UnityEngine;

public class Item : ScriptableObject, IEquatable<Item>
{
    [SerializeField] public float price { get; protected set; }
    [SerializeField] public string itemName { get; protected set; }
    [SerializeField] public Sprite icon { get; protected set; }

    protected int ownedAmount = 0;

    private float Sell(int amount)
    {
        if (!HasEnough(amount))
        {
            Debug.Log("Don't have enough");
            return 0f;
        }

        Consume(amount);

        return price * amount;
    }

    public void Consume(int amount)
    {
        if (HasEnough(amount))
            ownedAmount -= amount;
    }

    private bool HasEnough(int amount)
    {
        return ownedAmount >= amount;
    }

    public void Add(int amount)
    {
        ownedAmount += amount;
    }

    public int GetAmount()
    {
        return ownedAmount;
    }

    public bool Equals(Item other)
    {
        if (other == null)
            return false;

        return this.itemName == other.itemName;
        
    }
}
