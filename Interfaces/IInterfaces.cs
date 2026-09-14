namespace ModernSnakeGame.Interfaces;

/// <summary>
/// Interface for objects that can move on the game board.
/// </summary>
public interface IMovable
{
    void Move();
    void ChangeDirection(Direction newDirection);
}

/// <summary>
/// Interface for objects that can collide with other objects.
/// </summary>
public interface ICollidable
{
    bool CheckCollision(Point position);
    Rectangle GetBounds();
}

/// <summary>
/// Interface for objects that can be saved and loaded.
/// </summary>
public interface ISaveable
{
    string ToJson();
    void FromJson(string json);
}

/// <summary>
/// Enumeration for movement directions.
/// </summary>
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

/// <summary>
/// Enumeration for game modes.
/// </summary>
public enum GameMode
{
    Classic,
    TimeAttack,
    Obstacle,
    Endless
}

/// <summary>
/// Enumeration for difficulty levels.
/// </summary>
public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Extreme
}

/// <summary>
/// Enumeration for food types.
/// </summary>
public enum FoodType
{
    Normal,
    Bonus,
    Rare,
    Speed
}

/// <summary>
/// Enumeration for power-up types.
/// </summary>
public enum PowerUpType
{
    Shield,
    SlowMotion,
    SpeedBoost,
    Shrink,
    Magnet
}

/// <summary>
/// Enumeration for game themes.
/// </summary>
public enum Theme
{
    Dark,
    Light,
    Neon
}

/// <summary>
/// Enumeration for snake skins.
/// </summary>
public enum SnakeSkin
{
    ClassicGreen,
    Blue,
    Red,
    Neon,
    Rainbow
}
