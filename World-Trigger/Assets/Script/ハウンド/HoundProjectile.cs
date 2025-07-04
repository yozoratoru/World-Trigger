using UnityEngine;

public class HoundProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float trackingStrength = 10f;
    public float maxLifeTime = 5f;
    public string targetTag = "Enemy";

    private Rigidbody rb;
    private Transform target;
    private float lifeTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindNearestTarget();
    }

    void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;
        if (lifeTimer > maxLifeTime) Destroy(gameObject);

        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * speed, trackingStrength * Time.fixedDeltaTime);
        }
    }

    void FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                target = enemy.transform;
            }
        }
    }
}
