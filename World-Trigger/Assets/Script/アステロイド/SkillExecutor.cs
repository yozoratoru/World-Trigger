using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance { get; private set; }

    [Header("スキルコントローラー参照")]
    [SerializeField] private AsteroidController asteroidController;
    [SerializeField] private GrasshopperSkill grasshopperSkill;

    [Header("アステロイドの親トランスフォーム（torion）")]
    [SerializeField] private Transform torion;  // ← ここを Inspector で設定してください

    private string activeSkillName = "";  // 現在有効なスキル名

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExecuteSkill(string skillName, KeyCode activationKey)
    {
        Debug.Log($"SkillExecutor: {skillName} をキー {activationKey} で実行");

        // スキル切り替え時にすべて無効化（かつ不要なオブジェクト削除）
        DisableAllSkills();

        if (skillName == "アステロイド")
        {
            if (asteroidController != null)
            {
                asteroidController.SetActivationKey(activationKey);
                activeSkillName = "アステロイド";
            }
            else
            {
                Debug.LogWarning("AsteroidController が未設定です！");
            }
        }
        else if (skillName == "グラスホッパー")
        {
            if (grasshopperSkill != null)
            {
                grasshopperSkill.SetActivationKey(activationKey);
                activeSkillName = "グラスホッパー";
            }
            else
            {
                Debug.LogWarning("GrasshopperSkill が未設定です！");
            }
        }
        else
        {
            Debug.LogWarning($"未対応のスキル: {skillName}");
        }
    }

    private void DisableAllSkills()
    {
        // アステロイドの無効化
        if (asteroidController != null)
        {
            asteroidController.SetActivationKey(KeyCode.None);
        }

        // グラスホッパーの無効化
        if (grasshopperSkill != null)
        {
            grasshopperSkill.SetActivationKey(KeyCode.None);
        }

        // ★ アステロイドがアクティブだった場合、torionの子オブジェクト削除
        if (activeSkillName == "アステロイド" && torion != null)
        {
            for (int i = torion.childCount - 1; i >= 0; i--)
            {
                Destroy(torion.GetChild(i).gameObject);
            }

            Debug.Log("[SkillExecutor] torion の子オブジェクトを削除しました。");
        }

        activeSkillName = "";
    }
}
