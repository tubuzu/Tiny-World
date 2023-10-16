// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MyMonoBehaviour
{
    protected static PlayerCtrl instance;
    public static PlayerCtrl Instance => instance;
    [SerializeField] protected Transform model;
    public Transform Model { get => model; }
    [SerializeField] protected PlayerAnimation playerAnimation;
    public PlayerAnimation PlayerAnimation { get => playerAnimation; }
    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement { get => playerMovement; }
    [SerializeField] protected PlayerAction playerAction;
    public PlayerAction PlayerAction { get => playerAction; }
    [SerializeField] protected PlayerDamageSender playerDamageSender;
    public PlayerDamageSender PlayerDamageSender { get => playerDamageSender; }

    protected override void Awake()
    {
        if (PlayerCtrl.instance != null)
        {
            Debug.LogError("Only 1 PlayerCtrl");
            return;
        }
        PlayerCtrl.instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadPlayerAnimation();
        this.LoadPlayerAction();
        this.LoadPlayerMovement();
        this.LoadPlayerDamageSender();
    }
    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model").transform;
    }
    protected virtual void LoadPlayerMovement()
    {
        if (this.playerMovement != null) return;
        this.playerMovement = transform.GetComponentInChildren<PlayerMovement>();
    }
    protected virtual void LoadPlayerAnimation()
    {
        if (this.playerAnimation != null) return;
        this.playerAnimation = transform.GetComponentInChildren<PlayerAnimation>();
    }
    protected virtual void LoadPlayerAction()
    {
        if (this.playerAction != null) return;
        this.playerAction = transform.GetComponentInChildren<PlayerAction>();
    }
    protected virtual void LoadPlayerDamageSender()
    {
        if (this.playerDamageSender != null) return;
        this.playerDamageSender = transform.GetComponentInChildren<PlayerDamageSender>();
    }
}
