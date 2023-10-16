// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

public class PlayerDamageSender : DamageSender
{
    public virtual void Send(DamageReceiver damageReceiver, int damage)
    {
        damageReceiver.Deduct(damage);
    }
}
