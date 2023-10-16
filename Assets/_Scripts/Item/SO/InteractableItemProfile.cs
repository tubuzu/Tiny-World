// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TargetType
{
    None = 0,
    Ground = 1,
    Resource = 2,
    Animal = 3,
}

[CreateAssetMenu(fileName = "InteractableItemProfileSO", menuName = "SO/InteractableItemProfile")]
public class InteractableItemProfileSO : BaseItemProfileSO
{
    [Header("Interaction")]
    public TileBase tile;
    public ActionType actionType = ActionType.None;
    public Vector2Int range = new Vector2Int(10, 10);
    public Vector2Int targetSize = Vector2Int.one;
}
