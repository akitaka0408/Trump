using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum GameResult
{
    PlayerWin,
    HostWin,
    Push,
    BlackJack   
}

public struct Card
{
    public int Suit;
    public int Rank; 

    public Card(int suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }
}

public class BlackjackGame
{
    public const int BLACKJACK = 21;
    const int HOST_STAND_MIN = 17;
    const int ACE_HIGH = 11;
    const int ACE_LOW = 1;
    const int JQK = 10;

    private List<Card> deck = new List<Card>();

    public List<Card> PlayerHand { get; } = new List<Card>();
    public List<Card> HostHand { get; } = new List<Card>();

    public int PlayerMoney { get; private set; }

    public void SetPlayerMoney(int money)
    {
        PlayerMoney = money;
    }

    public int CurrentBet { get; private set; }

    public int PlayerScore => ValueHand(PlayerHand);
    public int HostScore => ValueHand(HostHand);

    public bool HostSecondCard = false;

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
        if (IsBlackJack(PlayerHand) && IsBlackJack(HostHand))
        {
            return GameResult.Push;
        }

        if (IsBlackJack(PlayerHand))
        {
            return GameResult.BlackJack;
        }

        if (PlayerScore > BLACKJACK)
        {
            if (HostScore > BLACKJACK)
            {
                return GameResult.Push;
            }

            return GameResult.HostWin;
        }

        if (HostScore > BLACKJACK)
        {
            return GameResult.PlayerWin;
        }
        if (PlayerScore > HostScore)
        {
            return GameResult.PlayerWin;
        }
        if (PlayerScore < HostScore)
        {
            return GameResult.HostWin;
        }
        return GameResult.Push;
    }

    public void ApplyResult(GameResult result)
    {
        if (result == GameResult.BlackJack)
        {
            PlayerMoney += (int)(CurrentBet * 1.5f);
        }
        if (result == GameResult.PlayerWin)
        {
            PlayerMoney += CurrentBet;
        }
        else if (result == GameResult.HostWin)
        {
            PlayerMoney -= CurrentBet;
        }
    }

    void PrepareDeck()
    {
        deck.Clear();
        for (int suit = 0; suit < 4; suit++)
        {
            for (int rank = 1; rank <= 13; rank++)
            {
                deck.Add(new Card(suit, rank));
            }
        }
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

    Card Draw()
    {
        Card card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    bool IsBlackJack(List<Card> hand)
    {
        if (hand.Count != 2) return false;

        bool hasAce = (hand[0].Rank == 1 || hand[1].Rank == 1);
        bool hasTenValue = (hand[0].Rank >= 10 && hand[0].Rank <= 13) || (hand[1].Rank >= 10 && hand[1].Rank <= 13);

        return hasAce && hasTenValue;
    }

    public bool CanDoubleDown()
    {
        return PlayerHand.Count == 2 && PlayerMoney >= CurrentBet;
    }

    public void DoubleDown()
    {
        CurrentBet *= 2;
        PlayerHand.Add(Draw());
    }

    int GetCardValue(Card card)
    {
        if (card.Rank == 1)
        {
            return ACE_HIGH;
        }
        if (card.Rank >= 11)
        {
            return JQK;
        }
        return card.Rank;
    }

    int ValueHand(List<Card> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            int value = GetCardValue(card);
            if (value == ACE_HIGH)
            {
                aceCount++;
            }
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