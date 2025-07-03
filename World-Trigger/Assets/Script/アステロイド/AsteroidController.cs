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
    private bool justSet = false;

    private List<Transform> asteroidParts = new List<Transform>();
    private int currentShootIndex = 0;
    private float shootTimer = 0f;

    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
        justSet = true; // スキル切り替え直後の誤発動防止
    }

    void Update()
    {
        if (activationKey == KeyCode.None) return;

        if (justSet)
        {
            justSet = false;
            return;
        }

        // キーを押した瞬間：チャージ開始
        if (Input.GetKeyDown(activationKey))
        {
            StartCharging();
        }

        // チャージ中：レベルアップ、位置追従
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

        // キーを離した瞬間：チャージ終了
        if (Input.GetKeyUp(activationKey) && isCharging)
        {
            ConfirmAsteroid();
        }

        // 左クリック：分離Cubeを順に発射
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
        currentAsteroid.transform.SetParent(spawnPoint);
    }

    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation);
            currentAsteroid.transform.SetParent(spawnPoint);
        }
    }

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

                    // Rigidbody
                    Rigidbody rb = t.GetComponent<Rigidbody>();
                    if (rb == null) rb = t.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.FreezeAll;

                    // Collider
                    if (t.GetComponent<Collider>() == null)
                    {
                        t.gameObject.AddComponent<BoxCollider>();
                    }
                }
            }
        }
    }

    void ShootCube(Transform cube, int index)
    {
        if (cube == null) return;

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            cube.SetParent(null);
            cube.position += new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.05f, 0.05f));
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 shootDirection = Camera.main.transform.forward;
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
        }
    }
}
