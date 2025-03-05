using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public Text rightSkillText; // 右スキルのUI表示
    public Text leftSkillText;  // 左スキルのUI表示
    public InputField skillInput; // スキル入力欄

    void Start()
    {
        UpdateSkillUI();
    }

    // スキル追加ボタン処理
    public void AddSkillButton(bool isRight)
    {
        string skillName = skillInput.text;
        if (!string.IsNullOrEmpty(skillName))
        {
            bool added = DataManager.Instance.AddSkill(skillName, isRight);
            if (added)
            {
                UpdateSkillUI();
            }
            else
            {
                Debug.Log("スキルの上限(4つ)に達しています");
            }
        }
    }

    // スキル削除ボタン処理
    public void RemoveSkillButton(bool isRight)
    {
        string skillName = skillInput.text;
        if (!string.IsNullOrEmpty(skillName))
        {
            DataManager.Instance.RemoveSkill(skillName, isRight);
            UpdateSkillUI();
        }
    }

    // UI更新
    void UpdateSkillUI()
    {
        rightSkillText.text = "右スキル: " + string.Join(", ", DataManager.Instance.GetSkills(true));
        leftSkillText.text = "左スキル: " + string.Join(", ", DataManager.Instance.GetSkills(false));
    }
}
