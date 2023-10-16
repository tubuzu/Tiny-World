// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MyMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform holder;

    [SerializeField] protected int spawnedCount = 0;
    public int SpawnedCount => spawnedCount;

    [SerializeField] protected List<GameObject> prefabs;
    [SerializeField] protected List<GameObject> poolObjs;

    protected override void LoadComponents()
    {
        this.LoadPrefabs();
        this.LoadHolder();
    }
    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
    }
    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;
        Transform prefabsObj = transform.Find("Prefabs");
        foreach (Transform t in prefabsObj)
        {
            this.prefabs.Add(t.gameObject);
        }
        this.HidePrefabs();
    }
    protected virtual void HidePrefabs()
    {
        foreach (GameObject prefab in this.prefabs)
        {
            prefab.SetActive(false);
        }
    }
    public virtual GameObject Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        GameObject prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            // Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }
        return Spawn(prefab, spawnPos, rotation);
    }
    public virtual GameObject Spawn(GameObject prefab, Vector3 spawnPos, Quaternion rotation)
    {
        GameObject newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.transform.SetPositionAndRotation(spawnPos, rotation);
        newPrefab.transform.parent = this.holder;
        newPrefab.SetActive(true);

        this.spawnedCount++;

        return newPrefab;
    }
    public virtual void Despawn(GameObject obj)
    {
        this.poolObjs.Add(obj);
        obj.SetActive(false);
        this.spawnedCount--;
    }
    public virtual GameObject GetPrefabByName(string name)
    {
        foreach (GameObject t in this.prefabs)
        {
            if (t.name == name) return t;
        }
        return null;
    }
    protected virtual GameObject GetObjectFromPool(GameObject prefab)
    {
        foreach (GameObject poolObj in this.poolObjs)
        {
            if (poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }
        GameObject newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }
    public virtual GameObject RandomPrefab()
    {
        int rand = Random.Range(0, prefabs.Count);
        return prefabs[rand];
    }
}
