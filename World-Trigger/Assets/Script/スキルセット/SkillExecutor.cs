using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance { get; private set; }

    [Header("スキルコントローラー参照")]
    [SerializeField] private AsteroidController asteroidController;
    [SerializeField] private GrasshopperSkill grasshopperSkill;

    [Header("アステロイドの親トランスフォーム（torion）")]
    [SerializeField] private Transform torion;

    private string activeSkillName = "";

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

        DisableAllSkills();

        if (asteroidController == null && (skillName == "アステロイド" || skillName == "ハウンド" || skillName == "メテオラ"))
        {
            Debug.LogWarning("AsteroidController が未設定です！");
            return;
        }

        switch (skillName)
        {
            case "アステロイド":
                asteroidController.currentBulletType = AsteroidController.BulletType.Normal;
                asteroidController.SetActivationKey(activationKey);
                activeSkillName = "アステロイド";
                break;

            case "ハウンド":
                asteroidController.currentBulletType = AsteroidController.BulletType.Hound;
                asteroidController.SetActivationKey(activationKey);
                activeSkillName = "ハウンド";
                break;

            case "メテオラ":
                asteroidController.currentBulletType = AsteroidController.BulletType.Explosion;
                asteroidController.SetActivationKey(activationKey);
                activeSkillName = "メテオラ";
                break;

            case "グラスホッパー":
                if (grasshopperSkill != null)
                {
                    grasshopperSkill.SetActivationKey(activationKey);
                    activeSkillName = "グラスホッパー";
                }
                else
                {
                    Debug.LogWarning("GrasshopperSkill が未設定です！");
                }
                break;

            default:
                Debug.LogWarning($"未対応のスキル: {skillName}");
                break;
        }
    }

    private void DisableAllSkills()
    {
        if (asteroidController != null)
        {
            asteroidController.SetActivationKey(KeyCode.None);
        }

        if (grasshopperSkill != null)
        {
            grasshopperSkill.SetActivationKey(KeyCode.None);
        }

        if ((activeSkillName == "アステロイド" || activeSkillName == "ハウンド" || activeSkillName == "メテオラ") && torion != null)
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
