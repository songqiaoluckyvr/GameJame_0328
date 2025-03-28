# DeerAnimationAndSoundController Specification

## Overview
The `DeerAnimationAndSoundController` is a Unity MonoBehaviour that manages the animation states and sound effects for the deer character. It provides a clean interface for playing various animations and associated sound effects.

## Class Definition
```csharp
public class DeerAnimationAndSoundController : MonoBehaviour
```

## Dependencies
- Requires an `Animator` component on the same GameObject
- Will automatically add an `AudioSource` component if not present
- Requires access to sound effect assets via Resources.Load

## Animation States
The controller uses these hardcoded animation state names from the existing deer animator:
- `Idle_A`, `Idle_B`, `Idle_C` - Idle animation states
- `Jump` - Jumping animation state
- `Death` - Death animation state
- `Spin` - Spin animation state
- `Run` - Running animation state

## Animation Settings
- `_spinDuration` - Controls how long the Spin animation plays before returning to idle (default: 1.0 second)

## Sound Effects
The controller uses these specific hardcoded sound effects:
- `"FX48 - Click 2"` - Jump sound
- `"FX54"` - Death sound
- `"FX57"` - Antidote sound

Sound effects are loaded automatically from resources. The controller will log warning messages if any sound effects cannot be loaded.

## Public Methods

### SetIdle()
Sets the deer to the idle animation state.
- Stops any ongoing Spin animation coroutine
- Plays the Idle_A animation state
- Will not execute if the deer is dead

### Run(float speed)
Activates the deer's running animation at the specified speed.
- **Parameters:**
  - `speed`: The speed at which the deer runs (0.5-2.0 recommended)
- Stops any ongoing Spin animation coroutine
- Plays the Run animation state
- Adjusts the Animator's speed property to control run speed
- Will not execute if the deer is dead

### Jump()
Executes the deer's jumping animation and plays the associated sound effect.
- Stops any ongoing Spin animation coroutine
- Plays the Jump animation state
- Freezes the animation on the last frame to prevent looping
- Plays the jump sound effect ("FX48 - Click 2")
- Will not execute if the deer is dead
- Logs a warning if the jump sound cannot be loaded

### Die()
Activates the deer's death animation and plays the death sound effect.
- Stops any ongoing Spin animation coroutine
- Plays the Death animation state
- Plays the death sound effect ("FX54")
- Sets internal `_isDead` flag to prevent further animation changes
- Will execute only once (subsequent calls are ignored)
- Logs a warning if the death sound cannot be loaded

### TakeAntidote()
Plays the deer's spin animation for about 1 second and triggers the antidote sound effect.
- Stops any ongoing Spin animation coroutine
- Starts a coroutine that:
  - Plays the Spin animation state
  - Waits for the specified duration (default: 1 second)
  - Automatically returns to idle state
- Plays the antidote sound effect ("FX57")
- Specifically used when the deer consumes an antidote item
- Will not execute if the deer is dead
- Logs a warning if the antidote sound cannot be loaded

## Usage Example
```csharp
// Reference to the controller
private DeerAnimationAndSoundController _deerController;

// Get a reference to the controller
void Start()
{
    _deerController = GetComponent<DeerAnimationAndSoundController>();
}

// Example usage in game logic
void Update()
{
    // Set to idle state
    if (IsGrounded() && !IsMoving())
    {
        _deerController.SetIdle();
    }
    
    // Run when moving on ground
    if (IsGrounded() && IsMoving())
    {
        _deerController.Run(moveSpeed);
    }
    
    // Jump when jump button is pressed
    if (Input.GetButtonDown("Jump") && IsGrounded())
    {
        _deerController.Jump();
    }
    
    // Die when health is depleted
    if (health <= 0)
    {
        _deerController.Die();
    }
    
    // Take antidote when collecting one
    if (CollectedAntidote())
    {
        _deerController.TakeAntidote();
    }
}
```

## Implementation Notes
- The controller initializes in the Idle state by default
- Automatically adds an AudioSource component if not present on the GameObject
- Sound effects are loaded from resources during Awake()
- The AudioSource is used to play sound effects via PlayOneShot
- The controller uses Animator.Play() to directly play animation states by name
- The Jump animation is frozen at the end to prevent looping
- The Spin animation plays for about 1 second and then returns to idle
- The Die() method sets an internal flag to prevent any further animation changes
- Logs specific warning messages if sound files cannot be loaded 