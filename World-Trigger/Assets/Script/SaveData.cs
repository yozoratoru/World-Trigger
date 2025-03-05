using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int TotalCoins;
    public List<string> RightSkills = new List<string>(4); // 右用スキル（最大4つ）
    public List<string> LeftSkills = new List<string>(4);  // 左用スキル（最大4つ）
}
