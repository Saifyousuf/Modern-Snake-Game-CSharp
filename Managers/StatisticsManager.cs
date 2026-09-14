using ModernSnakeGame.Models;

namespace ModernSnakeGame.Managers;

/// <summary>
/// Manages game statistics.
/// </summary>
public class StatisticsManager
{
    private readonly SaveManager _saveManager;
    private GameStatistics _statistics;

    public GameStatistics Statistics => _statistics;

    public StatisticsManager(SaveManager saveManager)
    {
        _saveManager = saveManager;
        _statistics = _saveManager.LoadStatistics();
    }

    public void RecordGameEnd(int score, int level, TimeSpan survivalTime, int foodCollected, bool won)
    {
        _statistics.TotalGamesPlayed++;
        
        if (score > _statistics.HighestScore)
        {
            _statistics.HighestScore = score;
        }

        if (level > _statistics.HighestLevel)
        {
            _statistics.HighestLevel = level;
        }

        _statistics.TotalFoodCollected += foodCollected;

        if (survivalTime > _statistics.BestSurvivalTime)
        {
            _statistics.BestSurvivalTime = survivalTime;
        }

        _statistics.TotalPlayTime += survivalTime;

        if (won)
        {
            _statistics.GamesWon++;
        }
        else
        {
            _statistics.GamesLost++;
        }

        Save();
    }

    public void Save()
    {
        _saveManager.SaveStatistics(_statistics);
    }

    public void Reset()
    {
        _statistics = new GameStatistics();
        Save();
    }
}
