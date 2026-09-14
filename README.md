# 🐍 Modern Snake Game

A modern and feature-rich **Snake Game developed using C# and .NET 8 Windows Forms**. The project is designed as a university-level **Object-Oriented Programming (OOP)** project with multiple game modes, difficulty levels, power-ups, obstacles, persistent data, leaderboard, statistics, themes, and customizable snake skins.

## 🎮 Features

* 🐍 Classic Snake gameplay
* 🎯 Multiple game modes

  * Classic
  * Time Attack
  * Obstacle
  * Endless
* ⚡ Four difficulty levels

  * Easy
  * Medium
  * Hard
  * Extreme
* 🍎 Multiple food types

  * Normal Food
  * Bonus Food
  * Rare Food
  * Speed Food
* 💥 Power-ups

  * Shield
  * Slow Motion
  * Speed Boost
  * Shrink
  * Magnet
* 🧱 Dynamic obstacles
* 📈 Progressive level system
* 🏆 Persistent high score
* 🥇 Top 10 leaderboard
* 📊 Game statistics
* 💾 JSON-based data persistence
* 🎨 Multiple themes

  * Dark
  * Light
  * Neon
* 🐍 Multiple snake skins
* ⌨️ Keyboard controls

  * Arrow Keys
  * WASD
* 👆 Touch/on-screen controls
* ⏸️ Pause and Resume
* 🔄 Restart functionality
* 🔊 Sound effects and music settings
* 🛡️ Collision detection
* 🎮 Modern Windows Forms UI

## 🛠️ Technologies Used

* **Language:** C#
* **Framework:** .NET 8
* **UI Framework:** Windows Forms
* **Data Storage:** JSON
* **IDE:** Visual Studio
* **Platform:** Windows

## 🏗️ Project Architecture

The project follows an organized OOP-based architecture.

```text
ModernSnakeGame/
│
├── Game/
│   ├── GameManager.cs
│   ├── CollisionManager.cs
│   ├── DifficultyManager.cs
│   ├── LevelManager.cs
│   └── ScoreManager.cs
│
├── Models/
│   ├── Snake.cs
│   ├── Food.cs
│   ├── PowerUp.cs
│   ├── Obstacle.cs
│   └── Player.cs
│
├── Managers/
│   ├── SaveManager.cs
│   ├── LeaderboardManager.cs
│   ├── StatisticsManager.cs
│   └── SoundManager.cs
│
├── Rendering/
│   └── GameRenderer.cs
│
├── Interfaces/
│   └── IInterfaces.cs
│
├── UI/
│   ├── MainMenuForm.cs
│   ├── GameForm.cs
│   ├── GameOverForm.cs
│   ├── HowToPlayForm.cs
│   ├── LeaderboardForm.cs
│   ├── PlayerNameForm.cs
│   ├── SettingsForm.cs
│   └── StatisticsForm.cs
│
├── Program.cs
└── ModernSnakeGame.csproj
```

## 🧩 OOP Concepts Demonstrated

### Encapsulation

The project uses private fields and controlled public methods/properties.

Example:

```text
Snake
 ├── Private body data
 ├── Head property
 ├── ChangeDirection()
 └── Grow()
```

### Inheritance

Food types inherit from the base `Food` class:

```text
Food
├── NormalFood
├── BonusFood
├── RareFood
└── SpeedFood
```

Power-ups follow a similar inheritance structure:

```text
PowerUp
├── ShieldPowerUp
├── SlowMotionPowerUp
├── SpeedBoostPowerUp
├── ShrinkPowerUp
└── MagnetPowerUp
```

### Polymorphism

Different food and power-up classes provide their own behavior through overridden methods such as:

```text
Consume()
Activate()
Deactivate()
Draw()
```

### Abstraction

Abstract base classes such as `Food` and `PowerUp` define common behavior while allowing derived classes to implement specific functionality.

### Interfaces

The project uses interfaces such as:

```text
IMovable
ICollidable
ISaveable
```

to define common contracts between classes.

### Composition

`GameManager` coordinates major game components such as:

```text
Snake
Food
PowerUps
Obstacles
ScoreManager
LevelManager
CollisionManager
DifficultyManager
```

## 🎮 Controls

| Action         | Keyboard |
| -------------- | -------- |
| Move Up        | ↑ / W    |
| Move Down      | ↓ / S    |
| Move Left      | ← / A    |
| Move Right     | → / D    |
| Pause / Resume | P        |
| Restart        | R        |

On-screen touch controls are also available.

## 📊 Game System

The basic game flow is:

```text
Start Game
    ↓
Create Snake
    ↓
Spawn Food / Objects
    ↓
Move Snake
    ↓
Check Collision
    ↓
Consume Food / Power-up
    ↓
Update Score
    ↓
Check Level
    ↓
Render Game
    ↓
Continue
```

When the player loses:

```text
Game Over
    ↓
Save Score
    ↓
Update High Score
    ↓
Update Leaderboard
    ↓
Update Statistics
    ↓
Save Data
```

## 💾 Data Persistence

The game uses JSON-based persistence for storing important information such as:

* High scores
* Leaderboard
* Statistics
* Settings

Saved information remains available after restarting the application.

## 🖥️ Requirements

To run the project, you should have:

* Windows 10/11
* .NET 8 SDK
* Visual Studio 2022 or later
* Windows Forms support

## 🚀 How to Run

### Using Visual Studio

1. Clone or download the repository.
2. Open the `.csproj` file in Visual Studio.
3. Restore NuGet dependencies if required.
4. Build the project.
5. Run the application.

### Using .NET CLI

Open a terminal inside the project directory:

```bash
dotnet restore
dotnet build
dotnet run
```

## 🧪 Testing

The project should be tested for:

* Application startup
* Player name validation
* Snake movement
* Reverse movement prevention
* Food spawning
* Food consumption
* Collision detection
* Power-ups
* Obstacles
* Level progression
* Pause/Resume
* Restart
* Game Over
* High-score persistence
* Leaderboard
* Statistics
* Settings
* Theme switching
* Sound controls

## 🎓 Academic Purpose

This project was developed as a **C# Object-Oriented Programming project** to demonstrate practical application of:

* Encapsulation
* Inheritance
* Polymorphism
* Abstraction
* Interfaces
* Composition
* Event-driven programming
* File handling
* GUI programming
* Software design principles

## 👨‍💻 Project Members

* **Saif Yousuf**


## 📌 Project Status

**Development Status:** Completed / Final Version

The project focuses on providing a polished Snake Game experience while demonstrating practical C# OOP concepts and software architecture.

---

## 📄 License

This project was created for academic/educational purposes.
