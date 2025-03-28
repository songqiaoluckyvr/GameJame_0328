using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UI controls for interacting with the DeerAnimationAndSoundController
/// </summary>
public class DeerUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public DeerAnimationAndSoundController _deerController;
    
    [Header("UI Buttons")]
    [SerializeField] public Button _idleButton;
    [SerializeField] public Button _runButton;
    [SerializeField] public Button _jumpButton;
    [SerializeField] public Button _dieButton;
    [SerializeField] public Button _antidoteButton;
    
    [Header("Run Settings")]
    [SerializeField] public Slider _runSpeedSlider;
    [SerializeField] public Text _runSpeedText;
    [SerializeField] public float _minRunSpeed = 0.5f;
    [SerializeField] public float _maxRunSpeed = 2.0f;

    private void Awake()
    {
        // Find deer controller if not assigned
        if (_deerController == null)
        {
            _deerController = FindObjectOfType<DeerAnimationAndSoundController>();
            
            if (_deerController == null)
            {
                Debug.LogError("No DeerAnimationAndSoundController found in the scene!");
            }
        }
    }

    private void Start()
    {
        // Set up button event listeners
        if (_idleButton != null)
            _idleButton.onClick.AddListener(OnIdleButtonClick);
            
        if (_runButton != null)
            _runButton.onClick.AddListener(OnRunButtonClick);
            
        if (_jumpButton != null)
            _jumpButton.onClick.AddListener(OnJumpButtonClick);
            
        if (_dieButton != null)
            _dieButton.onClick.AddListener(OnDieButtonClick);
            
        if (_antidoteButton != null)
            _antidoteButton.onClick.AddListener(OnAntidoteButtonClick);

        // Set up the run speed slider
        if (_runSpeedSlider != null)
        {
            _runSpeedSlider.minValue = _minRunSpeed;
            _runSpeedSlider.maxValue = _maxRunSpeed;
            _runSpeedSlider.value = 1.0f;
            _runSpeedSlider.onValueChanged.AddListener(OnRunSpeedChanged);
            
            UpdateRunSpeedText(_runSpeedSlider.value);
        }

        // Set initial state
        if (_deerController != null)
        {
            _deerController.SetIdle();
        }
    }

    private void OnIdleButtonClick()
    {
        if (_deerController != null)
        {
            _deerController.SetIdle();
        }
    }

    private void OnRunButtonClick()
    {
        if (_deerController != null)
        {
            float runSpeed = _runSpeedSlider != null ? _runSpeedSlider.value : 1.0f;
            _deerController.Run(runSpeed);
        }
    }

    private void OnJumpButtonClick()
    {
        if (_deerController != null)
        {
            _deerController.Jump();
        }
    }

    private void OnDieButtonClick()
    {
        if (_deerController != null)
        {
            _deerController.Die();
        }
    }

    private void OnAntidoteButtonClick()
    {
        if (_deerController != null)
        {
            _deerController.TakeAntidote();
        }
    }

    private void OnRunSpeedChanged(float value)
    {
        UpdateRunSpeedText(value);

        // If we're already running, update the speed immediately
        if (_deerController != null && _runButton != null && _runButton.gameObject.activeInHierarchy)
        {
            _deerController.Run(value);
        }
    }

    private void UpdateRunSpeedText(float value)
    {
        if (_runSpeedText != null)
        {
            _runSpeedText.text = $"Speed: {value:F1}";
        }
    }
}