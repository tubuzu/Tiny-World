// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : Spawner
{
    private static ResourceSpawner instance;
    public static ResourceSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Debug.LogError("Only 1 ResourceSpawner allow to exist!");
        instance = this;
    }
}
