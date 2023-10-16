using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    None,
    InventoryUI,
    SeedMakerUI,
    ToolMakerUI,
    FurnaceUI,

}

public class UIManager : MyMonoBehaviour
{
    public static UIManager Instance;
    public UIType curOpenUI = UIType.None;

    public SeedMakerUI seedMakerUI;
    public InventoryUI inventoryUI;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ToggleUI(UIType type, bool show)
    {
        if (show) curOpenUI = type;
        else curOpenUI = UIType.None;
        switch (type)
        {
            case UIType.SeedMakerUI:
                seedMakerUI.ToggleUI(show);
                break;
            case UIType.InventoryUI:
                inventoryUI.Toggle(show);
                break;
            default: break;
        }
    }

    public void OpenInventory()
    {
        if (curOpenUI == UIType.InventoryUI) return;
        curOpenUI = UIType.InventoryUI;
        inventoryUI.Toggle(true);
    }

    public void CloseCurrentUI()
    {
        if (curOpenUI != UIType.None) ToggleUI(curOpenUI, false);
    }
}
