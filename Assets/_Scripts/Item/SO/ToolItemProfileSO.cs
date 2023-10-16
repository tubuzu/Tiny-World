using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolItemProfileSO", menuName = "SO/ToolItemProfile")]
public class ToolItemProfileSO : InteractableItemProfileSO
{
    [Header("OnlyGameplay - Tool")]
    public int damage = 1;
    public float cooldown = 1f;
    // public int curDuration = 9;
    // public int maxDuration = 9;
}
