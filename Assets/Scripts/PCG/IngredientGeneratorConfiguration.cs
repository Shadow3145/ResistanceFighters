using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName =  "SO/Generator/Configuration")]
public class IngredientGeneratorConfiguration : ScriptableObject
{
    public RaritySettings common;
    public RaritySettings rare;
    public RaritySettings epic;

    public int amountToGenerate;

    [Range(0, 1)] public float rareProbability;
    [Range(0,1)] public float epicProbability;
}
