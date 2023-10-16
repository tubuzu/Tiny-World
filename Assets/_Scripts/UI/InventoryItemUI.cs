using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;

    public InventoryItem item;
    public Transform parentAfterDrag;

    public void ReloadItem(InventoryItem newItem)
    {
        if (newItem.count == 0)
        {
            Destroy(this.gameObject);
            return;
        }
        item.Clone(newItem);
        image.sprite = newItem.profile.sprite;
        RefreshCountText();
    }

    public void RefreshCountText()
    {
        countText.text = item.count.ToString();
        countText.gameObject.SetActive(item.count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        countText.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        countText.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
