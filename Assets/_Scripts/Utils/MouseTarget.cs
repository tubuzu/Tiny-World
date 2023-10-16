using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MouseTarget : MyMonoBehaviour
{
    [SerializeField] protected Collider2D targetCollider;
    public Collider2D TargetCollider { get => targetCollider; }
    public bool shouldFocus = false;
    public bool isUIComponent = false;
    public bool displayInteractButton = false;

    public ActionType actionType = ActionType.None;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTargetCollider();
    }
    protected virtual void LoadTargetCollider()
    {
        if (this.targetCollider != null) return;
        this.targetCollider = transform.GetComponent<Collider2D>();
    }
}
