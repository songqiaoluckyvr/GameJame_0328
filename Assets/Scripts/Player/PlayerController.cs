using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Component References
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
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
        CurrentState = PlayerState.Dead;
        _movement.DisableMovement();
        // TODO: Trigger death sequence
    }

    private void HandleAntidoteCollected()
    {
        // TODO: Trigger antidote collection effects/feedback
    }
    #endregion

    #region Public Methods
    public void Reset()
    {
        CurrentState = PlayerState.Alive;
        _movement.EnableMovement();
        _health.ResetHealth();
    }
    #endregion
}

public enum PlayerState
{
    Alive,
    Dead
} 