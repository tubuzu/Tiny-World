// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class ResourceAbstract : InteractableObjectAbstract
{
    [SerializeField] protected ResourceCtrl resourceCtrl;
    public ResourceCtrl ResourceCtrl => resourceCtrl;
    protected override void LoadController()
    {
        if (this.resourceCtrl != null) return;
        this.resourceCtrl = transform.parent.GetComponent<ResourceCtrl>();
    }
}
