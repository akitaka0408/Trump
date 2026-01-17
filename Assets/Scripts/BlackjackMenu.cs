using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackjackMenu : MonoBehaviour
{
    // フィールド定義
    public GameObject darkOverlay;     // 暗転処理用のパネル
    public GameObject rulePanel;       // ルール表示パネル
    public GameObject recordPanel;     // 戦績表示パネル
    public TMP_Text moneyText;         // 所持金表示テキスト
    public TMP_Text playText;          // プレイ回数表示テキスト
    public TMP_Text winText;　　　　　 // 勝利数表示テキスト
    public TMP_Text winPerText;　　    // 勝率表示テキスト
    public GameObject rule1Text;　　   // ルール1ページ目のテキスト
    public GameObject rule2Text;       // ルール2ページ目のテキスト
    public GameObject rule3Text;       // ルール3ページ目のテキスト
    public GameObject rule4Text;       // ルール4ページ目のテキスト
    public GameObject ruleBackButton;  // ページを戻すボタン
    public GameObject ruleNextButton;  // ページを進めるボタン

    // Recordボタン
    public void OnRecordButton()
    {

        // 所持金
        moneyText.text = GameDataManager.Instance.data.money.ToString();

        // 戦績（Blackjack）
        Record record = GameDataManager.Instance.data.records.Find(r => r.GameType == "Blackjack");

        playText.text = record.PlayCount.ToString();
        winText.text = record.WinCount.ToString();

        float winRate = (record.PlayCount > 0)
            ? (record.WinCount * 100f / record.PlayCount)
            : 0f;

        winPerText.text = winRate.ToString("F1") + "%";

        darkOverlay.SetActive(true);
        recordPanel.SetActive(true);
        SEManager.Instance?.PlayClickSE();
    }

    // Recordボタン内のcloseボタン
    public void OnRecordCloseButton()
    {
        darkOverlay.SetActive(false);
        recordPanel.SetActive(false);
        SEManager.Instance?.PlayClickSE();
    }

    // Ruleボタンを押したときに実行されるメソッド
    public void OnRuleButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 背景を暗くする処理
        darkOverlay.SetActive(true);
        // Panelを表示する処理
        rulePanel.SetActive(true);
        // Rule2Textを非表示にする
        rule2Text.SetActive(false);
        // Rule3Textを非表示にする
        rule3Text.SetActive(false);
        // Rule4Textを非表示にする
        rule4Text.SetActive(false);
        // RuleBackButtonを非表示にする
        ruleBackButton.SetActive(false);
    }

    // RulePanel内の→(進む)ボタンを押したときに実行されるメソッド
    public void OnRuleNextButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // rule1Textがアクティブなら
        if (rule1Text.activeSelf)
        {
            //RuleBackButtonを表示
            ruleBackButton.SetActive(true);
            // Rule1Textを非表示
            rule1Text.SetActive(false);
            // Rule2Textを表示
            rule2Text.SetActive(true);
        }

        // rule2Textがアクティブなら
        else if (rule2Text.activeSelf)
        {
            // Rule2Textを非表示
            rule2Text.SetActive(false);
            // Rule3Textを表示
            rule3Text.SetActive(true);
        }

        // rule3Textがアクティブなら
        else if (rule3Text.activeSelf)
        {
            // Rule3Textを非表示
            rule3Text.SetActive(false);
            // Rule3Textを表示
            rule4Text.SetActive(true);
            //RuleNextButtonを非表示
            ruleNextButton.SetActive(false);
        }
    }

    // RulePanel内の←(戻る)ボタンを押したときに実行されるメソッド
    public void OnRuleBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // rule2Textがアクティブなら
        if (rule2Text.activeSelf)
        {
            //RuleBackButtonを非表示
            ruleBackButton.SetActive(false);
            // Rule1Textを表示
            rule1Text.SetActive(true);
            // Rule2Textを非表示
            rule2Text.SetActive(false);
        }

        // rule3Textがアクティブなら
        else if (rule3Text.activeSelf)
        {
            // Rule2Textを表示
            rule2Text.SetActive(true);
            // Rule3Textを非表示
            rule3Text.SetActive(false);
        }

        // rule4Textがアクティブなら
        else if (rule4Text.activeSelf)
        {
            // Rule3Textを表示
            rule3Text.SetActive(true);
            // Rule4Textを非表示
            rule4Text.SetActive(false);
            //RuleNextButtonを表示
            ruleNextButton.SetActive(true);
        }
    }

    // RulePanel内のcloseボタンを押したときに実行されるメソッド
    public void OnRuleCloseButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 背景を明るくする処理
        darkOverlay.SetActive(false);
        // Panelを閉じる処理
        rulePanel.SetActive(false);
        // Rule2Textを非表示
        rule2Text.SetActive(false);
        // Rule3Textを非表示
        rule3Text.SetActive(false);
        // Rule3Textを非表示
        rule4Text.SetActive(false);
        // Rule1Textを表示
        rule1Text.SetActive(true);
        // RuleBackButtonを非表示
        ruleBackButton.SetActive(false);
        // RuleNextButtonを非表示
        ruleNextButton.SetActive(true);
    }

    // ←(戻る)ボタンを押したときに実行されるメソッド
    public void OnBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // StartMenu画面に遷移
        SceneManager.LoadScene("StartMenuScene");
    }

    // Startボタンを押したときに実行されるメソッド
    public void OnStartButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // BlackjackGame画面に遷移
        SceneManager.LoadScene("BlackjackGameScene");
    }
}
