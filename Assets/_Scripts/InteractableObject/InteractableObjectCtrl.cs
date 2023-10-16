using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectCtrl : MyMonoBehaviour
{
    [SerializeField] protected Transform model;
    public Transform Model { get => model; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
    }
    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
    }
}
