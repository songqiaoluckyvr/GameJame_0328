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

## Animation Parameters
The controller uses these hardcoded animation parameter names which must match the Animator controller:
- `Idle` - Boolean parameter for idle state
- `Run` - Boolean parameter for running state
- `Jump` - Boolean parameter for jumping state
- `Death` - Boolean parameter for death state
- `Spin` - Boolean parameter for spin state
- `Speed` - Float parameter for run speed variation

## Sound Effects
The controller requires the following sound clips to be assigned:
- Jump sound - Plays when the deer jumps
- Death sound - Plays when the deer dies
- Antidote sound - Plays when the deer takes an antidote

Sound clips are specified via SerializedField inputs in the inspector. The controller will log warning messages if any sound clips are not assigned.

## Public Methods

### SetIdle()
Sets the deer to the idle animation state.
- Disables all other animation states
- Sets Idle parameter to true and Speed to 0
- Will not execute if the deer is dead

### Run(float speed)
Activates the deer's running animation at the specified speed.
- **Parameters:**
  - `speed`: The speed at which the deer runs (0.5-2.0 recommended)
- Disables all other animation states
- Sets Run parameter to true and Speed to the provided value
- Will not execute if the deer is dead

### Jump()
Executes the deer's jumping animation and plays the associated sound effect.
- Disables all other animation states
- Sets Jump parameter to true
- Plays the jump sound effect
- Remains in the jump animation until explicitly told to change state
- Will not execute if the deer is dead
- Logs a warning if the jump sound is not assigned

### Die()
Activates the deer's death animation and plays the death sound effect.
- Disables all other animation states
- Sets Death parameter to true
- Plays the death sound effect
- Sets internal `_isDead` flag to prevent further animation changes
- Will execute only once (subsequent calls are ignored)
- Logs a warning if the death sound is not assigned

### TakeAntidote()
Plays the deer's spin animation and triggers the antidote sound effect.
- Disables all other animation states
- Sets Spin parameter to true
- Plays the antidote sound effect
- Specifically used when the deer consumes an antidote item
- Will not execute if the deer is dead
- Logs a warning if the antidote sound is not assigned

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
- The AudioSource is used to play sound effects via PlayOneShot
- Animation state transitions are handled by disabling all other states before enabling the desired one
- The Die() method sets an internal flag to prevent any further animation changes
- Logs warning messages when sound clips are not assigned and their corresponding methods are called 