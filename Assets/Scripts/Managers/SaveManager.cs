using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Unity.Collections;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public int high_score;
}



public class SaveManager : MonoBehaviour
{
    private SaveData data;

    const string file_name = "SaveData";

    string GetPath => Application.persistentDataPath + "/" + file_name + ".json";

    void Start()
    {
        InitResources.GetEventChannel.OnDeath += SaveScore;
    }

    public void LoadData()
    {
        if (!File.Exists(GetPath))
        {
            data.high_score = 0;
            SaveData();
            return;
        }
        string jsonfile = File.ReadAllText(GetPath);
        data = JsonUtility.FromJson<SaveData>(jsonfile);
    }

    public void SaveData()
    {
        string jsonfile = JsonUtility.ToJson(data);
        if (!File.Exists(GetPath)) File.Create(GetPath);
        File.WriteAllText(GetPath, jsonfile);
    }
    public void SaveScore()
    {
        LoadData();
        int score = InitResources.GetUpgradeSystem.GetScore;
        if (data.high_score < score)
        {
            data.high_score = score;
            SaveData();
        }
    }
}

