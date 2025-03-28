using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Antidote : MonoBehaviour
{
    #region Parameters
    [Header("Antidote Settings")]
    [SerializeField] private float _fallSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region Component References
    private Rigidbody _rb;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Get or add required components
        _rb = GetComponent<Rigidbody>();
        
        // Configure Rigidbody
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        
        if (_debugMode)
        {
            Debug.Log($"[Antidote] Initialized at {transform.position}");
        }
    }

    private void FixedUpdate()
    {
        // Apply constant falling velocity while maintaining Z position
        Vector3 velocity = _rb.velocity;
        velocity.y = -_fallSpeed;
        velocity.z = 0f; // Ensure no Z movement
        _rb.velocity = velocity;
        
        // Rotate around Z axis
        transform.Rotate(0f, 0f, _rotationSpeed * Time.fixedDeltaTime);

        // Ensure Z position stays at 5
        if (transform.position.z != 5f)
        {
            Vector3 position = transform.position;
            position.z = 5f;
            transform.position = position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player collected the antidote
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.CollectAntidote();
                if (_debugMode)
                {
                    Debug.Log("[Antidote] Collected by player");
                }
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Antidote hit the ground
            if (_debugMode)
            {
                Debug.Log("[Antidote] Hit ground and destroyed");
            }
            Destroy(gameObject);
        }
    }
    #endregion
} 