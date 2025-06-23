using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance { get; private set; }

    [SerializeField] private AsteroidController asteroidController;

    void Awake()
    {
        // Singleton pattern
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

        if (skillName == "アステロイド") // 日本語名に対応
        {
            if (asteroidController != null)
            {
                asteroidController.SetActivationKey(activationKey);
            }
            else
            {
                Debug.LogWarning("AsteroidController が未設定です！");
            }
        }
        else
        {
            Debug.LogWarning($"未対応のスキル: {skillName}");
        }
    }
}
