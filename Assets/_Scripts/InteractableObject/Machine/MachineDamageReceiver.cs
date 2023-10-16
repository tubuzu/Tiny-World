using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDamageReceiver : InteractableObjectDamageReceiver
{
    [SerializeField] protected MachineCtrl machineCtrl;
    public MachineCtrl MachineCtrl => machineCtrl;

    protected override void LoadController()
    {
        if (this.machineCtrl != null) return;
        this.machineCtrl = transform.parent.GetComponent<MachineCtrl>();
    }
    protected override void OnDead()
    {
        base.OnDead();
        this.machineCtrl.MachineProfile.OnDeadDropItem();
        FXSpawner.Instance.Spawn(FXName.DestroyEffect.ToString(), transform.position, Quaternion.identity);
        MachineSpawner.Instance.Despawn(this.machineCtrl.gameObject);
    }
}
