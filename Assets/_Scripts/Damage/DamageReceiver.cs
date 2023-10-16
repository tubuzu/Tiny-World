// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public abstract class DamageReceiver : MyMonoBehaviour
{
    [Header("Damage Receiver")]
    [SerializeField] protected int hp = 1;
    [SerializeField] protected int hpMax = 1;
    [SerializeField] protected bool isImmortal = false;
    [SerializeField] protected bool isDead = false;

    public int HP => hp;
    public int HPMax => hpMax;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.Reborn();
    }
    protected override void Reset()
    {
        base.Reset();
        this.Reborn();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    public virtual void Reborn()
    {
        this.hp = this.hpMax;
        this.isDead = false;
    }
    public virtual void Add(int deduct)
    {
        this.hp += deduct;
        if (this.hp > this.hpMax) this.hp = this.hpMax;
        this.OnAdd();
    }
    public virtual void Deduct(int deduct)
    {
        this.hp -= deduct;
        if (this.hp < 0)
        {
            this.hp = 0;
        }
        this.OnDeduct();
        if (this.IsDead())
        {
            this.isDead = true;
            this.OnDead();
        }
    }
    public virtual bool IsDead()
    {
        return this.hp <= 0;
    }
    protected virtual void OnAdd()
    {

    }
    protected virtual void OnDeduct()
    {

    }
    protected virtual void OnDead()
    {
        
    }
}
