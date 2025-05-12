using UnityEngine;
using TMPro;

public class AsteroidChargeUI : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    private GameObject currentAsteroid;

    private float holdTime = 0f;
    private int currentLevel = 0;
    private bool isCharging = false;

    public TextMeshProUGUI levelText;

    // 分割数を定義（順番に1, 8, 27, 64）
    private int[] splitCounts = { 1, 8, 27, 64 };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCharging();
        }

        if (Input.GetKey(KeyCode.C))
        {
            holdTime += Time.deltaTime;
            UpdateAsteroid();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            // ※今後ここで発射処理を追加予定
        }
    }

    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;

        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], transform.position, Quaternion.identity);
        UpdateLevelText();
    }

    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], transform.position, Quaternion.identity);
            UpdateLevelText();
        }
    }

    void UpdateLevelText()
    {
        if (levelText != null && currentLevel < splitCounts.Length)
        {
            levelText.text = $"{splitCounts[currentLevel]} ";
        }
    }
}
