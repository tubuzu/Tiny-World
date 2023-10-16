using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class InteractableObjectDamageReceiver : DamageReceiver
{
    [SerializeField] Slider healthBar;

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.gameObject.SetActive(false);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadController();
        this.LoadHealthBar();
    }
    protected abstract void LoadController();
    protected virtual void LoadHealthBar()
    {
        if (this.healthBar != null) return;
        this.healthBar = transform.GetComponentInChildren<Slider>();
    }
    protected override void OnDeduct()
    {
        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);
        healthBar.value = (float)this.hp / this.hpMax;
    }
}
