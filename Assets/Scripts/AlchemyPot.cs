using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlchemyPot : MonoBehaviour
{
    [SerializeField] private List<PotionRecipe> recipes; 

    public static AlchemyPot instance;

    private List<Ingredient> ingredients;

    private IngredientSlot[] slots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ingredients = new List<Ingredient>();
        }
    }

    private void Start()
    {
        slots = FindObjectsOfType<IngredientSlot>();
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
        if (ingredients.Count == 0)
            return;
        ConsumeIngredients();
        List<Potion> potions = GetCraftablePotions();
        ClearTable();
        if (potions.Count == 0)
        {
            CraftingMessage.instance.Show();
            return;
        }
        Potion potion = GetPotionOfHighestRarity(potions);
        int index = ItemManager.instance.GetIndex(potion);
        InventoryItem potionItem = new InventoryItem(index, ItemType.Potion);
        CraftingMessage.instance.Show(potionItem);
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

            if (pot.GetRarity() > potion.GetRarity())
                potion = pot;
        }

        return potion;
    }

    private void ClearTable()
    {
        ingredients.Clear();

        foreach (IngredientSlot slot in slots)
            slot.Reset();
    }
}
