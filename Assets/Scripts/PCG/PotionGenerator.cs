using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PotionGenerator : MonoBehaviour
{
    private List<Ingredient> ingredients;
    [SerializeField] PotionRecipeGeneratorConfiguration defaultConfig;

    private PotionRecipeGeneratorConfiguration config;

   /* [SerializeField] private List<PotionRaritySettings> raritySettings;
    [SerializeField] private string folderPath;

    private int minStrength = 10;*/

    public void GeneratePotionRecipes(PotionRecipeGeneratorConfiguration config = null)
    {
        if (config == null)
            this.config = defaultConfig;
        else
            this.config = config;
        ingredients = GetComponentInParent<AlchemyGeneratorManager>().GetIngredients();
        foreach (Effect effect in GetComponentInParent<AlchemyGeneratorManager>().effects)
        { 
            for (int i = 0; i < 3; i++)
            {
                PotionRecipe potionRecipe = GeneratePotionRecipe(effect, (Rarity)i);
                if (potionRecipe == null)
                    continue;
                string fileName = GetRarityName((Rarity)i) + effect.GetEffectName() + "PotionRecipe.asset";
                AssetDatabase.CreateAsset(potionRecipe, config.folderPath + "/" + fileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }

    public void Delete()
    {
        string[] folder = { config.folderPath };
        foreach (var asset in AssetDatabase.FindAssets("", folder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
    }

    private PotionRecipe GeneratePotionRecipe(Effect effect, Rarity rarity)
    {
        List<Ingredient> requiredIngredients = GetRequiredIngredients(effect);
        int requiredAmount = GetRequiredAmountOfIngredients(rarity, requiredIngredients.Count);
        if (requiredAmount == -1)
            return null;
        float mainEffectStrength = GetMainEffectStrength(rarity);
        IngredientEffect mainEffect = new IngredientEffect(effect, mainEffectStrength);
        List<IngredientEffect> ingredientEffects = new List<IngredientEffect>();
        ingredientEffects.Add(mainEffect);

        int amountOfEffects = GetAmountOfEffects(rarity);
        if (amountOfEffects == 0)
        {
           return ScriptableObject.CreateInstance<PotionRecipe>().Init(requiredIngredients, requiredAmount, ingredientEffects);
        }

        List<IngredientEffect> secondaryEffects = GetSecondaryEffects(amountOfEffects, requiredIngredients, effect);
        GetPotionSecondaryEffects(rarity, secondaryEffects, ingredientEffects);

        return ScriptableObject.CreateInstance<PotionRecipe>().Init(requiredIngredients, requiredAmount, ingredientEffects); 
    }

    private void GetPotionSecondaryEffects(Rarity rarity, List<IngredientEffect> secondaryEffects, List<IngredientEffect> final)
    {
        foreach (IngredientEffect iEffect in secondaryEffects)
        {
            float strength = GetSecondaryEffectStrength(rarity, iEffect.GetEffectStrength());
            if (strength == -1)
                continue;
            final.Add(new IngredientEffect(iEffect.GetEffect(), strength));
        }
    }

    private List<IngredientEffect> GetSecondaryEffects(int amountOfEffects, List<Ingredient> requiredIngredients, Effect mainEffect)
    {
        List<IngredientEffect> secondaryEffects = new List<IngredientEffect>();
        List<EffectTypeCombo> effectTypeCombos = (GetComponentInParent<AlchemyGeneratorManager>().effectTypeRules).FindAll(er => er.Contains(mainEffect.GetEffectType()));
        List<IngredientEffect> effects = GetSortedEffects(requiredIngredients, mainEffect);

        foreach (IngredientEffect iEffect in effects)
        {
            if (amountOfEffects <= 0)
                break;
            foreach (EffectTypeCombo combo in effectTypeCombos)
            {
                if (combo.Contains(iEffect.GetEffect().GetEffectType()))
                {
                    secondaryEffects.Add(iEffect);
                    amountOfEffects--;
                    break;
                }
            }
        }

        return secondaryEffects;
    }

    private List<IngredientEffect> GetSortedEffects(List<Ingredient> requiredIngredients, Effect mainEffect)
    {
        List<IngredientEffect> effects = new List<IngredientEffect>();
        foreach (Ingredient ingredient in requiredIngredients)
        {
            foreach (IngredientEffect iEffect in ingredient.GetEffects())
            {
                if (iEffect.GetEffect() == mainEffect)
                    continue;
                if (effects.Contains(iEffect))
                {
                    effects[effects.IndexOf(iEffect)].ChangeEffectStrength(iEffect.GetEffectStrength());
                    continue;
                }
                effects.Add(iEffect);
            }
        }
        effects.Sort();
        return effects;
    }

    private List<Ingredient> GetRequiredIngredients(Effect effect)
    {
        List<Ingredient> requiredIngredients = new List<Ingredient>();
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.GetMainEffect().GetEffect() == effect)
                requiredIngredients.Add(ingredient);
            foreach (IngredientEffect iEffect in ingredient.GetSecondaryEffects())
            {
                if (iEffect.GetEffect() == effect && iEffect.GetEffectStrength() >= config.minStrength)
                    requiredIngredients.Add(ingredient);
            }
        }

        return requiredIngredients;
    }

    private PotionRaritySettings GetRaritySettings(Rarity rarity)
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

    private int GetRequiredAmountOfIngredients(Rarity rarity, int ingredientsCount)
    {
        Range range = GetRaritySettings(rarity).GetIngredientAmount();
        if (ingredientsCount < range.minValue)
            return -1;
        int amount = Random.Range(range.minValue, (Mathf.Min((range.maxValue + 1), ingredientsCount)));

        return amount;
    }

    private float GetMainEffectStrength(Rarity rarity)
    {
        Range range = GetRaritySettings(rarity).GetMainEffectStrength();
        float strength = Random.Range(range.minValue, range.maxValue);

        return strength;
    }

    private float GetSecondaryEffectStrength(Rarity rarity, float maxVal)
    {
        Range range = GetRaritySettings(rarity).GetSecondaryEffectStrength();
        if (maxVal < range.minValue)
            return -1;
        float strength = Random.Range(range.minValue, Mathf.Max(range.maxValue, maxVal));

        return Mathf.Round(strength);
    }

    private int GetAmountOfEffects(Rarity rarity)
    {
        PotionRaritySettings settings = GetRaritySettings(rarity);
        float rand = Random.Range(0f, 1f);
        if (rand < settings.GetAmountOfEffects(0))
            return 0;
        if (rand < (settings.GetAmountOfEffects(0) + settings.GetAmountOfEffects(1)))
            return 1;
        return 2;
    }

    private string GetRarityName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "Weak";
            case Rarity.Rare:
                return "Medium";
            case Rarity.Epic:
                return "Strong";
        }

        return "";
    }
}