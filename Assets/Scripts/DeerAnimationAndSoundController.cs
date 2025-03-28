using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for the deer character that handles animations and sound effects using the existing animation states
/// </summary>
public class DeerAnimationAndSoundController : MonoBehaviour
{
    // Animation state names from the model's animator
    private const string ANIM_IDLE_A = "Idle_A";
    private const string ANIM_IDLE_B = "Idle_B";
    private const string ANIM_IDLE_C = "Idle_C";
    private const string ANIM_JUMP = "Jump";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_SPIN = "Spin";
    private const string ANIM_RUN = "Run";

    // Animation settings
    [Header("Animation Settings")]
    [SerializeField] public float _spinDuration = 1.0f;
    
    // Audio clips
    [Header("Audio")]
    [SerializeField] public AudioClip _jumpSound;
    [SerializeField] public AudioClip _deathSound;
    [SerializeField] public AudioClip _antidoteSound;

    // Component references
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isDead = false;
    private Coroutine _spinCoroutine;

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

        // Stop any ongoing spin coroutine
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
            _spinCoroutine = null;
        }

        // Play one of the idle animations
        PlayAnimation(ANIM_IDLE_A);
    }

    /// <summary>
    /// Activates the deer's running animation at the specified speed
    /// </summary>
    /// <param name="speed">The speed at which the deer runs (0.5-2.0 recommended)</param>
    public void Run(float speed)
    {
        if (_isDead) return;

        // Stop any ongoing spin coroutine
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
            _spinCoroutine = null;
        }

        PlayAnimation(ANIM_RUN);
        
        // Although we don't use a Speed parameter, we can adjust animation speed using the Animator
        _animator.speed = speed;
    }

    /// <summary>
    /// Executes the deer's jumping animation and plays the associated sound effect
    /// </summary>
    public void Jump()
    {
        if (_isDead) return;

        // Stop any ongoing spin coroutine
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
            _spinCoroutine = null;
        }

        PlayAnimation(ANIM_JUMP);
        
        // Freeze the animation on the last frame to prevent looping
        StartCoroutine(FreezeJumpAnimation());

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
        
        // Stop any ongoing spin coroutine
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
            _spinCoroutine = null;
        }
        
        PlayAnimation(ANIM_DEATH);

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
    /// Plays the deer's spin animation for about 1 second and triggers the powerup sound effect
    /// </summary>
    public void TakeAntidote()
    {
        if (_isDead) return;

        // Stop any ongoing spin coroutine
        if (_spinCoroutine != null)
        {
            StopCoroutine(_spinCoroutine);
        }
        
        // Start a new spin coroutine
        _spinCoroutine = StartCoroutine(PlaySpinAnimation());

        // Play antidote/powerup sound
        if (_antidoteSound != null)
        {
            _audioSource.PlayOneShot(_antidoteSound);
        }
        else
        {
            Debug.LogWarning("Antidote sound is not assigned to the DeerAnimationAndSoundController.");
        }
    }

    /// <summary>
    /// Helper method to play an animation by name using the animator's Play method
    /// </summary>
    private void PlayAnimation(string animationName)
    {
        // We use the state name directly instead of setting parameters
        // This utilizes the deer model's built-in animations
        _animator.Play(animationName, 0, 0f);
        
        // Reset animation speed if it was changed
        if (animationName != ANIM_RUN)
        {
            _animator.speed = 1f;
        }
    }
    
    /// <summary>
    /// Coroutine to handle the spin animation duration
    /// </summary>
    private IEnumerator PlaySpinAnimation()
    {
        // Play the spin animation
        PlayAnimation(ANIM_SPIN);
        
        // Wait for the specified duration
        yield return new WaitForSeconds(_spinDuration);
        
        // Return to idle if not dead
        if (!_isDead)
        {
            SetIdle();
        }
        
        _spinCoroutine = null;
    }
    
    /// <summary>
    /// Coroutine to freeze the jump animation at the end (prevent looping)
    /// </summary>
    private IEnumerator FreezeJumpAnimation()
    {
        // Get the animation clip info to find its length
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float jumpDuration = clipInfo[0].clip.length;
            
            // Wait until near the end of the jump animation
            yield return new WaitForSeconds(jumpDuration * 0.95f);
            
            // Freeze the animation at the current point
            _animator.speed = 0;
        }
    }
} 