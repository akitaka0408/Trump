using System;

// ミッションを管理するためのクラス
[Serializable]
public class Mission
{
    public string missionID;   // 例: "Win_1", "Win_10"
    public bool isCleared;     // 達成済みか
}
