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
    private List<string> newlyClearedMissions = new List<string>();　// 新しくクリアしたミッションを保存

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

    // 記録の更新
    public void UpdateRecord(string gameType, int result, bool isBlackJack)
    {
        // Record を入れるための変数
        Record record = null;

        // リストから探す
        for (int i = 0; i < data.records.Count; i++)
        {
            // recordが特定のgameTypeだった場合
            if (data.records[i].gameType == gameType)
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
            record.gameType = gameType;
            // リストに追加
            data.records.Add(record);
        }

        // プレイ回数を1回増やす
        record.playCount++;
        data.missionPlayCount++;

        // 勝利した場合
        if (result == 0)
        {
            // 勝利数を増やす
            record.winCount++;
            data.missionWinCount++;
            data.winStreak++;
            data.loseStreak = 0;
        }
        // 敗北した場合
        else if(result == 1)
        {
            // 敗北数を増やす
            record.loseCount++;
            data.loseStreak++;
            data.winStreak = 0;
        }
        // 引き分けの場合
        else
        {
            // 連勝、連敗カウントを初期化
            data.winStreak = 0;
            data.loseStreak = 0;
        }

        // ブラックジャックの場合
        if (isBlackJack)
        {
            // ブラックジャック回数を増やす
            record.totalBlackJackCount++;
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

    // ブラックジャック回数を加算
    public void AddBlackjackCount()
    {
        // blackjack回数をカウント
        data.blackjackCount++;
        CheckMissions();
        Save();
    }


    // SEの更新
    public void SetSE(bool se)
    {
        // 引数で渡されてきたSEを格納
        data.se = se;
        // 保存
        Save();
    }

    // BGMの更新
    public void SetBGM(bool bgm)
    {
        // 引数で渡されてきたBGMを格納
        data.bgm = bgm;
        // 保存
        Save();
    }

    // ミッション達成チェック
    void CheckMissions()
    {

        // ミッションの数分回す
        foreach (var mis in data.missions)
        {
            // すでにクリア済みの場合
            if (mis.isCleared)
            {
                continue;
            }

            // クリアしたミッションがあるかの判定
            bool clear = false;

            switch (mis.missionID)
            {
                // プレイ回数が1回以上の場合
                case "初":
                    clear = data.missionPlayCount >= 1;
                    break;

                // 勝利数が1回以上の場合
                case "レギュラー":
                    clear = data.missionWinCount >= 1;
                    break;

                // 勝利数が10回以上の場合
                case "プロ":
                    clear = data.missionWinCount >= 10;
                    break;

                // 勝利数が30回以上の場合
                case "マスター":
                    clear = data.missionWinCount >= 30;
                    break;

                // 勝利数が50回以上の場合
                case "レジェンド":
                    clear = data.missionWinCount >= 50;
                    break;

                // 連勝数が5以上の場合
                case "一番星":
                    clear = data.winStreak >= 5;
                    break;

                // 連敗数が5以上の場合
                case "ままならないね":
                    clear = data.loseStreak >= 5;
                    break;

                // 所持マニーが10000以上の場合
                case "GOLD RUSH":
                    clear = data.money >= 10000;
                    break;

                // 最大ベット額が1998以上の場合
                case "全力":
                    clear = data.maxBet >= 1998;
                    break;

                // 10回Blackjackを達成した場合
                case "THE bLACKJ@CK":
                    clear = data.blackjackCount >= 10;
                    break;
            }

            // クリアしていた場合
            if (clear)
            {
                // ミッションをクリア済みにする
                mis.isCleared = true;

                // まだ追加されていないミッションIDだけ登録する
                if (!newlyClearedMissions.Contains(mis.missionID))
                {
                    // 新規クリアとして追加
                    newlyClearedMissions.Add(mis.missionID);
                }
            }
        }
    }

    // この試合で達成したミッションリストをリセットする
    public void ClearNewlyClearedMissions()
    {
        newlyClearedMissions.Clear();
    }

    // 今回の処理でミッションを1つ以上クリアしたか
    public bool HasNewlyClearedMission()
    {
        return newlyClearedMissions.Count > 0;
    }

    // 表示用（ミッションID一覧）
    public List<string> GetNewlyClearedMissions()
    {
        return newlyClearedMissions;
    }

    // 特定のミッションが達成済みか
    public bool IsMissionCleared(string missionID)
    {
        // 全ミッションを順番にチェック
        foreach (Mission mission in data.missions)
        {
            if (mission.missionID == missionID)
            {
                return mission.isCleared;
            }
        }

        // 見つからなかった場合は false
        return false;
    }

    // すべてのミッションが達成済みか
    public bool IsAllMissionsCleared()
    {
        // 全ミッションを順番にチェック
        foreach (Mission mission in data.missions)
        {
            // 1つでも未達成がある場合
            if (!mission.isCleared)
            {
                return false; 
            }
        }

        // 全て true なら達成済み
        return true; 
    }


    // ミッション初期化用
    public void ResetMissions()
    {
        data.missions = new List<Mission>
        {
            new Mission { missionID = "初" },
            new Mission { missionID = "レギュラー" },
            new Mission { missionID = "プロ" },
            new Mission { missionID = "マスター" },
            new Mission { missionID = "レジェンド" },
            new Mission { missionID = "一番星" },
            new Mission { missionID = "ままならないね" },
            new Mission { missionID = "GOLD RUSH" },
            new Mission { missionID = "全力" },
            new Mission { missionID = "THE bLACKJ@CK" },
        };

        // ミッションの勝利数・連勝・連敗・最大掛け金・BlackJack回数も初期化
        GameDataManager.Instance.data.missionPlayCount = 0; 
        GameDataManager.Instance.data.missionWinCount = 0;
        GameDataManager.Instance.data.winStreak = 0;
        GameDataManager.Instance.data.loseStreak = 0;
        GameDataManager.Instance.data.maxBet = 0;
        GameDataManager.Instance.data.blackjackCount = 0;
    }


    // 全体の初期化
    public void ResetData()
    {
        // dataの初期化用のインスタンス生成
        data = new Data
        {
            records = new List<Record>
            {
                new Record { gameType = "Blackjack", playCount = 0, winCount = 0, loseCount = 0 }
            },
            bgm = true,
            se = true,
            money = 1000,
            missionPlayCount = 0,
            missionWinCount = 0,      　　　　　　　　 
            winStreak = 0,                          
            loseStreak = 0,                          
            maxBet = 0,
            blackjackCount = 0,
            bgmIndex = 0,

    missions = new List<Mission>
            {
                new Mission { missionID = "初" },
                new Mission { missionID = "レギュラー" },
                new Mission { missionID = "プロ" },
                new Mission { missionID = "マスター" },
                new Mission { missionID = "レジェンド" },
                new Mission { missionID = "一番星" },
                new Mission { missionID = "ままならないね" },
                new Mission { missionID = "GOLD RUSH" },
                new Mission { missionID = "全力" },
                new Mission { missionID = "THE bLACKJ@CK" },
            }
        };

        // 保存
        Save();
    }
}