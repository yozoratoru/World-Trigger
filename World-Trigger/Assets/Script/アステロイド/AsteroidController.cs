using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // アステロイドのプレハブ（チャージレベルごとに複数用意）
    public GameObject[] asteroidPrefabs;

    // 発射位置（プレイヤーの前など）
    public Transform spawnPoint;

    // 発射時の力
    public float shootForce = 10f;

    // 現在チャージ中または準備済みのアステロイド
    private GameObject currentAsteroid;

    // チャージ時間
    private float holdTime = 0f;

    // 現在のチャージレベル（0〜最大）
    private int currentLevel = 0;

    // チャージ中かどうかのフラグ
    private bool isCharging = false;

    // チャージが確定されたかどうか（発射可能状態）
    private bool isConfirmed = false;

    // 発動に使うキー（Z/X/C/Vのどれか）
    private KeyCode activationKey = KeyCode.None;

    // 外部（SkillExecutorなど）から使用キーを設定する
    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
    }

    void Update()
    {
        // キーが設定されていなければ何もしない
        if (activationKey == KeyCode.None) return;

        // チャージ開始（キー押下）
        if (Input.GetKeyDown(activationKey))
        {
            StartCharging();
        }

        // チャージ継続（キー押し続け）
        if (Input.GetKey(activationKey) && isCharging)
        {
            holdTime += Time.deltaTime;
            UpdateAsteroid();  // チャージレベルに応じて見た目を変更

            // アステロイドの位置を常に発射位置に維持
            if (currentAsteroid != null)
            {
                currentAsteroid.transform.position = spawnPoint.position;
                currentAsteroid.transform.rotation = spawnPoint.rotation;
            }
        }

        // チャージ確定（キー離す）
        if (Input.GetKeyUp(activationKey) && isCharging)
        {
            ConfirmAsteroid();
        }

        // 左クリックで発射
        if (Input.GetMouseButtonDown(0) && isConfirmed)
        {
            ShootAsteroid();
        }
    }

    // チャージ開始時の初期化
    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;
        isConfirmed = false;

        // 既にアステロイドが存在すれば破棄
        if (currentAsteroid != null)
        {
            Destroy(currentAsteroid);
        }

        // 最小レベルのアステロイドを生成
        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
    }

    // チャージ時間に応じてアステロイドのレベルを更新
    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            // レベルが変わったら古いのを破棄して新しく生成
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
        }
    }

    // チャージを確定して発射可能な状態にする
void ConfirmAsteroid()
{
    isCharging = false;
    isConfirmed = true;

    if (currentAsteroid != null && currentAsteroid.GetComponent<Rigidbody>() == null)
    {
        Rigidbody rb = currentAsteroid.AddComponent<Rigidbody>();

        // Z軸の動きを固定（落下や移動を防止）
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
}

    // アステロイドを前方に発射
    void ShootAsteroid()
    {
        if (currentAsteroid != null)
        {
            Rigidbody rb = currentAsteroid.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 速度リセット
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // カメラの前方に向かって力を加える
                Vector3 shootDirection = Camera.main.transform.forward;
                rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

                // 状態リセット
                currentAsteroid = null;
                isConfirmed = false;
                activationKey = KeyCode.None;  // 次回発動まで無効化
            }
        }
    }
}
