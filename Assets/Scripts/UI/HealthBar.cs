using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _healthFill;
    private const float HEALTH_TO_WIDTH_RATIO = 3f; // 300 pixels : 100 health = 3:1 ratio
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogError("[HealthBar] PlayerHealth component not found!");
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (_healthFill != null)
        {
            float width = currentHealth * HEALTH_TO_WIDTH_RATIO; // 100 health = 300 width
            _healthFill.sizeDelta = new Vector2(width, _healthFill.sizeDelta.y);
        }
    }
} 