using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Alchemy/PotionRecipe")]
public class PotionRecipe : ScriptableObject
{
    [SerializeField] private List<Ingredient> requiredIngredients;
    [SerializeField] private int amountOfIngredients;

    [SerializeField] private List<IngredientEffect> effects;

    [SerializeField] private Potion result;


    public PotionRecipe Init(List<Ingredient> requiredIngredients, int amountOfIngredients, List<IngredientEffect> effects)
    {
        this.requiredIngredients = requiredIngredients;
        this.amountOfIngredients = amountOfIngredients;
        this.effects = effects;

        return this; 
    }

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
                    if (effect.GetEffect() == eff.GetEffect())
                        strength += eff.GetEffectStrength();
                }
            }
            if (strength < effect.GetEffectStrength())
                return false;
        }
        return true;
    }

    public bool CanCraft(List<Ingredient> ingredients)
    {
        return HasIngredients(ingredients) && MeetsEffectRequirements(ingredients);
    }

    public List<IngredientEffect> GetEffects()
    {
        return effects;
    }

    public void SetResult(Potion potion)
    {
        result = potion;
    }
    public Potion GetResult()
    {
        return result;
    }
}
