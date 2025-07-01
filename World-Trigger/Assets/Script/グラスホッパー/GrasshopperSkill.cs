using UnityEngine;

public class GrasshopperSkill : MonoBehaviour
{
    public GameObject grasshopperPrefab;       // グラスホッパーのプレハブ
    public Transform grasshopperPoint;         // 足元の設置ポイント
    public float jumpPower = 10f;              // 飛ばす力
    public float cooldownTime = 5f;            // クールダウン
    private float nextUseTime = 0f;            // 次に使える時間

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && Time.time >= nextUseTime)
        {
            // グラスホッパーを設置
            Instantiate(grasshopperPrefab, grasshopperPoint.position, Quaternion.identity);

            // クールダウン更新
            nextUseTime = Time.time + cooldownTime;
        }
    }
}
