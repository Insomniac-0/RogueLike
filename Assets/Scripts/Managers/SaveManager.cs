using System;
using System.IO;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public int HightScore;
    public float SurvivalTime;

}

public class SaveManager : MonoBehaviour
{
    private SaveData data;

    [SerializeField] string file_name;

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
}
