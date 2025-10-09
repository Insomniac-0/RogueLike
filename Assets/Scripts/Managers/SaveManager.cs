using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public int HighScore;
    public int EnemiesKilled;
}



public class SaveManager : MonoBehaviour
{
    private SaveData data;
    [SerializeField] string file_name;


    string GetPath => Application.persistentDataPath + "/" + file_name + ".json";

    void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (!File.Exists(GetPath))
        {
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

    public void SaveScore(int score, int kills)
    {

        if (data.HighScore < score)
        {
            data.HighScore = score;
            data.EnemiesKilled = kills;
            SaveData();

            InitResources.GetEventChannel.TriggerOnNewHighScore();
        }
    }

    public SaveData GetSaveData => data;

}

