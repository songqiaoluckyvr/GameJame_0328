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
> let's do a sequential thinking on how we are gonna proceed with the game jam today

#### Assistant
Conducted sequential thinking process:
1. Assessed current project state (fresh Unity project with basic scene)
2. Initially planned for VR development
3. Outlined development pipeline and key steps
4. Discussed project management approach
5. Considered technical requirements
6. Suggested next steps for game concept discussion

#### User
> We will not work on VR game this time, instead it would be PC game

#### Assistant
Readjusted planning for PC game development:
1. Simplified scope (removed VR-specific considerations)
2. Discussed potential game types:
   - 2D platformers
   - Top-down adventures
   - First-person exploration
   - Puzzle games
   - Arcade-style games
3. Outlined PC-specific development pipeline
4. Adjusted technical focus for PC development
5. Set 60 FPS as minimum performance target

#### User
> cool, lets generate a project rules for the cursor, dont need seq thinking now

#### Assistant
Created comprehensive ProjectRules.md with:
1. Project Structure
2. Coding Standards
3. Game Design Guidelines
4. Asset Management
5. Version Control
6. Testing Procedures
7. Build Settings
8. Documentation Requirements
9. Time Management

#### User
> Cool, now lets make several basic setup for the folder structure, just the essentials we will for sure need

#### Assistant
Created essential folder structure:
```
Assets/
├── Scripts/     (for all C# scripts)
├── Scenes/      (for Unity scenes)
├── Prefabs/     (for reusable game objects)
├── Art/
│   ├── Materials/   (for Unity materials)
│   └── Textures/    (for texture files)
└── Audio/
    ├── SFX/         (for sound effects)
    └── Music/       (for background music)
```

#### User
> BTW, let's have a record documentation on how we interact each other, record on my prompt and your response in that doc. named it Journey.md

#### Assistant
Created Journey.md to document our development process and interactions, including timestamps, prompts, and key decisions (this document).

#### User
> perfect, don't forget to update this doc in the later interactions between us, mate

#### Assistant
Acknowledged the request to maintain documentation of our interactions in Journey.md and committed to updating it with all future exchanges.

#### User
> tell me what I just added in the scene

#### Assistant
Checked the scene and found that a new "Sphere" object has been added to the SampleScene, which now contains:
- Main Camera
- Directional Light
- Sphere (newly added)

#### User
> help me setup the git repo for this project

#### Assistant
Set up Git repository with the following steps:
1. Created `.gitignore` file with Unity-specific patterns
2. Initialized Git repository with `git init`
3. Renamed default branch to 'main'
4. Added all files to staging
5. Created initial commit with project setup and folder structure

The repository is now ready for version control with appropriate Unity-specific ignore patterns in place.

#### User
> lets push the repo to remote

#### Assistant
Successfully set up remote repository and pushed our code:
1. Added remote origin: https://github.com/songqiaoluckyvr/GameJame_0328
2. Pushed main branch to remote
3. Set up tracking between local and remote main branch

All our project files are now safely stored on GitHub and ready for collaboration.

#### User
> Alright, today we will make platformer game called antidote seeker, where player's health is decreasing constantily if he didn't find the next available antidote, along the way there will be many obstacles that player needs to avoid. the game will be 2.5d. let's have a GDD planned first

#### Assistant
Created a comprehensive GDD (Game Design Document) for "Antidote Seeker" with the following key elements:

1. **Game Overview**
   - 2.5D Platformer with survival elements
   - PC platform targeting 60 FPS
   - Focus on time pressure and platforming skill

2. **Core Mechanics**
   - Constantly decreasing health system
   - Antidote collection mechanics
   - Platform movement (run, jump, double jump, wall slide, wall jump)
   - Obstacle avoidance

3. **Level Design**
   - Linear progression with branching paths
   - Strategic antidote placement
   - Various obstacles (static and dynamic)
   - Checkpoint system

4. **Technical Requirements**
   - Cinemachine for camera control
   - Post-processing for visual effects
   - Physics-based movement
   - Particle systems
   - Dynamic audio system

5. **Development Priorities**
   - Phase 1: Core Mechanics
   - Phase 2: Level Building
   - Phase 3: Polish
   - Phase 4: Refinement

Full details available in GDD.md

#### User
> Now, lets focus on build the player related logics, create a sub folder in scripts name Player, where we will have playermovement, and playerhealth script, also have a playercontroller which will later talk with other systems such as ui

#### Assistant
Created the Player scripts structure with three main components:

1. **PlayerController.cs**
   - Central coordinator for player systems
   - Manages communication between components
   - Handles player state (Alive/Dead)
   - Event system for state changes
   - Interfaces with UI and other game systems

2. **PlayerMovement.cs**
   - Physics-based 2.5D movement system
   - Features implemented:
     - Run with acceleration/deceleration
     - Jump with coyote time
     - Double jump
     - Wall slide and wall jump
     - Character flipping
   - Movement parameters for easy tuning
   - Ground and wall detection

3. **PlayerHealth.cs**
   - Health management system
   - Features implemented:
     - Constant health drain
     - Antidote collection
     - Critical health threshold
     - Health restoration
     - Event system for UI updates
   - Configurable parameters:
     - Max/starting health
     - Drain rate
     - Antidote restore amount
     - Critical health threshold

All scripts follow the project's coding standards with:
- Proper region organization
- SerializeField for Unity inspector exposure
- Event-based communication
- Clear documentation
- Modular design for easy expansion

#### User
> Let's focus on player control first, since we decide it will be a 2.5d game which player will just move left and right and jump, so the control would be "A" move left, 'D' move right and "Space" jump. clear up the previous implementation and update with this request

#### Assistant
Simplified the PlayerMovement script to focus on essential 2.5D controls:

1. **Movement Controls**
   - A/D or Left/Right arrows for horizontal movement
   - Space for jumping
   - Removed complex features:
     - Wall sliding/jumping
     - Double jumping
     - Coyote time
     - Jump buffering
     - Variable jump height

2. **Key Changes**
   - Simplified movement parameters:
     - Basic move speed
     - Jump force
     - Ground check distance
   - Streamlined physics:
     - Direct velocity control for responsive movement
     - Basic ground detection
     - Constrained Z-axis movement and rotation
   - Maintained core features:
     - Character flipping based on movement direction
     - Ground checking
     - Movement enable/disable functionality

3. **Code Organization**
   - Kept clean region organization
   - Maintained essential comments
   - Preserved Unity lifecycle methods
   - Kept SerializeField for easy tuning 

#### User
> Find out why jump is not working, is it about the isGround check? also have some debug log added for me to understand in game play

#### Assistant
Investigated and fixed the PlayerMovement script's ground check issues:

1. **Identified Problems**
   - Ground check was using undefined "Ground" layer
   - Ground check distance was too short (0.1f)
   - Lack of debug visualization and logging

2. **Key Fixes**
   - Added serializable `_groundLayer` field for proper layer assignment
   - Increased ground check distance to 0.2f
   - Added comprehensive debug logging system

3. **Debug Features Added**
   - Visual ray for ground check (green when grounded, red when not)
   - Console logs for:
     - Jump attempts and executions
     - Ground state changes
     - Current velocity and grounded state
     - Initial setup confirmation
   - Added `_debugMode` toggle in inspector

4. **Code Improvements**
   - Added ground state change tracking
   - Moved ground check before jump input handling
   - Improved code organization and comments
   - Added more descriptive debug messages 

### Development Progress - March 28, 2024

#### Initial Project Setup
1. Created Unity project for "Antidote Seeker"
2. Set up Git repository and pushed to remote
3. Established basic project structure

#### Player Movement Implementation
1. Created PlayerMovement script with basic 2.5D movement
2. Implemented ground detection using collision events
3. Added jump mechanics with 0.3s cooldown
4. Enhanced falling mechanics with increased gravity multiplier

#### Health System Development
1. Implemented PlayerHealth script with:
   - Health drain system (100 to 0 in 1 second)
   - Event system for health changes
   - Antidote collection functionality
   - Critical health state monitoring

2. Created UI Health Bar system:
   - Set up UI folder structure
   - Implemented HealthBar script with:
     - 300px max width scaling with 100 health
     - Color changes (green to red) at critical health
     - Event-based updates
     - Clean event handling

#### Health System Enhancements
- Added debug functionality to test health restoration:
  - Implemented a debug mode toggle with `_debugMode` serialized field
  - Added keyboard input detection for health testing (0 key adds 10 health points)
  - Health restoration respects max health limit of 100
  - Debug messages show current health after restoration when debug mode is enabled
  - Health bar UI updates automatically through event system
- Debug features help test:
  - Health restoration mechanics
  - Maximum health clamping
  - UI update responsiveness
  - Event system functionality
  - Health drain and restoration interaction

#### Death System Enhancement
- Enhanced death handling between PlayerHealth and PlayerController:
  - Added comprehensive debug logging system
  - Improved death sequence flow:
    1. Health depletion triggers death event
    2. PlayerController handles state change
    3. Movement is disabled
    4. Death animation is played
    5. Health drain is paused
  - Added detailed health change logging
  - Implemented proper reset functionality
  - Connected all components through event system
  - Added state validation and safety checks

#### Animation System Integration
- Integrated DeerAnimationAndSoundController with player systems:
  - Added animation triggers to PlayerMovement:
    - Run animation during horizontal movement
    - Jump animation when jumping
    - Idle animation when stationary
  - Connected PlayerController with animations:
    - Death animation on health depletion
    - Spin animation on antidote collection
  - Implemented automatic component reference handling
  - Added null checks for robustness
  - Maintained existing debug functionality

#### Animation System Refinement
- Improved animation state transitions in PlayerMovement:
  - Run animation now only plays while movement keys are actively pressed
  - Added direct key state checking (A/D and Left/Right arrows)
  - Immediate transition to idle when keys are released
  - Added debug logging for animation state changes
  - Maintained ground check for idle state
  - Improved responsiveness of animation transitions

#### Animation Priority System
- Enhanced animation state handling:
  - Prevented run animation during jumps
  - Prioritized jump animation over run animation
  - Maintained clean transitions between states
  - Added clear code documentation
  - Improved animation state logic organization

#### Animation System Bug Fix
- Fixed animation playback issues:
  - Added state tracking with `_isRunning` flag
  - Prevented animation state updates every frame
  - Only trigger animations on actual state changes
  - Added proper state handling for:
    - Starting to run
    - Stopping running
    - Landing from jump
    - Leaving ground
  - Improved debug messages for state transitions
  - Fixed animation interruption issues
  - Added proper state cleanup in DisableMovement

#### Camera System Implementation
- Created CameraController for platformer-style camera:
  - Smooth player following with Cinemachine
  - Look-ahead system based on player movement
  - Configurable parameters:
    - Camera offset (default: slightly above and behind player)
    - Follow speed and damping
    - Look-ahead distance and speed
  - Debug features:
    - Movement logging
    - Component validation
    - Setup confirmation
  - Quality of life features:
    - Automatic player finding
    - Runtime parameter adjustment
    - Smooth transitions

#### Camera System Simplification
- Replaced Cinemachine with standard Unity camera following:
  - Removed Cinemachine dependency
  - Implemented smooth following using SmoothDamp
  - Added look-ahead system based on player velocity
  - Maintained key features:
    - Configurable offset and speeds
    - Debug logging
    - Smooth transitions
  - Improved performance with:
    - Optimized component caching
    - Efficient position updates in LateUpdate
    - Reduced memory allocation
    - Simplified state management

#### Map Generation System
- Implemented tile-based map generation:
  - Three tile types:
    - Start tile (initial spawn point)
    - Connecting tiles (main gameplay segments)
    - End tile (for future use)
  - Efficient tile recycling system:
    - Maintains only 3 active tiles
    - 30-unit spacing between tiles
    - Automatic tile recycling based on player position
  - Features:
    - Random connecting tile selection
    - Seamless tile transitions
    - Bidirectional movement support
    - Memory-efficient design
  - Debug features:
    - Tile recycling logging
    - Initialization validation
    - Player position tracking
  - Reset functionality for game restart

#### Map Generation Enhancement
- Added special tile handling:
  - Start tile management:
    - Removed after player leaves initial area
    - Prevents backtracking past start
    - Replaced with connecting tile
  - End tile implementation:
    - Appears after 10 tile rotations
    - Marks level completion point
    - Prevents further tile recycling
  - Progress tracking:
    - Added tile rotation counter
    - State tracking for start/end tiles
    - Debug logging for tile changes
  - Improved tile recycling logic:
    - Prevents recycling past boundaries
    - Handles special tile transitions
    - Maintains game flow direction

#### Antidote System Implementation
- Created comprehensive antidote spawning and collection system:
  - AntidoteManager features:
    - Random spawn intervals (3-7 seconds)
    - Spawn position ahead of player
    - Configurable spawn height
    - Automatic cleanup
  - Individual Antidote behavior:
    - Constant fall speed
    - Z-axis rotation animation
    - Trigger-based collection
    - Ground destruction
  - Integration with player:
    - Health restoration on collection
    - Automatic cleanup
    - Event system connection
  - Debug features:
    - Spawn timing logs
    - Collection verification
    - Position tracking
    - Component validation

#### Antidote System Refinement
- Updated antidote positioning for 2.5D setup:
  - Set spawn position to Z = 5
  - Added Z-position maintenance:
    - Prevents Z-axis drift
    - Locks antidotes to 2.5D plane
    - Improved velocity handling
  - Enhanced position control:
    - Direct Z-position correction
    - Velocity Z-component zeroing
    - Position validation each frame
  - Maintained existing features:
    - Falling behavior
    - Rotation animation
    - Collection mechanics

#### Next Steps
- Implement antidote spawning system
- Add obstacles and hazards
- Create level design
- Implement game over and restart mechanics

#### Technical Notes
- Using Unity version 2022.3.29f1
- Project follows component-based architecture
- Event-driven communication between systems 