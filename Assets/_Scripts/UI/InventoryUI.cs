// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class InventoryUI : MyMonoBehaviour
{
    [Header("Inventory UI")]
    private static InventoryUI instance;
    public static InventoryUI Instance => instance;

    protected List<InventorySlotUI> slots;

    public bool isOpen = false;

    public GameObject openButton;
    public GameObject inventoryItemUIPrefab;

    public GameObject Toolbar;
    public GameObject MainInventory;
    public GameObject MainInventoryGroup;

    int selectedSlot = -1;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null) Debug.LogError("Only 1 InventoryUI allow to exist");
        instance = this;

        slots = new List<InventorySlotUI>();

        InventorySlotUI[] toolbarSlots = Toolbar.transform.GetComponentsInChildren<InventorySlotUI>();
        InventorySlotUI[] mainInventorySlots = MainInventory.transform.GetComponentsInChildren<InventorySlotUI>();
        foreach (var item in toolbarSlots) slots.Add(item);
        foreach (var item in mainInventorySlots) slots.Add(item);
    }

    protected override void Start()
    {
        base.Start();

        ChangeSelectedSlot(0);
        PlayerCtrl.Instance.PlayerAction.lateAction += PlayerCtrl.Instance.PlayerAction.ChangeSelectedItem;
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            slots[selectedSlot].Deselect();
        }
        selectedSlot = newValue;
        slots[selectedSlot].Select();
    }

    public virtual void Toggle(bool show)
    {
        if (isOpen == show) return;
        isOpen = show;
        MainInventoryGroup.SetActive(isOpen);
    }

    void SpawnNewItem(InventoryItem item, InventorySlotUI slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemUIPrefab, slot.transform);
        InventoryItemUI inventoryItem = newItemGo.GetComponent<InventoryItemUI>();
        inventoryItem.ReloadItem(item);
    }

    public void UpdateAtSlotIndex(int index, InventoryItem item)
    {
        InventoryItemUI itemUI = slots[index].transform.GetComponentInChildren<InventoryItemUI>();
        if (itemUI != null && itemUI.item.profile.stackable) itemUI.ReloadItem(item);
        else
        {
            SpawnNewItem(item, slots[index]);
        }
    }

    public BaseItemProfileSO GetSelectedItem()
    {
        InventoryItemUI itemUI = slots[selectedSlot].transform.GetComponentInChildren<InventoryItemUI>();
        return itemUI?.item?.profile;
    }
}
