using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PotionGenerator : MonoBehaviour
{
    private List<Ingredient> ingredients;
    [SerializeField] private List<PotionRaritySettings> raritySettings;
    [SerializeField] private string folderPath;

    public void GeneratePotionRecipes()
    {
        ingredients = GetComponentInParent<AlchemyGeneratorManager>().GetIngredients();
        foreach (Effect effect in GetComponentInParent<AlchemyGeneratorManager>().effects)
        { 
            for (int i = 0; i < 3; i++)
            {
                PotionRecipe potionRecipe = GeneratePotionRecipe(effect, (Rarity)i);
                string fileName = GetRarityName((Rarity)i) + effect.GetEffectName() + "PotionRecipe.asset";
                AssetDatabase.CreateAsset(potionRecipe, folderPath + "/" + fileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
    }

    private PotionRecipe GeneratePotionRecipe(Effect effect, Rarity rarity)
    {
        List<Ingredient> requiredIngredients = GetRequiredIngredients(effect);
        int requiredAmount = GetRequiredAmountOfIngredients(rarity);
        float mainEffectStrength = GetMainEffectStrength(rarity);
        IngredientEffect mainEffect = new IngredientEffect(effect, mainEffectStrength);
        List<IngredientEffect> ingredientEffects = new List<IngredientEffect>();
        ingredientEffects.Add(mainEffect);

        int amountOfEffects = GetAmountOfEffects(rarity);
        if (amountOfEffects == 0)
        {
           return ScriptableObject.CreateInstance<PotionRecipe>().Init(requiredIngredients, requiredAmount, ingredientEffects);
        }

        return null; 
    }

    private List<Effect> GetSecondaryEffects(int amountOfEffects, List<Ingredient> requiredIngredients, Effect mainEffect)
    {
        List<Effect> secondaryEffects = new List<Effect>();
        List<EffectTypeCombo> effectTypeCombos = (GetComponentInParent<AlchemyGeneratorManager>().effectTypeRules).FindAll(er => er.Contains(mainEffect.GetEffectType()));
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

        return secondaryEffects;
    }

    private List<Ingredient> GetRequiredIngredients(Effect effect)
    {
        List<Ingredient> requiredIngredients = new List<Ingredient>();
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.GetMainEffect().GetEffect() == effect)
                requiredIngredients.Add(ingredient);
        }

        return requiredIngredients;
    }

    private int GetRequiredAmountOfIngredients(Rarity rarity)
    {
        Range range = raritySettings[(int)rarity].GetIngredientAmount();
        int amount = Random.Range(range.minValue, range.maxValue + 1);

        return amount;
    }

    private float GetMainEffectStrength(Rarity rarity)
    {
        Range range = raritySettings[(int)rarity].GetMainEffectStrength();
        float strength = Random.Range(range.minValue, range.maxValue);

        return strength;
    }

    private int GetAmountOfEffects(Rarity rarity)
    {
        PotionRaritySettings settings = raritySettings[(int)rarity];
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