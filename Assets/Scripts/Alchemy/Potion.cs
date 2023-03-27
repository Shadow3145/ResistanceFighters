using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : Item
{
    [SerializeField] private Rarity rarity;
    public abstract void Use();

    public Rarity GetRarity()
    {
        return rarity;
    }
}
