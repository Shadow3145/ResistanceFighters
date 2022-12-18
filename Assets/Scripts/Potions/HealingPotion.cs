using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Alchemy/Potions/HealingPotion")]
public class HealingPotion : Potion
{
    [SerializeField] private float value;
    public override void Use()
    {
        Debug.Log("Increase Health");
    }
}
