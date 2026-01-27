using System;

// 戦績を保持するクラス(JsonUtilityによる保存・読み込み対象として使用)
[Serializable]
public class Record
{
    public string GameType;    // ゲームの種別の判断
    public int PlayCount = 0;  // プレイ回数(初期値は0)
    public int WinCount = 0;   // 勝利回数(初期値は0)
    public int LoseCount = 0;  // 敗北回数(初期値は0)
}