// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Tilemaps;

public class PlayerAction : PlayerAbstract
{
    public delegate void LateActionHandler();
    public event LateActionHandler lateAction;

    protected bool canAction = true;
    protected bool canActionLeftMouse = true;
    protected bool canActionRightMouse = true;

    public float mouseClickDelay = 0.1f;

    public void HandleUpdate()
    {
        if (!canAction || InventoryUI.Instance.isOpen) return;

        lateAction?.Invoke();

        if (Input.GetMouseButton(0) && canActionLeftMouse)
            HandleLeftMouseClick();
        if (Input.GetMouseButton(1) && canActionRightMouse)
            HandleRightMouseClick();
    }

    public void HandleRightMouseClick()
    {
        ResourceCtrl resourceCtrl = PlayerInput.Instance.GetMouseTargetObject()?.GetComponent<ResourceCtrl>();
        if (resourceCtrl != null)
        {
            ResourceProfile resourceProfile = resourceCtrl.ResourceProfile;

            if (resourceCtrl != null && resourceProfile.canBeHarvest)
            {
                playerCtrl.PlayerDamageSender.Send(resourceCtrl.ResourceDamageReceiver, resourceCtrl.ResourceDamageReceiver.HPMax);
                StartActionRightMouseCoolDown();
            }
        }
    }

    public void HandleLeftMouseClick()
    {
        BaseItemProfileSO selectedItemProfile = InventoryUI.Instance.GetSelectedItem();

        if (selectedItemProfile == null || selectedItemProfile is not InteractableItemProfileSO) return;
        InteractableItemProfileSO interactableItem = (InteractableItemProfileSO)selectedItemProfile;

        Vector3 mousePos = PlayerInput.Instance.GetMousePosition();
        Vector3Int mousePosInt = new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), 0);
        Vector3Int playerPos = new Vector3Int(Mathf.FloorToInt(playerCtrl.transform.position.x), Mathf.FloorToInt(playerCtrl.transform.position.y), Mathf.FloorToInt(playerCtrl.transform.position.z));
        Vector3Int playerToMouse = mousePosInt - playerPos;

        if (playerToMouse.x > interactableItem.range.x || playerToMouse.y > interactableItem.range.y) return;

        if (interactableItem is ToolItemProfileSO)
        {
            HandleToolAction(interactableItem, mousePosInt);
        }
        else if (interactableItem is PlaceableItemProfileSO)
        {
            HandlePlaceableItemAction(interactableItem, mousePosInt);
        }
    }

    protected void HandlePlaceableItemAction(InteractableItemProfileSO interactableItem, Vector3Int mousePosInt)
    {
        PlaceableItemProfileSO placeableItem = (PlaceableItemProfileSO)interactableItem;

        switch (interactableItem.actionType)
        {
            case ActionType.Seeding:
                if (placeableItem is not SeedItemProfileSO) break;
                SeedItemProfileSO seedItem = (SeedItemProfileSO)placeableItem;
                if (TileManager.Instance.CanSeedAt(mousePosInt, seedItem.groundType))
                {
                    if (!Inventory.Instance.CheckHaveEnoughItem(seedItem.itemCode, 1)) break;
                    Vector3 cellCenter = new Vector3(mousePosInt.x + .5f, mousePosInt.y + .5f, 0f);
                    ResourceSpawner.Instance.Spawn(seedItem.resourceName, cellCenter, Quaternion.identity);
                    Inventory.Instance.DeductItem(seedItem.itemCode, 1);
                    PlayerInput.Instance.TogglePreviewCanvas(false);
                    StartActionLeftMouseCoolDown();
                }
                break;
            default: break;
        }
    }

    protected void HandleToolAction(InteractableItemProfileSO interactableItem, Vector3Int mousePosInt)
    {
        ToolItemProfileSO toolProfile = (ToolItemProfileSO)interactableItem;
        GameObject mouseTarget = PlayerInput.Instance.GetMouseTargetObject();

        switch (interactableItem.actionType)
        {
            case ActionType.Dig:
                if (mouseTarget != null) break;
                if (TileManager.Instance.TillGroundAtPos(mousePosInt, true))
                    StartCoolDown(toolProfile.cooldown);
                break;
            case ActionType.Watering:
                if (TileManager.Instance.WaterGroundAtPos(mousePosInt, toolProfile.targetSize))
                    StartCoolDown(toolProfile.cooldown);
                break;
            case ActionType.Chop:
                if (mouseTarget != null)
                {
                    ResourceCtrl resourceCtrl = mouseTarget.GetComponent<ResourceCtrl>();
                    if (resourceCtrl == null) break;
                    ResourceProfile resourceProfile = resourceCtrl.ResourceProfile;

                    if (resourceCtrl != null && !resourceProfile.canBeHarvest && resourceProfile.destroyAction == ActionType.Chop)
                    {
                        playerCtrl.PlayerDamageSender.Send(resourceCtrl.ResourceDamageReceiver, toolProfile.damage);
                        StartCoolDown(toolProfile.cooldown);
                    }
                }
                break;
            case ActionType.Mine:
                if (mouseTarget != null)
                {
                    ResourceCtrl resourceCtrl = mouseTarget.GetComponent<ResourceCtrl>();
                    if (resourceCtrl == null) break;
                    ResourceProfile resourceProfile = resourceCtrl.ResourceProfile;

                    if (resourceCtrl != null && !resourceProfile.canBeHarvest && resourceProfile.destroyAction == ActionType.Mine)
                    {
                        playerCtrl.PlayerDamageSender.Send(resourceCtrl.ResourceDamageReceiver, toolProfile.damage);
                        StartCoolDown(toolProfile.cooldown);
                    }
                }
                break;
            default: break;
        }
    }

    protected void StartCoolDown(float cd)
    {
        canAction = false;
        playerCtrl.PlayerAnimation.UseTool();
        Invoke(nameof(ResetActionCoolDown), cd);
    }

    protected void StartActionLeftMouseCoolDown()
    {
        canActionLeftMouse = false;
        Invoke(nameof(ResetActionLeftMouseCoolDown), mouseClickDelay);
    }
    protected void StartActionRightMouseCoolDown()
    {
        canActionRightMouse = false;
        Invoke(nameof(ResetActionRightMouseCoolDown), mouseClickDelay);
    }
    protected void ResetActionCoolDown() => canAction = true;
    protected void ResetActionLeftMouseCoolDown() => canActionLeftMouse = true;
    protected void ResetActionRightMouseCoolDown() => canActionRightMouse = true;

    public void ChangeSelectedItem()
    {
        BaseItemProfileSO basicItem = InventoryUI.Instance.GetSelectedItem();
        if (basicItem != null && (basicItem.itemType == ItemType.Tool || basicItem.itemType == ItemType.Weapon))
        {
            playerCtrl.PlayerAnimation.ChangeItemSprite(basicItem.sprite);
        }
        else playerCtrl.PlayerAnimation.ChangeItemSprite(null);
        this.lateAction -= ChangeSelectedItem;
    }
}
