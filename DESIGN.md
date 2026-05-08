# Tile Trip Match - Design Document

## Architecture Decisions
- **MVC Pattern**: Separated data (`LevelData`, `GameProgress`), logic (`GameManager`, `BoardManager`, `RackManager`), and view (`Tile`, UI managers).
- **Scene-Based Flow**: Implemented `Loading` -> `Home` -> `Gameplay` as requested.
- **Async/Await**: Used `System.Threading.Tasks` for tile movement and match animations to avoid coroutine spaghetti and ensure smooth sequences.
- **ScriptableObjects**: Used for Level Data and the Level Database to allow easy tweaking of level layouts and difficulty.

## Level Data Structure
- **LevelData SO**: Contains a list of `TileLayoutData` (position and layer), rack slots, and target triples.
- **LevelDatabase SO**: A central registry for all 10 levels, making it easy for the `GameManager` to load the correct level.
- **Dynamic Generation**: Levels were generated with a script that ensures the total number of tiles is a multiple of 3, guaranteeing solvability.

## Solvability Assurance
- **Multiple of 3**: The level generator calculates the total tiles as `targetTriples * 3` plus optional buffer triples.
- **Icon Pool**: Icons are assigned from a pool that contains exactly 3 instances of each icon used in the level.
- **Layering**: While layout is randomized, the `BoardManager` checks for overlaps to determine which tiles are "exposed" (tappable).

## Difficulty Progression
- **Levels 1-10**:
    - **Increasing Target**: Target triples increase from 3 to 8.
    - **Increasing Icon Variety**: Number of unique icons increases from 4 to 10+.
    - **Increasing Layers**: More overlapping tiles in later levels (up to 3 layers).
    - **Tighter Layout**: Tiles are placed closer together in later levels.

## Improvements with More Time
- **Object Pooling**: For tiles to improve performance on low-end devices.
- **More "Juice"**: Particle effects for matching, screen shake on match, and better UI transitions.
- **Level Editor**: A visual tool to design levels instead of relying on procedural generation scripts.
- **Tutorial System**: Using the provided `hand.png` to guide the player in the first level.
- **Save System**: Use JSON instead of PlayerPrefs for more complex data (like individual level scores).
