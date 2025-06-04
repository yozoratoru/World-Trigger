using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public Transform spawnPoint; // ← 追加：空のオブジェクトをInspectorから指定
    private GameObject currentAsteroid;

    private float holdTime = 0f;
    private int currentLevel = 0;
    private bool isCharging = false;

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

            // チャージ中も位置を追従させたい場合
            if (currentAsteroid != null)
            {
                currentAsteroid.transform.position = spawnPoint.position;
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            ConfirmAsteroid(); // ← ここで確定処理を実行
        }
    }

    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;

        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, Quaternion.identity);
    }

    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, Quaternion.identity);
        }
    }

    void ConfirmAsteroid()
    {
        isCharging = false;

        if (currentAsteroid != null)
        {
            Animator animator = currentAsteroid.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Start"); // Animatorの"Start"トリガーを使ってアニメーション開始
            }
        }
    }
}