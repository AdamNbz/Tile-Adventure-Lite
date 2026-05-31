# Tile Adventure Lite 🎮

A match-3 puzzle game built with **Unity**, featuring dynamic tile-based gameplay with a clean MVC architecture. Complete levels by matching identical tiles in groups of three!

## 📋 Table of Contents

- [Game Overview](#game-overview)
- [How to Play](#how-to-play)
- [Game Mechanics](#game-mechanics)
- [Level Progression](#level-progression)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [Controls](#controls)
- [Technical Highlights](#technical-highlights)
- [Future Improvements](#future-improvements)

## Game Overview

**Tile Adventure Lite** is a casual puzzle game where players tap tiles from the game board and place them in a rack. When three identical tiles are grouped together in the rack, they automatically match and disappear. The goal is to clear the entire board by strategically selecting tiles and making matches.

The game features:
- **10 Progressive Levels** with increasing difficulty
- **Dynamic Difficulty Scaling** with more icons, layers, and tighter layouts as you progress
- **Sound Effects & Music** for immersive gameplay
- **Smooth Animations** using async/await patterns
- **Home, Loading, and Gameplay Scenes** for a complete user experience

## How to Play

### Objective
Clear all tiles from the board to win the level.

### Gameplay Steps
1. **Tap a Tile**: Select an exposed tile from the game board
2. **Fill Your Rack**: Tiles are automatically placed into your 7-slot rack
3. **Make Matches**: When three or more tiles with the same icon are in the rack together, they automatically match and disappear
4. **Clear the Board**: Continue matching tiles until both the board and rack are empty to win
5. **Don't Overfill**: If your rack reaches capacity before clearing tiles, you lose

### Tips
- Plan your moves strategically to avoid filling your rack without making matches
- Tiles are grouped by icon in the rack, making it easier to identify matching opportunities
- Later levels have overlapping tiles in multiple layers—only exposed tiles can be tapped

## Game Mechanics

### Tile System
- **Exposed Tiles**: Tiles that can be tapped (visually brighter)
- **Hidden Tiles**: Tiles covered by other tiles (visually dimmed)
- **Layering**: Tiles are arranged in layers (1-3 depending on level difficulty)
- **Icon Pool**: Each level uses a pool of icons with exactly 3 instances per icon, ensuring solvability

### Rack System
- **Capacity**: 7 slots per level
- **Grouping**: Tiles automatically group by icon type
- **Match Detection**: When 3+ identical tiles are adjacent, they match automatically
- **Clearing**: Matched tiles disappear and remaining tiles shift to fill gaps

### Board System
- **Dynamic Generation**: Tiles are generated from level data with procedurally varied layouts
- **Overlap Detection**: The system automatically calculates which tiles are "exposed" (not covered)
- **Progressive Removal**: As tiles are tapped, more tiles become exposed for future moves

### Win/Lose Conditions
- **Win**: Both the board and rack are completely empty
- **Lose**: Your rack is full and you can't tap any more tiles

## Level Progression

The game features 10 levels with progressive difficulty:

| Level | Target Triples | Unique Icons | Max Layers | Difficulty |
|-------|----------------|--------------|-----------|------------|
| 1-2   | 3-4            | 4-5          | 1         | Easy       |
| 3-4   | 4-5            | 5-7          | 1-2       | Medium     |
| 5-7   | 5-7            | 7-9          | 2-3       | Hard       |
| 8-10  | 7-8            | 9-10+        | 2-3       | Very Hard  |

**Difficulty Features:**
- ✅ Increasing target triples (more tiles to match)
- ✅ Growing icon variety (harder to find matches)
- ✅ More overlapping tiles (fewer exposed tiles)
- ✅ Tighter board layouts (complex spatial arrangements)

## Architecture

The project follows the **Model-View-Controller (MVC)** pattern for clean separation of concerns:

### Model (Data)
- `GameProgress.cs`: Tracks player progress and level completion
- `LevelData.cs`: Defines individual level configuration (tile layouts, icons, targets)
- `LevelDatabase.cs`: Central registry of all 10 levels

### View (UI & Visuals)
- `Tile.cs`: Individual tile rendering and tap animations
- UI Managers: Handle menu, loading, and game screens
- Audio effects and visual feedback

### Controller (Logic)
- `GameManager.cs`: Overall game flow and win/lose logic
- `BoardManager.cs`: Tile generation, exposure detection, removal logic
- `RackManager.cs`: Tile placement, movement, and match detection
- `LevelManager.cs`: Level selection and progression

### Key Design Decisions

1. **Async/Await Over Coroutines**: Smooth tile movement and match animations without callback spaghetti
2. **ScriptableObjects**: Easy tweaking of levels and difficulty parameters without code changes
3. **Scene-Based Navigation**: Clear flow from Loading → Home → Gameplay
4. **Solvability Assurance**: All levels guaranteed solvable by using exact multiples of 3 for tiles

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── AudioManager.cs         # Singleton for sound/music
│   │   └── LevelManager.cs         # Level selection management
│   ├── Gameplay/
│   │   ├── GameManager.cs          # Main game controller
│   │   ├── BoardManager.cs         # Board logic
│   │   ├── RackManager.cs          # Rack logic
│   │   └── Tile.cs                 # Tile view & interaction
│   ├── Data/
│   │   ├── GameProgress.cs         # Player progress tracking
│   │   ├── LevelData.cs            # Level configuration
│   │   └── LevelDatabase.cs        # Level registry
│   └── UI/
│       ├── HomeManager.cs          # Home screen
│       └── LoadingManager.cs       # Loading screen
├── Scenes/
│   ├── Loading Scene.unity
│   ├── Home Scene.unity
│   └── Gameplay Scene.unity
├── Prefabs/
│   ├── Tile.prefab
│   └── UI elements
├── Data/
│   └── Level ScriptableObjects (Level 1-10)
├── Fonts/
├── Images/
├── Animations/
├── Resources/
└── Settings/

ProjectSettings/              # Unity project configuration
Packages/                     # Package dependencies
Library/                      # Generated by Unity (local cache)
Logs/                         # Runtime logs
```

## Installation & Setup

### Requirements
- **Unity 2022 LTS** or later
- macOS / Windows / Linux
- Visual Studio or Rider (optional, for code editing)

### Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/AdamNbz/Tile-Adventure-Lite.git
   cd "Tile Adventure Lite"
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open Project"
   - Select the project folder
   - Wait for Unity to import assets (first time takes ~2-5 minutes)

3. **Play the Game**
   - Open the "Loading Scene" (Assets/Scenes/Loading Scene.unity)
   - Press **Play** in the Unity Editor or build for your target platform

### Building for Distribution

**Build for WebGL (Browser):**
```
File → Build Settings → Select "WebGL" → Build
```

**Build for Windows/Mac/Linux:**
```
File → Build Settings → Select your platform → Build
```

## Controls

| Input | Action |
|-------|--------|
| **Left Mouse Click / Tap** | Select exposed tile |
| **ESC / Back Button** | Return to home menu |

## Technical Highlights

### Performance Optimizations
- ✅ **Async Tile Movement**: Smooth animations without frame stuttering
- ✅ **Lazy Loading**: Levels loaded on-demand
- ✅ **Object Pooling Ready**: Infrastructure for tile reuse (not yet implemented)

### Code Quality
- ✅ **MVC Architecture**: Clean separation of concerns
- ✅ **ScriptableObjects**: Flexible, data-driven design
- ✅ **Event System**: Decoupled communication between managers
- ✅ **Type-Safe Level Loading**: Null checking and error handling

### User Experience
- ✅ **Visual Feedback**: Tiles dim when not exposed, scale when tapped
- ✅ **Audio Cues**: Sound effects for taps and matches
- ✅ **Smooth Transitions**: Scenes and animations flow naturally
- ✅ **Progress Tracking**: Resume from the last played level

## Future Improvements

With additional development time, consider:

1. **Object Pooling** 🔄
   - Reuse tile instances to improve performance on low-end devices
   - Reduce garbage collection spikes

2. **Enhanced Juice** ✨
   - Particle effects for tile matches
   - Screen shake on successful matches
   - Confetti animations for level completion
   - Score/combo counter animations

3. **Visual Level Editor** 🎨
   - Drag-and-drop tile placement in the editor
   - Visual preview of level difficulty
   - Real-time solvability checking

4. **Tutorial System** 📚
   - Animated hand guide (hand.png asset already available)
   - Interactive tooltips for first level
   - Difficulty-specific tips

5. **Save System Upgrade** 💾
   - JSON-based save system replacing PlayerPrefs
   - Track individual level scores and times
   - Statistics dashboard

6. **Additional Features** 🎯
   - Power-ups and special tiles
   - Daily challenges
   - Leaderboards
   - Additional level packs
   - Sound settings toggle
   - Difficulty modes (Easy/Normal/Hard)

## Credits

**Game Design & Development**: Adam NBZ  
**Engine**: Unity 3D  
**Built For**: Interview / Portfolio Project

---

**Enjoy the game and happy puzzle solving!** 🎉
