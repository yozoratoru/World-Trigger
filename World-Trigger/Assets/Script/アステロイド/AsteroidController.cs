using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; 
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

}
