using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Models;
using Newtonsoft.Json;

namespace ModernSnakeGame.Managers;

/// <summary>
/// Manages saving and loading game data using JSON.
/// </summary>
public class SaveManager
{
    private readonly string _saveDirectory;
    private readonly string _settingsPath;
    private readonly string _leaderboardPath;
    private readonly string _statisticsPath;
    private readonly string _playerPath;

    public SaveManager()
    {
        _saveDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ModernSnakeGame");

        if (!Directory.Exists(_saveDirectory))
        {
            Directory.CreateDirectory(_saveDirectory);
        }

        _settingsPath = Path.Combine(_saveDirectory, "settings.json");
        _leaderboardPath = Path.Combine(_saveDirectory, "leaderboard.json");
        _statisticsPath = Path.Combine(_saveDirectory, "statistics.json");
        _playerPath = Path.Combine(_saveDirectory, "player.json");
    }

    #region Settings

    public void SaveSettings(GameSettings settings)
    {
        try
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_settingsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    public GameSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                var settings = JsonConvert.DeserializeObject<GameSettings>(json);
                return settings ?? new GameSettings();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }
        return new GameSettings();
    }

    #endregion

    #region Leaderboard

    public void SaveLeaderboard(List<ScoreEntry> scores)
    {
        try
        {
            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(_leaderboardPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving leaderboard: {ex.Message}");
        }
    }

    public List<ScoreEntry> LoadLeaderboard()
    {
        try
        {
            if (File.Exists(_leaderboardPath))
            {
                string json = File.ReadAllText(_leaderboardPath);
                var scores = JsonConvert.DeserializeObject<List<ScoreEntry>>(json);
                return scores ?? new List<ScoreEntry>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading leaderboard: {ex.Message}");
        }
        return new List<ScoreEntry>();
    }

    #endregion

    #region Statistics

    public void SaveStatistics(GameStatistics stats)
    {
        try
        {
            string json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            File.WriteAllText(_statisticsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving statistics: {ex.Message}");
        }
    }

    public GameStatistics LoadStatistics()
    {
        try
        {
            if (File.Exists(_statisticsPath))
            {
                string json = File.ReadAllText(_statisticsPath);
                var stats = JsonConvert.DeserializeObject<GameStatistics>(json);
                return stats ?? new GameStatistics();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading statistics: {ex.Message}");
        }
        return new GameStatistics();
    }

    #endregion

    #region Player

    public void SavePlayer(Player player)
    {
        try
        {
            string json = player.ToJson();
            File.WriteAllText(_playerPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving player: {ex.Message}");
        }
    }

    public Player? LoadPlayer()
    {
        try
        {
            if (File.Exists(_playerPath))
            {
                string json = File.ReadAllText(_playerPath);
                var player = JsonConvert.DeserializeObject<Player>(json);
                return player;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading player: {ex.Message}");
        }
        return null;
    }

    #endregion
}

/// <summary>
/// Game settings model.
/// </summary>
public class GameSettings
{
    public Theme Theme { get; set; } = Theme.Dark;
    public SnakeSkin SnakeSkin { get; set; } = SnakeSkin.ClassicGreen;
    public bool SoundEnabled { get; set; } = true;
    public bool MusicEnabled { get; set; } = true;
    public Difficulty Difficulty { get; set; } = Difficulty.Medium;
    public GameMode GameMode { get; set; } = GameMode.Classic;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static GameSettings FromJson(string json)
    {
        return JsonConvert.DeserializeObject<GameSettings>(json) ?? new GameSettings();
    }
}

/// <summary>
/// Score entry for leaderboard.
/// </summary>
public class ScoreEntry
{
    public string PlayerName { get; set; } = string.Empty;
    public int Score { get; set; }
    public GameMode GameMode { get; set; }
    public Difficulty Difficulty { get; set; }
    public int Level { get; set; }
    public DateTime Date { get; set; }

    public ScoreEntry()
    {
        Date = DateTime.Now;
    }

    public ScoreEntry(string playerName, int score, GameMode mode, Difficulty difficulty, int level)
    {
        PlayerName = playerName;
        Score = score;
        GameMode = mode;
        Difficulty = difficulty;
        Level = level;
        Date = DateTime.Now;
    }
}

/// <summary>
/// Game statistics model.
/// </summary>
public class GameStatistics
{
    public int TotalGamesPlayed { get; set; }
    public int HighestScore { get; set; }
    public int HighestLevel { get; set; }
    public int TotalFoodCollected { get; set; }
    public TimeSpan BestSurvivalTime { get; set; }
    public TimeSpan TotalPlayTime { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static GameStatistics FromJson(string json)
    {
        return JsonConvert.DeserializeObject<GameStatistics>(json) ?? new GameStatistics();
    }
}
