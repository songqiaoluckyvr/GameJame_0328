# Antidote Seeker - Game Design Document

## Game Overview
**Genre**: 2.5D Platformer with Survival Elements
**Target Platform**: PC
**Target Audience**: Casual to Mid-core gamers
**Game Flow**: Linear levels with increasing difficulty

## Core Mechanics

### Health System
- Player's health constantly decreases over time
- Health drain rate increases as levels progress
- Visual and audio feedback for health status
- Death occurs when health reaches zero

### Antidote System
- Antidotes are scattered throughout levels
- Each antidote restores a portion of health
- Antidotes glow/pulse to be easily visible
- Collection triggers particle effects and sound
- Limited time window to reach next antidote

### Movement & Controls
- Basic Movement:
  - Run left/right
  - Jump
  - Double jump
  - Wall slide
  - Wall jump
- Smooth, responsive controls
- Camera follows player with slight lead in movement direction

### Obstacles
1. **Static Obstacles**
   - Platforms with varying heights
   - Gaps and pits
   - Walls requiring wall jumps
   - Spikes and hazards

2. **Dynamic Obstacles**
   - Moving platforms
   - Swinging objects
   - Falling debris
   - Rotating hazards

### Level Design
- Linear progression with branching paths
- Each level introduces new challenges
- Antidotes placed strategically to create tension
- Multiple routes with risk/reward choices
- Checkpoints at key positions

## Visual Style
- Modern minimalist 3D assets
- High contrast between interactive and background elements
- Clear visual language for:
  - Hazards (red highlights)
  - Antidotes (green glow)
  - Safe zones (blue tint)
- Particle effects for:
  - Antidote collection
  - Health drain
  - Death sequence
  - Jump effects

## Audio Design
1. **Sound Effects**
   - Jump sounds
   - Landing impacts
   - Antidote collection
   - Health drain warning
   - Death sound
   - Environmental sounds

2. **Music**
   - Dynamic soundtrack that intensifies with low health
   - Ambient background music
   - Success/failure jingles

## UI Elements
- Health bar (constantly draining)
- Time to next antidote needed
- Current level progress
- Death counter
- Minimal tutorial prompts
- Clean, non-intrusive design

## Game Flow
1. **Level Structure**
   - Tutorial level introducing mechanics
   - 5 main levels with increasing complexity
   - Final challenge level
   - Each level completable in 3-5 minutes

2. **Progression**
   - New obstacles introduced gradually
   - Health drain rate increases
   - Antidote spacing becomes more challenging
   - Optional hard-to-reach antidotes for skilled players

## Technical Requirements
1. **Unity Features Needed**
   - Cinemachine for camera control
   - Post-processing for visual effects
   - Physics system for movement
   - Particle systems for effects
   - Audio mixer for dynamic sound

2. **Performance Targets**
   - 60 FPS minimum
   - Quick respawn times
   - Responsive controls

## Development Priorities
1. **Phase 1 - Core Mechanics**
   - Player movement
   - Health system
   - Basic antidote collection
   - Simple test level

2. **Phase 2 - Level Building**
   - Obstacle creation
   - Level design
   - Checkpoint system
   - Basic UI

3. **Phase 3 - Polish**
   - Visual effects
   - Sound implementation
   - Menu system
   - Tutorial

4. **Phase 4 - Refinement**
   - Playtesting
   - Difficulty balancing
   - Bug fixing
   - Performance optimization 