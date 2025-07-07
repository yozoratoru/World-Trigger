using UnityEngine;

public class ExplosionProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float damage = 50f;
    public float lifeTime = 5f;
    public string targetTag = "Enemy";

    private Rigidbody rb;
    private float timer = 0f;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!hasLaunched) return;

        timer += Time.deltaTime;
        if (timer > lifeTime) Explode();
    }

    public void Launch(Vector3 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
        hasLaunched = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLaunched) Explode();
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                // ダメージ処理
                Debug.Log($"Damaged {hit.name} for {damage}");
            }

            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
                hitRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
