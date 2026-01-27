using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class BlackjackUI : MonoBehaviour
{
    // フィールド定義

    // パネル類
    public GameObject darkOverlay;        // 画面暗転用
    public GameObject betPanel;           // ベット額入力パネル
    public GameObject blackjackUIPanel;   // ゲーム中 UI パネル
    public GameObject rulePanel;          // ルール説明パネル
    public GameObject resultPanel;        // 結果表示パネル
    public GameObject retirePanel;        // リタイア確認パネル
    public GameObject dontTouchPanel;     // ホストのターン中に操作禁止にするパネル

    // 画像参照用
    public GameObject[] playerCardImages; // プレイヤーのカード画像オブジェクト
    public GameObject[] hostCardImages;   // ホストのカード画像オブジェクト
    public GameObject backgroundImage;    // 背景画像
    public Sprite[] cardSprites;          // カード画像（0～51）
    public Sprite nullCardSprite;         // 空のカード画像
    public Sprite backCardSprite;         // 裏向きカード画像
    private Sprite turnCardFrontSprite;   // 表面スプライト（アニメーションに使用）
    private Image turnCardImg;            // カード画像（アニメーションに使用）

    // テキスト参照用
    public TMP_Text playerScoreText;      // プレイヤーのスコア表示テキスト
    public TMP_Text hostScoreText;        // ホストのスコア表示テキスト
    public TMP_Text resultText;           // 結果テキスト
    public TMP_Text betsText;             // 現在のベット額表示テキスト
    public TMP_Text money1Text;           // 所持金表示（ベットパネル）
    public TMP_Text money2Text;           // 所持金表示（ゲーム中）
    public GameObject rule1Text;          // ルール1ページ目のテキスト
    public GameObject rule2Text;          // ルール2ページ目のテキスト
    public GameObject rule3Text;          // ルール3ページ目のテキスト
    public GameObject rule4Text;          // ルール4ページ目のテキスト
    public TMP_Text[] numberTexts;        // ベットパネルの3桁の数字テキスト

    // ボタン参照用
    public GameObject hitButton;          // Hitボタン
    public GameObject standButton;        // Standボタン
    public GameObject doubleDownButton;   // Doubleボタン
    public GameObject homeButton;         // Homeボタン
    public GameObject continueButton;     // Continueボタン
    public GameObject ruleBackButton;     // ページを戻すボタン
    public GameObject ruleNextButton;     // ページを進めるボタン

    // 座標参照用
    private RectTransform turnCardRT;　　// カードの RectTransform(座標)

    // ゲームロジック
    private int[] number = new int[3];    // ベット額（100, 10, 1 の桁）
    private bool TurnHostCard = false;    // ホストの2枚目を表にするかどうか判定
    private BlackjackGame game;           // ゲームロジックインスタンス                                      

    // 初めに呼び出される
    void Start()
    {
        // ゲームロジックを生成し
        game = new BlackjackGame();
        // 所持金をセット
        game.SetPlayerMoney(GameDataManager.Instance.data.money);
        // 所持金を UI に反映
        money1Text.text = game.PlayerMoney.ToString();
        // ホストの二枚目のカードを裏側にする
        TurnHostCard = false;
        // ベット入力 UI の初期表示
        UpdateDisplay();
    }

    // ベット入力欄の数字を UI に反映
    void UpdateDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            // UIに反映
            numberTexts[i].text = number[i].ToString();
        }
    }

    // 現在のベット額を計算
    int GetBetAmount()
    {
        // 添え字が左からそれぞれ100の位・10の位・1の位
        return number[0] * 100 + number[1] * 10 + number[2];
    }

    // ベット数字を+1
    public void OnIncrementNumberButton(int index)
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 0～9でループ
        number[index] = (number[index] + 1) % 10;
        // 表示を更新
        UpdateDisplay();
    }

    // ベット数字を -1
    public void OnDecrementNumberButton(int index)
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 0～9でループ
        number[index] = (number[index] + 9) % 10;
        // 表示を更新
        UpdateDisplay();
    }

    // Betボタン
    public void OnBetButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // ベット額を取得
        int bet = GetBetAmount();
        // ベット額が0 or 所持金より多い場合
        if (bet <= 0 || bet > game.PlayerMoney)
        {
            // クリックを無効
            return;
        }

        // ベット額をセット
        game.SetBet(bet);
        // 所持金をインスタンスにセット
        GameDataManager.Instance.data.money = game.PlayerMoney;
        // 所持金を保存
        GameDataManager.Instance.Save();
        // スタートラウンド開始
        game.StartRound();
        // カードのUIを更新
        UpdateCardUI();
        // ベット額のUIを更新
        UpdateBetUI();
        // 暗転解除
        darkOverlay.SetActive(false);
        // ベットパネル非表示
        betPanel.SetActive(false);
        // ゲームUIパネルを表示
        blackjackUIPanel.SetActive(true);
    }

    // Back(←)ボタン
    public void OnBackButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // BlackjackGame画面に遷移
        SceneManager.LoadScene("BlackjackMenuScene");
    }

    // Retire(×)ボタン
    public void OnRetireButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 背景を暗転
        darkOverlay.SetActive(true);
        // リタイア確認パネルを表示
        retirePanel.SetActive(true);
    }

    // はいボタン
    public void OnYesButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // 操作不可にする
        dontTouchPanel.SetActive(true);
        // 負け判定にする
        game.ApplyResult(GameResult.HostWin);
        // 負けなのでfalseにして更新
        GameDataManager.Instance.UpdateRecord("Blackjack", false);
        // 所持金を更新
        GameDataManager.Instance.data.money = game.PlayerMoney;
        // 保存する
        GameDataManager.Instance.Save();
        // リザルトパネルにリタイアを入れる
        resultText.text = "Retire";
        // フォントカラーを灰色にする
        resultText.color = Color.gray;
        // 少し待ち、リザルトパネルを開く
        StartCoroutine(ShowResultPanelDelay());
        // リタイア確認パネルを非表示
        retirePanel.SetActive(false);
        // 暗転を解除
        darkOverlay.SetActive(false);
    }

    // いいえボタン
    public void OnNoButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // リタイア確認パネルを非表示
        retirePanel.SetActive(false);
        // 暗転を解除
        darkOverlay.SetActive(false);
    }
    // Hitボタン
    public void OnHitButton()
    {
        // 手札が21以上の場合
        if (game.PlayerScore >= BlackjackGame.BLACKJACK)
        {
            // クリックを無効
            return;
        }

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // カードを引く
        game.PlayerHit();
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        // カードのUI更新
        UpdateCardUI();

        // バースト(21より大きい数字)の場合
        if (game.PlayerScore > BlackjackGame.BLACKJACK)
        {
            // ホストの二枚目のカードを表向きの判定
            game.HostSecondCard = true;
            // カードのUI更新
            UpdateCardUI();
            // 操作不可にする
            dontTouchPanel.SetActive(true);
            // 少し待った後相手ターンに移行
            StartCoroutine(DelayBeforeHostTurn());
            return;
        }
    }

    public void OnStandButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // ホストの二枚目のカードを表向きの判定
        game.HostSecondCard = true;
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        // カードのUI更新
        UpdateCardUI();
        // 操作不可にする
        dontTouchPanel.SetActive(true);
        // 少し待った後相手ターンに移行
        StartCoroutine(DelayBeforeHostTurn());
    }

    // Doubleボタン
    public void OnDoubleDownButton()
    {
        // 手札が21以上の場合
        if (game.PlayerScore >= BlackjackGame.BLACKJACK)
        {
            // クリックを無効
            return;
        }

        // ダブルダウンが不可の場合
        if (!game.CanDoubleDown())
        {
            // クリックを無効
            return;
        }

        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // Doubledown(ベット2倍＋1枚引く)する
        game.DoubleDown();
        // カードを引く音を鳴らす
        SEManager.Instance?.PlayCardSE();
        // ホストの二枚目のカードを表向きの判定
        game.HostSecondCard = true;
        // カードのUI更新
        UpdateCardUI();
        // ベット額のUI更新
        UpdateBetUI();
        // 操作不可にする
        dontTouchPanel.SetActive(true);

        // カードを引いた結果バーストした場合
        if (game.PlayerScore > BlackjackGame.BLACKJACK)
        {
            // ホストの二枚目のカードを表向きの判定
            game.HostSecondCard = true;
            // カードのUI更新
            UpdateCardUI();
            // 少し待った後相手ターンに移行
            StartCoroutine(DelayBeforeHostTurn());
            return;
        }

        // 少し待った後相手ターンに移行
        StartCoroutine(DelayBeforeHostTurn());
    }

    // Continueボタン
    public void OnContinueButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // ホストの2枚目を裏にする判定
        game.HostSecondCard = false;
        // カードをめくるアニメーションのフラグをリセット
        TurnHostCard = false;
        // フォントカラーを白に戻す
        resultText.color = Color.white;
        // 結果テキストをクリア
        resultText.text = "";

        // プレイヤーの手札数分回す
        foreach (var img in playerCardImages)
        {
            // カード画像のリセット
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        // ホストのの手札数分回す
        foreach (var img in hostCardImages)
        {
            // カード画像のリセット
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        // ルールパネルを非表示
        rulePanel.SetActive(false);
        // リザルトパネルを非表示
        resultPanel.SetActive(false);
        // ベットパネルを表示
        betPanel.SetActive(true);
        // ゲームUIパネルを非表示
        blackjackUIPanel.SetActive(false);
    }

    // Homeボタン
    public void OnHomeButton()
    {
        // クリック音を鳴らす
        SEManager.Instance?.PlayClickSE();
        // リザルトテキストをクリア
        resultText.text = "";
        // ホストの2枚目を裏にする判定
        game.HostSecondCard = false;
        // カードをめくるアニメーションのフラグをリセット
        TurnHostCard = false;
        // フォントカラーを白に戻す
        resultText.color = Color.white;

        // プレイヤーの手札数分回す
        foreach (var img in playerCardImages)
        {
            // カード画像のリセット
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        // ホストのの手札数分回す
        foreach (var img in hostCardImages)
        {
            // カード画像のリセット
            img.GetComponent<Image>().sprite = nullCardSprite;
        }

        // ルールパネルを非表示
        rulePanel.SetActive(false);
        // リザルトテキストをクリア
        resultPanel.SetActive(false);
        // ゲームUIパネルを非表示
        blackjackUIPanel.SetActive(false);
        // BlackjackMenu画面に遷移
        SceneManager.LoadScene("BlackjackMenuScene");
    }

    // ホストのターン
    IEnumerator HostTurn()
    {
        // ホストがカードを引くべき間はループ
        while (game.HostShouldHit())
        {
            // 1秒待つ
            yield return new WaitForSeconds(1f);
            // カードを引く
            game.HostHit();
            // カードを引く音を鳴らす
            SEManager.Instance?.PlayCardSE();
            // カードUIの更新
            UpdateCardUI();
        }
        // Endroundへ
        EndRound();
    }

    void EndRound()
    {
        // 勝敗判定
        GameResult result = game.Evaluate();
        // 所持金の更新
        game.ApplyResult(result);
        // データ保存用の所持金を更新
        GameDataManager.Instance.data.money = game.PlayerMoney;

        // 勝敗記録を更新
        switch (result)
        {
            // ブラックジャックかプレイヤー勝利判定の場合
            case GameResult.BlackJack:
            case GameResult.PlayerWin:
                // 勝利なのでtrue
                GameDataManager.Instance.UpdateRecord("Blackjack", true);
                break;

            // ホスト勝利の場合
            case GameResult.HostWin:
                // 敗北なのでfalse
                GameDataManager.Instance.UpdateRecord("Blackjack", false);
                break;

            // 引き分けの場合
            case GameResult.Push:
                // 勝利ではないのでfalse
                GameDataManager.Instance.UpdateRecord("Blackjack", false);
                break;
        }

        // データ保存
        GameDataManager.Instance.Save();

        // 結果テキストを設定
        switch (result)
        {
            // ブラックジャック勝利の場合
            case GameResult.BlackJack:
                // リザルトパネルにBlackJackを入れる
                resultText.text = "BlackJack";
                // フォントカラーをゴールドにする
                resultText.color = Color.gold;
                break;

            // プレイヤー勝利の場合
            case GameResult.PlayerWin:
                // リザルトパネルにWinを入れる
                resultText.text = "Win";
                // フォントカラーを赤にする
                resultText.color = Color.red;
                break;

            // ホスト勝利の場合
            case GameResult.HostWin:
                // リザルトパネルにLoseを入れる
                resultText.text = "Lose";
                // フォントカラーを青にする
                resultText.color = Color.blue;
                break;

            // 引き分けの場合
            case GameResult.Push:
                resultText.text = "Draw";
                break;
        }

        // リザルトパネルを少し遅らせて表示
        StartCoroutine(ShowResultPanelDelay());
        // カードUIの更新
        UpdateCardUI();
    }

    // ホストターン移行時
    IEnumerator DelayBeforeHostTurn()
    {
        // 1秒待つ
        yield return new WaitForSeconds(1f); 
        // ホストターン開始
        StartCoroutine(HostTurn());
    }

    IEnumerator ShowResultPanelDelay()
    {
        // 1秒待つ
        yield return new WaitForSeconds(1f);
        // ベット額UIの更新
        UpdateBetUI();

        // リザルトパネル表示
        resultPanel.SetActive(true);
        // 背景の暗転
        darkOverlay.SetActive(true);
        // プレイヤー操作不可パネル表示
        dontTouchPanel.SetActive(false);

        switch (resultText.text)
        {
            case "BlackJack":
                // ブラックジャック時の音を鳴らす
                SEManager.Instance?.PlayBlackjackSE();
                break;

            case "Win":
                // 勝利時の音を鳴らす
                SEManager.Instance?.PlayWinSE();
                break;

            case "Lose":
                // 敗北時の音を鳴らす
                SEManager.Instance?.PlayLoseSE();
                break;

            case "Retire":
                // リタイア時の音を鳴らす
                SEManager.Instance?.PlayRetireSE();
                break;

            case "Draw":
                // 引き分け時の音を鳴らす
                SEManager.Instance?.PlayDrawSE();
                break;
        }

    }

    // ホストのスコア表示
    int GetVisibleHostScore()
    {
        // スコアの合計
        int total = 0;

        // ホストの手札が1枚以上の場合
        if (game.HostHand.Count > 0)
        {
            // カードを取得
            Card card = game.HostHand[0];
            // 値の定義
            int value;

            // カードがAの場合
            if (card.Rank == 1)
            {
                // ACE_HIGH
                value = 11;
            }
            // JQKの場合
            else if (card.Rank >= 11)
            {
                // 10として扱う
                value = 10;
            }
            // それ以外のカードの場合
            else
            {
                // 数字通り
                value = card.Rank;
            }

            // 合計に足す
            total += value;
        }

        return total;
    }

    // カードUI更新
    void UpdateCardUI()
    {
        // プレイヤーのカード画像スロット文回す
        for (int i = 0; i < playerCardImages.Length; i++)
        {
            // プレイヤーの手札数分回す
            if (i < game.PlayerHand.Count)
            {
                // cardに情報を格納
                Card card = game.PlayerHand[i];
                // カード画像配列の中で、対応する位置を計算している
                int index = card.Suit * 13 + (card.Rank - 1);
                // Imageコンポーネントの取得
                Image img = playerCardImages[i].GetComponent<Image>();
                // nullCard(新しくカードが置かれたか)の判定
                bool isNewCard = img.sprite == nullCardSprite; 
                // 計算したindexを用いて、対応するカード画像をセットしている
                img.sprite = cardSprites[index];

                // 新しくカードが置かれた場所だった場合
                if (isNewCard)
                {
                    // カードを引いてくるアニメーションをつける
                    AnimetionDrawCard(playerCardImages[i]);
                }
            }
        }

        // ホストのカード画像スロット文回す
        for (int i = 0; i < hostCardImages.Length; i++)
        {
            if (i < game.HostHand.Count)
            {
                // cardに情報を格納
                Card card = game.HostHand[i];
                // カード画像配列の中で、対応する位置を計算している
                int index = card.Suit * 13 + (card.Rank - 1);
                // Imageコンポーネントの取得
                Image img = hostCardImages[i].GetComponent<Image>();
                // nullCard(新しくカードが置かれたか)の判定
                bool isNewCard = img.sprite == nullCardSprite;
                // 計算したindexを用いて、対応するカード画像をセットする
                img.sprite = cardSprites[index];

                // 手札の二枚目のカードなら
                if (i == 1 && !game.HostSecondCard)
                {
                    // 画像を裏面のカードを表示する
                    img.sprite = backCardSprite;
                }
                // それ以外のカードなら
                else
                {
                    // 二枚目のカードが裏側表示の場合
                    if (i == 1 &&game.HostSecondCard &&!TurnHostCard)
                    {
                        // ホストの二枚目のカードを表向きの判定
                        TurnHostCard = true;
                        // めくるアニメーションをつける
                        AnimetionTurnCard(hostCardImages[i], cardSprites[index]);
                    }
                    // それ以外のカードなら
                    else
                    {
                        // 計算したindexを用いて、対応するカード画像をセットしている
                        img.sprite = cardSprites[index];
                    }
                }

                // 新しくカードが置かれた場所だった場合
                if (isNewCard)
                {
                    // カードを引いてくるアニメーションをつける
                    AnimetionDrawCard(hostCardImages[i]);
                }
            }

        }

        // プレイヤーのスコア表示
        playerScoreText.text = game.PlayerScore.ToString();

        // ホストの二枚目のカードが裏側表示の場合
        if (!game.HostSecondCard)
        {
            // ホストの1枚目だけのスコアを表示
            hostScoreText.text = GetVisibleHostScore().ToString();
        }
        // ホストの二枚目のカードが表側表示の場合
        else
        {
            // すべてのスコア合計を表示
            hostScoreText.text = game.HostScore.ToString();
        }
    }

    // ベット額のUIの更新
    void UpdateBetUI()
    {
        // ベット額の表示
        betsText.text = game.CurrentBet.ToString();
        // ベットパネルの所持金表示
        money1Text.text = game.PlayerMoney.ToString();
        // ゲーム中の所持金表示
        money2Text.text = game.PlayerMoney.ToString();
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
    }

    // ルールパネル内の→(進む)ボタン
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

    // ルールパネル内の←(戻る)ボタン
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

    // ルールパネル内のcloseボタン
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

    // カードを引いてくるアニメーション
    void AnimetionDrawCard(GameObject cardObj)
    {
        // カードのUIを操作するためにRectTransformを取得
        RectTransform rt = cardObj.GetComponent<RectTransform>();
        // 描画を最前面にする
        rt.SetAsLastSibling();
        // 最終的に移動したい位置
        Vector2 targetPos = rt.anchoredPosition;
        // アニメーション開始位置を画面右上あたりに設定
        rt.anchoredPosition = new Vector2(700, 500);
        // DOTweenを使って移動(画面外から移動させてきて引いたように見せる)
        rt.DOAnchorPos(targetPos, 0.4f).SetEase(Ease.OutCubic);

    }

    // カードをめくるアニメーション
    public void AnimetionTurnCard(GameObject cardObj, Sprite frontSprite)
    {
        // カードのUIを操作するためにRectTransformを取得
        RectTransform rt = cardObj.GetComponent<RectTransform>();
        // カード画像
        Image img = cardObj.GetComponent<Image>();

        // カードのUIを操作するための値を格納
        turnCardRT = rt;                    // 座標
        turnCardImg = img;                  // カードの裏面の画像
        turnCardFrontSprite = frontSprite;  // カードの表面の画像

        // DOTweenを使ってカードを縮める処理(縮めてめくられたに見せる)
        rt.DOScaleX(0f, 0.15f).SetEase(Ease.InQuad).OnComplete(OnTurnCardHalf);
    }

    // カードが裏面になる瞬間に呼ばれる
    private void OnTurnCardHalf()
    {
        // 画像をカードの裏面から表面に差し替え
        turnCardImg.sprite = turnCardFrontSprite;

        // カードを元の横幅に戻すアニメーション
        turnCardRT.DOScaleX(1f, 0.15f).SetEase(Ease.OutQuad);
    }
}
