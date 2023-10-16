using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAbstract : InteractableObjectAbstract
{
    [SerializeField] protected MachineCtrl machineCtrl;
    public MachineCtrl MachineCtrl => machineCtrl;
    protected override void LoadController()
    {
        if (this.machineCtrl != null) return;
        this.machineCtrl = transform.parent.GetComponent<MachineCtrl>();
    }
}
