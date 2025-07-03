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

        [Header("グラスホッパースキル設定")]
        [SerializeField] private GameObject grasshopperPrefab;
        [SerializeField] private Transform grasshopperPoint;
        [SerializeField] private float grasshopperCooldown = 5f;
        [SerializeField] private KeyCode grasshopperKey = KeyCode.V;

        private float nextGrasshopperUseTime = 0f;

        private float xRotation = 0f;
        private Vector2 currentMouseDelta;
        private Vector2 currentMouseDeltaVelocity;

        private CharacterController characterController;
        private Vector3 velocity;
        private bool isGrounded;

        // スキル用
        private bool isMovementLocked = false;
        private Vector3 externalVelocity = Vector3.zero;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
                characterController.height = 2f;
                characterController.radius = 0.5f;
            }

            Cursor.lockState = CursorLockMode.Locked;

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            HandleCameraLook();
            HandleMovementAndJump();
            HandleGrasshopperSkill();
        }

        private void HandleCameraLook()
        {
            Vector2 targetMouseDelta = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

            currentMouseDelta = Vector2.SmoothDamp(
                currentMouseDelta,
                targetMouseDelta,
                ref currentMouseDeltaVelocity,
                mouseSmoothTime
            );

            xRotation -= currentMouseDelta.y * mouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }

            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity * Time.deltaTime);
        }

        private void HandleMovementAndJump()
        {
            isGrounded = characterController.isGrounded;

            float horizontalInput = 0f;
            float verticalInput = 0f;
            if (Input.GetKey(leftKey)) horizontalInput = -1f;
            if (Input.GetKey(rightKey)) horizontalInput = 1f;
            if (Input.GetKey(forwardKey)) verticalInput = 1f;
            if (Input.GetKey(backwardKey)) verticalInput = -1f;

            Vector3 inputDir = new Vector3(horizontalInput, 0, verticalInput).normalized;
            Vector3 move = transform.TransformDirection(inputDir) * moveSpeed;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            // === 合成移動（スキル + 通常移動 + 重力）===
            Vector3 totalMove;
            if (isMovementLocked)
            {
                totalMove = externalVelocity; // スキル発動中はそのまま使う（yも上書きしない）
            }
            else
            {
                totalMove = move;
                totalMove.y = velocity.y;
            }

            characterController.Move(totalMove * Time.deltaTime);
        }

        private void HandleGrasshopperSkill()
        {
            if (Input.GetKeyDown(grasshopperKey) && Time.time >= nextGrasshopperUseTime)
            {
                // 必要に応じてプレハブ生成
                // if (grasshopperPrefab != null && grasshopperPoint != null)
                // {
                //     Instantiate(grasshopperPrefab, grasshopperPoint.position, Quaternion.identity);
                // }

                Vector3 inputDir = Vector3.zero;
                if (Input.GetKey(forwardKey)) inputDir += Vector3.forward;
                if (Input.GetKey(backwardKey)) inputDir += Vector3.back;
                if (Input.GetKey(leftKey)) inputDir += Vector3.left;
                if (Input.GetKey(rightKey)) inputDir += Vector3.right;

                inputDir = inputDir.normalized;

                Vector3 moveDir;
                if (inputDir == Vector3.zero)
                {
                    moveDir = Vector3.up; // ニュートラル入力時は真上ジャンプ
                }
                else
                {
                    moveDir = transform.TransformDirection(inputDir); // 入力方向に飛ぶ
                }

                LockMovement(0.5f, moveDir);
                nextGrasshopperUseTime = Time.time + grasshopperCooldown;
            }
        }

        private void LockMovement(float duration, Vector3 direction)
        {
            isMovementLocked = true;
            externalVelocity = direction.normalized * moveSpeed * 3f; // 通常の3倍速度で移動
            Invoke(nameof(UnlockMovement), duration);
        }

        private void UnlockMovement()
        {
            isMovementLocked = false;
            externalVelocity = Vector3.zero;
        }

        // 外部取得用プロパティ
        public Vector3 Position => transform.position;
        public bool IsGrounded => isGrounded;
        public float VelocityY => velocity.y;
    }
}
