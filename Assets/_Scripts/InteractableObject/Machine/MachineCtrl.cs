using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCtrl : InteractableObjectCtrl
{
    [SerializeField] protected MachineProfile machineProfile;
    public MachineProfile MachineProfile { get => machineProfile; }
    [SerializeField] protected MachineDamageReceiver machineDamageReceiver;
    public MachineDamageReceiver MachineDamageReceiver { get => machineDamageReceiver; }
    [SerializeField] protected MouseTarget mouseTarget;
    public MouseTarget MouseTarget { get => mouseTarget; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMachineProfile();
        this.LoadMachineDamageReceiver();
        this.LoadMouseTarget();
    }
    protected virtual void LoadMachineProfile()
    {
        if (this.machineProfile != null) return;
        this.machineProfile = transform.Find("Profile").GetComponent<MachineProfile>();
    }
    protected virtual void LoadMachineDamageReceiver()
    {
        if (this.machineDamageReceiver != null) return;
        this.machineDamageReceiver = transform.Find("DamageReceiver").GetComponent<MachineDamageReceiver>();
    }
    protected virtual void LoadMouseTarget()
    {
        if (this.mouseTarget != null) return;
        this.mouseTarget = transform.Find("MouseTarget").GetComponent<MouseTarget>();
    }
}
