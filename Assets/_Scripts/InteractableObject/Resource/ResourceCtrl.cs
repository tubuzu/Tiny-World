using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCtrl : InteractableObjectCtrl
{
    [SerializeField] protected ResourceProfile resourceProfile;
    public ResourceProfile ResourceProfile { get => resourceProfile; }
    [SerializeField] protected ResourceDamageReceiver resourceDamageReceiver;
    public ResourceDamageReceiver ResourceDamageReceiver { get => resourceDamageReceiver; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadResourceProfile();
        this.LoadResourceDamageReceiver();
    }
    protected virtual void LoadResourceProfile()
    {
        if (this.resourceProfile != null) return;
        this.resourceProfile = transform.Find("Profile").GetComponent<ResourceProfile>();
    }
    protected virtual void LoadResourceDamageReceiver()
    {
        if (this.resourceDamageReceiver != null) return;
        this.resourceDamageReceiver = transform.Find("DamageReceiver").GetComponent<ResourceDamageReceiver>();
    }
}
