using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Range
{
    public int minValue;
    public int maxValue;

    public Range(int min, int max)
    {
        minValue = min;
        maxValue = max;
    }
}


[CreateAssetMenu(menuName = "SO/Generator/PotionRaritySettings")]
public class PotionRaritySettings : ScriptableObject
{
    [SerializeField] private Range ingredientsAmount;
    [SerializeField] [Range(0,1)] private List<float> amountProbabilities;
    [SerializeField] private Range mainEffectStrength;
    [SerializeField] private Range secondaryEffectStrength;

    public PotionRaritySettings Init(Range ingredientsAmount, List<float> amountProbabilities, 
        Range mainEffect, Range secondaryEffect)
    {
        this.ingredientsAmount = ingredientsAmount;
        this.amountProbabilities = amountProbabilities;
        this.mainEffectStrength = mainEffect;
        this.secondaryEffectStrength = secondaryEffect;
        return this;
    }

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
