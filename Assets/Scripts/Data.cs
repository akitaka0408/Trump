using System;
using System.Collections.Generic;

// ゲーム全体のセーブデータを保持するクラス(JsonUtilityによる保存・読み込み対象として使用)
[Serializable]
public class Data
{
    public List<Record> records = new List<Record>();  // 戦績データを格納するリスト。
    public bool bgm = true;                            // BGMの ON/OFF(初期値はON)
    public bool se = true;                             // SEの ON/OFF(初期値はON)
    public int bgmIndex = 0;                           // 流すBGMの添え字(初期値は0)
    public int money = 1000;                           // プレイヤーの所持金(初期値は1000)

    // ミッション用
    public int missionWinCount = 0;      　　　　　　　　// ミッション用勝利数(初期値は0)
    public int missionPlayCount = 0;      　　　　　　　 // ミッション用プレイ回数(初期値は0)
    public int winStreak = 0;                            // 現在の連勝数(初期値は0)
    public int loseStreak = 0;                           // 現在の連敗数(初期値は0)
    public int maxBet = 0;                               // 1回の最大ベット額(初期値は0)
    public int blackjackCount = 0;                       // blackjack回数(初期値は0)

    public List<Mission> missions = new List<Mission>(); // ミッションデータを格納するリスト。
}