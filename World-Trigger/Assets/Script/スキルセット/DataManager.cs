using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // シングルトン

    [HideInInspector] public SaveData data; // 保存するデータ
    private string filepath;
    private string fileName = "skillsetData.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        filepath = Application.dataPath + "/" + fileName;

        if (!File.Exists(filepath))
        {
            data = new SaveData();
            Save();
        }
        else
        {
            data = Load();
        }
    }

    // JSONとして保存
    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filepath, json);
    }

    // JSONから読み込み
    public SaveData Load()
    {
        if (!File.Exists(filepath)) return new SaveData();
        string json = File.ReadAllText(filepath);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
