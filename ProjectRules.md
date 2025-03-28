# Game Jam Project Rules

## Project Structure
- All scripts should be placed in appropriate folders under `Assets/Scripts/`
- Scenes should be stored in `Assets/Scenes/`
- Prefabs should be organized in `Assets/Prefabs/`
- Art assets should be in `Assets/Art/` with subdirectories for different types (Models, Textures, Materials, etc.)
- Audio files should be in `Assets/Audio/`

## Coding Standards
### Naming Conventions
- Use PascalCase for class names and public members
- Use camelCase for private members and local variables
- Prefix private fields with underscore (_)
- Use descriptive names that reflect purpose

### Script Organization
```csharp
// Standard script template
using UnityEngine;

public class ComponentName : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private Type _privateField;
    public Type PublicField;
    #endregion

    #region Private Fields
    private Type _internalVariable;
    #endregion

    #region Properties
    public Type Property { get; private set; }
    #endregion

    #region Unity Lifecycle Methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    #endregion

    #region Public Methods
    public void PublicMethod() { }
    #endregion

    #region Private Methods
    private void PrivateMethod() { }
    #endregion
}
```

### Best Practices
1. **Performance**
   - Cache component references in Awake()
   - Use object pooling for frequently spawned objects
   - Minimize operations in Update()
   - Use coroutines for delayed operations

2. **Code Quality**
   - Keep methods focused and single-purpose
   - Limit method length (max 30 lines recommended)
   - Add comments for complex logic
   - Use [SerializeField] instead of public for inspector variables

3. **Scene Management**
   - Use descriptive names for GameObjects
   - Organize hierarchy logically
   - Use empty GameObjects as containers/managers
   - Tag and layer objects appropriately

## Game Design Guidelines
1. **Core Mechanics**
   - Focus on one core mechanic and polish it
   - Ensure responsive controls
   - Provide clear feedback for player actions
   - Keep mechanics consistent

2. **Player Experience**
   - Clear objectives
   - Intuitive UI/UX
   - Consistent difficulty curve
   - Meaningful feedback systems

3. **Performance Targets**
   - Maintain 60 FPS minimum
   - Optimize asset usage
   - Monitor memory usage
   - Profile regularly during development

## Asset Guidelines
1. **Art**
   - Maintain consistent style
   - Optimize textures appropriately
   - Use proper texture compression
   - Keep poly counts reasonable

2. **Audio**
   - Use appropriate formats (WAV for short SFX, OGG for music)
   - Implement proper audio mixing
   - Add audio feedback for important actions
   - Keep file sizes optimized

## Version Control
1. **Commits**
   - Write clear commit messages
   - Commit frequently
   - Keep commits focused and atomic
   - Use meaningful branch names

2. **Unity-Specific**
   - Use .gitignore for Unity projects
   - Don't commit Library folder
   - Be careful with scene merges
   - Back up regularly

## Testing
1. **Gameplay Testing**
   - Test core mechanics thoroughly
   - Verify input responsiveness
   - Check edge cases
   - Test on different screen resolutions

2. **Technical Testing**
   - Profile performance regularly
   - Check memory usage
   - Verify scene loading
   - Test build in release mode

## Build Settings
1. **PC Build**
   - Target Windows 10/11
   - Support common resolutions
   - Include quality settings
   - Implement proper error handling

2. **Quality Assurance**
   - Create build checklist
   - Test on different hardware
   - Verify all features work in build
   - Check performance in built version

## Documentation
1. **Code Documentation**
   - Document public methods and properties
   - Explain complex algorithms
   - Note any dependencies
   - Document known issues or limitations

2. **Project Documentation**
   - Maintain README.md
   - Document setup procedures
   - List external assets/packages
   - Include build instructions

## Time Management
1. **Development Phases**
   - Prototype (30% of time)
   - Core Features (40% of time)
   - Polish (20% of time)
   - Testing and Fixes (10% of time)

2. **Priorities**
   - Core gameplay first
   - Essential features second
   - Polish and extras last
   - Always maintain a playable build 