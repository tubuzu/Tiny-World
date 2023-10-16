// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerAbstract
{
    [SerializeField] protected float moveSpeed = 5f;
    public bool IsMoving { get; private set; }

    private Rigidbody2D _rb;

    protected override void Awake()
    {
        base.Awake();
        this._rb = this.playerCtrl.GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveVec)
    {
        if (moveVec == Vector2.zero)
        {
            IsMoving = false;
            _rb.velocity = Vector2.zero;
        }
        else
        {
            IsMoving = true;
            _rb.velocity = moveVec.normalized * moveSpeed;
            playerCtrl.PlayerAnimation.SetPlayerDirection(moveVec);
        }

        playerCtrl.PlayerAnimation.SetIsMoving(IsMoving);
    }
}
