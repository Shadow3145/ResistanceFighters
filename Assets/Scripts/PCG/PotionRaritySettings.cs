using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Range
{
    public int minValue;
    public int maxValue;
}


[CreateAssetMenu(menuName = "SO/Generator/PotionRaritySettings")]
public class PotionRaritySettings : ScriptableObject
{
    [SerializeField] private Range ingredientsAmount;
    [SerializeField] [Range(0,1)] private List<float> amountProbabilities;
    [SerializeField] private Range mainEffectStrength;
    [SerializeField] private Range secondaryEffectStrength;

    public Range GetIngredientAmount()
    {
        return ingredientsAmount;
    }

    public float GetAmountOfEffects(int index)
    {
        return amountProbabilities[index];
    }

    public Range GetMainEffectStrength()
    {
        return mainEffectStrength;
    }

    public Range GetSecondaryEffectStrength()
    {
        return secondaryEffectStrength;
    }
}
