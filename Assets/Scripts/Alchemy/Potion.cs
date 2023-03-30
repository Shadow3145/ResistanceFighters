using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Alchemy/Potion")]
public class Potion : Item
{
    [SerializeField] private Rarity rarity;

    public Potion Init(string name, string description, Rarity rarity)
    {
        this.itemName = name;
        this.description = description;
        this.rarity = rarity;
        return this;
    }
    public virtual void Use()
    {

    }

    public Rarity GetRarity()
    {
        return rarity;
    }
}
