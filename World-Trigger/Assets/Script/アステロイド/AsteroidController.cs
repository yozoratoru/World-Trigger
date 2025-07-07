using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // アステロイドのプレハブ（チャージレベル別に設定）
    public GameObject[] asteroidPrefabs;

    // アステロイドを生成する位置（カメラ前など）
    public Transform spawnPoint;

    // 発射時の力
    public float shootForce = 10f;

    // 各Cube発射の間隔（秒）
    public float shootInterval = 0.1f;

    // 弾の種類
    public enum BulletType
    {
        Normal,
        Hound,
        Explosion,
        Bind
    }

    // 現在選択されている弾の種類
    public BulletType currentBulletType = BulletType.Normal;

    private GameObject currentAsteroid; // チャージ中のアステロイド
    private float holdTime = 0f;        // チャージ時間
    private int currentLevel = 0;       // チャージ段階（0〜）
    private bool isCharging = false;    // チャージ中かどうか
    private bool isConfirmed = false;   // チャージ確定済みか

    private KeyCode activationKey = KeyCode.None; // 発動キー
    private bool justSet = false;                 // キー設定直後に誤動作しないようにするフラグ

    private List<Transform> asteroidParts = new List<Transform>(); // 分離Cubeのリスト
    private int currentShootIndex = 0;     // 次に発射するCubeのインデックス
    private float shootTimer = 0f;         // 発射タイマー

    // 発動キーを外部から設定する関数
    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
        justSet = true; // 次のフレームでは無視する
    }

    void Update()
    {
        if (activationKey == KeyCode.None) return; // キー未設定なら何もしない

        if (justSet)
        {
            justSet = false;
            return;
        }

        // チャージ開始
        if (Input.GetKeyDown(activationKey))
        {
            StartCharging();
        }

        // チャージ中の更新処理
        if (Input.GetKey(activationKey) && isCharging)
        {
            holdTime += Time.deltaTime; // チャージ時間加算
            UpdateAsteroid();          // レベルに応じてプレハブ切り替え

            if (currentAsteroid != null)
            {
                // チャージ中は位置と回転を常に更新
                currentAsteroid.transform.position = spawnPoint.position;
                currentAsteroid.transform.rotation = spawnPoint.rotation;
            }
        }

        // チャージ終了・確定
        if (Input.GetKeyUp(activationKey) && isCharging)
        {
            ConfirmAsteroid();
        }

        // Cubeの順次発射（マウス左クリック中）
        if (Input.GetMouseButton(0) && isConfirmed && asteroidParts.Count > 0)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval && currentShootIndex < asteroidParts.Count)
            {
                ShootCube(asteroidParts[currentShootIndex]);
                currentShootIndex++;
                shootTimer = 0f;
            }
        }
    }

    // チャージ開始処理
    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;
        isConfirmed = false;
        currentShootIndex = 0;
        asteroidParts.Clear();

        // 既存アステロイドがあれば破棄
        if (currentAsteroid != null)
        {
            Destroy(currentAsteroid);
        }

        // 初期レベルのアステロイドを生成して親に付ける
        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
        currentAsteroid.transform.SetParent(spawnPoint);
    }

    // チャージ時間に応じたプレハブ切り替え
    void UpdateAsteroid()
    {
        // チャージ時間に応じて新しいレベルを計算
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        // レベルが変化したらプレハブを差し替え
        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
            currentAsteroid.transform.SetParent(spawnPoint);
        }
    }

    // チャージ確定：分離Cubeを登録・弾種の挙動を付与
    void ConfirmAsteroid()
    {
        isCharging = false;
        isConfirmed = true;
        currentShootIndex = 0;
        asteroidParts.Clear();

        if (currentAsteroid != null)
        {
            // 子オブジェクトの中で「Cube」で始まるものを取得
            Transform[] allChildren = currentAsteroid.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in allChildren)
            {
                if (t.name.StartsWith("Cube"))
                {
                    t.gameObject.SetActive(true);
                    asteroidParts.Add(t);

                    // Rigidbody付与（なければ）
                    Rigidbody rb = t.GetComponent<Rigidbody>();
                    if (rb == null) rb = t.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.FreezeAll; // 発射前は固定

                    // Colliderも追加（なければ）
                    if (t.GetComponent<Collider>() == null)
                        t.gameObject.AddComponent<BoxCollider>();

                    // 弾タイプに応じてスクリプトを追加
                    AddBulletBehavior(t.gameObject);
                }
            }
        }
    }

    // 弾のタイプに応じたスクリプトを追加
    void AddBulletBehavior(GameObject cube)
    {
        switch (currentBulletType)
        {
            case BulletType.Hound:
                cube.AddComponent<HoundProjectile>();
                break;
            case BulletType.Explosion:
                cube.AddComponent<ExplosionProjectile>();
                break;
            case BulletType.Bind:
                cube.AddComponent<BindProjectile>();
                break;
            case BulletType.Normal:
            default:
                cube.AddComponent<NormalProjectile>();
                break;
        }
    }

    // Cubeを順に発射
    void ShootCube(Transform cube)
{
    if (cube == null) return;

    Rigidbody rb = cube.GetComponent<Rigidbody>();
    if (rb != null)
    {
        // 子オブジェクトから切り離す
        cube.SetParent(null);

        // 発射位置を少しランダムにずらす（密集しすぎないように）
        cube.position += new Vector3(
            Random.Range(-0.05f, 0.05f),
            Random.Range(-0.05f, 0.05f),
            Random.Range(-0.05f, 0.05f)
        );

        // 速度リセット
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 重力を無効にする（角度を保って飛ばす）
        rb.useGravity = false;

        // 回転だけ固定（自由に移動できるように）
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // 空気抵抗なし（必要に応じて）
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        // カメラの向いている方向に力を加える（角度を維持して飛ばす）
        rb.AddForce(Camera.main.transform.forward * shootForce, ForceMode.Impulse);
    }
}

}
