using System;
using System.IO;
using UnityEngine;

[Serializable]
public class JsonSaveLoader
{
    public void SaveLevelData(LevelData data, string path)
    {
        string json = JsonUtility.ToJson(data);
        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.WriteLine(json);
        }
    }

    public LevelData LoadLevelData(string path)
    {
        if (File.Exists(path) == false)
            throw new FileNotFoundException();

        string json;
        using (StreamReader sr = new StreamReader(path))
        {
            json = sr.ReadToEnd();
        }

        return JsonUtility.FromJson<LevelData>(json);
    }
}
