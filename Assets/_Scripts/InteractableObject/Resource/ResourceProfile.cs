using System;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowState
{
    public Sprite sprite;
}

public class ResourceProfile : InteractableObjectProfile
{
    public int startState = 0;
    public int curState = 0;
    public List<GrowState> growStates;

    public int timeBetweenStates = 1;
    public bool canBeHarvest = false;
    public bool canBeHarvestWhenGrown = true;
    public bool watered = false;
    public bool needWaterToGrow = true;

    protected SpriteRenderer spriteRenderer;

    [SerializeField] protected ResourceCtrl resourceCtrl;
    public ResourceCtrl ResourceCtrl => resourceCtrl;

    protected override void LoadController()
    {
        if (this.resourceCtrl != null) return;
        this.resourceCtrl = transform.parent.GetComponent<ResourceCtrl>();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        spriteRenderer = resourceCtrl.Model.GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        curState = startState;
        canBeHarvest = false;
        spriteRenderer.sprite = growStates[curState].sprite;

        Vector3Int position = new((int)transform.parent.position.x, (int)transform.parent.position.y, 0);
        if ((TileManager.Instance != null && TileManager.Instance.IsWatered(position)) || !needWaterToGrow)
        {
            Grow();
        }
    }

    public void WaterThis()
    {
        if (watered || !needWaterToGrow) return;
        watered = true;
        Grow();
    }

    public void Grow()
    {
        spriteRenderer.sprite = growStates[curState].sprite;
        if (curState < growStates.Count - 1)
        {
            curState++;
            Invoke(nameof(Grow), timeBetweenStates);
        }
        else if (canBeHarvestWhenGrown)
        {
            Vector3Int pos = new(Mathf.FloorToInt(transform.parent.position.x), Mathf.FloorToInt(transform.parent.position.y), 0);
            TileManager.Instance.DespawnWaterGroundAtPos(pos);
            canBeHarvest = true;
        }
    }

    public override void OnDeadDropItem()
    {
        float itemSpacing = 0.5f;
        float totalWidth = (dropItems.Count - 1) * itemSpacing;
        for (int i = 0; i < dropItems.Count; i++)
        {
            int num = UnityEngine.Random.Range(dropItems[i].minCount, dropItems[i].maxCount);
            Vector3 spawnPosition = resourceCtrl.transform.position;
            spawnPosition.x += i * itemSpacing - totalWidth * 0.5f;
            ItemDropSpawner.Instance.DropItem(dropItems[i].profile, num, spawnPosition, Quaternion.identity);
        }
    }
}
