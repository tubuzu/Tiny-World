using System;
// using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

[Serializable]
public class ItemDrop
{
    public BaseItemProfileSO profile;
    public int minCount;
    public int maxCount;
}

public abstract class InteractableObjectProfile : InteractableObjectAbstract
{
    public ActionType destroyAction;

    public List<ItemDrop> dropItems;

    public virtual void OnDeadDropItem()
    {

    }
}
