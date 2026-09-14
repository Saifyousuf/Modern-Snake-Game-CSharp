using ModernSnakeGame.Managers;

namespace ModernSnakeGame.Managers;

/// <summary>
/// Manages the leaderboard system.
/// </summary>
public class LeaderboardManager
{
    private readonly SaveManager _saveManager;
    private List<ScoreEntry> _scores;
    private const int MaxScores = 10;

    public List<ScoreEntry> TopScores => _scores.OrderByDescending(s => s.Score).Take(MaxScores).ToList();

    public LeaderboardManager(SaveManager saveManager)
    {
        _saveManager = saveManager;
        _scores = _saveManager.LoadLeaderboard();
    }

    public void AddScore(string playerName, int score, GameMode mode, Difficulty difficulty, int level)
    {
        var entry = new ScoreEntry(playerName, score, mode, difficulty, level);
        _scores.Add(entry);
        
        // Sort and keep only top scores
        _scores = _scores.OrderByDescending(s => s.Score).Take(MaxScores * 2).ToList();
        
        Save();
    }

    public bool IsHighScore(int score)
    {
        if (_scores.Count < MaxScores)
        {
            return true;
        }

        return score > _scores.Min(s => s.Score);
    }

    public void Save()
    {
        _saveManager.SaveLeaderboard(_scores);
    }

    public void Clear()
    {
        _scores.Clear();
        Save();
    }
}
