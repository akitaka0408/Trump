// カードを表すクラス
public class Card
{
    public int Suit;  // カードのスート(0:クラブ, 1:ダイヤ, 2:ハート, 3:スペード)
    public int Rank;  // カードの数字

    // コンストラクタ
    public Card(int suit, int rank)
    {
        Suit = suit;
        Rank = rank;
    }
}


