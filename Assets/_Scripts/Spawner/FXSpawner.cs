// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum FXName
{
    SheriffBulletExplodeEffect,
    DestroyEffect,
}

public class FXSpawner : Spawner
{
    private static FXSpawner instance;
    public static FXSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (FXSpawner.Instance != null) Debug.LogError("Only 1 FXSpawner allow to exist!");
        FXSpawner.instance = this;
    }
}
