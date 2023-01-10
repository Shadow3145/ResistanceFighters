using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Alchemy/Ingredient")]
public class Ingredient : Item
{
    [SerializeField] private Rarity rarity;
    [SerializeField] private IngredientEffect mainEffect;
    [SerializeField] private List<IngredientEffect> secondaryEffects;

    public Ingredient(string name, float price, Sprite icon, Rarity rarity, IngredientEffect main, List<IngredientEffect> effects, float dropProbability)
    {
        this.itemName = name;
        this.price = price;
        this.icon = icon;
        this.rarity = rarity;
        this.mainEffect = main;
        this.secondaryEffects = effects;
        this.dropProbability = dropProbability;
    }

    public Ingredient Init(float price, Rarity rarity, IngredientEffect main, List<IngredientEffect> effects, float dropProbability)
    {
        this.price = price;
        this.rarity = rarity;
        this.mainEffect = main;
        this.secondaryEffects = effects;
        this.dropProbability = dropProbability;

        return this;
    }

    public List<IngredientEffect> GetEffects()
    {
        List<IngredientEffect> effects = new List<IngredientEffect>();
        foreach (IngredientEffect e in secondaryEffects)
            effects.Add(e);
        effects.Add(mainEffect);
        return effects;
    }
}
