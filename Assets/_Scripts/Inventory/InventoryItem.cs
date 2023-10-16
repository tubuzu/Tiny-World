// using System.Collections;
// using System.Collections.Generic;
using System;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

[Serializable]
public class InventoryItem
{
    public BaseItemProfileSO profile;
    public int count;
    public static int MaxCount = 999;
    // public int currentLevel;

    public InventoryItem()
    {
        this.profile = null;
        this.count = 0;
        // this.currentLevel = 0;
    }

    public InventoryItem(BaseItemProfileSO profile = null, int count = 0)
    {
        this.profile = profile;
        this.count = count;
        // this.currentLevel = currentLevel;
    }

    public InventoryItem(InventoryItem item)
    {
        this.profile = item.profile;
        this.count = item.count;
        // this.currentLevel = item.currentLevel;
    }

    public void Clone(InventoryItem item)
    {
        this.profile = item.profile;
        this.count = item.count;
        // this.currentLevel = item.currentLevel;
    }

    public void Clone(BaseItemProfileSO profile, int count = 0)
    {
        this.profile = profile;
        this.count = count;
        // this.currentLevel = currentLevel;
    }

    public void ResetProfile()
    {
        this.profile = null;
        this.count = 0;
        // this.currentLevel = 0;
    }
}
