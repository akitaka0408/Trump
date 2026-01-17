using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class BlackjackUI : MonoBehaviour
{
    public GameObject darkOverlay;
    public GameObject betPanel;
    public GameObject blackjackUIPanel;
    public GameObject rulePanel;
    public GameObject resultPanel;
    public GameObject dontTouchPanel;

    public GameObject rule1Text;
    public GameObject rule2Text;
    public GameObject rule3Text;
    public GameObject rule4Text;
    public GameObject ruleBackButton;
    public GameObject ruleNextButton;

    public GameObject[] playerCardImages;
    public GameObject[] hostCardImages;
    public Sprite[] cardSprites;
    public Sprite nullCardSprite;
    public Sprite backCardSprite;
    public GameObject backgroundImage;

    public TMP_Text playerScoreText;
    public TMP_Text hostScoreText;
    public TMP_Text resultText;
    public TMP_Text betsText;
    public TMP_Text money1Text;
    public TMP_Text money2Text;

    public GameObject hitButton;
    public GameObject standButton;
    public GameObject doubleDownButton;
    public GameObject homeButton;
    public GameObject continueButton;

    public TMP_Text[] numberTexts;
    private int[] number = new int[3];

    private bool TurnHostCard = false;

    private BlackjackGame game;

    void Start()
    {
        game = new BlackjackGame();
        game.SetPlayerMoney(GameDataManager.Instance.data.money);
        money1Text.text = game.PlayerMoney.ToString();
        TurnHostCard = false;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            numberTexts[i].text = number[i].ToString();
        }
    }

    int GetBetAmount()
    {
        return number[0] * 100 + number[1] * 10 + number[2];
    }

    public void OnIncrementNumberButton(int index)
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        number[index] = (number[index] + 1) % 10;
        UpdateDisplay();
    }

    public void OnDecrementNumberButton(int index)
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        number[index] = (number[index] + 9) % 10;
        UpdateDisplay();
    }

    public void OnBetButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        int bet = GetBetAmount();
        if (bet <= 0 || bet > game.PlayerMoney)
        {
            return;
        }

        game.SetBet(bet);
        game.StartRound();
        UpdateCardUI();
        UpdateBetUI();

        darkOverlay.SetActive(false);
        betPanel.SetActive(false);
        blackjackUIPanel.SetActive(true);
    }

    public void OnHitButton()
    {
        if (game.PlayerScore >= BlackjackGame.BLACKJACK)
            return;

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        game.PlayerHit();
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        UpdateCardUI();

        if (game.PlayerScore > BlackjackGame.BLACKJACK)
        {
            game.HostSecondCard = true;
            UpdateCardUI();
            dontTouchPanel.SetActive(true);
            StartCoroutine(DelayBeforeHostTurn());
            return;
        }
    }

    public void OnStandButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        game.HostSecondCard = true;
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        UpdateCardUI();
        dontTouchPanel.SetActive(true);
        StartCoroutine(DelayBeforeHostTurn());
    }

    public void OnDoubleDownButton()
    {
        if (game.PlayerScore >= BlackjackGame.BLACKJACK)
            return;

        if (!game.CanDoubleDown())
        {
            return;
        }

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        game.DoubleDown();
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        game.HostSecondCard = true;
        UpdateCardUI();
        UpdateBetUI();
        dontTouchPanel.SetActive(true);

        if (game.PlayerScore > BlackjackGame.BLACKJACK)
        {
            game.HostSecondCard = true;
            UpdateCardUI();
            StartCoroutine(DelayBeforeHostTurn());
            return;
        }

        StartCoroutine(DelayBeforeHostTurn());
    }

    public void OnContinueButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        game.HostSecondCard = false;

        TurnHostCard = false;

        resultText.text = "";

        foreach (var img in playerCardImages)
        {
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        foreach (var img in hostCardImages)
        {
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        rulePanel.SetActive(false);
        resultPanel.SetActive(false);
        betPanel.SetActive(true);
        blackjackUIPanel.SetActive(false);
    }

    public void OnHomeButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        resultText.text = "";

        game.HostSecondCard = false;

        TurnHostCard = false;

        foreach (var img in playerCardImages)
        {
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        foreach (var img in hostCardImages)
        {
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        rulePanel.SetActive(false);
        resultPanel.SetActive(false);
        blackjackUIPanel.SetActive(false);

        // BlackjackMenu画面に遷移
        SceneManager.LoadScene("BlackjackMenuScene");
    }

    IEnumerator HostTurn()
    {
        while (game.HostShouldHit())
        {
            yield return new WaitForSeconds(1f);
            game.HostHit();
            // カードを引く音を鳴らす
            SEManager.Instance?.PlayCardSE();
            UpdateCardUI();
        }
        EndRound();
    }

    void EndRound()
    {
        GameResult result = game.Evaluate();
        game.ApplyResult(result);

        GameDataManager.Instance.data.money = game.PlayerMoney;

        switch (result)
        {
            case GameResult.BlackJack:
            case GameResult.PlayerWin:
                GameDataManager.Instance.UpdateRecord("Blackjack", true);
                break;

            case GameResult.HostWin:
                GameDataManager.Instance.UpdateRecord("Blackjack", false);
                break;

            case GameResult.Push:
                GameDataManager.Instance.UpdateRecord("Blackjack", false);
                break;
        }

        GameDataManager.Instance.Save();

        switch (result)
        {
            case GameResult.BlackJack:
                resultText.text = "BlackJack";
                break;
            case GameResult.PlayerWin:
                resultText.text = "Win";
                break;
            case GameResult.HostWin:
                resultText.text = "Lose";
                break;
            case GameResult.Push:
                resultText.text = "Draw";
                break;
        }

        StartCoroutine(ShowResultPanelDelay());

        UpdateCardUI();
    }

    IEnumerator DelayBeforeHostTurn()
    {
        yield return new WaitForSeconds(1f); 
        StartCoroutine(HostTurn());
    }

    IEnumerator ShowResultPanelDelay()
    {
        // 2秒待つ
        yield return new WaitForSeconds(2f);

        UpdateBetUI();

        // 待ったあとにパネルを表示
        resultPanel.SetActive(true);
        darkOverlay.SetActive(true);
        dontTouchPanel.SetActive(false);
    }

    int GetVisibleHostScore()
    {
        int total = 0;

        if (game.HostHand.Count > 0)
        {
            Card card = game.HostHand[0];
            int value;

            if (card.Rank == 1)
                value = 11;       // ACE_HIGH
            else if (card.Rank >= 11)
                value = 10;       // JQK
            else
                value = card.Rank;

            total += value;
        }

        return total;
    }

    void UpdateCardUI()
    {
        for (int i = 0; i < playerCardImages.Length; i++)
        {
            if (i < game.PlayerHand.Count)
            {
                Card card = game.PlayerHand[i];
                int index = card.Suit * 13 + (card.Rank - 1);

                Image img = playerCardImages[i].GetComponent<Image>();

                bool isNewCard = img.sprite == nullCardSprite; 

                img.sprite = cardSprites[index];

                if (isNewCard)
                {
                    AnimetionDrawCard(playerCardImages[i]);
                }
            }
        }

        for (int i = 0; i < hostCardImages.Length; i++)
        {
            if (i < game.HostHand.Count)
            {
                if (i < game.HostHand.Count)
                {
                    Card card = game.HostHand[i];
                    int index = card.Suit * 13 + (card.Rank - 1);

                    Image img = hostCardImages[i].GetComponent<Image>();

                    bool isNewCard = img.sprite == nullCardSprite;

                    img.sprite = cardSprites[index];

                    if (i == 1 && !game.HostSecondCard)
                    {
                        img.sprite = backCardSprite;
                    }
                    else
                    {
                        if (i == 1 &&game.HostSecondCard &&!TurnHostCard)
                        {
                            TurnHostCard = true;
                            AnimetionTurnCard(hostCardImages[i], cardSprites[index]);
                        }
                        else
                        {
                            img.sprite = cardSprites[index];
                        }
                    }

                    if (isNewCard)
                    {
                        AnimetionDrawCard(hostCardImages[i]);
                    }
                }

            }
        }

        playerScoreText.text = game.PlayerScore.ToString();

        if (!game.HostSecondCard)
        {
            hostScoreText.text = GetVisibleHostScore().ToString();
        }
        else
        {
            hostScoreText.text = game.HostScore.ToString();
        }

    }

    void UpdateBetUI()
    {
        betsText.text = game.CurrentBet.ToString();
        money1Text.text = game.PlayerMoney.ToString();
        money2Text.text = game.PlayerMoney.ToString();
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

    void AnimetionDrawCard(GameObject cardObj)
    {
        RectTransform rt = cardObj.GetComponent<RectTransform>();

        rt.SetAsLastSibling();

        Vector2 targetPos = rt.anchoredPosition;

        rt.anchoredPosition = new Vector2(700, 500);

        rt.DOAnchorPos(targetPos, 0.4f).SetEase(Ease.OutCubic);

    }

    public void AnimetionTurnCard(GameObject cardObj, Sprite frontSprite)
    {
        RectTransform rt = cardObj.GetComponent<RectTransform>();
        Image img = cardObj.GetComponent<Image>();

        rt.DOScaleX(0f, 0.15f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            img.sprite = frontSprite;

            rt.DOScaleX(1f, 0.15f).SetEase(Ease.OutQuad);
        });
    }
    
}
