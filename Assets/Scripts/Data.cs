using System;
using System.Collections.Generic;

// ゲーム全体のセーブデータを保持するクラス(JsonUtilityによる保存・読み込み対象として使用)
[Serializable]
public class Data
{
    public List<Record> records = new List<Record>();  // 戦績データを格納するリスト。
    public bool BGM = true;                            // BGMの ON/OFF(初期値はON)
    public bool SE = true;                             // SEの ON/OFF(初期値はON)
    public int money = 1000;                           // プレイヤーの所持金(初期値は1000)
}