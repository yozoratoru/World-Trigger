using UnityEngine;
using CameraSample.Scripts._3D; // ← PlayerController3D が定義されている名前空間

public class GrasshopperSkill : MonoBehaviour
{
    public GameObject grasshopperPrefab;
    public Transform grasshopperPoint;
    public float cooldownTime = 5f;

    public PlayerController3D playerController;

    private float nextUseTime = 0f;

    //キーカスタマイズ対応
    private KeyCode activationKey = KeyCode.V;

    void Update()
    {
        if (Input.GetKeyDown(activationKey) && Time.time >= nextUseTime)
        {
            if (grasshopperPrefab && grasshopperPoint)
            {
                GameObject instance = Instantiate(grasshopperPrefab, grasshopperPoint.position, Quaternion.identity);
                Destroy(instance, 1f);
            }

            Vector3 inputDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) inputDir += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) inputDir += Vector3.back;
            if (Input.GetKey(KeyCode.A)) inputDir += Vector3.left;
            if (Input.GetKey(KeyCode.D)) inputDir += Vector3.right;

            inputDir = inputDir.normalized;

            Vector3 moveDir;
            if (inputDir == Vector3.zero)
            {
                moveDir = Vector3.up;
            }
            else
            {
                moveDir = transform.TransformDirection(inputDir);
            }

            if (playerController != null)
            {
                playerController.ActivateGrasshopper(moveDir);
            }

            nextUseTime = Time.time + cooldownTime;
        }
    }

    // スキルキーを外部から設定可能に
    public void SetActivationKey(KeyCode key)
    {
        activationKey = key;
    }
}
