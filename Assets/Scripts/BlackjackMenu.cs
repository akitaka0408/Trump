using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// メニュー画面(起動時に表示される画面)
public class BlackjackMenu : MonoBehaviour
{
    // フィールド定義

    // パネル参照用
    public GameObject darkOverlay;        // 暗転処理用のパネル
    public GameObject rulePanel;          // ルール表示パネル
    public GameObject recordPanel;        // 戦績表示パネル
    public GameObject achievePanel;       // 称号表示パネル
    public GameObject optionPanel;        // オプション表示パネル
    public GameObject gameOverPanel;      // ゲームオーバーパネル

    // テキスト参照用
    public TMP_Text moneyText;            // 所持金表示テキスト
    public TMP_Text playText;             // プレイ回数表示テキスト
    public TMP_Text winText;　　　　　    // 勝利数表示テキスト
    public TMP_Text winPerText;           // 勝率表示テキスト
    public GameObject rule1Text;　　      // ルール1ページ目のテキスト
    public GameObject rule2Text;          // ルール2ページ目のテキスト
    public GameObject rule3Text;          // ルール3ページ目のテキスト
    public GameObject rule4Text;          // ルール4ページ目のテキスト

    // ボタン参照用
    public GameObject ruleBackButton;     // ページを戻すボタン
    public GameObject ruleNextButton;     // ページを進めるボタン

    // トグル参照用
    public RectTransform bgmHandle;      // BGMトグルの位置
    public Toggle bgmToggle;             // BGMトグル
    public Image bgmBackgroundImage;     // BGMトグルの背景画像
    public Color bgmBackgroundOnColor;   // BGMトグルがONの時の背景色
    public Color bgmBackgroundOffColor;  // BGMトグルがOFFの時の背景色
    public RectTransform seHandle;       // SEトグルの位置
    public Toggle seToggle;              // SEトグル
    public Image seBackgroundImage;      // SEトグルの背景画像
    public Color seBackgroundOnColor;    // SEトグルがONの時の背景色
    public Color seBackgroundOffColor;   // SEトグルがOFFの時の背景色

    // 隠しコマンド用
    private int t = 0;
    private int b = 0;
    private int s = 0;

    // 開始時に実行される
    void Start()
    {
        // 所持金が0の場合、ミッションリセットと所持金を1000に設定
        if (GameDataManager.Instance.data.money <= 0)
        {
            // ミッションデータをリセット
            GameDataManager.Instance.ResetMissions();

            // ミッションの勝利数・連勝・連敗・最大掛け金も初期化
            GameDataManager.Instance.data.totalWinCount = 0;
            GameDataManager.Instance.data.winStreak = 0;
            GameDataManager.Instance.data.loseStreak = 0;
            GameDataManager.Instance.data.maxBet = 0;

            // 所持金を1000にする
            GameDataManager.Instance.data.money = 1000;

            // データを保存
            GameDataManager.Instance.Save();

            // 背景を暗転
            darkOverlay.SetActive(true);
            // ゲームオーバーパネルを表示
            gameOverPanel.SetActive(true);
            // クリック音を鳴らす
            SEManager.Instance?.PlayGameOverSE();
        }

        // SE トグルの初期化

        // seToggleとSEManagerのインスタンスが存在する場合
        if (seToggle != null && SEManager.Instance != null)
        {
            // SEトグルの状態をGameDataManagerから取得する
            seToggle.isOn = GameDataManager.Instance.data.SE;

            // seToggleがONなら
            if (seToggle.isOn)
            {
                // 背景色をONの色(緑色)にする
                seBackgroundImage.color = seBackgroundOnColor;
                // SEをONにする
                SEManager.Instance.EnableSE();
            }
            // seToggleがOFFなら
            else
            {
                // 背景色をOFFの色(灰色)にする
                seBackgroundImage.color = seBackgroundOffColor;
                // SEをOFFにする
                SEManager.Instance.DisableSE();
            }
        }

        // BGM トグルの初期化

        // bgmToggleとBGMManagerのインスタンスが存在する場合
        if (bgmToggle != null && BGMManager.Instance != null)
        {
            // BGMトグルの状態をGameDataManagerから取得する
            bgmToggle.isOn = GameDataManager.Instance.data.BGM;

            // bgmToggleがONなら
            if (bgmToggle.isOn)
            {
                // 背景色をONの色(緑色)にする
                bgmBackgroundImage.color = bgmBackgroundOnColor;
                // BGMをONにする
                BGMManager.Instance.PlayBGM();
            }
            // bgmToggleがOFFなら
            else
            {
                // 背景色をONの色(緑色)にする
                bgmBackgroundImage.color = bgmBackgroundOffColor;
                // BGMをOFFにする
                BGMManager.Instance.StopBGM();
            }
        }
    }

    // 戦績があるかどうかの判定
    bool IsBlackjackRecord(Record r)
    {
        return r.GameType == "Blackjack";
    }

    // Recordボタン
    public void OnRecordButton()
    {
        // 所持金をGameDataManagerから取得する
        moneyText.text = GameDataManager.Instance.data.money.ToString();

        // 戦績をGameDataManagerから取得する
        Record record = GameDataManager.Instance.data.records.Find(IsBlackjackRecord);

        // GameDataManagerに戦績がない場合
        if (record == null)
        {
            // 初期値を入れる
            record = new Record
            {
                GameType = "Blackjack",
                PlayCount = 0,
                WinCount = 0,
                LoseCount = 0
            };
            
            //GameDataManagerに追加する
            GameDataManager.Instance.data.records.Add(record);
            // 保存する
            GameDataManager.Instance.Save();
        }

        // GameDataManagerから取得したプレイ回数を表示する
        playText.text = record.PlayCount.ToString();
        // GameDataManagerから取得した勝利回数を表示する
        winText.text = record.WinCount.ToString();

        // 計算結果を格納する変数
        float winRate;

        // プレイ回数が0より大きい場合
        if (record.PlayCount > 0)
        {
            // 勝率を計算し、格納
            winRate = record.WinCount * 100f / record.PlayCount;
        }
        // プレイ回数が0の場合
        else
        {
            // 勝率は0
            winRate = 0;
        }

        // 計算された勝率を少数第一位まで表示
        winPerText.text = winRate.ToString("F1") + "%";

        // 背景を暗転
        darkOverlay.SetActive(true);
        // 戦績パネルを表示
        recordPanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 隠しコマンド用
        b++;
    }

    // Recordボタン内のcloseボタン
    public void OnRecordCloseButton()
    {
        // 背景を暗転解除
        darkOverlay.SetActive(false);
        // 戦績パネルを表示
        recordPanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // Ruleボタン
    public void OnRuleButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 背景を暗転
        darkOverlay.SetActive(true);
        // ルールパネルを表示
        rulePanel.SetActive(true);
        // ルール2ページ目テキストを非表示
        rule2Text.SetActive(false);
        // ルール3ページ目テキストを非表示
        rule3Text.SetActive(false);
        // ルール4ページ目テキストを非表示
        rule4Text.SetActive(false);
        // ページを戻すボタンを非表示
        ruleBackButton.SetActive(false);
        // 隠しコマンド用
        s++;
    }

    // ルールパネル内の→(進む)ボタンを押したときに実行されるメソッド
    public void OnRuleNextButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // 現在ルール1ページ目の場合
        if (rule1Text.activeSelf)
        {
            // ページを戻すボタンを表示
            ruleBackButton.SetActive(true);
            // ルール1ページ目テキストを非表示
            rule1Text.SetActive(false);
            // ルール2ページ目テキストを表示
            rule2Text.SetActive(true);
        }

        // 現在ルール2ページ目の場合
        else if (rule2Text.activeSelf)
        {
            // ルール2ページ目テキストを非表示
            rule2Text.SetActive(false);
            // ルール3ページ目テキストを表示
            rule3Text.SetActive(true);
        }

        // 現在ルール3ページ目の場合
        else if (rule3Text.activeSelf)
        {
            // ルール3ページ目テキストを非表示
            rule3Text.SetActive(false);
            // ルール4ページ目テキストを表示
            rule4Text.SetActive(true);
            //ページを進めるを非表示
            ruleNextButton.SetActive(false);
        }
    }

    // ルールパネル内の←(戻る)ボタンを押したときに実行されるメソッド
    public void OnRuleBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // 現在ルール2ページ目の場合
        if (rule2Text.activeSelf)
        {
            // ページを戻るボタンを非表示
            ruleBackButton.SetActive(false);
            // ルール1ページ目テキストを表示
            rule1Text.SetActive(true);
            // ルール2ページ目テキストを非表示
            rule2Text.SetActive(false);
        }

        // 現在ルール3ページ目の場合
        else if (rule3Text.activeSelf)
        {
            // ルール2ページ目テキストを表示
            rule2Text.SetActive(true);
            // ルール3ページ目テキストを非表示
            rule3Text.SetActive(false);
        }

        // 現在ルール3ページ目の場合
        else if (rule4Text.activeSelf)
        {
            // ルール3ページ目テキストを表示
            rule3Text.SetActive(true);
            // ルール4ページ目テキストを非表示
            rule4Text.SetActive(false);
            // ページを進めるボタンを表示
            ruleNextButton.SetActive(true);
        }
    }

    // ルールパネル内のcloseボタンを押したときに実行されるメソッド
    public void OnRuleCloseButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 暗転を解除
        darkOverlay.SetActive(false);
        // ルールパネルを閉じる
        rulePanel.SetActive(false);
        // ルール2ページ目テキストを非表示
        rule2Text.SetActive(false);
        // ルール3ページ目テキストを非表示
        rule3Text.SetActive(false);
        // ルール3ページ目テキストを非表示
        rule4Text.SetActive(false);
        // ルール1ページ目テキストを表示
        rule1Text.SetActive(true);
        // ページを戻すボタンを非表示
        ruleBackButton.SetActive(false);
        // ページを進めるを非表示
        ruleNextButton.SetActive(true);
    }

    // Optionボタン
    public void OnOptionButton()
    {
        // 背景を暗転
        darkOverlay.SetActive(true);
        // オプションパネルを表示
        optionPanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        t++;
    }

    public void OnOptionCloseButton()
    {
        // 暗転を解除
        darkOverlay.SetActive(false);
        // オプションパネルを非表示
        optionPanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // BGMトグル
    public void ToggleChangedBGM()
    {
        // ハンドルの位置を反転させる
        bgmHandle.anchoredPosition *= -1.0f;

        // bgmToggleがONの場合
        if (bgmToggle.isOn)
        {
            // 背景色をONの色(緑色)にする
            bgmBackgroundImage.color = bgmBackgroundOnColor;
            // BGMをONにする
            BGMManager.Instance?.PlayBGM();
        }
        // bgmToggleがOFFの場合
        else
        {
            // 背景色をOFFの色(灰色)にする
            bgmBackgroundImage.color = bgmBackgroundOffColor;
            // BGMをOFFにする
            BGMManager.Instance?.StopBGM();
        }

        // GameDataManagerにセットする
        GameDataManager.Instance.SetBGM(bgmToggle.isOn);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // SEトグル
    public void ToggleChangedSE()
    {
        // ハンドルの位置を反転させる
        seHandle.anchoredPosition *= -1.0f;

        // seToggleがONの場合
        if (seToggle.isOn)
        {
            // 背景色をONの色(緑色)にする
            seBackgroundImage.color = seBackgroundOnColor;
            // SEをONにする
            SEManager.Instance?.EnableSE();
        }
        // seToggleがOFFの場合
        else
        {
            // 背景色をOFFの色(灰色)にする
            seBackgroundImage.color = seBackgroundOffColor;
            // SEをOFFにする
            SEManager.Instance?.DisableSE();
        }

        // GameDataManagerにセットする
        GameDataManager.Instance.SetSE(seToggle.isOn);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }


    // ミッションボタン
    public void OnAchieveButton()
    {
        // 背景を暗転
        darkOverlay.SetActive(true);
        // 実績パネルを表示
        achievePanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // 隠しコマンド

        // それぞれのボタンが特定の回数押されていたら
        if (t == 2 && b == 8 && s == 3)
        {
            // ゲームのデータを初期値にリセット
            GameDataManager.Instance.ResetData();

            // ログを表示
            Debug.Log("データを初期化しました");
        }
        // 変数の初期化
        t = 0;
        b = 0;
        s = 0;
    }

    // Achiveパネル内のcloseボタン
    public void OnAchieveCloseButton()
    {
        // 暗転を解除
        darkOverlay.SetActive(false);
        // 実績パネルを非表示
        achievePanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // Achiveパネル内のcloseボタン
    public void OnGameOverCloseButton()
    {
        // 暗転を解除
        darkOverlay.SetActive(false);
        // 実績パネルを非表示
        gameOverPanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // Exitボタン
    public void OnExitButton()
    {
        // ゲームを閉じる
        Application.Quit();
    }

    // Startボタン
    public void OnStartButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // BlackjackGame画面に遷移
        SceneManager.LoadScene("BlackjackGameScene");
    }
}
