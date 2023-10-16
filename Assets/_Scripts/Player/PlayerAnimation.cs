// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerAbstract
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator itemAnimator;
    // [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private Transform player;
    [SerializeField] private Transform item;

    [SerializeField] private bool lookAtRight = true;

    public void SetPlayerDirection(Vector2 direction)
    {
        bool lookedAtRight = lookAtRight;
        if (direction[0] >= 0) lookAtRight = true;
        else lookAtRight = false;
        if (lookedAtRight != lookAtRight)
        {
            if (lookAtRight)
            {
                item.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                player.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            else
            {
                item.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                player.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
        }
    }
    public void SetIsMoving(bool isMoving)
    {
        playerAnimator.SetBool("IsMoving", isMoving);
    }

    public void UseTool()
    {
        itemAnimator.SetTrigger("UseToolOrWeapon");
    }

    public void ChangeItemSprite(Sprite sprite)
    {
        itemSprite.sprite = sprite;
    }
}
