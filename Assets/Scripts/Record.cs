using System;

// 記録を保持するクラス(JsonUtilityによる保存・読み込み対象として使用)
[Serializable]
public class Record
{
    public string gameType;              // ゲームの種別の判断
    public int playCount = 0;            // プレイ回数(初期値は0)
    public int winCount = 0;             // 勝利回数(初期値は0)
    public int loseCount = 0;            // 敗北回数(初期値は0)
    public int totalBlackJackCount = 0;  // トータルブラックジャック回数(初期値は0)
}