using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Button[] skillButtons; // スキル一覧のボタン
    public Button[] rightSkillSlots; // 右スキル（＋）ボタン
    public Button[] leftSkillSlots;  // 左スキル（＋）ボタン

    private string selectedSkill = ""; // 現在選択中のスキル

    void Start()
    {
        // スキルボタンにリスナーを設定
        foreach (Button btn in skillButtons)
        {
            btn.onClick.AddListener(() => SelectSkill(btn));
        }

        // 右スキルセット
        for (int i = 0; i < rightSkillSlots.Length; i++)
        {
            int index = i;
            rightSkillSlots[i].onClick.AddListener(() => SetSkill(index, true));
        }

        // 左スキルセット
        for (int i = 0; i < leftSkillSlots.Length; i++)
        {
            int index = i;
            leftSkillSlots[i].onClick.AddListener(() => SetSkill(index, false));
        }

        LoadSkillSet();
    }

    // スキル選択
    void SelectSkill(Button btn)
    {
        selectedSkill = btn.GetComponentInChildren<Text>().text;
    }

    // スキルをセット
    void SetSkill(int index, bool isRight)
    {
        if (string.IsNullOrEmpty(selectedSkill)) return;

        if (isRight)
        {
            DataManager.Instance.data.skillSet.rightSkills[index] = selectedSkill;
            rightSkillSlots[index].GetComponentInChildren<Text>().text = selectedSkill;
        }
        else
        {
            DataManager.Instance.data.skillSet.leftSkills[index] = selectedSkill;
            leftSkillSlots[index].GetComponentInChildren<Text>().text = selectedSkill;
        }

        // データ保存
        DataManager.Instance.Save();
    }

    // JSONからスキルセットを復元
    void LoadSkillSet()
    {
        var skillSet = DataManager.Instance.data.skillSet;

        for (int i = 0; i < rightSkillSlots.Length; i++)
        {
            if (!string.IsNullOrEmpty(skillSet.rightSkills[i]))
            {
                rightSkillSlots[i].GetComponentInChildren<Text>().text = skillSet.rightSkills[i];
            }
        }

        for (int i = 0; i < leftSkillSlots.Length; i++)
        {
            if (!string.IsNullOrEmpty(skillSet.leftSkills[i]))
            {
                leftSkillSlots[i].GetComponentInChildren<Text>().text = skillSet.leftSkills[i];
            }
        }
    }
}
