using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Component References
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private DeerAnimationAndSoundController _animationController;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region Events
    public delegate void PlayerStateChangeHandler(PlayerState newState);
    public event PlayerStateChangeHandler OnPlayerStateChanged;
    #endregion

    #region State Management
    private PlayerState _currentState = PlayerState.Alive;
    public PlayerState CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnPlayerStateChanged?.Invoke(_currentState);
            }
        }
    }
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Ensure we have references to required components
        if (_movement == null) _movement = GetComponent<PlayerMovement>();
        if (_health == null) _health = GetComponent<PlayerHealth>();
        if (_animationController == null) _animationController = GetComponent<DeerAnimationAndSoundController>();

        // Subscribe to health events
        if (_health != null)
        {
            _health.OnHealthDepleted += HandleHealthDepleted;
            _health.OnAntidoteCollected += HandleAntidoteCollected;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (_health != null)
        {
            _health.OnHealthDepleted -= HandleHealthDepleted;
            _health.OnAntidoteCollected -= HandleAntidoteCollected;
        }
    }
    #endregion

    #region Event Handlers
    private void HandleHealthDepleted()
    {
        if (_debugMode)
        {
            Debug.Log("[PlayerController] Player death sequence initiated");
        }

        CurrentState = PlayerState.Dead;
        _movement.DisableMovement();
        
        if (_animationController != null)
        {
            _animationController.Die();
            if (_debugMode)
            {
                Debug.Log("[PlayerController] Death animation triggered");
            }
        }

        // Pause health drain since player is dead
        if (_health != null)
        {
            _health.PauseDrain();
            if (_debugMode)
            {
                Debug.Log("[PlayerController] Health drain paused");
            }
        }
    }

    private void HandleAntidoteCollected()
    {
        if (_animationController != null)
        {
            _animationController.TakeAntidote();
        }
    }
    #endregion

    #region Public Methods
    public void Reset()
    {
        if (_debugMode)
        {
            Debug.Log("[PlayerController] Resetting player state");
        }

        CurrentState = PlayerState.Alive;
        _movement.EnableMovement();
        _health.ResetHealth();
        
        if (_debugMode)
        {
            Debug.Log("[PlayerController] Player reset complete - Movement enabled, Health reset");
        }
    }
    #endregion
}

public enum PlayerState
{
    Alive,
    Dead
} 