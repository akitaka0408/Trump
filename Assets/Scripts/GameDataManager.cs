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

        // ミッションリストが空なら初期化する
        if (data.missions == null || data.missions.Count == 0)
        {
            ResetMissions();
            Save(); // 初期ミッションを書き込む
        }
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
            data.totalWinCount++;
            data.winStreak++;
            data.loseStreak = 0;
        }
        // 敗北した場合
        else
        {
            // 敗北数を増やす
            record.LoseCount++;
            data.loseStreak++;
            data.winStreak = 0;
        }

        // ミッションが達成したかどうか
        CheckMissions();
        Save();
    }

    // 所持金更新
    public void AddMoney(int amount)
    {
        // 所持金を増やす
        data.money += amount;

        CheckMissions();
        Save();
    }

    // 掛け金セット
    public void SetBet(int bet)
    {
        // 掛け金が保存されている掛け金より多い場合
        if (bet > data.maxBet)
        {
            data.maxBet = bet;
        }

        // ミッションが達成したかどうか
        CheckMissions();
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

    // ミッション達成チェック
    void CheckMissions()
    {
        // ミッションの数まで
        foreach (var mis in data.missions)
        {
            // そのミッションがクリア済みの場合
            if (mis.IsCleared)
            {
                // 処理を飛ばす
                continue;
            }

            // ミッションIDごとに達成条件を判定
            switch (mis.MissionID)
            {
                // 勝利数が1以上ならクリア
                case "Win_1":
                    if (data.totalWinCount >= 1)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 勝利数が10以上ならクリア
                case "Win_10":
                    if (data.totalWinCount >= 10)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 勝利数が50以上ならクリア
                case "Win_50":
                    if (data.totalWinCount >= 50)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 連勝数が10以上ならクリア
                case "WinStreak_10":
                    if (data.winStreak >= 10)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 連敗数が5以上ならクリア
                case "LoseStreak_5":
                    if (data.loseStreak >= 5)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 所持金がが10000以上ならクリア
                case "Money_10000":
                    if (data.money >= 10000)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 掛け金がが1998ならクリア
                case "Bet_1998":
                    if (data.maxBet >= 1998)
                    {
                        mis.IsCleared = true;
                    }
                    break;

                // 所持金が100以下ならクリア
                case "Money_100":
                    if (data.money <= 100)
                    {
                        mis.IsCleared = true;
                    }
                    break;
            }
        }
    }

    // ミッション初期化用
    public void ResetMissions()
    {
        data.missions = new List<Mission>
        {
            new Mission { MissionID = "Win_1" },
            new Mission { MissionID = "Win_10" },
            new Mission { MissionID = "Win_50" },
            new Mission { MissionID = "WinStreak_10" },
            new Mission { MissionID = "LoseStreak_5" },
            new Mission { MissionID = "Money_10000" },
            new Mission { MissionID = "Bet_1998" },
            new Mission { MissionID = "Money_100" },
        };
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
            money = 1000,

            missions = new List<Mission>
            {
                new Mission { MissionID = "Win_1" },
                new Mission { MissionID = "Win_10" },
                new Mission { MissionID = "Win_50" },
                new Mission { MissionID = "WinStreak_10" },
                new Mission { MissionID = "LoseStreak_5" },
                new Mission { MissionID = "Money_10000" },
                new Mission { MissionID = "Bet_1998" },
                new Mission { MissionID = "Money_100" },
            }
        };

        // 保存
        Save();
    }
}