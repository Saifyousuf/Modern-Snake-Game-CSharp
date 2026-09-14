using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Models;

namespace ModernSnakeGame.Game;

/// <summary>
/// Manages the score system for the game.
/// </summary>
public class ScoreManager
{
    private int _currentScore;
    private int _highScore;
    private int _foodCollected;

    public int CurrentScore => _currentScore;
    public int HighScore => _highScore;
    public int FoodCollected => _foodCollected;

    public event Action<int>? OnScoreChanged;
    public event Action<int>? OnHighScoreChanged;
    public event Action<int>? OnFoodCollectedChanged;

    public ScoreManager()
    {
        _currentScore = 0;
        _highScore = 0;
        _foodCollected = 0;
    }

    public void AddPoints(int points)
    {
        _currentScore += points;
        OnScoreChanged?.Invoke(_currentScore);

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            OnHighScoreChanged?.Invoke(_highScore);
        }
    }

    public void IncrementFoodCollected()
    {
        _foodCollected++;
        OnFoodCollectedChanged?.Invoke(_foodCollected);
    }

    public void Reset()
    {
        _currentScore = 0;
        _foodCollected = 0;
        OnScoreChanged?.Invoke(_currentScore);
        OnFoodCollectedChanged?.Invoke(_foodCollected);
    }

    public void SetHighScore(int highScore)
    {
        _highScore = highScore;
    }
}
