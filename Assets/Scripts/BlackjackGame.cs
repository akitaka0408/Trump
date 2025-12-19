using System.Collections.Generic;

public enum GameResult
{
    PlayerWin,
    HostWin,
    Push
}

public class BlackjackGame
{
    public const int BLACKJACK = 21;
    const int HOST_STAND_MIN = 17;
    const int ACE_HIGH = 11;
    const int ACE_LOW = 1;
    const int JQK = 10;

    private List<int> deck = new List<int>();

    public List<int> PlayerHand { get; } = new List<int>();
    public List<int> HostHand { get; } = new List<int>();

    public int PlayerChips { get; private set; } = 100;
    public int CurrentBet { get; private set; }

    public int PlayerScore => CalculateHand(PlayerHand);
    public int HostScore => CalculateHand(HostHand);

    public void SetBet(int bet)
    {
        CurrentBet = bet;
    }

    public void StartRound()
    {
        PlayerHand.Clear();
        HostHand.Clear();
        PrepareDeck();

        PlayerHand.Add(Draw());
        HostHand.Add(Draw());
        PlayerHand.Add(Draw());
        HostHand.Add(Draw());
    }

    public void PlayerHit()
    {
        PlayerHand.Add(Draw());
    }

    public bool HostShouldHit()
    {
        return HostScore < HOST_STAND_MIN;
    }

    public void HostHit()
    {
        HostHand.Add(Draw());
    }

    public GameResult Evaluate()
    {
        if (PlayerScore > BLACKJACK) return GameResult.HostWin;
        if (HostScore > BLACKJACK) return GameResult.PlayerWin;

        if (PlayerScore > HostScore) return GameResult.PlayerWin;
        if (PlayerScore < HostScore) return GameResult.HostWin;
        return GameResult.Push;
    }

    public void ApplyResult(GameResult result)
    {
        if (result == GameResult.PlayerWin)
            PlayerChips += CurrentBet;
        else if (result == GameResult.HostWin)
            PlayerChips -= CurrentBet;
    }

    void PrepareDeck()
    {
        deck.Clear();
        for (int i = 0; i < 4; i++)
            for (int j = 1; j <= 13; j++)
                deck.Add(j);

        Shuffle();
    }

    void Shuffle()
    {
        System.Random rng = new System.Random();
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    int Draw()
    {
        if (deck.Count == 0)
            PrepareDeck(); // ƒfƒbƒL‚ª‹ó‚É‚È‚Á‚½‚çÄì¬

        int card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    public bool CanDoubleDown()
    {
        return PlayerHand.Count == 2 && PlayerChips >= CurrentBet;
    }

    public void DoubleDown()
    {
        CurrentBet *= 2;
        PlayerHand.Add(Draw());
    }

    int GetCardValue(int raw)
    {
        if (raw == 1) return ACE_HIGH;
        if (raw >= 11) return JQK;
        return raw;
    }

    int CalculateHand(List<int> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (int card in hand)
        {
            int value = GetCardValue(card);
            if (value == ACE_HIGH) aceCount++;
            total += value;
        }

        while (total > BLACKJACK && aceCount > 0)
        {
            total -= (ACE_HIGH - ACE_LOW);
            aceCount--;
        }

        return total;
    }
}
