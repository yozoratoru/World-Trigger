using UnityEngine;

public class GrasshopperSkill : MonoBehaviour
{
    public GameObject grasshopperPrefab;       // グラスホッパーのプレハブ
    public Transform grasshopperPoint;         // 足元の設置ポイント
    public float cooldownTime = 5f;            // クールダウン時間（秒）
    private float nextUseTime = 0f;            // 次に使用可能な時間

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && Time.time >= nextUseTime)
        {
            // グラスホッパーを足元に設置
            GameObject instance = Instantiate(grasshopperPrefab, grasshopperPoint.position, Quaternion.identity);

            // 1秒後に自動で削除
            Destroy(instance, 1f);

            // クールダウンを設定
            nextUseTime = Time.time + cooldownTime;
        }
    }
}
