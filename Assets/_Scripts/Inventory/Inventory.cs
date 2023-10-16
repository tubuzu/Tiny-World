// using System;
// using System.Linq;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Inventory : MyMonoBehaviour
{
    private static Inventory instance;
    public static Inventory Instance => instance;

    [SerializeField] protected int maxInventorySlot = 45;
    public int MaxInventorySlot => maxInventorySlot;
    // [SerializeField] protected int maxToolbarSlot = 9;
    [SerializeField] protected InventoryItem[] inventoryItems;
    public InventoryItem[] InventoryItems => inventoryItems;

    public InventoryItem[] startItems;

    protected BaseItemProfileSO[] allItemProfiles;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null) Debug.LogError("Only 1 Inventory allow to exist");
        instance = this;

        inventoryItems = new InventoryItem[maxInventorySlot];
        
        for (int i = 0; i < maxInventorySlot; i++)
        {
            inventoryItems[i] = new InventoryItem();
        }

        allItemProfiles = Resources.LoadAll<BaseItemProfileSO>("Items");
    }

    protected override void Start()
    {
        base.Start();
        foreach (var item in startItems) AddItem(item.profile, item.count);
    }

    public BaseItemProfileSO GetBaseItemByItemCode(ItemCode itemCode)
    {
        foreach (BaseItemProfileSO item in allItemProfiles)
        {
            if (item.itemCode == itemCode) return item;
        }
        return null;
    }

    public virtual bool CheckHaveEnoughItem(ItemCode itemCode, int itemCount)
    {
        int totalCount = this.ItemTotalCount(itemCode);
        return totalCount >= itemCount;
    }
    public virtual int ItemTotalCount(ItemCode itemCode)
    {
        int totalCount = 0;
        foreach (InventoryItem item in this.inventoryItems)
        {
            if (item.profile == null || item.profile.itemCode != itemCode) continue;
            totalCount += item.count;
        }
        return totalCount;
    }

    public virtual (int, bool) AddItem(BaseItemProfileSO profile, int count)
    {
        int addRemain = count;
        int idx = 0;

        int itemNotFullIdx = GetItemNotFullStack(profile);
        if (itemNotFullIdx >= 0)
        {
            if (inventoryItems[itemNotFullIdx].count + addRemain > InventoryItem.MaxCount)
            {
                addRemain -= InventoryItem.MaxCount - inventoryItems[itemNotFullIdx].count;
                inventoryItems[itemNotFullIdx].count = InventoryItem.MaxCount;
            }
            else
            {
                inventoryItems[itemNotFullIdx].count += addRemain;
                addRemain = 0;
            }

            InventoryUI.Instance.UpdateAtSlotIndex(itemNotFullIdx, inventoryItems[itemNotFullIdx]);
        }
        while (addRemain >= 1 && idx < inventoryItems.Length)
        {
            InventoryItem curItem = inventoryItems[idx];

            if (curItem.profile == null)
            {
                int addCount = profile.stackable ? Mathf.Min(addRemain, InventoryItem.MaxCount) : 1;
                curItem.Clone(profile, addCount);
                addRemain -= addCount;
                InventoryUI.Instance.UpdateAtSlotIndex(idx, curItem);
            }
            idx++;
        }

        if (addRemain < 0) addRemain = 0;

        return (addRemain, addRemain < count);
    }

    protected virtual int GetItemNotFullStack(BaseItemProfileSO profile)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null || inventoryItems[i].profile == null) continue;
            if (profile != inventoryItems[i].profile) continue;
            if (this.IsFullStack(inventoryItems[i])) continue;
            return i;
        }
        return -1;
    }
    protected virtual bool IsFullStack(InventoryItem item)
    {
        return item.count >= InventoryItem.MaxCount && item.profile.stackable;
    }

    protected virtual BaseItemProfileSO GetItemProfile(ItemCode itemCode)
    {
        var profiles = Resources.LoadAll("Item", typeof(BaseItemProfileSO));
        foreach (BaseItemProfileSO profile in profiles)
        {
            if (profile.itemCode != itemCode) continue;
            return profile;
        }
        return null;
    }

    public virtual void DeductItem(ItemCode itemCode, int deductCount)
    {
        for (int i = inventoryItems.Length - 1; i >= 0; i--)
        {
            if (deductCount <= 0) break;

            if (inventoryItems[i].profile == null || inventoryItems[i].profile.itemCode != itemCode) continue;

            if (deductCount >= inventoryItems[i].count)
            {
                deductCount -= inventoryItems[i].count;
                inventoryItems[i].count = 0;
                inventoryItems[i].ResetProfile();
            }
            else
            {
                inventoryItems[i].count -= deductCount;
                deductCount = 0;
            }

            InventoryUI.Instance.UpdateAtSlotIndex(i, inventoryItems[i]);
        }
    }

    public virtual void ItemPickup(ItemPickupable itemPickupable)
    {
        InventoryItem itemInventory = itemPickupable.ItemCtrl.InventoryItem;
        (int remainingToAdd, bool addedSuccessfully) = AddItem(itemInventory.profile, itemInventory.count);

        if (addedSuccessfully)
        {
            itemPickupable.Picked(remainingToAdd);
        }
    }
}
