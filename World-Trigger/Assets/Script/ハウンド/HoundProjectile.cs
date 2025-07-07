using UnityEngine;

public class HoundProjectile : MonoBehaviour
{
    public float speed = 5f;                   // 弾の速度
    public float trackingStrength = 10f;       // 追尾の強さ
    public float maxLifeTime = 5f;             // 弾の寿命
    public float trackingDuration = 2f;        // 追尾する時間（秒）
    public string targetTag = "Enemy";         // 追尾対象のタグ

    private Rigidbody rb;                      // 弾の物理ボディ
    private Transform target;                  // 現在のターゲット
    private float lifeTimer = 0f;              // 経過時間
    private bool trackingActive = true;        // 追尾中かどうか

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindNearestTarget();
    }

    void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;

        // 一定時間経過で弾を削除
        if (lifeTimer > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // 追尾期間が終了したらフラグをオフにする
        if (lifeTimer > trackingDuration)
        {
            trackingActive = false;
        }

        // 追尾中のみターゲットに向けて進行方向を補正
        if (trackingActive && target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * speed, trackingStrength * Time.fixedDeltaTime);
        }
    }

    // 最も近い敵を探す
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
