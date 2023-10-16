// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class ItemCraft : MyMonoBehaviour
{
    private static ItemCraft instance;
    public static ItemCraft Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null) Debug.LogError("Only 1 ItemCraft allow to exist");
        instance = this;
    }

    public virtual bool CraftItem(ItemCode itemCode, int count = 1, bool addToInventoryAfterCrafted = true)
    {
        BaseItemProfileSO baseItem = Inventory.Instance.GetBaseItemByItemCode(itemCode);

        return CraftItem(baseItem, count, addToInventoryAfterCrafted);
    }

    public virtual bool CraftItem(BaseItemProfileSO profile, int count = 1, bool addToInventoryAfterCrafted = true)
    {
        ItemRecipe itemRecipe = profile.itemRecipe;
        if (!HaveEnoughIngredients(itemRecipe, count)) return false;

        DeductIngredients(itemRecipe, count);

        if (addToInventoryAfterCrafted)
            Inventory.Instance.AddItem(profile, count);

        return true;
    }

    protected virtual bool ItemCraftable(ItemRecipe itemRecipe)
    {
        if (itemRecipe.ingredients.Count == 0) return false;
        return true;
    }
    protected virtual bool HaveEnoughIngredients(ItemRecipe itemRecipe, int count = 1)
    {
        if (!ItemCraftable(itemRecipe))
        {
            Debug.Log("Item can not be craft!");
            return false;
        }

        foreach (ItemRecipeIngredient ingredient in itemRecipe.ingredients)
        {
            ItemCode itemCode = ingredient.itemProfile.itemCode;
            int itemCount = ingredient.count * count;
            if (!Inventory.Instance.CheckHaveEnoughItem(itemCode, itemCount)) return false;
        }
        return true;
    }
    protected virtual void DeductIngredients(ItemRecipe itemRecipe, int count = 1)
    {
        foreach (ItemRecipeIngredient ingredient in itemRecipe.ingredients)
        {
            ItemCode itemCode = ingredient.itemProfile.itemCode;
            int itemCount = ingredient.count * count;
            Inventory.Instance.DeductItem(itemCode, itemCount);
        }
    }
}
