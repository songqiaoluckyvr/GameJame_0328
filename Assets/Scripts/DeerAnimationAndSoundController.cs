using UnityEngine;

/// <summary>
/// Controller for the deer character that handles animations and sound effects
/// </summary>
public class DeerAnimationAndSoundController : MonoBehaviour
{
    // Animation parameter constants
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_RUN = "Run";
    private const string ANIM_JUMP = "Jump";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_SPIN = "Spin";
    private const string ANIM_SPEED = "Speed";

    // Audio clips
    [Header("Audio")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _antidoteSound;

    // Component references
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isDead = false;

    private void Awake()
    {
        // Cache component references
        _animator = GetComponent<Animator>();
        
        // Add AudioSource if not present
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Validate components
        if (_animator == null)
        {
            Debug.LogError("Animator component is missing on the deer GameObject.");
        }
    }

    private void Start()
    {
        // Start in idle state
        SetIdle();
    }

    /// <summary>
    /// Sets the deer to the idle animation state
    /// </summary>
    public void SetIdle()
    {
        if (_isDead) return;

        _animator.SetBool(ANIM_RUN, false);
        _animator.SetBool(ANIM_JUMP, false);
        _animator.SetBool(ANIM_SPIN, false);
        _animator.SetBool(ANIM_IDLE, true);
        _animator.SetFloat(ANIM_SPEED, 0f);
    }

    /// <summary>
    /// Activates the deer's running animation at the specified speed
    /// </summary>
    /// <param name="speed">The speed at which the deer runs (0.5-2.0 recommended)</param>
    public void Run(float speed)
    {
        if (_isDead) return;

        _animator.SetBool(ANIM_IDLE, false);
        _animator.SetBool(ANIM_JUMP, false);
        _animator.SetBool(ANIM_SPIN, false);
        _animator.SetBool(ANIM_RUN, true);
        _animator.SetFloat(ANIM_SPEED, speed);
    }

    /// <summary>
    /// Executes the deer's jumping animation and plays the associated sound effect
    /// </summary>
    public void Jump()
    {
        if (_isDead) return;

        _animator.SetBool(ANIM_IDLE, false);
        _animator.SetBool(ANIM_RUN, false);
        _animator.SetBool(ANIM_SPIN, false);
        _animator.SetBool(ANIM_JUMP, true);

        // Play jump sound
        if (_jumpSound != null)
        {
            _audioSource.PlayOneShot(_jumpSound);
        }
        else
        {
            Debug.LogWarning("Jump sound is not assigned to the DeerAnimationAndSoundController.");
        }
    }

    /// <summary>
    /// Activates the deer's death animation and plays the death sound effect
    /// </summary>
    public void Die()
    {
        // Prevent calling Die multiple times
        if (_isDead) return;

        _isDead = true;
        
        _animator.SetBool(ANIM_IDLE, false);
        _animator.SetBool(ANIM_RUN, false);
        _animator.SetBool(ANIM_JUMP, false);
        _animator.SetBool(ANIM_SPIN, false);
        _animator.SetBool(ANIM_DEATH, true);

        // Play death sound
        if (_deathSound != null)
        {
            _audioSource.PlayOneShot(_deathSound);
        }
        else
        {
            Debug.LogWarning("Death sound is not assigned to the DeerAnimationAndSoundController.");
        }
    }

    /// <summary>
    /// Plays the deer's spin animation and triggers the powerup sound effect
    /// </summary>
    public void TakeAntidote()
    {
        if (_isDead) return;

        _animator.SetBool(ANIM_IDLE, false);
        _animator.SetBool(ANIM_RUN, false);
        _animator.SetBool(ANIM_JUMP, false);
        _animator.SetBool(ANIM_SPIN, true);

        // Play antidote/powerup sound
        if (_antidoteSound != null)
        {
            _audioSource.PlayOneShot(_antidoteSound);
        }
        else
        {
            Debug.LogWarning("Antidote sound is not assigned to the DeerAnimationAndSoundController.");
        }

        // This animation returns to idle automatically through the animation state machine or events
    }
} 