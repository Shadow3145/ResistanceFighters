using System.Collections.Generic;
using UnityEngine;

public class PotionRecipe : ScriptableObject
{
    private List<Ingredient> requiredIngredients;
    private int amountOfIngredients;

    private List<IngredientEffect> effects;

    private Potion result;


    private bool HasIngredients(List<Ingredient> ingredients)
    {
        int amount = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            if (requiredIngredients.Contains(ingredient))
                amount++;
        }

        return amount >= amountOfIngredients;
    }

    private bool MeetsEffectRequirements(List<Ingredient> ingredients)
    {
        foreach (IngredientEffect effect in effects)
        {
            float strength = 0;
            foreach (Ingredient ingredient in ingredients)
            {
                foreach (IngredientEffect eff in ingredient.GetEffects())
                {
                    if (effect.effect == eff.effect)
                        strength += eff.effectStrenght;
                }
            }
            if (strength < effect.effectStrenght)
                return false;
        }
        return true;
    }

    public bool CanCraft(List<Ingredient> ingredients)
    {
        return HasIngredients(ingredients) && MeetsEffectRequirements(ingredients);
    }

    public Potion GetResult()
    {
        return result;
    }
}
