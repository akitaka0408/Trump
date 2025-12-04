using TMPro;
using UnityEngine;

public class BlackjackUI : MonoBehaviour
{
    // フィールド定義
    public GameObject darkOverlay;     // 暗転処理用のパネル
    public GameObject betPanel;        // マニーベット用パネル
    public GameObject rulePanel;       // ルール表示パネル
    public GameObject rule1Text;　　   // ルール1ページ目のテキスト
    public GameObject rule2Text;       // ルール2ページ目のテキスト
    public GameObject rule3Text;       // ルール3ページ目のテキスト
    public GameObject ruleBackButton;  // ページを戻すボタン
    public GameObject ruleNextButton;  // ページを進めるボタン
    public TMP_Text[] numberTexts; 
    private int[] number = new int[3];


    // 開始時に実行されるメソッド
    void Start()
    {

    }
    public void OnIncrementNumberButton(int index)
    {
        SEManager.Instance?.PlayClickSE();
        number[index] = (number[index] + 1) % 10;
        UpdateDisplay();
    }

    public void OnDecrementNUmberButton(int index)
    {
        SEManager.Instance?.PlayClickSE();
        number[index] = (number[index] + 9) % 10; 
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            numberTexts[i].text = number[i].ToString();
        }
    }

    public int GetTotalValue()
    {
        return number[0] * 100 + number[1] * 10 + number[2];
    }

    public void OnBetButton()
    {
        SEManager.Instance?.PlayClickSE();
        // 
        darkOverlay.SetActive(false);
        betPanel.SetActive(false);
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
            // Rule3Textを非表示
            rule3Text.SetActive(false);
            // Rule2Textを表示
            rule2Text.SetActive(true);
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
        // Rule1Textを表示
        rule1Text.SetActive(true);
        // RuleBackButtonを非表示
        ruleBackButton.SetActive(false);
        // RuleNextButtonを非表示
        ruleNextButton.SetActive(true);
    }

}
