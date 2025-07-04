using UnityEngine;

public class BindProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float stunDuration = 3f;
    public string targetTag = "Enemy";

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.linearVelocity = Vector3.zero;
                enemyRb.constraints = RigidbodyConstraints.FreezeAll;
                StartCoroutine(Release(enemyRb));
            }
        }

        Destroy(gameObject);
    }

    System.Collections.IEnumerator Release(Rigidbody enemyRb)
    {
        yield return new WaitForSeconds(stunDuration);
        if (enemyRb != null)
        {
            enemyRb.constraints = RigidbodyConstraints.None;
        }
    }
}
