using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : Item
{
    public Rarity rarity { get; private set; }
    public abstract void Use();
}
