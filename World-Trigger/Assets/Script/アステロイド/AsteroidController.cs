using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // 0:1分割, 1:4分割, 2:8分割, 3:16分割,
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
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            ReleaseAsteroid();
        }
    }

    void StartCharging()
    {
        holdTime = 0f;
        currentLevel = 0;
        isCharging = true;
        currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], transform.position, Quaternion.identity);
    }

    void UpdateAsteroid()
    {
        int newLevel = Mathf.Min((int)(holdTime), asteroidPrefabs.Length - 1);

        if (newLevel != currentLevel)
        {
            Destroy(currentAsteroid);
            currentLevel = newLevel;
            currentAsteroid = Instantiate(asteroidPrefabs[currentLevel], transform.position, Quaternion.identity);
        }
    }

    void ReleaseAsteroid()
    {
        isCharging = false;
        if (currentAsteroid != null)
        {
            // 発射処理（Rigidbodyを加えるなど）
            Rigidbody rb = currentAsteroid.AddComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * 10f;
            // Freeze RotationをXYZすべて固定
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
