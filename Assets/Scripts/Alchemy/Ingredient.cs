using System.Collections.Generic;
using UnityEngine;
using System.Text;

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

    public List<IngredientEffect> GetSecondaryEffects()
    {
        return secondaryEffects;
    }

    public List<IngredientEffect> GetEffects()
    {
        List<IngredientEffect> effects = new List<IngredientEffect>();
        effects.Add(mainEffect);
        foreach (IngredientEffect e in secondaryEffects)
            effects.Add(e);
        return effects;
    }

    public IngredientEffect GetMainEffect()
    {
        return mainEffect;
    }

    public Rarity GetRarity()
    {
        return rarity;
    }

    public void SetDescription()
    {
        StringBuilder description = new StringBuilder();
        description.AppendLine("<b>Main Effect:</b>");
        description.AppendLine(mainEffect.GetEffect().GetEffectDescription());
        if (secondaryEffects == null || secondaryEffects.Count == 0)
        {
            this.description = description.ToString();
            return;
        }
        description.AppendLine("<b>Secondary Effects:</b>");
        foreach (IngredientEffect effect in secondaryEffects)
        {
            description.AppendLine(effect.GetEffect().GetEffectDescription());
        }

        this.description = description.ToString();
    }
}
