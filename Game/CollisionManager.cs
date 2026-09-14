using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Models;

namespace ModernSnakeGame.Game;

/// <summary>
/// Manages collision detection between game objects.
/// </summary>
public class CollisionManager
{
    private readonly int _cellSize;

    public CollisionManager(int cellSize)
    {
        _cellSize = cellSize;
    }

    /// <summary>
    /// Checks if the snake collides with walls.
    /// </summary>
    public bool CheckWallCollision(Point headPosition, Rectangle gameBoardBounds)
    {
        return headPosition.X < gameBoardBounds.Left ||
               headPosition.X >= gameBoardBounds.Right ||
               headPosition.Y < gameBoardBounds.Top ||
               headPosition.Y >= gameBoardBounds.Bottom;
    }

    /// <summary>
    /// Checks if the snake collides with itself.
    /// </summary>
    public bool CheckSelfCollision(Point headPosition, Snake snake)
    {
        // Skip the first few segments to avoid false positives during turns
        var bodySegments = snake.Body.Skip(1).ToList();
        return bodySegments.Contains(headPosition);
    }

    /// <summary>
    /// Checks if the snake collides with any obstacle.
    /// </summary>
    public bool CheckObstacleCollision(Point headPosition, List<Obstacle> obstacles)
    {
        foreach (var obstacle in obstacles)
        {
            if (obstacle.CheckCollision(headPosition))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the snake collides with any food.
    /// </summary>
    public Food? CheckFoodCollision(Point headPosition, List<Food> foods)
    {
        foreach (var food in foods)
        {
            if (food.IsActive && food.CheckCollision(headPosition))
            {
                return food;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks if the snake collides with any power-up.
    /// </summary>
    public PowerUp? CheckPowerUpCollision(Point headPosition, List<PowerUp> powerUps)
    {
        foreach (var powerUp in powerUps)
        {
            if (powerUp.IsActive && powerUp.CheckCollision(headPosition))
            {
                return powerUp;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets a random valid position for spawning objects.
    /// </summary>
    public Point GetRandomValidPosition(Rectangle bounds, Snake snake, List<Obstacle> obstacles)
    {
        Random random = new Random();
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            int x = (random.Next(bounds.Width / _cellSize) * _cellSize) + bounds.Left;
            int y = (random.Next(bounds.Height / _cellSize) * _cellSize) + bounds.Top;
            Point position = new Point(x, y);

            // Check if position is valid
            if (!snake.CheckCollision(position) &&
                !obstacles.Any(o => o.CheckCollision(position)))
            {
                return position;
            }

            attempts++;
        }

        // Fallback: return center of board
        return new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
    }
}
