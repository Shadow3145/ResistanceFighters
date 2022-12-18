using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTable : MonoBehaviour
{
    [SerializeField] private List<PotionRecipe> recipes;

    public static AlchemyTable instance;

    private List<Ingredient> ingredients;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ingredients.Remove(ingredient);
    }


    public void Craft()
    {
        ConsumeIngredients();
        List<Potion> potions = GetCraftablePotions();
        if (potions.Count == 0)
        {
            Debug.Log("This is not a potion.");
            return;
        }

        Potion potion = GetPotionOfHighestRarity(potions);
        int index = ItemManager.instance.GetIndex(potion);
        InventoryItem potionItem = new InventoryItem(index, ItemType.Potion);
        Inventory.instance.AddItem(potionItem);
        //TODO: Add recipe to recipes
    }

    private void ConsumeIngredients()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            int index = ItemManager.instance.GetIndex(ingredient);
            InventoryItem ingredientItem = new InventoryItem(index, ItemType.Ingredient);
            Inventory.instance.ConsumeItem(ingredientItem);
        }
    }

    private List<Potion> GetCraftablePotions()
    {
        List<Potion> potions = new List<Potion>();
        foreach (PotionRecipe recipe in recipes)
        {
            if (recipe.CanCraft(ingredients))
                potions.Add(recipe.GetResult());
        }

        return potions;
    }

    private Potion GetPotionOfHighestRarity(List<Potion> potions)
    {
        Potion potion = null;
        foreach (Potion pot in potions)
        {
            if (potion == null)
            {
                potion = pot;
                continue;
            }

            if (pot.rarity > potion.rarity)
                potion = pot;
        }

        return potion;
    }
}
