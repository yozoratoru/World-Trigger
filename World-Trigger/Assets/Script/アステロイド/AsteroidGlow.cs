using UnityEngine;

public class AsteroidGlow : MonoBehaviour
{
    public Material asteroidMaterial;
    private bool isCharging = false;
    private float glowIntensity = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.C)) // C長押し
        {
            isCharging = true;
            glowIntensity += Time.deltaTime * 0.1f;
            glowIntensity = Mathf.Clamp(glowIntensity, 0f, 5f);

            // Shader Graphに強度を渡す
            asteroidMaterial.SetFloat("_EmissionStrength", glowIntensity);
        }
        else if (isCharging)
        {
            isCharging = false;
            glowIntensity = 0f;
            asteroidMaterial.SetFloat("_EmissionStrength", glowIntensity);
        }
    }
}
