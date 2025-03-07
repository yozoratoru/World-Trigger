using System;

[Serializable]
public class SkillData
{
    public string skillName; // スキル名
}

[Serializable]
public class SkillSetData
{
    public string[] rightSkills = new string[4]; // 右スキル4枠
    public string[] leftSkills = new string[4];  // 左スキル4枠
}

[Serializable]
public class SaveData
{
    public int TotalCoins = 100; // 初期コイン
    public SkillSetData skillSet = new SkillSetData(); // スキルセット情報
}
