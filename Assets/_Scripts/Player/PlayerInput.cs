// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : PlayerAbstract
{
    protected static PlayerInput instance;
    public static PlayerInput Instance => instance;

    private Vector2 input;

    public SpriteRenderer tileSelection;
    public GameObject EInteractButton;
    public GameObject previewCanvas;
    public Image previewImage;

    [SerializeField] protected MouseTarget target;
    protected Vector2Int targetSize;
    public MouseTarget Target { get => target; set => target = value; }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only 1");
            return;
        }
        instance = this;

        this.previewImage = previewCanvas.GetComponentInChildren<Image>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1)) EditorApplication.isPaused = !EditorApplication.isPaused;
#endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseCurrentUI();
        }

        if (UIManager.Instance.curOpenUI != UIType.None) return;

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        playerCtrl.PlayerMovement.Move(input);

        playerCtrl.PlayerAction.HandleUpdate();

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= 9)
            {
                InventoryUI.Instance.ChangeSelectedSlot(number - 1);
                playerCtrl.PlayerAction.lateAction += playerCtrl.PlayerAction.ChangeSelectedItem;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
    }

    private void FixedUpdate()
    {
        HandleMouseInteraction();
    }

    public void InteractWithObject()
    {
        // GameObject target = GetMouseTargetObject(true);
        GameObject target = this.target.transform.parent.gameObject;
        InteractableObjectCtrl ctrl = target.GetComponent<InteractableObjectCtrl>();
        if (ctrl is MachineCtrl machineCtrl)
        {
            machineCtrl.MachineProfile.OpenMachineUI();
        }
    }

    public void SetTileSelection(Vector3 position, int xSize = 1, int ySize = 1)
    {
        tileSelection.transform.position = new Vector3(Mathf.FloorToInt(position.x + 1), Mathf.FloorToInt(position.y), 0);
        tileSelection.size = new Vector2(xSize, ySize);
        tileSelection.gameObject.SetActive(true);
    }

    public void ToggleTileSelection(bool show)
    {
        tileSelection.gameObject.SetActive(show);
    }

    public void SetDisplayInteractButton(bool show, Vector3? position = null)
    {
        if (show)
            EInteractButton.transform.position = (Vector3)position;
        EInteractButton.SetActive(show);
    }

    protected void HandleMouseInteraction()
    {
        BaseItemProfileSO baseItem = InventoryUI.Instance.GetSelectedItem();
        InteractableItemProfileSO interactItem = baseItem as InteractableItemProfileSO;

        Vector3 mousePos = GetMousePosition();
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        bool isUIOpen = UIManager.Instance.curOpenUI != UIType.None;

        MouseTarget mouseTarget = null;

        bool hitMouseTarget = false;
        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("MouseTarget") && !hitMouseTarget)
            {
                mouseTarget = hitObject.GetComponent<MouseTarget>();
                hitMouseTarget = true;
            }

            if (hitObject.CompareTag("ItemPickupable") && !isUIOpen)
            {
                ItemPickupable itemPickupable = hitObject.GetComponent<ItemPickupable>();
                itemPickupable?.Pick();
            }
        }

        UpdateTileSelection(mousePos, interactItem, mouseTarget, isUIOpen);

        if (!isUIOpen)
        {
            UpdateDisplayInteractButton(mouseTarget);
            UpdatePreviewAndPlacement(mousePos, interactItem);
        }
    }

    private void UpdateTileSelection(Vector3 mousePos, InteractableItemProfileSO interactItem, MouseTarget mouseTarget, bool isUIOpen)
    {
        if (mouseTarget != target)
        {
            target = mouseTarget;
            if (target != null)
            {
                bool shouldSetActive = false;

                if (isUIOpen && mouseTarget.isUIComponent)
                {
                    shouldSetActive = true;
                }
                else if (!isUIOpen && (mouseTarget.shouldFocus || mouseTarget.actionType == interactItem.actionType))
                {
                    shouldSetActive = true;
                }

                if (shouldSetActive)
                {
                    tileSelection.transform.position = mouseTarget.TargetCollider.bounds.min;
                    tileSelection.size = mouseTarget.TargetCollider.bounds.size;
                }

                tileSelection.gameObject.SetActive(shouldSetActive);
            }
        }
        if (target == null)
        {
            if (isUIOpen)
            {
                tileSelection.gameObject.SetActive(false);
                return;
            }
            if (interactItem.actionType == ActionType.Seeding || interactItem.actionType == ActionType.Watering)
            {
                Vector3 snappedPoint = new Vector3(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));
                tileSelection.transform.position = snappedPoint;
                if (interactItem is not null)
                {
                    tileSelection.size = new Vector2(interactItem.targetSize.x, interactItem.targetSize.y);
                }
                tileSelection.gameObject.SetActive(true);
            }
            else
            {
                tileSelection.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateDisplayInteractButton(MouseTarget mouseTarget)
    {
        if (mouseTarget != null && mouseTarget.displayInteractButton)
        {
            Vector2 displayPos = mouseTarget.TargetCollider.bounds.min;
            displayPos.x += mouseTarget.TargetCollider.bounds.size.x / 2;
            displayPos.y = displayPos.y + mouseTarget.TargetCollider.bounds.size.y + 0.5f;
            SetDisplayInteractButton(true, displayPos);
        }
        else
        {
            SetDisplayInteractButton(false);
        }
    }

    private void UpdatePreviewAndPlacement(Vector3 mousePos, InteractableItemProfileSO item)
    {
        if (item is PlaceableItemProfileSO placeableItem)
        {
            Vector3 previewPosition = new Vector3(Mathf.FloorToInt(mousePos.x + 1), Mathf.FloorToInt(mousePos.y), 0);

            TogglePreviewCanvas(true);
            UpdatePreviewSprite(placeableItem.previewSprite);
            UpdatePreviewPosition(previewPosition);
        }
        else
        {
            TogglePreviewCanvas(false);
        }
    }


    public GameObject GetMouseTargetObject(bool useMousePosition = true, float xPos = 0, float yPos = 0)
    {
        Vector3 mousePos = useMousePosition ? GetMousePosition() : new Vector3(xPos, yPos, 0);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("MouseTarget")) return hit.collider.transform.parent.gameObject;
        }

        return null;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void UpdatePreviewSprite(Sprite sprite)
    {
        if (previewImage.sprite != sprite) previewImage.sprite = sprite;
    }

    private void UpdatePreviewPosition(Vector3 position)
    {
        if (previewCanvas.transform.position != position) previewCanvas.transform.position = position;
    }

    public void TogglePreviewCanvas(bool on)
    {
        if (on == previewCanvas.activeSelf) return;
        previewCanvas.SetActive(on);
    }
}