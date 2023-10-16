// using System.Collections;
// using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MachineType
{
    SeedMaker,
    ToolMaker,
    Furnace,

}

public class MachineProfile : InteractableObjectProfile
{
    public MachineType machineType;

    [SerializeField] protected MachineCtrl machineCtrl;
    public MachineCtrl MachineCtrl => machineCtrl;

    protected override void LoadController()
    {
        if (this.machineCtrl != null) return;
        this.machineCtrl = transform.parent.GetComponent<MachineCtrl>();
    }

    public virtual void OpenMachineUI()
    {
        switch (machineType)
        {
            case MachineType.SeedMaker:
                UIManager.Instance.ToggleUI(UIType.SeedMakerUI, true);
                SeedMakerUI.Instance.curMachine = this;
                break;
            default: break;
        }
    }

    public override void OnDeadDropItem()
    {
        float itemSpacing = 0.5f;
        float totalWidth = (dropItems.Count - 1) * itemSpacing;
        for (int i = 0; i < dropItems.Count; i++)
        {
            int num = Random.Range(dropItems[i].minCount, dropItems[i].maxCount);
            Vector3 spawnPosition = machineCtrl.transform.position;
            spawnPosition.x += i * itemSpacing - totalWidth * 0.5f;
            ItemDropSpawner.Instance.DropItem(dropItems[i].profile, num, spawnPosition, Quaternion.identity);
        }
    }
}
