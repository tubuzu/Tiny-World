using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemRecipe
{
    public List<ItemRecipeIngredient> ingredients;
    public int timeNeeded = 1;
}
