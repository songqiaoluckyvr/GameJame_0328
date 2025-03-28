# Deer Controller Interface Specification

## Overview
The `DeerController` is a MonoBehaviour component that will be attached to the Pudu deer model to manage its animations and sound effects. This controller provides a simple interface for triggering different animations and associated sound effects.

## Class Definition
```csharp
public class DeerController : MonoBehaviour
```

## Dependencies
- Requires an Animator component attached to the same GameObject
- Requires AudioSource component attached to the same GameObject
- Requires animation clips for idle, run, jump, death, and spin animations
- Requires audio clips for jump, death, and powerup sounds

## Public Methods

### SetIdle()
Plays the deer's idle animation, which is the default state.

**Description:**
- Transitions the deer to its idle animation state
- Resets any movement parameters
- Can be called to return to the default state after other animations complete

**Parameters:** None

**Returns:** void

---

### Run(float speed)
Activates the deer's running animation.

**Description:**
- Plays the running animation at the specified speed
- Animation speed scales with the speed parameter
- Intended for horizontal movement in gameplay

**Parameters:**
- `speed` (float): The speed at which the deer runs. Values between 0.5-2.0 recommended.

**Returns:** void

---

### Jump()
Executes the deer's jumping animation and plays the associated jumping sound effect.

**Description:**
- Triggers the jumping animation sequence
- Plays the jumping sound effect once at the start of the animation
- Remains in the jump animation until explicitly told to change state
- Can be used for gameplay mechanics requiring vertical movement

**Parameters:** None

**Returns:** void

---

### Die()
Activates the deer's death animation and plays the death sound effect.

**Description:**
- Plays the death animation sequence
- Triggers the death sound effect
- Animation leaves the deer in a final "deceased" pose
- Should be called when the deer's health reaches zero

**Parameters:** None

**Returns:** void

---

### TakeAntidote()
Plays the deer's spin animation and triggers the powerup sound effect.

**Description:**
- Executes a special spinning animation to indicate the deer consuming an antidote
- Plays the powerup sound effect
- Used specifically when the deer consumes an antidote item
- Returns to idle state after the animation completes

**Parameters:** None

**Returns:** void

## Usage Example
```csharp
// Reference to the deer controller
private DeerController _deerController;

void Start()
{
    // Get reference to the deer controller
    _deerController = GetComponent<DeerController>();
    
    // Set deer to idle state initially
    _deerController.SetIdle();
}

void Update()
{
    // Example input handling
    if (Input.GetKeyDown(KeyCode.Space))
    {
        _deerController.Jump();
    }
    
    if (Input.GetKey(KeyCode.RightArrow))
    {
        _deerController.Run(1.0f);
    }
    
    if (Input.GetKeyUp(KeyCode.RightArrow))
    {
        _deerController.SetIdle();
    }
    
    if (Input.GetKeyDown(KeyCode.P))
    {
        _deerController.TakeAntidote();
    }
    
    if (Input.GetKeyDown(KeyCode.X))
    {
        _deerController.Die();
    }
}
```

## Implementation Notes
- Animation transitions should be smooth to avoid jerky movement
- Sound effects should be balanced for consistent volume levels
- Animation states should handle interruptions gracefully
- Consider using animation events to trigger precise sound timing
- The controller should handle edge cases, such as preventing new animations during death 