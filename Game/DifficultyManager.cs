using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Game;

/// <summary>
/// Manages difficulty settings and their effects on gameplay.
/// </summary>
public class DifficultyManager
{
    private readonly Difficulty _difficulty;

    public Difficulty Difficulty => _difficulty;

    public DifficultyManager(Difficulty difficulty)
    {
        _difficulty = difficulty;
    }

    public int GetBaseSpeed()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 150,
            Difficulty.Medium => 120,
            Difficulty.Hard => 90,
            Difficulty.Extreme => 60,
            _ => 120
        };
    }

    public float GetObstacleDensity()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 0.02f,
            Difficulty.Medium => 0.05f,
            Difficulty.Hard => 0.08f,
            Difficulty.Extreme => 0.12f,
            _ => 0.05f
        };
    }

    public float GetBonusFoodChance()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 0.3f,
            Difficulty.Medium => 0.25f,
            Difficulty.Hard => 0.2f,
            Difficulty.Extreme => 0.15f,
            _ => 0.25f
        };
    }

    public float GetRareFoodChance()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 0.1f,
            Difficulty.Medium => 0.08f,
            Difficulty.Hard => 0.05f,
            Difficulty.Extreme => 0.03f,
            _ => 0.08f
        };
    }

    public float GetPowerUpChance()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 0.15f,
            Difficulty.Medium => 0.12f,
            Difficulty.Hard => 0.1f,
            Difficulty.Extreme => 0.08f,
            _ => 0.12f
        };
    }

    public int GetTimeAttackDuration()
    {
        return _difficulty switch
        {
            Difficulty.Easy => 90,
            Difficulty.Medium => 75,
            Difficulty.Hard => 60,
            Difficulty.Extreme => 45,
            _ => 75
        };
    }
}
