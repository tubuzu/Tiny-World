using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectAbstract : MyMonoBehaviour
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadController();
    }
    protected abstract void LoadController();
}
