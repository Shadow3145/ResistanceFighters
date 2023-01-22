using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Generator/PriceSettings")]
public class PriceSettings : ScriptableObject
{
    [SerializeField] private List<float> rarityPriceBase;
    [SerializeField] private float amountMod;
    [SerializeField] private float primaryStrengthMod;
    [SerializeField] private float secondaryStrengthMod;

    public float GetRarityPrice(int index)
    {
        return rarityPriceBase[index];
    }

    public float GetAmountMod()
    {
        return amountMod;
    }

    public float GetPrimaryMod()
    {
        return primaryStrengthMod;
    }

    public float GetSecondaryMod()
    {
        return secondaryStrengthMod;
    }
}
