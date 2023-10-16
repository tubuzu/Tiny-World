using System;
using System.Collections;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickupable : ItemAbstract
{
    [Header("Item Pickupable")]
    [SerializeField] protected Collider2D _collider;

    [SerializeField] protected float delayBeforePickupable = 0.5f;

    private bool pickupable = false;
    private bool picked = false;

    public static ItemCode String2ItemCode(string itemName)
    {
        try
        {
            return (ItemCode)System.Enum.Parse(typeof(ItemCode), itemName, true);
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.ToString());
            return ItemCode.NoItem;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.pickupable = false;
        this.picked = false;

        Invoke(nameof(SetPickupable), delayBeforePickupable);
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTrigger();
    }
    protected virtual void LoadTrigger()
    {
        if (this._collider != null) return;
        this._collider = transform.GetComponent<Collider2D>();
        this._collider.isTrigger = true;
    }
    public virtual ItemCode GetItemCode()
    {
        return String2ItemCode(transform.parent.name);
    }
    public virtual void Picked(int remain)
    {
        if (remain == 0)
        {
            picked = true;
            StartCoroutine(MoveToBagAndDespawn());
        }
        else this.itemCtrl.InventoryItem.count = remain;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!pickupable || picked) return;
        if (other.gameObject.GetComponent<PlayerPickup>() != null)
        {
            Inventory.Instance.ItemPickup(this);
        }
    }

    // private void OnMouseEnter()
    // {
    //     if (!pickupable || picked) return;
    //     Inventory.Instance.ItemPickup(this);
    // }

    public void Pick()
    {
        if (!pickupable || picked) return;
        Inventory.Instance.ItemPickup(this);
    }

    protected void SetPickupable()
    {
        pickupable = true;
    }

    private IEnumerator MoveToBagAndDespawn()
    {
        float duration = .5f; // Thời gian di chuyển về túi
        float elapsedTime = 0f;
        Vector3 startPos = itemCtrl.transform.position;
        Vector3 endPos = InventoryUI.Instance.openButton.transform.position;

        while (elapsedTime < duration)
        {
            itemCtrl.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ItemDropSpawner.Instance.Despawn(this.itemCtrl.gameObject);
    }
}
