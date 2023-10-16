// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GroundType
{
    Grass,
    TilledGround,
    Sand,
}

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    // [SerializeField] private Tilemap seaTilemap;
    [SerializeField] private Tilemap sandTilemap;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tilemap tilledGroundTilemap;
    [SerializeField] private Tilemap waterGroundTilemap;

    // [SerializeField] private Tile seaTile;
    // [SerializeField] private Tile sandTile;
    [SerializeField] private RuleTile grassTile;
    [SerializeField] private RuleTile tilledGroundTile;
    [SerializeField] private RuleTile wateredTile;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // void Start()
    // {
    //     foreach (var position in grassTilemap.cellBounds.allPositionsWithin)
    //     {
    //         grassTilemap.SetTile(position, hiddenInteractableTile);
    //     }
    // }

    public bool CanSeedAt(Vector3Int position, GroundType groundType)
    {
        switch (groundType)
        {
            case GroundType.Sand:
                if (sandTilemap.GetTile(position) == null || grassTilemap.GetTile(position) != null) return false;
                break;
            case GroundType.Grass:
                if (grassTilemap.GetTile(position) == null) return false;
                if (tilledGroundTilemap.GetTile(position) != null) TillGroundAtPos(position, false);
                break;
            case GroundType.TilledGround:
                if (tilledGroundTilemap.GetTile(position) == null) return false;
                break;
            default: break;
        }

        Vector3 cellCenter = tilledGroundTilemap.GetCellCenterWorld(position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(cellCenter, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || hit.collider.gameObject.tag != "MouseTarget") continue;
            return false;
        }

        return true;
    }

    protected Tilemap GetTilemapByGroundType(GroundType groundType)
    {
        switch (groundType)
        {
            case GroundType.Sand: return sandTilemap;
            case GroundType.Grass: return grassTilemap;
            case GroundType.TilledGround: return tilledGroundTilemap;
            default: return null;
        }
    }

    public bool IsTillable(Vector3Int position) => grassTilemap.GetTile(position) != null && tilledGroundTilemap.GetTile(position) == null;
    public bool IsWaterable(Vector3Int position) => tilledGroundTilemap.GetTile(position) != null && waterGroundTilemap.GetTile(position) == null;
    public bool IsWatered(Vector3Int position) => waterGroundTilemap.GetTile(position) != null;

    public bool TillGroundAtPos(Vector3Int position, bool till)
    {
        if (IsTillable(position) != till) return false;
        tilledGroundTilemap.SetTile(position, till ? tilledGroundTile : null);
        return true;
    }

    public bool WaterGroundAtPos(Vector3Int position, Vector2Int size)
    {
        bool success = false;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int pos = position;
                pos.x -= i; pos.y += j;
                if (!IsWaterable(pos)) continue;
                success = true;
                waterGroundTilemap.SetTile(pos, wateredTile);

                Vector2 pos2D = new(pos.x + .5f, pos.y + .5f);
                RaycastHit2D[] hits = Physics2D.RaycastAll(pos2D, Vector2.zero);

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("MouseTarget"))
                    {
                        ResourceCtrl resourceCtrl = hit.collider.gameObject.transform.parent.GetComponent<ResourceCtrl>();
                        resourceCtrl?.ResourceProfile.WaterThis();
                    }
                }
            }
        }
        return success;
    }

    public void DespawnWaterGroundAtPos(Vector3Int position)
    {
        waterGroundTilemap.SetTile(position, null);
    }
}
