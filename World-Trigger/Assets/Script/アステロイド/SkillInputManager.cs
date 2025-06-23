using UnityEngine;

public class SkillInputManager : MonoBehaviour
{
    private bool isRightSkillMode = false;  // Q押されたらtrue
    private bool isLeftSkillMode = false;   // E押されたらtrue

    private SaveData data;

    void Start()
    {
        data = DataManager.Instance.data;
    }

    void Update()
    {
        // QかEの判定（どちらか一つだけ許可）
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isRightSkillMode = true;
            isLeftSkillMode = false;
            Debug.Log("右手スキルモードに切替");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            isRightSkillMode = false;
            isLeftSkillMode = true;
            Debug.Log("左手スキルモードに切替");
        }

        // QかEを押した後のみZ/X/C/Vの判定をする
        if (isRightSkillMode || isLeftSkillMode)
        {
            int slotIndex = -1;

            if (Input.GetKeyDown(KeyCode.Z)) slotIndex = 0;
            else if (Input.GetKeyDown(KeyCode.X)) slotIndex = 1;
            else if (Input.GetKeyDown(KeyCode.C)) slotIndex = 2;
            else if (Input.GetKeyDown(KeyCode.V)) slotIndex = 3;

            if (slotIndex != -1)
            {
                string skillName = "";

                if (isRightSkillMode)
                {
                    skillName = data.skillSet.rightSkills[slotIndex];
                }
                else if (isLeftSkillMode)
                {
                    skillName = data.skillSet.leftSkills[slotIndex];
                }

                if (!string.IsNullOrEmpty(skillName))
                {
                    // 修正ポイント：キーコードを正しく取得
                    KeyCode[] keys = { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V };
                    KeyCode key = keys[slotIndex];

                    Debug.Log($"選択スキル: {skillName}");
                    SkillExecutor.Instance.ExecuteSkill(skillName, key);
                }
                else
                {
                    Debug.Log("スキルがセットされていません");
                }

                // 一回判定したらモードリセット（連続判定防止）
                isRightSkillMode = false;
                isLeftSkillMode = false;
            }
        }
    }
}
