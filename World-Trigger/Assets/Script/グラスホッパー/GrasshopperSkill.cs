using UnityEngine;
using CameraSample.Scripts._3D; // PlayerController3D が定義されている名前空間

public class GrasshopperSkill : MonoBehaviour
{
    [Header("スキル設定")]
    public GameObject grasshopperPrefab;
    public Transform grasshopperPoint;
    public float cooldownTime = 5f;

    [Header("プレイヤーコントローラー参照")]
    public PlayerController3D playerController;

    private KeyCode activationKey = KeyCode.None;
    private bool justSet = false;
    private float nextUseTime = 0f;

    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
        justSet = true; // 発動をワンフレーム抑制
    }

    void Update()
    {
        // スキルがSetされた直後のフレームは無視
        if (justSet)
        {
            justSet = false;
            return;
        }

        // 発動条件：キーが押された、かつクールダウン中でない
        if (activationKey != KeyCode.None && Input.GetKeyDown(activationKey) && Time.time >= nextUseTime)
        {
            Execute();
        }
    }

    private void Execute()
    {
        Debug.Log("[GrasshopperSkill] グラスホッパーを発動！");

        // プレハブを生成（オプション）
        if (grasshopperPrefab && grasshopperPoint)
        {
            GameObject instance = Instantiate(grasshopperPrefab, grasshopperPoint.position, Quaternion.identity);
            Destroy(instance, 1f); // 自動削除
        }

        // 入力方向取得（WASD）
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDir += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputDir += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputDir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputDir += Vector3.right;

        inputDir = inputDir.normalized;

        // 入力がないときは真上、あるときはカメラの向きを基準に変換
        Vector3 moveDir = (inputDir == Vector3.zero) ? Vector3.up : transform.TransformDirection(inputDir);

        // プレイヤーにスキルを適用
        if (playerController != null)
        {
            playerController.ActivateGrasshopper(moveDir);
        }

        // クールダウン更新
        nextUseTime = Time.time + cooldownTime;
    }
}
