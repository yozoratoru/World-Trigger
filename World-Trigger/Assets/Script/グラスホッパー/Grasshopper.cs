using UnityEngine;

namespace CameraSample.Scripts._3D
{
    public class Grasshopper : MonoBehaviour
    {
        [Header("グラスホッパー設定")]
        [SerializeField] private float blinkDistance = 5f;
        [SerializeField] private float cooldownTime = 3f;
        [SerializeField] private KeyCode skillKey = KeyCode.V;

        private float cooldownTimer = 0f;
        private PlayerController3D playerController;

        private void Start()
        {
            // プレイヤーコントローラー取得
            playerController = GetComponent<PlayerController3D>();
            if (playerController == null)
            {
                Debug.LogError("Grasshopper: PlayerController3D が必要です。");
            }
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;

            if (Input.GetKeyDown(skillKey) && cooldownTimer <= 0f)
            {
                TryBlink();
            }
        }

        private void TryBlink()
        {
            // 地面にいないならスキル発動不可
            if (!playerController.IsGrounded) return;

            // 前方向にブリンク
            Vector3 blinkDirection = transform.forward;
            Vector3 targetPosition = transform.position + blinkDirection * blinkDistance;

            // 壁貫通が気になる場合は Raycast などで補正可能
            transform.position = targetPosition;

            // クールダウン開始
            cooldownTimer = cooldownTime;
        }
    }
}
