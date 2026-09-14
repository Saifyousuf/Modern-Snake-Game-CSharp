using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Models;

/// <summary>
/// Represents a player with their statistics and progress.
/// Implements ISaveable for JSON serialization.
/// </summary>
public class Player : ISaveable
{
    public string Name { get; set; } = string.Empty;
    public int GamesPlayed { get; set; }
    public int HighestScore { get; set; }
    public int HighestLevel { get; set; }
    public int TotalFoodCollected { get; set; }
    public TimeSpan BestSurvivalTime { get; set; }
    public TimeSpan TotalPlayTime { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }

    public Player() { }

    public Player(string name)
    {
        Name = name;
    }

    public string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }

    public void FromJson(string json)
    {
        var player = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(json);
        if (player != null)
        {
            Name = player.Name;
            GamesPlayed = player.GamesPlayed;
            HighestScore = player.HighestScore;
            HighestLevel = player.HighestLevel;
            TotalFoodCollected = player.TotalFoodCollected;
            BestSurvivalTime = player.BestSurvivalTime;
            TotalPlayTime = player.TotalPlayTime;
            GamesWon = player.GamesWon;
            GamesLost = player.GamesLost;
        }
    }
}
