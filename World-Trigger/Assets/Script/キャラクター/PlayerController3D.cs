using UnityEngine;

namespace CameraSample.Scripts._3D
{
    public class PlayerController3D : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -20f;

        [Header("入力設定")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode forwardKey = KeyCode.W;
        [SerializeField] private KeyCode backwardKey = KeyCode.S;

        [Header("カメラ設定")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private float mouseSmoothTime = 0.05f;

        private float xRotation = 0f;
        private Vector2 currentMouseDelta;
        private Vector2 currentMouseDeltaVelocity;

        private CharacterController characterController;
        private Vector3 velocity;
        private bool isGrounded;

        private void Start()
        {
            // CharacterControllerの取得または追加
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
                characterController.height = 2f;
                characterController.radius = 0.5f;
            }

            // カーソルロック
            Cursor.lockState = CursorLockMode.Locked;

            // カメラの設定（自動取得）
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            HandleCameraLook();
            HandleMovementAndJump();
        }

        private void HandleCameraLook()
        {
            // マウス入力の取得
            Vector2 targetMouseDelta = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

            // スムーズに補間
            currentMouseDelta = Vector2.SmoothDamp(
                currentMouseDelta,
                targetMouseDelta,
                ref currentMouseDeltaVelocity,
                mouseSmoothTime
            );

            // 垂直回転（カメラ）
            xRotation -= currentMouseDelta.y * mouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }

            // 水平回転（プレイヤー本体）
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity * Time.deltaTime);
        }

        private void HandleMovementAndJump()
        {
            isGrounded = characterController.isGrounded;

            // 入力
            float horizontalInput = 0f;
            float verticalInput = 0f;
            if (Input.GetKey(leftKey)) horizontalInput = -1f;
            if (Input.GetKey(rightKey)) horizontalInput = 1f;
            if (Input.GetKey(forwardKey)) verticalInput = 1f;
            if (Input.GetKey(backwardKey)) verticalInput = -1f;

            // 移動方向を作成
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed;

            // 接地時の処理
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // ジャンプ入力
            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            // 重力の適用
            velocity.y += gravity * Time.deltaTime;

            // 移動にY成分を加える
            move.y = velocity.y;

            // 移動
            characterController.Move(move * Time.deltaTime);
        }

        // 外部から情報取得用プロパティ
        public Vector3 Position => transform.position;
        public bool IsGrounded => isGrounded;
        public float VelocityY => velocity.y;
    }
}
