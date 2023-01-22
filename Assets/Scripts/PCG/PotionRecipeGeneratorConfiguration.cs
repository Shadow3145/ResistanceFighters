using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Generator/RecipeConfiguration")]
public class PotionRecipeGeneratorConfiguration : ScriptableObject
{
    public PotionRaritySettings common;
    public PotionRaritySettings rare;
    public PotionRaritySettings epic;

    public int minStrength;

    public string folderPath;
}
