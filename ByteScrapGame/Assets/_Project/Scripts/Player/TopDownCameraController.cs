using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    
    [Header("Height Settings")]
    public float height = 10f;
    public float heightChangeSpeed = 5f;
    public float minHeight = 5f;
    public float maxHeight = 50f;
    
    [Header("Flight Zone")]
    public Vector3 zoneCenter = Vector3.zero;
    public Vector3 zoneSize = new Vector3(50f, 0f, 50f);
    
    [Header("Camera Tilt Settings")]
    public float defaultTilt = 60f;
    public float maxTilt = 85f;
    public float minTilt = 30f;
    public float tiltSmoothness = 5f;
    public float edgeThreshold = 0.1f;
    
    private Vector3 _currentVelocity;
    private float _currentHeight;
    private Camera _camera;
    private float _currentTilt;
    private Vector3 _lookAtPoint;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _currentHeight = height;
        _currentTilt = defaultTilt;
        UpdateCameraPosition();
    }

    void Update()
    {
        HandleMovementInput();
        HandleHeightInput();
        ApplyMovement();
        UpdateCameraTilt();
    }

    private void HandleMovementInput()
    {
        Vector3 inputDirection = Vector3.zero;
        
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.z = Input.GetAxisRaw("Vertical");
        
        Vector3 targetDirection = new Vector3(inputDirection.x, 0f, inputDirection.z);
        
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
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // Сохраняем текущую точку фокуса
        _lookAtPoint = transform.position;
        _lookAtPoint.y = 0;
        
        // Обновляем высоту камеры
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Lerp(transform.position.y, _currentHeight, Time.deltaTime * 5f);
        transform.position = newPosition;
    }

    private void UpdateCameraTilt()
    {
        // Рассчитываем нормализованное расстояние до краев зоны
        float edgeFactor = CalculateEdgeFactor();
        
        // Рассчитываем целевой наклон на основе близости к краю
        float targetTilt = Mathf.Lerp(minTilt, maxTilt, edgeFactor);
        
        // Плавно интерполируем текущий наклон
        _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, tiltSmoothness * Time.deltaTime);
        
        // Применяем поворот камеры
        transform.rotation = Quaternion.Euler(_currentTilt, transform.rotation.eulerAngles.y, 0);
        
        // Корректируем позицию, чтобы сохранить точку фокуса
        //MaintainLookAtPoint();
    }

    private float CalculateEdgeFactor()
    {
        Vector3 localPos = transform.position - zoneCenter;
        Vector3 halfSize = zoneSize * 0.5f;

        // Рассчитываем нормализованные расстояния до границ
        float xFactor = Mathf.Clamp01(Mathf.Abs(localPos.x) / (halfSize.x - edgeThreshold * zoneSize.x));
        float zFactor = Mathf.Clamp01(Mathf.Abs(localPos.z) / (halfSize.z - edgeThreshold * zoneSize.z));
        
        // Используем максимальное значение из двух
        return Mathf.Max(xFactor, zFactor);
    }

    private void MaintainLookAtPoint()
    {
        // Рассчитываем смещение для сохранения точки фокуса
        float heightOffset = transform.position.y - _lookAtPoint.y;
        float distance = heightOffset / Mathf.Tan(_currentTilt * Mathf.Deg2Rad);
        
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        
        Vector3 targetPosition = _lookAtPoint - direction * distance;
        targetPosition.y = transform.position.y;
        
        transform.position = targetPosition;
    }

    private Vector3 ClampPositionToZone(Vector3 position)
    {
        Vector3 minBounds = zoneCenter - zoneSize / 2f;
        Vector3 maxBounds = zoneCenter + zoneSize / 2f;
        
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.z = Mathf.Clamp(position.z, minBounds.z, maxBounds.z);
        
        return position;
    }

    // Визуализация зоны в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Vector3 zoneHeight = new Vector3(zoneSize.x, 0.1f, zoneSize.z);
        Gizmos.DrawCube(zoneCenter, zoneHeight);
        
        // Рисуем границы
        Gizmos.color = Color.yellow;
        DrawRect(zoneCenter, zoneSize.x, zoneSize.z);
        
        // Показываем точку фокуса
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_lookAtPoint, 0.5f);
        
        // Показываем направление взгляда камеры
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _lookAtPoint);
        }
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