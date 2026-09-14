# 🐍 Modern Snake Game

A complete, polished, modern Snake game desktop application built with C# and Windows Forms. This project demonstrates clean Object-Oriented Programming principles and is suitable as a university-level C#/OOP project.

## Features

### Core Gameplay
- **Classic Snake mechanics** with smooth movement and controls
- **Multiple game modes**: Classic, Time Attack, Obstacle, Endless
- **Four difficulty levels**: Easy, Medium, Hard, Extreme
- **Level progression system** with increasing challenge

### Food System
- 🍎 Normal Food (+10 points)
- ⭐ Bonus Food (+25 points)
- 💎 Rare Food (+50 points, limited time)
- ⚡ Speed Food (temporary speed boost)

### Power-Ups
- 🔵 Shield - Protects from one collision
- 🔷 Slow Motion - Slows down temporarily
- 🟠 Speed Boost - Faster movement
- 🟣 Shrink - Reduces snake length
- 🟪 Magnet - Attracts nearby food

### UI & Visual Features
- Three visual themes: Dark, Light, Neon
- Five snake skins: Classic Green, Blue, Red, Neon, Rainbow
- Touch-friendly on-screen controls
- Smooth animations and visual effects
- Modern, polished interface

### Data Persistence
- JSON-based save system
- Persistent high scores (Top 10 leaderboard)
- Player statistics tracking
- Settings persistence

### Audio
- Background music
- Sound effects for actions
- Configurable sound/music settings

## Technologies Used

- **C#** (.NET 8)
- **Windows Forms (WinForms)**
- **System.Drawing** for graphics
- **Newtonsoft.Json** for data persistence
- **Object-Oriented Programming** principles

## How to Run

### Prerequisites
- .NET 8 SDK or later
- Windows OS (for WinForms)

### Steps

1. **Clone or download** the project to your local machine

2. **Navigate to the project folder**:
   ```bash
   cd ModernSnakeGame
   ```

3. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

4. **Build the project**:
   ```bash
   dotnet build
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

Or open `ModernSnakeGame.csproj` in Visual Studio and press F5.

## Controls

### Keyboard
| Key | Action |
|-----|--------|
| ↑ / W | Move Up |
| ↓ / S | Move Down |
| ← / A | Move Left |
| → / D | Move Right |
| P | Pause/Resume |
| R | Restart |

### Touch/Mouse
- Use the on-screen directional buttons (▲ ▼ ◀ ▶)

## OOP Concepts Demonstrated

### Encapsulation
- Private fields with public properties
- Controlled access through methods
- Internal state management in classes like `Snake`, `GameManager`

### Inheritance
```
Food (base class)
 ├── NormalFood
 ├── BonusFood
 ├── RareFood
 └── SpeedFood

PowerUp (base class)
 ├── ShieldPowerUp
 ├── SlowMotionPowerUp
 ├── SpeedBoostPowerUp
 ├── ShrinkPowerUp
 └── MagnetPowerUp
```

### Polymorphism
- Overridden `Consume()` method in different Food types
- Overridden `Activate()` and `Deactivate()` in PowerUp subclasses
- Overridden `Draw()` methods for custom rendering

### Abstraction
- Abstract base classes `Food` and `PowerUp`
- Clear separation between game logic and UI

### Interfaces
- `IMovable` - For objects that can move
- `ICollidable` - For objects that can collide
- `ISaveable` - For objects that can be serialized

## Project Structure

```
ModernSnakeGame/
│
├── Models/           # Data models
│   ├── Player.cs
│   ├── Snake.cs
│   ├── Food.cs
│   ├── Obstacle.cs
│   └── PowerUp.cs
│
├── Game/             # Game logic
│   ├── GameManager.cs
│   ├── CollisionManager.cs
│   ├── ScoreManager.cs
│   ├── LevelManager.cs
│   └── DifficultyManager.cs
│
├── Managers/         # System managers
│   ├── SaveManager.cs
│   ├── SettingsManager.cs
│   ├── SoundManager.cs
│   ├── LeaderboardManager.cs
│   └── StatisticsManager.cs
│
├── UI/               # Windows Forms
│   ├── MainMenuForm.cs
│   ├── GameForm.cs
│   ├── SettingsForm.cs
│   ├── LeaderboardForm.cs
│   ├── StatisticsForm.cs
│   ├── HowToPlayForm.cs
│   ├── GameOverForm.cs
│   └── PlayerNameForm.cs
│
├── Rendering/        # Graphics rendering
│   └── GameRenderer.cs
│
├── Interfaces/       # Interface definitions
│   └── IInterfaces.cs
│
├── Assets/           # Game assets
│   └── Sounds/       # Audio files (optional)
│
├── Program.cs        # Application entry point
├── ModernSnakeGame.csproj
└── README.md
```

## Class Responsibilities

| Class | Responsibility |
|-------|---------------|
| `GameManager` | Orchestrates all game logic |
| `Snake` | Manages snake movement, growth, collision |
| `Food` (and subclasses) | Different food types with unique behaviors |
| `PowerUp` (and subclasses) | Temporary power-up effects |
| `CollisionManager` | Handles all collision detection |
| `ScoreManager` | Tracks and manages scores |
| `LevelManager` | Manages level progression |
| `DifficultyManager` | Controls difficulty settings |
| `GameRenderer` | Handles all graphics rendering |
| `SaveManager` | JSON serialization/deserialization |
| `LeaderboardManager` | Manages high scores |
| `StatisticsManager` | Tracks player statistics |
| `SoundManager` | Audio playback |

## Testing Checklist

- ✅ Snake movement in all directions
- ✅ Invalid reverse direction prevention
- ✅ Food collision and score calculation
- ✅ Snake growth on eating
- ✅ Wall collision detection
- ✅ Self-collision detection
- ✅ Obstacle collision
- ✅ Level progression
- ✅ Difficulty affects gameplay
- ✅ Power-up activation and expiration
- ✅ Pause/resume functionality
- ✅ Restart functionality
- ✅ Game over handling
- ✅ High score saving
- ✅ Leaderboard sorting
- ✅ Settings persistence
- ✅ Theme switching
- ✅ Sound toggle

## Architecture Explanation

The game follows a **component-based architecture** with clear separation of concerns:

1. **Models** - Pure data classes representing game entities
2. **Game Logic** - Core gameplay mechanics independent of UI
3. **Managers** - Cross-cutting concerns (save, sound, stats)
4. **UI** - Windows Forms for user interaction
5. **Rendering** - Dedicated graphics layer

This architecture makes the code:
- **Maintainable** - Changes in one area don't affect others
- **Testable** - Game logic can be tested independently
- **Extensible** - New features can be added easily
- **Understandable** - Clear responsibilities for each class

## License

This project is created for educational purposes. Feel free to use and modify for learning.
