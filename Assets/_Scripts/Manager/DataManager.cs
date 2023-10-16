using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MyMonoBehaviour
{
    public static DataManager Instance;
    public List<ItemUnlockable> seeds;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SaveSeedsData()
    {
        string seedsJSON = JsonUtility.ToJson(seeds);
        string saveDataPath = "Assets/Resources/SaveData/SeedsJSON.json";

        System.IO.File.WriteAllText(saveDataPath, seedsJSON);
    }

    public void LoadSeedsData()
    {
        string saveDataPath = "SaveData/SeedsJSON";

        TextAsset jsonFile = Resources.Load<TextAsset>(saveDataPath);
        if (jsonFile != null)
        {
            string seedsJSON = jsonFile.text;

            seeds = JsonUtility.FromJson<List<ItemUnlockable>>(seedsJSON);
        }
        else
        {
            Debug.LogError("Could not load JSON file: " + saveDataPath);
        }
    }
}
