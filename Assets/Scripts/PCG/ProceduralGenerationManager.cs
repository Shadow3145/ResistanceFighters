using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<RaritySettings> raritySettings;

    [Header("Rarity probabilities")]
    [SerializeField] [Range(0f, 1f)] private float rareProbability;
    [SerializeField] [Range(0f, 1f)] private float epicProbability;


    public void GenerateIngredient()
    {
        Rarity rarity = GetRarity();
        int amount = GetAmountOfEffects(rarity);
        List<EffectType> effectTypes = GetEffectTypes(amount);
        Effect primaryEffect = GetPrimaryEffect(effectTypes);
        amount--;
        float strength = GetPrimaryEffectStrenght(rarity);
        IngredientEffect mainEffect = new IngredientEffect(primaryEffect, strength);
        List<IngredientEffect> secondaryEffects = GetSecondaryIngredientEffects(amount, effectTypes, rarity, mainEffect);
        
        // Create Ingredient Scriptable object
    }


    public static void Test()
    {
        Debug.Log("Test");
        Ingredient testIngredient = ScriptableObject.CreateInstance<Ingredient>().Init(42, Rarity.Epic, null, null);
        string path = "Assets/ScriptableObjects/Ingredients/Generated/TestIngredient.asset";
        AssetDatabase.CreateAsset(testIngredient, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void Delete()
    {
        string[] folder = { "Assets/ScriptableObjects/Ingredients/Generated" };
        foreach (var asset in AssetDatabase.FindAssets("", folder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
    }

    private Rarity GetRarity()
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= epicProbability)
            return Rarity.Epic;
        if (rand <= rareProbability)
            return Rarity.Rare;
        return Rarity.Common;
    }

    private int GetAmountOfEffects(Rarity rarity)
    {
        float rand = Random.Range(0f, 1f);
        RaritySettings settings = GetRaritySettings(rarity);


        if (rand < settings.GetProbability(0))
            return 1;
        if (rand < (settings.GetProbability(0) + settings.GetProbability(1)))
            return 2;
        return 3;        
    }

    private EffectType GetRandomEffectType()
    {
        int rand = Random.Range(1, 8);
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
        int rand = Random.Range(0, combos.Count);
        return combos[rand];
    }

    private Effect GetPrimaryEffect(List<EffectType> effectTypes)
    {
        int rand = Random.Range(0, effectTypes.Count);
        EffectType type = effectTypes[rand];

        List<Effect> eff = effects.FindAll(e => e.GetEffectType() == type);
        rand = Random.Range(0, eff.Count);

        return eff[rand];
    }

    private Effect GetEffect(List<EffectType> effectTypes, List<Effect> usedEffects)
    {
        Effect ret;
        while (true)
        {
            int rand = Random.Range(0, effectTypes.Count);
            EffectType type = effectTypes[rand];
            List<Effect> eff = effects.FindAll(e => e.GetEffectType() == type && !usedEffects.Contains(e));
            if (eff.Count > 0)
            {
                rand = Random.Range(0, eff.Count);
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
        float strength = Random.Range(range.min, range.max);
        
        return strength;
    }

    private RaritySettings GetRaritySettings(Rarity rarity)
    {
        return raritySettings[(int)rarity];
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
        float sum = Random.Range(range.min, range.max);

        sum -= primaryStrength;
        if (sum <= 0)
            return values;
        
        while (amount > 1)
        {
            float avg = sum / amount;
            float mod = Random.Range(0f, avg - amount);
            float val = Random.Range(avg - mod, avg + mod);
            values.Add(val);
            sum -= val;
            amount--;
        }
        values.Add(sum);

        return values;
    }
}
