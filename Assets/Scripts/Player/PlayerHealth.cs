using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    #region Health Parameters
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _startingHealth = 100f;
    [SerializeField] private float _healthDrainRate = 5f;
    [SerializeField] private float _antidoteHealthRestore = 40f;
    [SerializeField] private float _criticalHealthThreshold = 30f;
    #endregion

    #region Events
    public event Action OnHealthDepleted;
    public event Action OnAntidoteCollected;
    public event Action<float> OnHealthChanged;
    public event Action OnCriticalHealth;
    #endregion

    #region State Variables
    private float _currentHealth;
    private bool _isDead;
    private bool _isInCriticalHealth;
    #endregion

    #region Properties
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public float HealthPercentage => _currentHealth / _maxHealth;
    public bool IsDead => _isDead;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (_isDead) return;

        DrainHealth();
        CheckCriticalHealth();
    }
    #endregion

    #region Health Management
    private void DrainHealth()
    {
        ModifyHealth(-_healthDrainRate * Time.deltaTime);
    }

    private void ModifyHealth(float amount)
    {
        if (_isDead) return;

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0f && !_isDead)
        {
            _isDead = true;
            OnHealthDepleted?.Invoke();
        }
    }

    private void CheckCriticalHealth()
    {
        bool isCritical = _currentHealth <= _criticalHealthThreshold;
        if (isCritical && !_isInCriticalHealth)
        {
            _isInCriticalHealth = true;
            OnCriticalHealth?.Invoke();
        }
        else if (!isCritical)
        {
            _isInCriticalHealth = false;
        }
    }
    #endregion

    #region Public Methods
    public void CollectAntidote()
    {
        ModifyHealth(_antidoteHealthRestore);
        OnAntidoteCollected?.Invoke();
    }

    public void ResetHealth()
    {
        _currentHealth = _startingHealth;
        _isDead = false;
        _isInCriticalHealth = false;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void SetDrainRate(float newRate)
    {
        _healthDrainRate = Mathf.Max(0f, newRate);
    }
    #endregion

    #region Collision Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Antidote"))
        {
            CollectAntidote();
            Destroy(other.gameObject);
        }
    }
    #endregion
} 