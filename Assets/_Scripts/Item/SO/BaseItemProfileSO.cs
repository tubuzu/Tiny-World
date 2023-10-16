// using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]


[CreateAssetMenu(fileName = "BaseItemProfileSO", menuName = "SO/BaseItemProfile")]
public class BaseItemProfileSO : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemCode itemCode = ItemCode.NoItem;
    public ItemType itemType = ItemType.NoType;
    public string itemName = "no_name";
    public int maxCount = 999;
    public bool unlocked = false;

    [Header("Item Craft")]
    public ItemRecipe itemRecipe;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite sprite;

    public static BaseItemProfileSO FindByItemCode(ItemCode itemCode)
    {
        var profiles = Resources.LoadAll("Item", typeof(BaseItemProfileSO));
        foreach (BaseItemProfileSO profile in profiles)
        {
            if (profile.itemCode != itemCode) continue;
            return profile;
        }
        return null;
    }
}
