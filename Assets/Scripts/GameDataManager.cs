using UnityEngine;
using System.Collections.Generic;
using System.IO;

// データを管理し、Jsonファイルに保存するクラス
public class GameDataManager : MonoBehaviour
{
    // フィールド定義
    private static GameDataManager instance;  // シングルトン管理用
    private string filePath;                  // ファイルパス用変数
    public Data data;                         // Data参照用

    // 一番初めに動く
    void Awake()
    {
        // Instanceがすでに存在している場合
        if (Instance != null && Instance != this)
        {
            // シングルトンを破壊する
            Destroy(gameObject);
            return;
        }

        // シングルトンとして登録
        Instance = this;
        // シーン切り替えでも破棄されないようにする
        DontDestroyOnLoad(gameObject);
        // 保存ファイルのパスを設定
        filePath = Path.Combine(Application.persistentDataPath, "save.json");
        // データ読み込み
        Load();
    }

    // シングルトン参照用
    public static GameDataManager Instance
    {
        get {
            return instance; 
        }
        private set { 
            instance = value; 
        }
    }


    // 保存
    public void Save()
    {
        // Json形式の文字列に変換
        string json = JsonUtility.ToJson(data, true);
        // ファイルにJson文字列を書き込む(ファイルが存在しなければ作られる)
        File.WriteAllText(filePath, json);
    }

    // ファイルからデータ読み込み
    public void Load()
    {
        // ファイルが存在している場合
        if (File.Exists(filePath))
        {
            // ファイルの中身をすべて読み込む
            string json = File.ReadAllText(filePath);
            // 読み込んだJsonをDataクラスのオブジェクトに変換している
            data = JsonUtility.FromJson<Data>(json);
        }
        // ファイルが存在しなかった場合
        else
        {
            // 初期状態のデータを作る
            data = new Data();
        }
    }

    // 戦績更新
    public void UpdateRecord(string gameType, bool isWin)
    {
        // Record を入れるための変数
        Record record = null;

        // リストから探す
        for (int i = 0; i < data.records.Count; i++)
        {
            // recordが特定のgameTypeだった場合
            if (data.records[i].GameType == gameType)
            {
                // recordに値を格納
                record = data.records[i];
                break;
            }
        }

        // 見つからなかった場合
        if (record == null)
        {
            // 新規作成
            record = new Record();
            record.GameType = gameType;
            // リストに追加
            data.records.Add(record);
        }

        // プレイ回数を1回増やす
        record.PlayCount++;

        // 勝利した場合
        if (isWin)
        {
            // 勝利数を増やす
            record.WinCount++;
        }
        // 敗北した場合
        else
        {
            // 敗北数を増やす
            record.LoseCount++;
        }
    }

    // 所持金更新
    public void AddMoney(int amount)
    {
        // 所持金を増やす
        data.money += amount;
        Save();
    }

    // SEの更新
    public void SetSE(bool se)
    {
        // 引数で渡されてきたSEを格納
        data.SE = se;
        // 保存
        Save();
    }

    public void SetBGM(bool bgm)
    {
        // 引数で渡されてきたBGMを格納
        data.BGM = bgm;
        // 保存
        Save();
    }

    // データの初期化
    public void ResetData()
    {
        // dataの初期化用のインスタンス生成
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

        // 保存
        Save();
    }
}