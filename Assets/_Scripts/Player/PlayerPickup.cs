using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MyMonoBehaviour
{
    [Header("Player Pickup")]
    [SerializeField] protected Collider2D _collider;
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
}
