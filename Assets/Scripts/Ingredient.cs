using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity : int
{
    Common = 1,
    Rare = 2,
    Epic = 3
}

public class Ingredient : Item
{
    private Rarity rarity;
    private IngredientEffect mainEffect;
    private List<IngredientEffect> secondaryEffects;

    public List<IngredientEffect> GetEffects()
    {
        List<IngredientEffect> effects = secondaryEffects;
        effects.Add(mainEffect);
        return effects;
    }
}
