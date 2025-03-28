# Game Jam Development Journey

This document records the interaction and development process during our game jam.

## March 28, 2024

### Initial Setup and Planning

#### User
> Hello there

#### Assistant
Introduced myself and offered assistance with:
- Creating or modifying Unity scripts
- Scene setup and management
- VR interaction systems
- Performance optimization
- Component development
- Unity asset management
- Other Unity/VR related tasks

#### User
> let's create a 2.5D infinite runner game where the player needs to collect antidotes to maintain health while running forward.

#### Assistant
I'll help set up the core systems needed for a 2.5D runner. We'll need:
1. Player movement and health systems
2. Camera following in 2.5D space
3. Infinite map generation
4. Collectible system for antidotes

Let's start with setting up the project structure:
```
Assets/
  ├── Scripts/
  │   ├── Player/
  │   │   ├── PlayerController.cs
  │   │   ├── PlayerHealth.cs
  │   │   └── PlayerMovement.cs
  │   ├── UI/
  │   │   └── HealthBar.cs
  │   └── Managers/
  │       └── GameManager.cs
  ├── Prefabs/
  ├── Scenes/
  └── Art/
```

#### User
> We need smooth player movement with jumping ability, and the character should face the direction it's moving.

#### Assistant
Created PlayerMovement system with:
- Horizontal movement using Input.GetAxis
- Jump mechanics with ground check
- Character rotation based on movement direction
- Rigidbody-based physics for smooth motion

Implementation:
```csharp
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 12f;
    private bool _isFacingRight = true;
    private bool _isGrounded;
    
    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Movement and jump logic
        UpdateFacing(horizontalInput);
    }
}
```

#### User
> The player's health should constantly drain over time, and they need to collect antidotes to survive.

#### Assistant
Implemented health system with:
- Continuous health drain
- Event system for health changes
- Antidote collection mechanics
- Death handling when health depletes

Implementation:
```csharp
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _healthDrainRate = 5f;
    
    public event Action OnHealthDepleted;
    public event Action<float, float> OnHealthChanged;
    
    private void Update()
    {
        if (!_isDead)
        {
            DrainHealth();
        }
    }
}
```

#### User
> Please simplify the interaction with the health bar. The health bar width should change based on the player's current health, with a scale from 100 to 0 health corresponding to a width from 300 to 0 pixels (3:1 ratio).

#### Assistant
Simplified the health bar system with direct width scaling:
- Implemented 3:1 ratio (300px width = 100 health)
- Removed unnecessary complexity
- Added clear visual feedback

Implementation:
```csharp
public class HealthBar : MonoBehaviour
{
    private const float HEALTH_TO_WIDTH_RATIO = 3f;
    
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float width = currentHealth * HEALTH_TO_WIDTH_RATIO;
        _healthFill.sizeDelta = new Vector2(width, _healthFill.sizeDelta.y);
    }
}
```

#### User
> Add a debug function that allows pressing the 0 key to add 10 health to the player, ensuring that the player's max health does not exceed 100.

#### Assistant
Added debug health functionality:
- 0 key adds 10 health points
- Maintains max health limit of 100
- Added debug logging for testing

Implementation:
```csharp
private void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
    {
        ModifyHealth(10f);
        if (_debugMode)
        {
            Debug.Log($"[PlayerHealth] Added 10 health. Current health: {_currentHealth}");
        }
    }
}
```

#### User
> Let's simplify the camera system by removing the Cinemachine logic

#### Assistant
Created simplified camera controller:
- Removed Cinemachine dependency
- Implemented basic smooth following
- Maintained 2.5D perspective

Implementation:
```csharp
public class CameraController : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            _target.position.x,
            transform.position.y,
            transform.position.z
        );
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
    }
}
```

#### User
> Remove the start tile from the pool after the player moves out and spawn the end tile after the player has moved forward after 10 tile rotations

#### Assistant
Modified the MapManager to handle special tiles:
- Added tile rotation counting
- Implemented start tile removal
- Added end tile spawning logic
- Improved progress tracking

Implementation:
```csharp
public class MapManager : MonoBehaviour
{
    private int _tileRotationCount = 0;
    private bool _hasRemovedStartTile = false;
    private bool _hasSpawnedEndTile = false;

    private void HandleTileRotation()
    {
        _tileRotationCount++;
        if (_tileRotationCount >= 10 && !_hasSpawnedEndTile)
        {
            SpawnEndTile();
            _hasSpawnedEndTile = true;
        }
    }
}
```

#### User
> Please generate the antidote in a random range in front or behind player, not right above

#### Assistant
Modified antidote spawning system:
- Added random position generation
- Implemented range-based spawning
- Maintained proper Z-axis position

Implementation:
```csharp
private void SpawnAntidote()
{
    float randomOffset = Random.Range(_minSpawnDistance, _maxSpawnDistance);
    float spawnX = _playerTransform.position.x + randomOffset;
    Vector3 spawnPosition = new Vector3(spawnX, _spawnHeight, 5f);
    // Spawn antidote at calculated position
}
```

### Technical Challenges Overcome

1. **Animation State Management**
   - Challenge: Initial animations stuck on first frame
   - Solution: Implemented proper state tracking

2. **Collision Detection**
   - Challenge: Double collision handling
   - Solution: Centralized collision in Antidote script

3. **Minor Challenges**
   - Fine-tuned animation speeds
   - Adjusted spawn distances
   - Improved debug logging
   - Fixed tag management

### Final Implementation Features

1. **Core Mechanics**
   - Smooth movement and jumping
   - Health management system
   - Antidote collection
   - Death sequence

2. **Environment**
   - Infinite runner generation
   - Dynamic tile recycling
   - Progress-based spawning
   - 2.5D perspective

3. **Technical Features**
   - Event-driven architecture
   - Component-based design
   - Debug systems
   - Optimized performance

### Future Improvements
- Enhanced visual feedback
- More varied obstacles
- Additional power-ups
- Score system
- Animation variety
- Particle effects 