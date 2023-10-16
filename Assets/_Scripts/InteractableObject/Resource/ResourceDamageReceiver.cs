using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDamageReceiver : InteractableObjectDamageReceiver
{
    [SerializeField] protected ResourceCtrl resourceCtrl;
    public ResourceCtrl ResourceCtrl => resourceCtrl;
    protected override void LoadController()
    {
        if (this.resourceCtrl != null) return;
        this.resourceCtrl = transform.parent.GetComponent<ResourceCtrl>();
    }
    protected override void OnDead()
    {
        base.OnDead();
        this.resourceCtrl.ResourceProfile.OnDeadDropItem();
        FXSpawner.Instance.Spawn(FXName.DestroyEffect.ToString(), transform.position, Quaternion.identity);
        ResourceSpawner.Instance.Despawn(this.resourceCtrl.gameObject);
    }
}
