// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    // public InventoryItemUI itemUI;
    public Image image;
    public Color selectedColor, notSelectedColor;
    private void Awake()
    {
        // itemUI = transform.GetComponentInChildren<InventoryItemUI>();
        Deselect();
    }
    public void Select()
    {
        image.color = selectedColor;
    }
    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItemUI item = eventData.pointerDrag.GetComponent<InventoryItemUI>();
            item.parentAfterDrag = transform;
        }
    }
}
