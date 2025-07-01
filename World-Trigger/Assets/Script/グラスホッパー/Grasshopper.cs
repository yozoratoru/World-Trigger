using UnityEngine;

public class Grasshopper : MonoBehaviour
{
    public float jumpPower = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーのカメラ方向を使ってジャンプ方向を決定
            Vector3 dir = Camera.main.transform.forward;
            dir.y = 0f; // 上方向の要素は手動で加えるため水平方向だけ使う
            dir.Normalize();

            Vector3 jumpVec = dir * jumpPower + Vector3.up * (jumpPower / 2f);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // 一旦速度リセット
                rb.AddForce(jumpVec, ForceMode.VelocityChange);
            }

            // 一度発動したら削除（使い捨て）
            Destroy(gameObject);
        }
    }
}
