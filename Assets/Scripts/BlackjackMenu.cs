using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// メニュー画面(起動時に表示される画面)
public class BlackjackMenu : MonoBehaviour
{
    // フィールド定義

    // パネル参照用
    public GameObject darkOverlay;           // 暗転処理用のパネル
    public GameObject rulePanel;             // ルール表示パネル
    public GameObject recordPanel;           // 記録表示パネル
    public GameObject missionPanel;          // ミッション表示パネル
    public GameObject optionPanel;           // オプション表示パネル
    public GameObject gameOverPanel;         // ゲームオーバーパネル
    public GameObject missionCompletePanel;  // ゲームオーバーパネル

    // テキスト参照用
    public TMP_Text moneyText;            // 所持金表示テキスト
    public TMP_Text playText;             // プレイ回数表示テキスト
    public TMP_Text winText;　　　　　    // 勝利数表示テキスト
    public TMP_Text winPerText;           // 勝率表示テキスト
    public TMP_Text blackJackText;        // BlackJack回数表示テキスト
    public TMP_Text bgmNameText;          // 曲名表示テキスト
    public TMP_Text missionTitleText1;　  // ミッションタイトル表示テキスト
    public TMP_Text missionTitleText2;　  // ミッションタイトル表示テキスト
    public TMP_Text missionTitleText3;　  // ミッションタイトル表示テキスト
    public TMP_Text missionTitleText4;　  // ミッションタイトル表示テキスト
    public TMP_Text missionText1;　       // ミッション内容表示テキスト
    public TMP_Text missionText2;　       // ミッション内容表示テキスト
    public TMP_Text missionText3;　       // ミッション内容表示テキスト
    public TMP_Text missionText4;         // ミッション内容表示テキスト
    public TMP_Text missionGoalText1;　   // ミッション達成目標表示テキスト
    public TMP_Text missionGoalText2;　   // ミッション達成目標表示テキスト
    public TMP_Text missionGoalText3;　   // ミッション達成目標表示テキスト
    public TMP_Text missionGoalText4;     // ミッション達成目標表示テキスト
    public TMP_Text missionGoalPerText1;　// ミッション達成率表示テキスト
    public TMP_Text missionGoalPerText2;　// ミッション達成率表示テキスト
    public TMP_Text missionGoalPerText3;  // ミッション達成率標表示テキスト
    public TMP_Text missionGoalPerText4;　// ミッション達成率標表示テキスト
    public TMP_Text missionPageText;　    // ミッションページ表示テキスト
    public GameObject rule1Text;　　      // ルール1ページ目のテキスト
    public GameObject rule2Text;          // ルール2ページ目のテキスト
    public GameObject rule3Text;          // ルール3ページ目のテキスト
    public GameObject rule4Text;          // ルール4ページ目のテキスト
    public GameObject rule1IndexText;　　 // ページ表示1/4のテキスト
    public GameObject rule2IndexText;     // ページ表示2/4のテキスト
    public GameObject rule3IndexText;     // ページ表示3/4のテキスト
    public GameObject rule4IndexText;     // ページ表示4/4のテキスト

    // ボタン参照用
    public GameObject ruleBackButton;         // ページを戻すボタン
    public GameObject ruleNextButton;         // ページを進めるボタン
    public GameObject missionBackButton;      // ページを戻すボタン
    public GameObject missionNextButton;  　　// ページを進めるボタン
    public GameObject missionCompleteButton;  // ページを進めるボタン

    // 画像参照尾用
    public GameObject missionCompleteImage1;   // ミッション達成時の画像
    public GameObject missionCompleteImage2;   // ミッションの達成時の画像
    public GameObject missionCompleteImage3;   // ミッションの達成時の画像
    public GameObject missionCompleteImage4;   // ミッションの達成時の画像
    public GameObject missionImage1;  　　　　 // ミッションの背景画像
    public GameObject missionImage2;  　　　　 // ミッションの背景画像

    // スライダー参照用
    public Slider bgmSlider;
    public Slider seSlider;

    // 隠しコマンド用
    private int ka = 0;
    private int ku = 0;
    private int shi = 0;

    // 現在のページ
    int currentPage = 0;

    // ミッションタイトル定義
    string[] missionTitle1 = { "レギュラー", "初", "全力" };
    string[] missionTitle2 = { "プロ", "一番星", "THE bLACKJ@CK" };
    string[] missionTitle3 = { "マスター", "ままならないね" };
    string[] missionTitle4 = { "レジェンド", "GOLD RUSH" };

    // ミッション定義
    string[] mission1 = { "1回勝利する", "1回プレイする", "マニーを1試合で1998ベットする" };
    string[] mission2 = { "10回勝利する", "5連勝する", "BlackJackを10回達成する" };
    string[] mission3 = { "30回勝利する", "5連敗する" };
    string[] mission4 = { "50回勝利する", "所持マニーを1万以上にする" };

    // ミッション達成目標定義
    string[] missionGoal1 = { "/ 1", "/ 1", "/ 1998" };
    string[] missionGoal2 = { "/ 10", "/ 5", "/ 10" };
    string[] missionGoal3 = { "/ 30", "/ 5" };
    string[] missionGoal4 = { "/ 50", "/ 10000" };

    // 開始時に実行される
    void Start()
    {
        // 所持金が0の場合、ミッションリセットと所持金を1000に設定
        if (GameDataManager.Instance.data.money <= 0)
        {
            // ミッションデータをリセット
            GameDataManager.Instance.ResetMissions();
            // 所持金を1000にする
            GameDataManager.Instance.data.money = 1000;
            // データを保存
            GameDataManager.Instance.Save();
            // 背景を暗転
            darkOverlay.SetActive(true);
            // ゲームオーバー音を鳴らす
            SEManager.Instance?.PlayGameOverSE();
            // ゲームオーバーパネルを表示
            gameOverPanel.SetActive(true);
        }

        // すべてのミッションを達成している場合
        if (GameDataManager.Instance.IsAllMissionsCleared())
        {
            // コンプリートボタンを表示する
            missionCompleteButton.SetActive(true);
        }
        // 達成していない場合
        else
        {
            // 非表示にする
            missionCompleteButton.SetActive(false);
        }

        // BGM名の初期化
        switch (GameDataManager.Instance.data.bgmIndex)
        {
            case 0:
                bgmNameText.text = "アコースティック";
                break;

            case 1:
                bgmNameText.text = "Burning Heart";
                break;

            case 2:
                bgmNameText.text = "シャイニングスター";
                break;

            case 3:
                bgmNameText.text = "12345";
                break;
        }

        // スライダー初期化
        bgmSlider.value = GameDataManager.Instance.data.bgmVolume;
        seSlider.value = GameDataManager.Instance.data.seVolume;

        // AudioMixerに反映
        BGMManager.Instance.SetVolume(bgmSlider.value);
        SEManager.Instance.SetVolume(seSlider.value);
    }

    // 記録があるかどうかの判定
    bool IsBlackjackRecord(Record r)
    {
        return r.gameType == "Blackjack";
    }

    // コンプリートボタン
    public void OnMissionCompleteButton()
    {
        // 背景を暗転
        darkOverlay.SetActive(true);
        // コンプリートパネルを表示
        missionCompletePanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // コンプリートボタン内の閉じるボタン
    public void OnMissionCompleteCloseButton()
    {
        // 背景を暗転解除
        darkOverlay.SetActive(false);
        // 記録パネルを非表示
        missionCompletePanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // 記録ボタン
    public void OnRecordButton()
    {
        // 所持金をGameDataManagerから取得する
        moneyText.text = GameDataManager.Instance.data.money.ToString();

        // 記録をGameDataManagerから取得する
        Record record = GameDataManager.Instance.data.records.Find(IsBlackjackRecord);

        // GameDataManagerに記録がない場合
        if (record == null)
        {
            // 初期値を入れる
            record = new Record
            {
                gameType = "Blackjack",
                playCount = 0,
                winCount = 0,
                loseCount = 0,
                totalBlackJackCount = 0
            };
            
            //GameDataManagerに追加する
            GameDataManager.Instance.data.records.Add(record);
            // 保存する
            GameDataManager.Instance.Save();
        }

        // GameDataManagerから取得したプレイ回数を表示する
        playText.text = record.playCount.ToString();
        // GameDataManagerから取得した勝利回数を表示する
        winText.text = record.winCount.ToString();

        // 計算結果を格納する変数
        float winRate;

        // プレイ回数が0より大きい場合
        if (record.playCount > 0)
        {
            // 勝率を計算し、格納
            winRate = record.winCount * 100f / record.playCount;
        }
        // プレイ回数が0の場合
        else
        {
            // 勝率は0
            winRate = 0;
        }

        // 計算された勝率を少数第一位まで表示
        winPerText.text = winRate.ToString("F1") + "%";

        // GameDataManagerから取得したブラックジャック回数を表示する
        blackJackText.text = record.totalBlackJackCount.ToString();

        // 背景を暗転
        darkOverlay.SetActive(true);
        // 戦績パネルを表示
        recordPanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 隠しコマンド用
        ku++;
    }

    // 記録ボタン内のcloseボタン
    public void OnRecordCloseButton()
    {
        // 背景を暗転解除
        darkOverlay.SetActive(false);
        // 戦績パネルを表示
        recordPanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // ルールボタン
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
        // ページ表示1を表示
        rule1IndexText.SetActive(true);
        // ページ表示2を非表示
        rule2IndexText.SetActive(false);
        // ページ表示3を非表示
        rule3IndexText.SetActive(false);
        // ページ表示4を非表示
        rule4IndexText.SetActive(false);
        // ページを戻すボタンを非表示
        ruleBackButton.SetActive(false);
        // 隠しコマンド用
        shi++;
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
            // ページ表示1を非表示
            rule1IndexText.SetActive(false);
            // ページ表示2を表示
            rule2IndexText.SetActive(true);
        }

        // 現在ルール2ページ目の場合
        else if (rule2Text.activeSelf)
        {
            // ルール2ページ目テキストを非表示
            rule2Text.SetActive(false);
            // ルール3ページ目テキストを表示
            rule3Text.SetActive(true);
            // ページ表示2を非表示
            rule2IndexText.SetActive(false);
            // ページ表示3を表示
            rule3IndexText.SetActive(true);
        }

        // 現在ルール3ページ目の場合
        else if (rule3Text.activeSelf)
        {
            // ルール3ページ目テキストを非表示
            rule3Text.SetActive(false);
            // ルール4ページ目テキストを表示
            rule4Text.SetActive(true);
            // ページ表示3を非表示
            rule3IndexText.SetActive(false);
            // ページ表示4を表示
            rule4IndexText.SetActive(true);
            // ページを進めるを非表示
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
            // ページ表示1を表示
            rule1IndexText.SetActive(true);
            // ページ表示2を非表示
            rule2IndexText.SetActive(false);
        }

        // 現在ルール3ページ目の場合
        else if (rule3Text.activeSelf)
        {
            // ルール2ページ目テキストを表示
            rule2Text.SetActive(true);
            // ルール3ページ目テキストを非表示
            rule3Text.SetActive(false);
            // ページ表示2を表示
            rule2IndexText.SetActive(true);
            // ページ表示3を非表示
            rule3IndexText.SetActive(false);
        }

        // 現在ルール3ページ目の場合
        else if (rule4Text.activeSelf)
        {
            // ルール3ページ目テキストを表示
            rule3Text.SetActive(true);
            // ルール4ページ目テキストを非表示
            rule4Text.SetActive(false);
            // ページ表示3を表示
            rule3IndexText.SetActive(true);
            // ページ表示4を非表示
            rule4IndexText.SetActive(false);
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
        // ページ表示1を表示
        rule1IndexText.SetActive(true);
        // ページ表示2を非表示
        rule2IndexText.SetActive(false);
        // ページ表示3を非表示
        rule3IndexText.SetActive(false);
        // ページ表示4を非表示
        rule4IndexText.SetActive(false);
        // ページを戻すボタンを非表示
        ruleBackButton.SetActive(false);
        // ページを進めるを非表示
        ruleNextButton.SetActive(true);
    }

    // 設定ボタン
    public void OnOptionButton()
    {
        // 背景を暗転
        darkOverlay.SetActive(true);
        // オプションパネルを表示
        optionPanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        ka++;
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

    // BGMスライダー
    public void OnBGMVolumeChanged(float value)
    {
        GameDataManager.Instance.SetBGMVolume(value);
    }

    // SEスライダー
    public void OnSEVolumeChanged(float value)
    {
        GameDataManager.Instance.SetSEVolume(value);
    }

    // 設定パネル内のBGMを次に進めるボタン
    public void OnNextBGMButton()
    {
        // bgmClipsの配列のサイズを取得
        int max = BGMManager.Instance.bgmClips.Length;
        // 保存されているBGM番号を取得
        int index = GameDataManager.Instance.data.bgmIndex;
        // 配列のサイズでループ
        index = (index + 1) % max;
        // indexの値を格納
        GameDataManager.Instance.data.bgmIndex = index;
        // 保存
        GameDataManager.Instance.Save();

        // BGMがONの場合
        if (GameDataManager.Instance.data.bgmVolume != 0)
        {
            // BGM番号に合ったBGMを再生
            BGMManager.Instance.PlayBGM(index);
        }

        // BGM名の初期化
        switch (index)
        {
            case 0:
                bgmNameText.text = "アコースティック";
                break;

            case 1:
                bgmNameText.text = "Burning Heart";
                break;

            case 2:
                bgmNameText.text = "シャイニングスター";
                break;

            case 3:
                bgmNameText.text = "12345";
                break;
        }

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // 設定パネル内のBGMを前に戻すボタン
    public void OnBackBGMButton()
    {
        // bgmClipsの配列のサイズを取得
        int max = BGMManager.Instance.bgmClips.Length;
        // 保存されているBGM番号を取得
        int index = GameDataManager.Instance.data.bgmIndex;
        // 配列のサイズでループ
        index = (index - 1 + max) % max;
        // indexの値を格納
        GameDataManager.Instance.data.bgmIndex = index;
        // 保存
        GameDataManager.Instance.Save();

        // BGMがONの場合
        if (GameDataManager.Instance.data.bgmVolume != 0)
        {
            // BGM番号に合ったBGMを再生
            BGMManager.Instance.PlayBGM(index);
        }

        // BGM名の初期化
        switch (index)
        {
            case 0:
                bgmNameText.text = "アコースティック";
                break;

            case 1:
                bgmNameText.text = "Burning Heart";
                break;

            case 2:
                bgmNameText.text = "シャイニングスター";
                break;

            case 3:
                bgmNameText.text = "12345";
                break;
        }

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }


    // ミッションボタン
    public void OnMissionButton()
    {
        // 背景を暗転
        darkOverlay.SetActive(true);
        // ミッションパネルを表示
        missionPanel.SetActive(true);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // ミッション一覧画面の初期化
        missionTitleText1.text = missionTitle1[0];
        missionTitleText2.text = missionTitle2[0];
        missionTitleText3.text = missionTitle3[0];
        missionTitleText4.text = missionTitle4[0];
        missionText1.text = mission1[0];
        missionText2.text = mission2[0];
        missionText3.text = mission3[0];
        missionText4.text = mission4[0];
        missionGoalText1.text = missionGoal1[0];
        missionGoalText2.text = missionGoal2[0];
        missionGoalText3.text = missionGoal3[0];
        missionGoalText4.text = missionGoal4[0];
        missionGoalPerText1.text = GameDataManager.Instance.data.missionWinCount.ToString();
        missionGoalPerText2.text = GameDataManager.Instance.data.missionWinCount.ToString();
        missionGoalPerText3.text = GameDataManager.Instance.data.missionWinCount.ToString();
        missionGoalPerText4.text = GameDataManager.Instance.data.missionWinCount.ToString();
        missionPageText.text = "1/3";
        missionNextButton.SetActive(true);
        missionBackButton.SetActive(false);
        missionImage1.SetActive(true);
        missionImage2.SetActive(true);
        currentPage = 0;

        // 1回勝利するが達成済みの場合
        if (GameDataManager.Instance.IsMissionCleared("レギュラー"))
        {
            missionCompleteImage1.SetActive(true);
            missionGoalPerText1.text = "1";
        }
        // 未達成の場合
        else
        {
            missionCompleteImage1.SetActive(false);
        }

        // 10回勝利するが達成済みの場合
        if (GameDataManager.Instance.IsMissionCleared("プロ"))
        {
            missionCompleteImage2.SetActive(true);
            missionGoalPerText2.text = "10";
        }
        // 未達成の場合
        else
        {
            missionCompleteImage2.SetActive(false);
        }

        // 30回勝利するが達成済みの場合
        if (GameDataManager.Instance.IsMissionCleared("マスター"))
        {
            missionCompleteImage3.SetActive(true);
            missionGoalPerText3.text = "30";
        }
        // 未達成の場合
        else
        {
            missionCompleteImage3.SetActive(false);
        }

        // 50回勝利するが達成済みの場合
        if (GameDataManager.Instance.IsMissionCleared("レジェンド"))
        {
            missionCompleteImage4.SetActive(true);
            missionGoalPerText4.text = "50";
        }
        // 未達成の場合
        else
        {
            missionCompleteImage4.SetActive(false);
        }

        // 隠しコマンド(それぞれのボタンが特定の回数押されていたら)

        // ミッション達成状況と進捗をリセット
        if (ka == 7 && ku == 6 && shi == 5)
        {
            // ゲームのデータを初期値にリセット
            GameDataManager.Instance.ResetMissions();

            // ログを表示
            Debug.Log("ミッションを初期化しました");

            // データを保存
            GameDataManager.Instance.Save();
        }

        // 記録を含むすべてのデータの初期化
        if (ka == 9 && ku == 6 && shi == 1)
        {
            // ゲームのデータを初期値にリセット
            GameDataManager.Instance.ResetData();

            // ログを表示
            Debug.Log("データを初期化しました");

            // データを保存
            GameDataManager.Instance.Save();
        }

        // 所持マニーを1000000にする
        if (ka == 2 && ku == 8 && shi == 3)
        {
            // ゲームのデータを初期値にリセット
            GameDataManager.Instance.data.money = 1000000;

            // ログを表示
            Debug.Log("所持金を1000000にしました");

            // データを保存
            GameDataManager.Instance.Save();
        }

        // 変数の初期化
        ka = 0;
        ku = 0;
        shi = 0;
    }

    // ミッションパネル内の→(進む)ボタンを押したときに実行されるメソッド
    public void OnMissionNextButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // 現在のページを進める
        currentPage++;

        // 現在1ページ目の場合
        if (currentPage == 0)
        {
            // 1ページ目のミッション表示
            missionTitleText1.text = missionTitle1[0];
            missionTitleText2.text = missionTitle2[0];
            missionTitleText3.text = missionTitle3[0];
            missionTitleText4.text = missionTitle4[0];
            missionText1.text = mission1[0];
            missionText2.text = mission2[0];
            missionText3.text = mission3[0];
            missionText4.text = mission4[0];
            missionGoalText1.text = missionGoal1[0];
            missionGoalText2.text = missionGoal2[0];
            missionGoalText3.text = missionGoal3[0];
            missionGoalText4.text = missionGoal4[0];
            missionGoalPerText1.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText3.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText4.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionPageText.text = "1/3";
            missionImage1.SetActive(true);
            missionImage2.SetActive(true);
            missionNextButton.SetActive(true);
            missionBackButton.SetActive(false);

            // 1回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("レギュラー"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 10回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("プロ"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "10";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // 30回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("マスター"))
            {
                missionCompleteImage3.SetActive(true);
                missionGoalPerText3.text = "30";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage3.SetActive(false);
            }

            // 50回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("レジェンド"))
            {
                missionCompleteImage4.SetActive(true);
                missionGoalPerText4.text = "50";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage4.SetActive(false);
            }
        }

        // 現在2ページ目の場合
        if (currentPage == 1)
        {
            // 2ページ目のミッション表示
            missionTitleText1.text = missionTitle1[1];
            missionTitleText2.text = missionTitle2[1];
            missionTitleText3.text = missionTitle3[1];
            missionTitleText4.text = missionTitle4[1];
            missionText1.text = mission1[1];
            missionText2.text = mission2[1];
            missionText3.text = mission3[1];
            missionText4.text = mission4[1];
            missionGoalText1.text = missionGoal1[1];
            missionGoalText2.text = missionGoal2[1];
            missionGoalText3.text = missionGoal3[1];
            missionGoalText4.text = missionGoal4[1];
            missionGoalPerText1.text = GameDataManager.Instance.data.missionPlayCount.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.winStreak.ToString();
            missionGoalPerText3.text = GameDataManager.Instance.data.loseStreak.ToString();
            missionGoalPerText4.text = GameDataManager.Instance.data.money.ToString();
            missionPageText.text = "2/3";
            missionImage1.SetActive(true);
            missionImage2.SetActive(true);
            missionNextButton.SetActive(true);
            missionBackButton.SetActive(true);

            // 1回プレイするが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("初"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 5連勝するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("一番星"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "5";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // 5連敗するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("ままならないね"))
            {
                missionCompleteImage3.SetActive(true);
                missionGoalPerText3.text = "5";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage3.SetActive(false);
            }

            // 所持マニー10000以上が達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("GOLD RUSH"))
            {
                missionCompleteImage4.SetActive(true);
                missionGoalPerText4.text = "10000";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage4.SetActive(false);
            }
        }

        // 現在3ページ目の場合
        if (currentPage == 2)
        {
            // 3ページ目のミッション表示
            missionTitleText1.text = missionTitle1[2];
            missionTitleText2.text = missionTitle2[2];
            missionTitleText3.text = "";
            missionTitleText4.text = "";
            missionText1.text = mission1[2];
            missionText2.text = mission2[2];
            missionText3.text = "";
            missionText4.text = "";
            missionGoalText1.text = missionGoal1[2];
            missionGoalText2.text = missionGoal2[2];
            missionGoalText3.text = "";
            missionGoalText4.text = "";
            missionPageText.text = "3/3";
            missionGoalPerText1.text = GameDataManager.Instance.data.maxBet.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.blackjackCount.ToString();
            missionGoalPerText3.text = "";
            missionGoalPerText4.text = "";
            missionImage1.SetActive(false);
            missionImage2.SetActive(false);
            missionNextButton.SetActive(false);
            missionBackButton.SetActive(true);

            // 1998賭けるが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("全力"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1998";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 10回ブラックジャックするが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("THE bLACKJ@CK"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "10";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // ここにはミッションがない
            missionCompleteImage3.SetActive(false);
            missionCompleteImage4.SetActive(false);
        }
    }

    // ミッションパネル内の←(戻る)ボタンを押したときに実行されるメソッド
    public void OnMissionBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        // 現在のページを進める
        currentPage--;

        // 現在1ページ目の場合
        if (currentPage == 0)
        {
            // 1ページ目のミッション表示
            missionTitleText1.text = missionTitle1[0];
            missionTitleText2.text = missionTitle2[0];
            missionTitleText3.text = missionTitle3[0];
            missionTitleText4.text = missionTitle4[0];
            missionText1.text = mission1[0];
            missionText2.text = mission2[0];
            missionText3.text = mission3[0];
            missionText4.text = mission4[0];
            missionGoalText1.text = missionGoal1[0];
            missionGoalText2.text = missionGoal2[0];
            missionGoalText3.text = missionGoal3[0];
            missionGoalText4.text = missionGoal4[0];
            missionGoalPerText1.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText3.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionGoalPerText4.text = GameDataManager.Instance.data.missionWinCount.ToString();
            missionPageText.text = "1/3";
            missionImage1.SetActive(true);
            missionImage2.SetActive(true);
            missionNextButton.SetActive(true);
            missionBackButton.SetActive(false);

            // 1回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("レギュラー"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 10回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("プロ"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "10";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // 30回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("マスター"))
            {
                missionCompleteImage3.SetActive(true);
                missionGoalPerText3.text = "30";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage3.SetActive(false);
            }

            // 50回勝利するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("レジェンド"))
            {
                missionCompleteImage4.SetActive(true);
                missionGoalPerText4.text = "50";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage4.SetActive(false);
            }
        }

        // 現在2ページ目の場合
        if (currentPage == 1)
        {
            // 2ページ目のミッション表示
            missionTitleText1.text = missionTitle1[1];
            missionTitleText2.text = missionTitle2[1];
            missionTitleText3.text = missionTitle3[1];
            missionTitleText4.text = missionTitle4[1];
            missionText1.text = mission1[1];
            missionText2.text = mission2[1];
            missionText3.text = mission3[1];
            missionText4.text = mission4[1];
            missionGoalText1.text = missionGoal1[1];
            missionGoalText2.text = missionGoal2[1];
            missionGoalText3.text = missionGoal3[1];
            missionGoalText4.text = missionGoal4[1];
            missionGoalPerText1.text = GameDataManager.Instance.data.missionPlayCount.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.winStreak.ToString();
            missionGoalPerText3.text = GameDataManager.Instance.data.loseStreak.ToString();
            missionGoalPerText4.text = GameDataManager.Instance.data.money.ToString();
            missionPageText.text = "2/3";
            missionImage1.SetActive(true);
            missionImage2.SetActive(true);
            missionNextButton.SetActive(true);
            missionBackButton.SetActive(true);

            // 1回プレイするが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("初"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 5連勝するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("一番星"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "5";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // 5連敗するが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("ままならないね"))
            {
                missionCompleteImage3.SetActive(true);
                missionGoalPerText3.text = "5";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage3.SetActive(false);
            }

            // 所持マニー10000以上が達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("GOLD RUSH"))
            {
                missionCompleteImage4.SetActive(true);
                missionGoalPerText4.text = "10000";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage4.SetActive(false);
            }
        }

        // 現在3ページ目の場合
        if (currentPage == 2)
        {
            // 3ページ目のミッション表示
            missionTitleText1.text = missionTitle1[2];
            missionTitleText2.text = missionTitle2[2];
            missionTitleText3.text = "";
            missionTitleText4.text = "";
            missionText1.text = mission1[2];
            missionText2.text = mission2[2];
            missionText3.text = "";
            missionText4.text = "";
            missionGoalText1.text = missionGoal1[2];
            missionGoalText2.text = missionGoal2[2];
            missionGoalText3.text = "";
            missionGoalText4.text = "";
            missionGoalPerText1.text = GameDataManager.Instance.data.maxBet.ToString();
            missionGoalPerText2.text = GameDataManager.Instance.data.blackjackCount.ToString();
            missionGoalPerText3.text = "";
            missionGoalPerText4.text = "";
            missionPageText.text = "3/3";
            missionImage1.SetActive(false);
            missionImage2.SetActive(false);
            missionNextButton.SetActive(false);
            missionBackButton.SetActive(true);

            // 1998賭けるが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("全力"))
            {
                missionCompleteImage1.SetActive(true);
                missionGoalPerText1.text = "1998";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage1.SetActive(false);
            }

            // 10回ブラックジャックするが達成済みの場合
            if (GameDataManager.Instance.IsMissionCleared("THE bLACKJ@CK"))
            {
                missionCompleteImage2.SetActive(true);
                missionGoalPerText2.text = "10";
            }
            // 未達成の場合
            else
            {
                missionCompleteImage2.SetActive(false);
            }

            // ここにはミッションがない
            missionCompleteImage3.SetActive(false);
            missionCompleteImage4.SetActive(false);
        }
    }

    // ミッションパネル内のcloseボタン
    public void OnMissionCloseButton()
    {
        // 暗転を解除
        darkOverlay.SetActive(false);
        // 実績パネルを非表示
        missionPanel.SetActive(false);
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
    }

    // ゲームオーバーパネル内のcloseボタン
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
