using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity : int
{
    Common = 0,
    Rare = 1,
    Epic = 2
}

[CreateAssetMenu(menuName = "SO/Generator/RaritySettings")]
public class RaritySettings : ScriptableObject
{
    [SerializeField]
    [Range(0, 1)] private List<float> amountProbabilities;
    
    [SerializeField] public float minSumValue;
    [SerializeField] private float maxSumValue;
    
    [SerializeField] private float minPrimaryValue;
    [SerializeField] private float maxPrimaryValue;

    public RaritySettings Init(List<float> amountProbabilities, float minSumValue, float maxSumValue, float minPrimaryValue, float maxPrimaryValue)
    {
        this.amountProbabilities = amountProbabilities;
        this.minSumValue = minSumValue;
        this.maxSumValue = maxSumValue;
        this.minPrimaryValue = minPrimaryValue;
        this.maxPrimaryValue = maxPrimaryValue;

        return this;
    }

    public float GetProbability(int index)
    {
        return (index > 0 && index < amountProbabilities.Count)
            ? amountProbabilities[index]
            : 0f;
    }

    public (float, float) GetSumRange()
    {
        return (minSumValue, maxSumValue);
    }

    public (float, float) GetPrimaryValueRange()
    {
        return (minPrimaryValue, maxPrimaryValue);
    }
}
