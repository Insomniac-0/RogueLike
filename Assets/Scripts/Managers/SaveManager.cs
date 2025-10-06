using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Unity.Collections;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public int user_id;
    public int high_score;
    public SaveData(int id, int score)
    {
        user_id = id;
        high_score = score;
    }
}



public class SaveManager : MonoBehaviour
{
    private SaveData data;


    [SerializeField] string file_name;

    const int InitialAllocSize = 20;


    Dictionary<string, SaveData> HighScores = new Dictionary<string, SaveData>();


    string GetPath => Application.persistentDataPath + "/" + file_name + ".json";


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

    public void AddUser(string user, int score)
    {
        if (HighScores.ContainsKey(user) && HighScores[user].high_score < score)
        {
            SaveData data = HighScores[user];
            data.high_score = score;
            HighScores[user] = data;
        }
        else
        {
            int newID = HighScores.Count;
            SaveData data = new SaveData(newID, score);
            HighScores.Add(user, data);
        }
    }
}

