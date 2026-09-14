using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Game;

/// <summary>
/// Manages the level progression system.
/// </summary>
public class LevelManager
{
    private int _currentLevel;
    private int _scoreThreshold;
    private readonly Difficulty _difficulty;

    public int CurrentLevel => _currentLevel;
    public event Action<int>? OnLevelUp;

    public LevelManager(Difficulty difficulty)
    {
        _difficulty = difficulty;
        _currentLevel = 1;
        _scoreThreshold = GetBaseThreshold(difficulty);
    }

    private int GetBaseThreshold(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 50,
            Difficulty.Medium => 40,
            Difficulty.Hard => 30,
            Difficulty.Extreme => 25,
            _ => 50
        };
    }

    public void CheckLevelUp(int currentScore)
    {
        while (currentScore >= _scoreThreshold)
        {
            _currentLevel++;
            _scoreThreshold += GetBaseThreshold(_difficulty);
            OnLevelUp?.Invoke(_currentLevel);
        }
    }

    public float GetSpeedMultiplier()
    {
        // Increase speed by 5% per level
        return 1.0f + (_currentLevel - 1) * 0.05f;
    }

    public int GetObstacleCount()
    {
        int baseCount = _difficulty switch
        {
            Difficulty.Easy => 0,
            Difficulty.Medium => 2,
            Difficulty.Hard => 5,
            Difficulty.Extreme => 8,
            _ => 0
        };

        // Add 1 obstacle every 2 levels
        return baseCount + (_currentLevel / 2);
    }

    public void Reset()
    {
        _currentLevel = 1;
        _scoreThreshold = GetBaseThreshold(_difficulty);
    }
}
