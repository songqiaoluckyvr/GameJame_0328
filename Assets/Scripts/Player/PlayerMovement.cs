using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement Parameters
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region State Variables
    private bool _isMovementEnabled = true;
    private bool _isGrounded;
    private bool _isFacingRight = true;
    private bool _wasGroundedLastFrame;
    #endregion

    #region Component References
    private Rigidbody _rb;
    private float _horizontalInput;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        
        if (_debugMode)
        {
            Debug.Log($"[PlayerMovement] Initialized with groundLayer: {_groundLayer.value}");
        }
    }

    private void Update()
    {
        if (!_isMovementEnabled) return;

        // Get horizontal input (A/D or Left/Right arrows)
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        // Check if grounded first
        CheckGrounded();

        // Handle jump input (Space)
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded)
            {
                Jump();
                if (_debugMode) Debug.Log("[PlayerMovement] Jump executed!");
            }
            else if (_debugMode)
            {
                Debug.Log("[PlayerMovement] Jump attempted but not grounded!");
            }
        }

        // Update facing direction
        UpdateFacing();

        // Debug movement state
        if (_debugMode)
        {
            Debug.DrawRay(transform.position, Vector3.down * _groundCheckDistance, _isGrounded ? Color.green : Color.red);
        }
    }

    private void FixedUpdate()
    {
        if (!_isMovementEnabled) return;
        Move();

        if (_debugMode)
        {
            Debug.Log($"[PlayerMovement] Velocity: {_rb.velocity}, IsGrounded: {_isGrounded}");
        }
    }
    #endregion

    #region Movement Methods
    private void Move()
    {
        // Set velocity directly for responsive movement
        Vector3 currentVelocity = _rb.velocity;
        _rb.velocity = new Vector3(_horizontalInput * _moveSpeed, currentVelocity.y, 0);
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, 0);
    }

    private void UpdateFacing()
    {
        if (_horizontalInput != 0)
        {
            bool shouldFaceRight = _horizontalInput > 0;
            if (_isFacingRight != shouldFaceRight)
            {
                _isFacingRight = shouldFaceRight;
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }
    #endregion

    #region Check Methods
    private void CheckGrounded()
    {
        _wasGroundedLastFrame = _isGrounded;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);

        // Log ground state changes
        if (_debugMode && _wasGroundedLastFrame != _isGrounded)
        {
            Debug.Log($"[PlayerMovement] Ground state changed: {_isGrounded}");
        }
    }
    #endregion

    #region Public Methods
    public void EnableMovement() => _isMovementEnabled = true;
    public void DisableMovement()
    {
        _isMovementEnabled = false;
        _rb.velocity = Vector3.zero;
    }
    #endregion
} 