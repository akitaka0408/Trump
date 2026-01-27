using System.Collections.Generic;

// ゲーム結果を表す列挙型
public enum GameResult
{
    PlayerWin,  // プレイヤー勝利
    HostWin,    // ホスト勝利(プレイヤー敗北)
    Push,       // 引き分け
    BlackJack   // プレイヤーのブラックジャック勝利
}

// Blackjackロジックのクラス
public class BlackjackGame
{
    // フィールド定義

    // 定数定義
    public const int BLACKJACK = 21;  // ブラックジャックの最大値
    const int HOST_STAND_MIN = 17;    // ホストがカードを引く基準
    const int ACE_HIGH = 11;          // A の高い値
    const int ACE_LOW = 1;            // A の低い値
    const int JQK = 10;               // JQK の値

    // 山札
    private List<Card> deck = new List<Card>();

    // プレイヤーの手札
    private List<Card> playerHand = new List<Card>();
    public List<Card> PlayerHand
    {
        get {
            return playerHand; 
        }
    }

    // ホストの手札
    private List<Card> hostHand = new List<Card>();
    public List<Card> HostHand
    {
        get {
            return hostHand; 
        }
    }

    // プレイヤーの所持金
    private int playerMoney;
    public int PlayerMoney
    {
        get {
            return playerMoney; 
        }
    }

    // 所持金をセット
    public void SetPlayerMoney(int money)
    {
        playerMoney = money;
    }

    // 現在のベット額
    private int currentBet;
    public int CurrentBet
    {
        get { 
            return currentBet; 
        }
    }

    // プレイヤーの現在スコア
    public int PlayerScore
    {
        get {
            return ValueHand(PlayerHand); 
        }
    }

    // ホストの現在スコア
    public int HostScore
    {
        get {
            return ValueHand(HostHand);
        }
    }

    // ホストの2枚目のカードを公開するかどうか
    public bool HostSecondCard = false;

    // ベット額を設定
    public void SetBet(int bet)
    {
        currentBet = bet;
        // ベット分を所持金から引く
        playerMoney -= bet;  
    }

    // ラウンド開始処理
    public void StartRound()
    {
        // 手札をクリア
        playerHand.Clear();
        hostHand.Clear();

        // 山札を準備
        PrepareDeck();

        // 順番にカードを配る
        playerHand.Add(Draw());
        hostHand.Add(Draw());
        playerHand.Add(Draw());
        hostHand.Add(Draw());
    }

    // プレイヤーがHitをする
    public void PlayerHit()
    {
        // カードを引く
        playerHand.Add(Draw());
    }

    // ホストがカードを引くべきか判定
    public bool HostShouldHit()
    {
        // ホストのスコアが17未満ならカードを引く
        return HostScore < HOST_STAND_MIN;
    }

    // ホストがHitをする
    public void HostHit()
    {
        // カードを引く
        hostHand.Add(Draw());
    }

    // 勝敗判定
    public GameResult Evaluate()
    {
        // 両者ブラックジャックの場合
        if (IsBlackJack(playerHand) && IsBlackJack(hostHand))
        {
            // 引き分け
            return GameResult.Push;
        }

        // プレイヤーのみブラックジャックの場合
        if (IsBlackJack(playerHand))
        {
            // ブラックジャック
            return GameResult.BlackJack;
        }

        // プレイヤーがバーストした場合
        if (PlayerScore > BLACKJACK)
        {
            // ホストもバーストした場合
            if (HostScore > BLACKJACK)
            {
                // 引き分け
                return GameResult.Push;
            }

            // ホスト勝利
            return GameResult.HostWin;
        }

        // ホストバーストした場合
        if (HostScore > BLACKJACK)
        {
            // プレイヤー勝利
            return GameResult.PlayerWin;
        }

        // スコア比較
        // プレイヤーのスコアの方が高い場合
        if (PlayerScore > HostScore)
        {
            // プレイヤー勝利
            return GameResult.PlayerWin;
        }
        // ホストのほうがスコアが高い場合
        if (PlayerScore < HostScore)
        {
            // ホスト勝利
            return GameResult.HostWin;
        }

        // 同点なので引き分け
        return GameResult.Push;
    }

    // 勝敗に応じて所持金を更新
    public void ApplyResult(GameResult result)
    {
        switch (result)
        {
            // ブラックジャックの場合
            case GameResult.BlackJack:
                // 所持金を2.5倍
                playerMoney += (int)(currentBet * 2.5f);
                break;

            // プレイヤー勝利の場合
            case GameResult.PlayerWin:
                // 所持金を2倍
                playerMoney += currentBet * 2;
                break;

            // 引き分けの場合
            case GameResult.Push:
                // 賭けた分を返却
                playerMoney += currentBet;
                break;

            // ホスト勝利の場合
            case GameResult.HostWin:
                // 何も返らない
                break;
        }
    }

    // 山札を作成
    void PrepareDeck()
    {
        // デッキを空にする
        deck.Clear();

        // 4スート × 13ランクのカードを作成
        for (int suit = 0; suit < 4; suit++)
        {
            // suit = 0,1,2,3 の4種類
            for (int rank = 1; rank <= 13; rank++)
            {
                // rank = 1～13 のカードを追加
                deck.Add(new Card(suit, rank));
            }
        }

        // シャッフル
        Shuffle();
    }

    // 山札をシャッフル
    void Shuffle()
    {
        // ランダム関数
        System.Random rnd = new System.Random();

        // 最後のカードから順にランダムな位置と交換していく
        for (int i = deck.Count - 1; i > 0; i--)
        {
            // 0～i の範囲でランダムな位置を選ぶ
            int j = rnd.Next(i + 1);

            // deck[i] と deck[j] を入れ替える
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    // 山札の先頭からカードを1枚引く
    Card Draw()
    {
        // 先頭のカードを取得
        Card card = deck[0];
        // 山札から削除
        deck.RemoveAt(0);
        // カードを返す
        return card;          
    }

    // ブラックジャック判定
    bool IsBlackJack(List<Card> hand)
    {
        // 手札が2枚ではない場合
        if (hand.Count != 2)
        {
            // Blackjackではない
            return false;
        }

        // A を持っているか
        bool hasAce = (hand[0].Rank == 1 || hand[1].Rank == 1);

        // 10,J,Q,Kを持っているか
        bool hasTenValue = (hand[0].Rank >= 10 && hand[0].Rank <= 13) || (hand[1].Rank >= 10 && hand[1].Rank <= 13);

        // A + 10 の組み合わせならブラックジャック
        return hasAce && hasTenValue;
    }

    // ダブルダウン可能か判定
    public bool CanDoubleDown()
    {
        // 手札が2枚且つ所持金がベット額以上ならTrue
        return playerHand.Count == 2 && playerMoney >= currentBet;
    }

    // ダブルダウン処理
    public void DoubleDown()
    {
        // 追加ベット
        playerMoney -= currentBet;
        // ベット額を2倍にすr
        currentBet *= 2;
        // カードを引く
        playerHand.Add(Draw());     
    }

    // カードの値を取得
    int GetCardValue(Card card)
    {
        // カードの数字が1の場合
        if (card.Rank == 1)
        {
            // A は 11 として扱う
            return ACE_HIGH; 
        }
        // カードの数字が11以上の場合
        if (card.Rank >= 11)
        {
            // JQK は 10
            return JQK; 
        }
        // それ以外は数字通り
        return card.Rank; 
    }

    // 手札の合計値を計算
    int ValueHand(List<Card> hand)
    {
        // 変数の定義
        int total = 0;     // 手札の合計
        int aceCount = 0;  // Ａの判定

        // Aを11として計算
        foreach (Card card in hand)
        {
            // 値を取得
            int value = GetCardValue(card);

            if (value == ACE_HIGH)
            {
                // A の枚数をインクリメント
                aceCount++; 
            }

            // 合計に値を足す
            total += value;
        }

        // 合計が21を超える場合
        while (total > BLACKJACK && aceCount > 0)
        {
            // Aを1として扱う(11 → 1 に変更)
            total -= (ACE_HIGH - ACE_LOW);
            // A の枚数をデクリメント
            aceCount--;
        }

        return total;
    }
}