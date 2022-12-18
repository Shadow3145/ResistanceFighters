using System.Collections.Generic;
using UnityEngine;

public enum Rarity : int
{
    Common = 1,
    Rare = 2,
    Epic = 3
}

[CreateAssetMenu(menuName = "SO/Alchemy/Ingredient")]
public class Ingredient : Item
{
    [SerializeField] private Rarity rarity;
    [SerializeField] private IngredientEffect mainEffect;
    [SerializeField] private List<IngredientEffect> secondaryEffects;

    public Ingredient(string name, float price, Sprite icon, Rarity rarity, IngredientEffect main, List<IngredientEffect> effects)
    {
        this.itemName = name;
        this.price = price;
        this.icon = icon;
        this.rarity = rarity;
        this.mainEffect = main;
        this.secondaryEffects = effects;
    }

    public List<IngredientEffect> GetEffects()
    {
        List<IngredientEffect> effects = secondaryEffects;
        effects.Add(mainEffect);
        return effects;
    }
}
