using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement Parameters
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _jumpCooldown = 0.3f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region State Variables
    private bool _isMovementEnabled = true;
    private bool _isGrounded;
    private bool _isFacingRight = true;
    private bool _canJump = true;
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
            Debug.Log("[PlayerMovement] Initialized player movement system");
        }
    }

    private void Update()
    {
        if (!_isMovementEnabled) return;

        // Get horizontal input (A/D or Left/Right arrows)
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle jump input (Space)
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded && _canJump)
            {
                Jump();
                if (_debugMode) Debug.Log("[PlayerMovement] Jump executed!");
            }
            else if (_debugMode)
            {
                Debug.Log($"[PlayerMovement] Jump attempted but conditions not met. Grounded: {_isGrounded}, CanJump: {_canJump}");
            }
        }

        // Update facing direction
        UpdateFacing();
    }

    private void FixedUpdate()
    {
        if (!_isMovementEnabled) return;
        Move();
    }
    #endregion

    #region Movement Methods
    private void Move()
    {
        Vector3 currentVelocity = _rb.velocity;
        
        // Apply horizontal movement
        float targetXVelocity = _horizontalInput * _moveSpeed;
        
        // Apply increased gravity when falling
        float yVelocity = currentVelocity.y;
        if (yVelocity < 0)
        {
            yVelocity += Physics.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        
        _rb.velocity = new Vector3(targetXVelocity, yVelocity, 0);
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, 0);
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
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

    #region Collision Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            if (_debugMode) Debug.Log("[PlayerMovement] Entered ground contact");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            if (_debugMode) Debug.Log("[PlayerMovement] Left ground contact");
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