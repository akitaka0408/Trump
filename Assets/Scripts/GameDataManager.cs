using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Record
{
    public string GameType;   // "OldMaid", "Blackjack"など
    public int PlayCount;
    public int WinCount;
    public int LoseCount;
}

[System.Serializable]
public class MoneyData
{
    public int Money;
}

[System.Serializable]
public class Data
{
    public List<Record> records = new List<Record>();
    public bool BGM;
    public bool SE;
    public MoneyData moneyData = new MoneyData();
}

public class GameDataManager : MonoBehaviour
{
    private string filePath;
    public Data data;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "save.json");
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
        Save();
    }

    // 所持金更新
    public void AddMoney(int amount)
    {
        data.moneyData.Money += amount;
        Save();
    }

    // 設定更新
    public void SetSettings(bool bgm, bool se)
    {
        data.BGM = bgm;
        data.SE = se;
        Save();
    }
}