using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class EffectTypeCombo
{
    [SerializeField] private List<EffectType> effects;

    public int GetSize()
    {
        return effects.Count;
    }

    public List<EffectType> GetEffectTypes()
    {
        return effects;
    }
}
public class ProceduralGenerationManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private List<Effect> effects;
    [SerializeField] private List<EffectTypeCombo> effectTypeRules;
    [SerializeField] private IngredientGeneratorConfiguration defaultConfig;
    [SerializeField] private PriceSettings priceSettings;

    private IngredientGeneratorConfiguration config;
    private int amountOfGenerated;

    public void GenerateIngredients(IngredientGeneratorConfiguration config)
    {
        this.config = config;
        string[] folder = { config.folderPath };
        amountOfGenerated = AssetDatabase.FindAssets("", folder).Length;
        
        for (int i = 0; i < config.amountToGenerate; i++)
        {
            Ingredient ingredient = GenerateIngredient();
            string fileName = "Ingredient_" + (i + amountOfGenerated).ToString();
            AssetDatabase.CreateAsset(ingredient, config.folderPath + "/" + fileName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private Ingredient GenerateIngredient()
    {
        Rarity rarity = GetRarity();
        int amount = GetAmountOfEffects(rarity);
        List<EffectType> effectTypes = GetEffectTypes(amount);
        Effect primaryEffect = GetPrimaryEffect(effectTypes);
        amount--;
        float strength = GetPrimaryEffectStrenght(rarity);
        IngredientEffect mainEffect = new IngredientEffect(primaryEffect, strength);
        List<IngredientEffect> secondaryEffects = GetSecondaryIngredientEffects(amount, effectTypes, rarity, mainEffect);
        float price = GetPrice(rarity, mainEffect, secondaryEffects);

        return ScriptableObject.CreateInstance<Ingredient>().Init(price, rarity, mainEffect, secondaryEffects, 0.5f);
    }

    public static void Delete(string folderPath)
    {
        string[] folder = { folderPath };
        foreach (var asset in AssetDatabase.FindAssets("", folder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
    }

    public static void DeleteDefault()
    {
        Delete("Assets/ScriptableObjects/Ingredients/Generated");
    }


    private Rarity GetRarity()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand <= config.epicProbability)
            return Rarity.Epic;
        if (rand <= config.rareProbability + config.epicProbability)
            return Rarity.Rare;
        return Rarity.Common;
    }

    private int GetAmountOfEffects(Rarity rarity)
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        RaritySettings settings = GetRaritySettings(rarity);


        if (rand < settings.GetProbability(0))
            return 1;
        if (rand < (settings.GetProbability(0) + settings.GetProbability(1)))
            return 2;
        return 3;        
    }

    private EffectType GetRandomEffectType()
    {
        int rand = UnityEngine.Random.Range(1, 8);
        return (EffectType)rand;
    }

    private List<EffectType> GetEffectTypes(int amount)
    {
        List<EffectType> effectTypes = new List<EffectType>();
        if (amount == 1)
        {
            effectTypes.Add(GetRandomEffectType());
            return effectTypes;
        }

        return GetComboOfSize(amount).GetEffectTypes();        
    }

    private EffectTypeCombo GetComboOfSize(int size)
    {
        List<EffectTypeCombo> combos = new List<EffectTypeCombo>();
        foreach (EffectTypeCombo etc in effectTypeRules)
        {
            if (etc.GetSize() == size)
                combos.Add(etc);
        }
        int rand = UnityEngine.Random.Range(0, combos.Count);
        return combos[rand];
    }

    private Effect GetPrimaryEffect(List<EffectType> effectTypes)
    {
        int rand = UnityEngine.Random.Range(0, effectTypes.Count);
        EffectType type = effectTypes[rand];

        List<Effect> eff = effects.FindAll(e => e.GetEffectType() == type);
        rand = UnityEngine.Random.Range(0, eff.Count);

        return eff[rand];
    }

    private Effect GetEffect(List<EffectType> effectTypes, List<Effect> usedEffects)
    {
        Effect ret;
        while (true)
        {
            int rand = UnityEngine.Random.Range(0, effectTypes.Count);
            EffectType type = effectTypes[rand];
            List<Effect> eff = effects.FindAll(e => e.GetEffectType() == type && !usedEffects.Contains(e));
            if (eff.Count > 0)
            {
                rand = UnityEngine.Random.Range(0, eff.Count);
                ret = eff[rand];
                break;
            }
        }

        return ret;        
    }

    private float GetPrimaryEffectStrenght(Rarity rarity)
    {
        RaritySettings raritySettings = GetRaritySettings(rarity);
        (float min, float max) range = raritySettings.GetPrimaryValueRange();
        float strength = UnityEngine.Random.Range(range.min, range.max);

        return (float)Math.Round(strength, 1);
    }

    private RaritySettings GetRaritySettings(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return config.common;
            case Rarity.Rare:
                return config.rare;
            case Rarity.Epic:
                return config.epic;
            default:
                return null;
        }
    }

    private List<IngredientEffect> GetSecondaryIngredientEffects(int amount, List<EffectType> effectTypes, Rarity rarity, IngredientEffect primary)
    {
        List<IngredientEffect> ingredientEffects = new List<IngredientEffect>();
        List<Effect> secondaryEffects = GetSecondaryEffects(amount, effectTypes, primary.GetEffect());
        List<float> secondaryValues = GetSecondaryEffectValues(amount, primary.GetEffectStrength(), rarity);
        if (secondaryValues.Count == 0)
            return ingredientEffects;

        for (int i = 0; i < amount; i++)
        {
            IngredientEffect eff = new IngredientEffect(secondaryEffects[i], secondaryValues[i]);
            ingredientEffects.Add(eff);
        }

        return ingredientEffects;
    }

    private List<Effect> GetSecondaryEffects(int amount, List<EffectType> effectTypes, Effect primaryEffect)
    {
        List<Effect> usedEffects = new List<Effect>();
        usedEffects.Add(primaryEffect);

        for (int i = 0; i < amount; i++)
        {
            Effect e = GetEffect(effectTypes, usedEffects);
            usedEffects.Add(e);
        }
        usedEffects.Remove(primaryEffect);
        return usedEffects;
    }

    private List<float> GetSecondaryEffectValues(int amount, float primaryStrength, Rarity rarity)
    {
        List<float> values = new List<float>();
        
        RaritySettings raritySettings = GetRaritySettings(rarity);
        (float min, float max) range = raritySettings.GetSumRange();
        float sum = UnityEngine.Random.Range(range.min, range.max);

        sum -= primaryStrength;
        if (sum <= 0)
            return values;
        
        while (amount > 1)
        {
            float avg = sum / amount;
            float mod = UnityEngine.Random.Range(0f, avg - amount);
            float val = UnityEngine.Random.Range(avg - mod, avg + mod);
            values.Add((float)Math.Round(val, 1));
            sum -= val;
            amount--;
        }
        values.Add((float)Math.Round(sum, 1));

        return values;
    }

    private float GetPrice(Rarity rarity, IngredientEffect main, List<IngredientEffect> secondary)
    {
        float price = priceSettings.GetRarityPrice((int)rarity);
        price += main.GetEffectStrength() * priceSettings.GetPrimaryMod();
        foreach (IngredientEffect ie in secondary)
        {
            price += ie.GetEffectStrength() * priceSettings.GetSecondaryMod() + priceSettings.GetAmountMod();
        }

        return (float) Math.Round(price, 3);
    }
}
