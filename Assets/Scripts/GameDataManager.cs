using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class Record
{
    public string GameType;   // "OldMaid", "Blackjack"
    public int PlayCount = 0;
    public int WinCount = 0;
    public int LoseCount = 0;
}

[System.Serializable]
public class Data
{
    public List<Record> records = new List<Record>();
    public bool BGM = true;
    public bool SE = true;
    public int money = 1000;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    private string filePath;
    public Data data;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log(filePath);
        Load();
    }

    // 保存
    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    // 読み込み
    public void Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(json);
        }
        else
        {
            data = new Data();
        }
    }

    // 戦績更新
    public void UpdateRecord(string gameType, bool isWin)
    {
        Record record = data.records.Find(r => r.GameType == gameType);
        if (record == null)
        {
            record = new Record { GameType = gameType };
            data.records.Add(record);
        }
        record.PlayCount++;
        if (isWin)
        {
            record.WinCount++;
        }
        else
        {
            record.LoseCount++;
        }
    }

    // 所持金更新
    public void AddMoney(int amount)
    {
        data.money += amount;
        Save();
    }

    // 設定更新
    public void SetSE(bool se)
    {
        data.SE = se;
        Save();
    }

    public void SetBGM(bool bgm)
    {
        data.BGM = bgm;
        Save();
    }

    // データの初期化
    public void ResetData()
    {
        data = new Data
        {
            records = new List<Record>
        {
            new Record { GameType = "OldMaid", PlayCount = 0, WinCount = 0, LoseCount = 0 },
            new Record { GameType = "Blackjack", PlayCount = 0, WinCount = 0, LoseCount = 0 }
        },
            BGM = true,
            SE = true,
            money = 1000
        };

        Save();
        Debug.Log("データを初期化しました");
    }

}