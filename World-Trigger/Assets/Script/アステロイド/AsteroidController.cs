using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public Transform spawnPoint;
    public float shootForce = 10f;
    public float shootInterval = 0.1f;

    private GameObject currentAsteroid;
    private float holdTime = 0f;
    private int currentLevel = 0;
    private bool isCharging = false;
    private bool isConfirmed = false;
    private KeyCode activationKey = KeyCode.None;

    private List<Transform> asteroidParts = new List<Transform>();
    private int currentShootIndex = 0;
    private float shootTimer = 0f;

    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
    }

    void Update()
    {
        if (activationKey == KeyCode.None) return;

        if (Input.GetKeyDown(activationKey))
        {
            StartCharging();
        }

        if (Input.GetKey(activationKey) && isCharging)
        {
            holdTime += Time.deltaTime;
            UpdateAsteroid();

            if (currentAsteroid != null)
            {
                currentAsteroid.transform.position = spawnPoint.position;
                currentAsteroid.transform.rotation = spawnPoint.rotation;
            }
        }

        if (Input.GetKeyUp(activationKey) && isCharging)
        {
            ConfirmAsteroid();
        }

        if (Input.GetMouseButton(0) && isConfirmed && asteroidParts.Count > 0)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval && currentShootIndex < asteroidParts.Count)
            {
                ShootCube(asteroidParts[currentShootIndex], currentShootIndex);
                currentShootIndex++;
                shootTimer = 0f;
            }
        }
    }

    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;
        isConfirmed = false;
        currentShootIndex = 0;
        asteroidParts.Clear();

        if (currentAsteroid != null)
        {
            Destroy(currentAsteroid);
        }

        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
    }

    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
        }
    }

    // チャージ完了後、各CubeにRigidbody・Colliderをつけて準備
    void ConfirmAsteroid()
    {
        isCharging = false;
        isConfirmed = true;
        currentShootIndex = 0;
        asteroidParts.Clear();

        if (currentAsteroid != null)
        {
            Transform[] allChildren = currentAsteroid.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in allChildren)
            {
                if (t.name.StartsWith("Cube"))
                {
                    t.gameObject.SetActive(true);
                    asteroidParts.Add(t);

                    Rigidbody rb = t.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = t.gameObject.AddComponent<Rigidbody>();
                        Debug.Log($"[追加] Rigidbody: {t.name}");
                    }

                    rb.isKinematic = false;

                    // すべての軸・回転を一旦固定（発射前）
                    rb.constraints = RigidbodyConstraints.FreezeAll;

                    if (t.GetComponent<Collider>() == null)
                    {
                        t.gameObject.AddComponent<BoxCollider>();
                        Debug.Log($"[追加] Collider: {t.name}");
                    }
                }
            }

            Debug.Log($"[確認完了] Cube数: {asteroidParts.Count}");
        }
    }

    // Cubeを発射（XZ移動を許可し、Y軸と回転は固定）
    void ShootCube(Transform cube, int index)
    {
        if (cube == null) return;

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 発射時：XZの固定を解除、Yと回転は固定のまま
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            // 少しずらして衝突を防止
            cube.position += new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.05f, 0.05f));

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 shootDirection = Camera.main.transform.forward;
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

            Debug.Log($"[発射] Cube {index}: {cube.name} 位置: {cube.position}");
        }
    }
}
