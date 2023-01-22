using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/Generator/DropProbabilitySettings")]
public class DropProbabilitySettings : ScriptableObject
{
    [SerializeField] [Range(0f,1f)] private List<float> baseProbability;

    [SerializeField] [Range(0f, 1f)] private float amountOfEffectsMod;
    [SerializeField] [Range(0f, 1f)] private float primaryEffectMod;
    [SerializeField] [Range(0f, 1f)] private float secondaryEffectMod;

    public float GetBaseProbability(int index)
    {
        return baseProbability[index];
    }

    public float GetAmountMod()
    {
        return amountOfEffectsMod;
    }

    public float GetPrimaryMod()
    {
        return primaryEffectMod;
    }

    public float GetSecondaryMod()
    {
        return secondaryEffectMod;
    }
}
