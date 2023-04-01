#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlchemyGeneratorManager : MonoBehaviour
{
    public List<Effect> effects;
    public List<EffectTypeCombo> effectTypeRules;

    [SerializeField]
    private List<Ingredient> ingredients;

    public static void UpdateIngredientsList()
    {
        FindObjectOfType<AlchemyGeneratorManager>().ingredients = new List<Ingredient>();
        AssetDatabase.Refresh();
        foreach (var ingredient in Resources.FindObjectsOfTypeAll<Ingredient>())
        {
            // Ignores generated ingredients that weren't finished yet
            if (ingredient.GetName() == "" || ingredient.GetName() == null)
                continue;
            FindObjectOfType<AlchemyGeneratorManager>().ingredients.Add(ingredient);
        }
    }

    public List<Ingredient> GetIngredients()
    {
        UpdateIngredientsList();
        return ingredients;
    }
}
#endif
