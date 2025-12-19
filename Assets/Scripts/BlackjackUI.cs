using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlackjackUI : MonoBehaviour
{
    public GameObject darkOverlay;
    public GameObject betPanel;
    public GameObject blackjackUIPanel;
    public GameObject rulePanel;
    public GameObject resultPanel;

    public GameObject rule1Text;
    public GameObject rule2Text;
    public GameObject rule3Text;
    public GameObject ruleBackButton;
    public GameObject ruleNextButton;

    public GameObject[] playerCardImages;
    public GameObject[] hostCardImages;
    public Sprite[] cardSprites;
    public GameObject backgroundImage;

    public TMP_Text playerScoreText;
    public TMP_Text hostScoreText;
    public TMP_Text resultText;
    public TMP_Text betsText;
    public TMP_Text moneyText;

    public GameObject hitButton;
    public GameObject standButton;
    public GameObject doubleDownButton;
    public GameObject homeButton;
    public GameObject continueButton;

    public TMP_Text[] numberTexts;
    private int[] number = new int[3];

    private BlackjackGame game;

    void Start()
    {
        game = new BlackjackGame();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < 3; i++)
            numberTexts[i].text = number[i].ToString();
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
        if (bet <= 0 || bet > game.PlayerChips)
            return;

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
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        game.PlayerHit();
        UpdateCardUI();

        if (game.PlayerScore > BlackjackGame.BLACKJACK)
            EndRound();
    }

    public void OnStandButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        StartCoroutine(HostTurn());
    }

    public void OnDoubleDownButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        if (!game.CanDoubleDown())
            return;

        game.DoubleDown();
        UpdateCardUI();
        UpdateBetUI();

        if (game.PlayerScore > BlackjackGame.BLACKJACK)
        {
            EndRound();
            return;
        }

        StartCoroutine(HostTurn());
    }

    public void OnContinueButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();

        resultText.text = "";

        foreach (var img in playerCardImages)
            img.GetComponent<Image>().sprite = null;

        foreach (var img in hostCardImages)
            img.GetComponent<Image>().sprite = null;

        resultPanel.SetActive(false);

        betPanel.SetActive(true);
        blackjackUIPanel.SetActive(false);
    }


    IEnumerator HostTurn()
    {
        while (game.HostShouldHit())
        {
            yield return new WaitForSeconds(1f);
            game.HostHit();
            UpdateCardUI();
        }
        EndRound();
    }

    void EndRound()
    {
        GameResult result = game.Evaluate();
        game.ApplyResult(result);

        UpdateCardUI();
        UpdateBetUI();

        switch (result)
        {
            case GameResult.PlayerWin: resultText.text = "Win"; break;
            case GameResult.HostWin: resultText.text = "Lose"; break;
            case GameResult.Push: resultText.text = "Draw"; break;
        }

        // コルーチンで待機処理を開始
        StartCoroutine(ShowResultPanelAfterDelay());
    }

    IEnumerator ShowResultPanelAfterDelay()
    {
        // 2秒待つ
        yield return new WaitForSeconds(2f);

        // 待ったあとにパネルを表示
        resultPanel.SetActive(true);
        darkOverlay.SetActive(true);
    }

    void UpdateCardUI()
    {
        for (int i = 0; i < playerCardImages.Length; i++)
        {
            if (i < game.PlayerHand.Count)
            {
                playerCardImages[i].GetComponent<Image>().sprite = cardSprites[game.PlayerHand[i] - 1];
            }
        }

        for (int i = 0; i < hostCardImages.Length; i++)
        {
            if (i < game.HostHand.Count)
            {
                hostCardImages[i].GetComponent<Image>().sprite = cardSprites[game.HostHand[i] - 1];
            }
        }

        playerScoreText.text = game.PlayerScore.ToString();
        hostScoreText.text = game.HostScore.ToString();
    }

    void UpdateBetUI()
    {
        betsText.text = game.CurrentBet.ToString();
        moneyText.text = game.PlayerChips.ToString();
    }

    public void OnRuleButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        darkOverlay.SetActive(true);
        rulePanel.SetActive(true);
        rule1Text.SetActive(true);
        rule2Text.SetActive(false);
        rule3Text.SetActive(false);
        ruleBackButton.SetActive(false);
        ruleNextButton.SetActive(true);
    }

    public void OnRuleNextButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        if (rule1Text.activeSelf)
        {
            rule1Text.SetActive(false);
            rule2Text.SetActive(true);
            ruleBackButton.SetActive(true);
        }
        else if (rule2Text.activeSelf)
        {
            rule2Text.SetActive(false);
            rule3Text.SetActive(true);
            ruleNextButton.SetActive(false);
        }
    }

    public void OnRuleBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        if (rule2Text.activeSelf)
        {
            rule2Text.SetActive(false);
            rule1Text.SetActive(true);
            ruleBackButton.SetActive(false);
        }
        else if (rule3Text.activeSelf)
        {
            rule3Text.SetActive(false);
            rule2Text.SetActive(true);
            ruleNextButton.SetActive(true);
        }
    }

    public void OnRuleCloseButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        darkOverlay.SetActive(false);
        rulePanel.SetActive(false);
    }
}
