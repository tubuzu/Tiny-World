// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ItemLooter : MyMonoBehaviour
{
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected Rigidbody2D _rigidbody;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTrigger();
        this.LoadRigidBody();
    }
    protected virtual void LoadRigidBody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = transform.GetComponent<Rigidbody2D>();
        this._rigidbody.isKinematic = true;
    }
    protected virtual void LoadTrigger()
    {
        if (this._collider != null) return;
        this._collider = transform.GetComponent<Collider2D>();
        this._collider.isTrigger = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        ItemPickupable itemPickupable = collider.GetComponent<ItemPickupable>();
        if (itemPickupable == null) return;

        InventoryItem itemInventory = new InventoryItem(itemPickupable.ItemCtrl.InventoryItem);
        (int remainingToAdd, bool addedSuccessfully) = Inventory.Instance.AddItem(itemInventory.profile, itemInventory.count);
        if (addedSuccessfully)
        {
            itemPickupable.Picked(remainingToAdd);
        }
    }
}
