// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class ItemDropSpawner : Spawner
{
    private static ItemDropSpawner instance;
    public static ItemDropSpawner Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Debug.LogError("Only 1 ItemDropSpawner allow to exist!");
        instance = this;
    }

    

    public virtual bool DropItem(BaseItemProfileSO profile, int count, Vector3 pos, Quaternion rot)
    {
        if (profile == null || count <= 0) return false;
        while (count > 0)
        {
            GameObject itemObject = Spawn(profile.itemName, pos, rot);
            ItemCtrl itemCtrl = itemObject.GetComponent<ItemCtrl>();
            if (itemCtrl != null)
            {
                itemCtrl.SetItemIventory(profile, Mathf.Min(count, InventoryItem.MaxCount));
                count -= Mathf.Min(count, InventoryItem.MaxCount);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    // TODO: sử lý với trường hợp drop item ko phải base
    public virtual bool DropItemFromInventory(InventoryItem item, Vector3 pos, Quaternion rot)
    {
        if (item == null) return false;

        GameObject itemDrop = this.Spawn(item.profile.itemName, pos, rot);
        if (itemDrop == null) return false;

        itemDrop.GetComponent<ItemCtrl>().SetItemIventory(item);

        return true;
    }
}
