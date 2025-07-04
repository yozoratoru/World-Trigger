using UnityEngine;

public class NormalProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) Destroy(gameObject);
    }
}
