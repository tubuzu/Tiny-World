using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSpawner : Spawner
{
    private static MachineSpawner instance;
    public static MachineSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Debug.LogError("Only 1 MachineSpawner allow to exist!");
        instance = this;
    }
}
