using _Project.Scripts.GameRoot;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerMovingController : MonoBehaviour
    {
        [Header("Movement Settings")] public float moveSpeed = 10f;
        public float acceleration = 15f;
        public float deceleration = 20f;

        [Header("Height Settings")] public float height = 10f;
        public float heightChangeSpeed = 5f;
        public float minHeight = 5f;
        public float maxHeight = 50f;
        
        [Header("Camera Tilt Settings")]
        public float defaultTilt = 60f;
        public float maxTilt = 85f;
        public float minTilt = 30f;
        public float tiltSmoothness = 5f;
        public float edgeThreshold = 0.1f;

        [Header("Flight Zone")] public Vector3 zoneCenter = Vector3.zero;
        public Vector3 zoneSize = new Vector3(50f, 0f, 50f);

        private Vector3 _currentVelocity;
        private float _currentHeight;
        private float _currentTilt;
        
        private GameInput _input;
        private Rigidbody _rigidbody;
        private bool inited;

        public void Init(GameInput input)
        {
            _input = input;
            _rigidbody = GetComponent<Rigidbody>();
            inited = true;
        }

        void Start()
        {
            _currentHeight = height;
            _currentTilt = defaultTilt;
            UpdateCameraHeight();
        }

        void Update()
        {
            if (inited)
            {
                HandleMovementInput();
                HandleHeightInput();
                ApplyMovement();
                UpdateCameraTilt();
            }
        }

        private void HandleMovementInput()
        {
            Vector2 inputDirection = _input.Player.Move.ReadValue<Vector2>();

            Vector3 targetDirection = new Vector3(inputDirection.y * -1, 0f, inputDirection.x);

            
            if (targetDirection.magnitude > 0.1f)
            {
                _currentVelocity = Vector3.Lerp(
                    _currentVelocity,
                    targetDirection.normalized * moveSpeed,
                    acceleration * Time.deltaTime
                );
            }
            else
            {
                _currentVelocity = Vector3.Lerp(
                    _currentVelocity,
                    Vector3.zero,
                    deceleration * Time.deltaTime
                );
            }
        }

        private void HandleHeightInput()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                _currentHeight = Mathf.Clamp(
                    _currentHeight - scrollInput * heightChangeSpeed,
                    minHeight,
                    maxHeight
                );
            }
        }

        private void ApplyMovement()
        {
            Vector3 newPosition = transform.position + _currentVelocity * Time.deltaTime;

            newPosition = ClampPositionToZone(newPosition);

            transform.position = newPosition;
            UpdateCameraHeight();
        }

        private Vector3 ClampPositionToZone(Vector3 position)
        {
            Vector3 minBounds = zoneCenter - zoneSize / 2f;
            Vector3 maxBounds = zoneCenter + zoneSize / 2f;

            position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
            position.z = Mathf.Clamp(position.z, minBounds.z, maxBounds.z);

            return position;
        }

        private void UpdateCameraHeight()
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.Lerp(transform.position.y, _currentHeight, Time.deltaTime * 5f);
            transform.position = newPosition;
        }
        
        private void UpdateCameraTilt()
        {
            Vector3 localPos = transform.position - zoneCenter;
            Vector3 halfSize = zoneSize * 0.5f;
            float edgeFactor = Mathf.Clamp01(Mathf.Abs(localPos.x) / (halfSize.x - edgeThreshold * zoneSize.x));
            
            float targetTilt = Mathf.Lerp(minTilt, maxTilt, edgeFactor);
        
            // Плавно интерполируем текущий наклон
            _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, tiltSmoothness * Time.deltaTime);
        
            transform.rotation = Quaternion.Euler(_currentTilt, transform.rotation.eulerAngles.y, 0);
        
        } 

        // Визуализация зоны
        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Vector3 zoneHeight = new Vector3(zoneSize.x, 0.1f, zoneSize.z);
            Gizmos.DrawCube(zoneCenter, zoneHeight);

            Gizmos.color = Color.yellow;
            DrawRect(zoneCenter, zoneSize.x, zoneSize.z);
        }

        private void DrawRect(Vector3 center, float width, float height)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            Vector3 topLeft = center + new Vector3(-halfWidth, 0f, halfHeight);
            Vector3 topRight = center + new Vector3(halfWidth, 0f, halfHeight);
            Vector3 bottomLeft = center + new Vector3(-halfWidth, 0f, -halfHeight);
            Vector3 bottomRight = center + new Vector3(halfWidth, 0f, -halfHeight);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}