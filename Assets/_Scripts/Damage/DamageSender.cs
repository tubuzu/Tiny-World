using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MyMonoBehaviour
{
    [SerializeField] protected int damage = 1;
    public virtual bool Send(Transform obj)
    {
        this.CreateImpactFX();
        DamageReceiver damageReceiver;
        damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver != null)
        {
            this.Send(damageReceiver);
            return true;
        }
        return false;
    }
    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.Deduct(this.damage);
    }
    protected virtual void CreateImpactFX()
    {
        string fxName = this.GetFXName();
        Vector3 hitPos = transform.position;
        Quaternion hitRot = transform.rotation;
        GameObject fxImpact = FXSpawner.Instance.Spawn(fxName, hitPos, hitRot);
        fxImpact?.SetActive(true);
    }
    protected virtual string GetFXName() => "";
    protected virtual void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
