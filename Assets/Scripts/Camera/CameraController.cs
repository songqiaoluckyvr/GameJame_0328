using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera Parameters
    [Header("Follow Settings")]
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 4f, -10f);
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _lookAheadDistance = 3f;
    [SerializeField] private float _lookAheadSpeed = 2f;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region Component References
    private Transform _playerTransform;
    private Camera _camera;
    #endregion

    #region State Variables
    private Vector3 _currentVelocity;
    private float _currentLookAheadX = 0f;
    private float _targetLookAheadX = 0f;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Get the camera component
        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError("[CameraController] Camera component not found!");
            return;
        }

        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            if (_debugMode)
            {
                Debug.Log("[CameraController] Found player as follow target");
            }
        }
        else
        {
            Debug.LogError("[CameraController] Player not found! Make sure it has the 'Player' tag.");
        }

        // Set initial camera position
        if (_playerTransform != null)
        {
            transform.position = _playerTransform.position + _cameraOffset;
        }
    }

    private void LateUpdate()
    {
        if (_playerTransform == null) return;

        UpdateCameraPosition();
    }
    #endregion

    #region Camera Methods
    private void UpdateCameraPosition()
    {
        // Get player's movement direction from Rigidbody
        Rigidbody playerRb = _playerTransform.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            float playerDirectionX = Mathf.Sign(playerRb.velocity.x);

            // Update look ahead target based on player's movement
            if (Mathf.Abs(playerRb.velocity.x) > 0.1f)
            {
                _targetLookAheadX = _lookAheadDistance * playerDirectionX;
            }
            else
            {
                _targetLookAheadX = 0f;
            }

            // Smoothly interpolate current look ahead
            _currentLookAheadX = Mathf.Lerp(_currentLookAheadX, _targetLookAheadX, Time.deltaTime * _lookAheadSpeed);
        }

        // Calculate target position with look-ahead
        Vector3 targetPosition = _playerTransform.position + _cameraOffset;
        targetPosition.x += _currentLookAheadX;

        // Smoothly move camera to target position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            1f / _followSpeed
        );

        if (_debugMode && Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Debug.Log($"[CameraController] Camera following - Target: {targetPosition}, Current: {transform.position}");
            Debug.Log($"[CameraController] Look ahead: {_currentLookAheadX:F2}");
        }
    }
    #endregion

    #region Public Methods
    public void SetOffset(Vector3 offset)
    {
        _cameraOffset = offset;
        if (_playerTransform != null)
        {
            transform.position = _playerTransform.position + _cameraOffset;
        }
    }

    public void SetFollowSpeed(float speed)
    {
        _followSpeed = speed;
    }
    #endregion
} 