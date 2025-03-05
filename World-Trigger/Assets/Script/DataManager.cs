using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // Singletonインスタンス
    [HideInInspector] public SaveData data; // 保存データ

    private string filepath;
    private string fileName = "Data.json"; // JSONファイル名

    private void Awake()
    {
        // Singletonパターンの設定
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

        // パス名取得
        filepath = Application.persistentDataPath + "/" + fileName;

        // ファイルがない場合、初期データを作成
        if (!File.Exists(filepath))
        {
            data = new SaveData();
            data.TotalCoins = 100; // 初期コイン数
            Save();
        }
        else
        {
            Load();
        }
    }

    // JSONとしてデータを保存
    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filepath, json);
    }

    // JSONファイルを読み込み
    public void Load()
    {
        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<SaveData>(json);
        }
    }

    // 右または左のスキルを追加
    public bool AddSkill(string skillName, bool isRight)
    {
        if (isRight)
        {
            if (data.RightSkills.Count < 4)
            {
                data.RightSkills.Add(skillName);
                Save();
                return true;
            }
        }
        else
        {
            if (data.LeftSkills.Count < 4)
            {
                data.LeftSkills.Add(skillName);
                Save();
                return true;
            }
        }
        return false;
    }

    // 右または左のスキルを削除
    public void RemoveSkill(string skillName, bool isRight)
    {
        if (isRight)
        {
            data.RightSkills.Remove(skillName);
        }
        else
        {
            data.LeftSkills.Remove(skillName);
        }
        Save();
    }

    // 右または左のスキルリストを取得
    public List<string> GetSkills(bool isRight)
    {
        return isRight ? data.RightSkills : data.LeftSkills;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Save();
        }
    }
}
