// using System.Collections;
// using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemCtrl : MyMonoBehaviour
{
    [SerializeField] protected Image itemImage;
    public Image ItemImage { get => itemImage; }
    [SerializeField] protected Text itemCount;
    public Text ItemCount { get => itemCount; }
    [SerializeField] protected InventoryItem itemInventory;
    public InventoryItem InventoryItem { get => itemInventory; }
    [SerializeField] protected ItemPickupable itemPickupable;
    public ItemPickupable ItemPickupable { get => itemPickupable; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadImageAndText();
        this.LoadItemPickupable();
    }
    // protected override void Reset()
    // {
    //     base.Reset();
    // this.itemInventory.count = 1;
    // ItemCode itemCode = ItemCodeParser.FromString(transform.name);
    // this.InventoryItem.profile = BaseItemProfile.FindByItemCode(itemCode);
    // }
    protected virtual void LoadImageAndText()
    {
        if (this.itemImage != null) return;
        Transform model = transform.Find("Model");
        this.itemImage = model.GetComponentInChildren<Image>();
        if (this.itemCount != null) return;
        this.itemCount = model.GetComponentInChildren<Text>();
    }
    protected virtual void LoadItemPickupable()
    {
        if (this.itemPickupable != null) return;
        this.itemPickupable = transform.GetComponentInChildren<ItemPickupable>();
    }
    public virtual void SetItemIventory(InventoryItem item)
    {
        // if (this.itemInventory == null)
        //     this.itemInventory = new InventoryItem(item);
        // else
        this.itemInventory.Clone(item);
        this.itemCount.text = item.count.ToString();
    }

    public virtual void SetItemIventory(BaseItemProfileSO profile, int count)
    {
        // if (this.itemInventory == null)
        //     this.itemInventory = new InventoryItem(profile, count, currentLevel);
        // else
        this.itemInventory.Clone(profile, count);
        this.itemCount.text = count.ToString();
    }
    // protected virtual void LoadInventoryItem()
    // {
    //     if (this.itemInventory.profile != null) return;
    //     ItemCode itemCode = ItemCodeParser.FromString(transform.name);
    //     BaseItemProfile itemProfile = BaseItemProfile.FindByItemCode(itemCode);
    //     this.itemInventory.profile = itemProfile;
    //     this.ResetItem();
    // }
}
