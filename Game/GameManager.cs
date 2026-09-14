using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Models;

namespace ModernSnakeGame.Game;

/// <summary>
/// Main game manager that orchestrates all game logic.
/// </summary>
public class GameManager
{
    // Game state
    private bool _isRunning;
    private bool _isPaused;
    private bool _isGameOver;
    private DateTime _gameStartTime;
    private TimeSpan _survivalTime;

    // Game objects
    private Snake _snake;
    private List<Food> _foods;
    private List<Obstacle> _obstacles;
    private List<PowerUp> _powerUps;

    // Managers
    private ScoreManager _scoreManager;
    private LevelManager _levelManager;
    private DifficultyManager _difficultyManager;
    private CollisionManager _collisionManager;

    // Power-up states
    private bool _hasShield;
    private bool _isSlowMotion;
    private bool _isSpeedBoost;
    private bool _isMagnet;
    private DateTime? _shieldEndTime;
    private DateTime? _slowMotionEndTime;
    private DateTime? _speedBoostEndTime;
    private DateTime? _magnetEndTime;

    // Configuration
    private readonly int _cellSize;
    private readonly Rectangle _gameBoardBounds;
    private readonly GameMode _gameMode;
    private readonly int _timeAttackDuration;
    private readonly Random _random;

    // Events
    public event Action? OnGameStateChanged;
    public event Action? OnPauseStateChanged;
    public event Action? OnGameOver;
    public event Action<Point>? OnFoodEaten;
    public event Action<Point>? OnPowerUpCollected;
    public event Action<string>? OnMessage;

    // Properties
    public bool IsRunning => _isRunning;
    public bool IsPaused => _isPaused;
    public bool IsGameOver => _isGameOver;
    public Snake Snake => _snake;
    public List<Food> Foods => _foods;
    public List<Obstacle> Obstacles => _obstacles;
    public List<PowerUp> PowerUps => _powerUps;
    public ScoreManager ScoreManager => _scoreManager;
    public LevelManager LevelManager => _levelManager;
    public TimeSpan SurvivalTime => _survivalTime;
    public bool HasShield => _hasShield;
    public bool IsMagnetActive => _isMagnet;

    public GameManager(
        Snake snake,
        Rectangle gameBoardBounds,
        int cellSize,
        GameMode gameMode,
        Difficulty difficulty)
    {
        _snake = snake;
        _gameBoardBounds = gameBoardBounds;
        _cellSize = cellSize;
        _gameMode = gameMode;

        _foods = new List<Food>();
        _obstacles = new List<Obstacle>();
        _powerUps = new List<PowerUp>();

        _scoreManager = new ScoreManager();
        _difficultyManager = new DifficultyManager(difficulty);
        _levelManager = new LevelManager(difficulty);
        _collisionManager = new CollisionManager(cellSize);
        _random = new Random();

        _timeAttackDuration = _difficultyManager.GetTimeAttackDuration();
        _isRunning = false;
        _isPaused = false;
        _isGameOver = false;

        InitializeObstacles();
    }

    public void Start()
    {
        _isRunning = true;
        _isPaused = false;
        _isGameOver = false;
        _gameStartTime = DateTime.Now;
        _survivalTime = TimeSpan.Zero;

        SpawnFood();
        UpdatePowerUps();

        OnGameStateChanged?.Invoke();
    }

    public void Pause()
    {
        if (_isRunning && !_isGameOver)
        {
            _isPaused = !_isPaused;
            OnPauseStateChanged?.Invoke();
        }
    }

    public void Resume()
    {
        if (_isPaused)
        {
            _isPaused = false;
            OnPauseStateChanged?.Invoke();
        }
    }

    public void Restart()
    {
        _snake.Reset(_gameBoardBounds.Left + _gameBoardBounds.Width / 2, 
                     _gameBoardBounds.Top + _gameBoardBounds.Height / 2);
        _foods.Clear();
        _obstacles.Clear();
        _powerUps.Clear();
        _scoreManager.Reset();
        _levelManager.Reset();
        
        ResetPowerUps();
        InitializeObstacles();
        
        _isPaused = false;
        _isGameOver = false;
        _gameStartTime = DateTime.Now;
        _survivalTime = TimeSpan.Zero;

        SpawnFood();
        OnGameStateChanged?.Invoke();
    }

    public void GameOver()
    {
        _isRunning = false;
        _isGameOver = true;
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Main update loop - called every game tick.
    /// </summary>
    public void Update()
    {
        if (!_isRunning || _isPaused || _isGameOver)
        {
            return;
        }

        // Update survival time
        _survivalTime = DateTime.Now - _gameStartTime;

        // Check time attack mode
        if (_gameMode == GameMode.TimeAttack)
        {
            if (_survivalTime.TotalSeconds >= _timeAttackDuration)
            {
                GameOver();
                return;
            }
        }

        // Apply magnet effect before moving
        ApplyMagnetEffect();

        // Move snake
        _snake.Move();

        // Check collisions
        CheckCollisions();

        // Update power-ups
        UpdateActivePowerUps();

        // Check level up
        _levelManager.CheckLevelUp(_scoreManager.CurrentScore);

        // Expire rare food
        ExpireRareFood();

        // Random power-up spawn
        TrySpawnPowerUp();

        OnGameStateChanged?.Invoke();
    }

    private void CheckCollisions()
    {
        Point headPosition = _snake.Head;

        // Wall collision
        if (_collisionManager.CheckWallCollision(headPosition, _gameBoardBounds))
        {
            if (_hasShield)
            {
                DeactivateShield();
                // Move snake back inside bounds and reverse direction safely
                MoveSnakeInsideBounds(headPosition);
            }
            else
            {
                GameOver();
                return;
            }
        }

        // Self collision
        if (_collisionManager.CheckSelfCollision(headPosition, _snake))
        {
            if (_hasShield)
            {
                DeactivateShield();
            }
            else
            {
                GameOver();
                return;
            }
        }

        // Obstacle collision
        if (_collisionManager.CheckObstacleCollision(headPosition, _obstacles))
        {
            if (_hasShield)
            {
                DeactivateShield();
            }
            else
            {
                GameOver();
                return;
            }
        }

        // Food collision
        Food? collidedFood = _collisionManager.CheckFoodCollision(headPosition, _foods);
        if (collidedFood != null)
        {
            ConsumeFood(collidedFood);
        }

        // Power-up collision
        PowerUp? collidedPowerUp = _collisionManager.CheckPowerUpCollision(headPosition, _powerUps);
        if (collidedPowerUp != null)
        {
            collidedPowerUp.Activate(this);
            OnPowerUpCollected?.Invoke(collidedPowerUp.Position);
        }
    }

    /// <summary>
    /// Moves the snake back inside bounds when shield protects from wall collision.
    /// </summary>
    private void MoveSnakeInsideBounds(Point headPosition)
    {
        // Calculate safe position based on which wall was hit
        int newX = headPosition.X;
        int newY = headPosition.Y;
        
        if (headPosition.X < _gameBoardBounds.Left)
        {
            newX = _gameBoardBounds.Left + _cellSize;
            _snake.ChangeDirection(Direction.Right);
        }
        else if (headPosition.X >= _gameBoardBounds.Right)
        {
            newX = _gameBoardBounds.Right - _cellSize * 2;
            _snake.ChangeDirection(Direction.Left);
        }
        
        if (headPosition.Y < _gameBoardBounds.Top)
        {
            newY = _gameBoardBounds.Top + _cellSize;
            _snake.ChangeDirection(Direction.Down);
        }
        else if (headPosition.Y >= _gameBoardBounds.Bottom)
        {
            newY = _gameBoardBounds.Bottom - _cellSize * 2;
            _snake.ChangeDirection(Direction.Up);
        }
        
        // Update snake head position to safe location
        // This is done by resetting the snake at the new position
        var bodyWithoutHead = _snake.Body.Skip(1).ToList();
        var newHead = new Point(newX, newY);
        
        // Rebuild snake body with new head
        _snake.Reset(newX, newY);
    }

    private Direction GetCurrentDirection()
    {
        // Get the actual current direction from the snake
        return _snake.Body.Skip(1).FirstOrDefault() != _snake.Head 
            ? GetCurrentDirectionFromMovement() 
            : Direction.Right;
    }

    private Direction GetCurrentDirectionFromMovement()
    {
        // Determine direction based on head movement relative to second segment
        if (_snake.Body.Count() < 2) return Direction.Right;
        
        var head = _snake.Head;
        var secondSegment = _snake.Body.Skip(1).First();
        
        if (head.Y < secondSegment.Y) return Direction.Up;
        if (head.Y > secondSegment.Y) return Direction.Down;
        if (head.X < secondSegment.X) return Direction.Left;
        if (head.X > secondSegment.X) return Direction.Right;
        
        return Direction.Right;
    }

    private Direction GetOppositeDirection(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Direction.Right
        };
    }

    private void ConsumeFood(Food food)
    {
        food.Consume();
        _scoreManager.AddPoints(food.Points);
        _scoreManager.IncrementFoodCollected();

        if (food.Type == FoodType.Speed)
        {
            ActivateSpeedBoost(5);
        }
        else
        {
            _snake.Grow();
        }

        OnFoodEaten?.Invoke(food.Position);

        // Remove consumed food and spawn new one
        _foods.Remove(food);
        SpawnFood();

        // Chance to spawn bonus food
        if (_random.NextDouble() < _difficultyManager.GetBonusFoodChance())
        {
            SpawnBonusFood();
        }

        // Chance to spawn rare food
        if (_random.NextDouble() < _difficultyManager.GetRareFoodChance())
        {
            SpawnRareFood();
        }
    }

    private void SpawnFood()
    {
        Point position = _collisionManager.GetRandomValidPosition(_gameBoardBounds, _snake, _obstacles);
        _foods.Add(new NormalFood(position, _cellSize));
    }

    private void SpawnBonusFood()
    {
        Point position = _collisionManager.GetRandomValidPosition(_gameBoardBounds, _snake, _obstacles);
        _foods.Add(new BonusFood(position, _cellSize));
    }

    private void SpawnRareFood()
    {
        Point position = _collisionManager.GetRandomValidPosition(_gameBoardBounds, _snake, _obstacles);
        _foods.Add(new RareFood(position, _cellSize, 10)); // 10 seconds duration
    }

    private void ExpireRareFood()
    {
        var expiredFood = _foods.OfType<RareFood>().Where(f => f.IsExpired()).ToList();
        foreach (var food in expiredFood)
        {
            _foods.Remove(food);
        }
    }

    private void InitializeObstacles()
    {
        _obstacles.Clear();
        int obstacleCount = _levelManager.GetObstacleCount();

        for (int i = 0; i < obstacleCount; i++)
        {
            Point position = _collisionManager.GetRandomValidPosition(_gameBoardBounds, _snake, _obstacles);
            int size = _cellSize * (_random.Next(1, 3));
            _obstacles.Add(new Obstacle(position.X, position.Y, size, size));
        }
    }

    private void UpdatePowerUps()
    {
        _powerUps.Clear();
    }

    private void TrySpawnPowerUp()
    {
        if (_random.NextDouble() < _difficultyManager.GetPowerUpChance() * 0.01)
        {
            Point position = _collisionManager.GetRandomValidPosition(_gameBoardBounds, _snake, _obstacles);
            
            PowerUp powerUp = _random.Next(5) switch
            {
                0 => new ShieldPowerUp(position, _cellSize),
                1 => new SlowMotionPowerUp(position, _cellSize),
                2 => new SpeedBoostPowerUp(position, _cellSize),
                3 => new ShrinkPowerUp(position, _cellSize),
                _ => new MagnetPowerUp(position, _cellSize)
            };

            _powerUps.Add(powerUp);
        }
    }

    private void UpdateActivePowerUps()
    {
        DateTime now = DateTime.Now;

        if (_hasShield && _shieldEndTime.HasValue && now > _shieldEndTime.Value)
        {
            DeactivateShield();
        }

        if (_isSlowMotion && _slowMotionEndTime.HasValue && now > _slowMotionEndTime.Value)
        {
            DeactivateSlowMotion();
        }

        if (_isSpeedBoost && _speedBoostEndTime.HasValue && now > _speedBoostEndTime.Value)
        {
            DeactivateSpeedBoost();
        }

        if (_isMagnet && _magnetEndTime.HasValue && now > _magnetEndTime.Value)
        {
            DeactivateMagnet();
        }
    }

    private void ResetPowerUps()
    {
        _hasShield = false;
        _isSlowMotion = false;
        _isSpeedBoost = false;
        _isMagnet = false;
        _shieldEndTime = null;
        _slowMotionEndTime = null;
        _speedBoostEndTime = null;
        _magnetEndTime = null;
    }

    // Power-up activation methods
    public void ActivateShield(int durationSeconds)
    {
        _hasShield = true;
        _shieldEndTime = DateTime.Now.AddSeconds(durationSeconds);
        OnMessage?.Invoke("Shield Activated!");
    }

    public void DeactivateShield()
    {
        _hasShield = false;
        _shieldEndTime = null;
    }

    public void ActivateSlowMotion(int durationSeconds)
    {
        _isSlowMotion = true;
        _slowMotionEndTime = DateTime.Now.AddSeconds(durationSeconds);
        OnMessage?.Invoke("Slow Motion!");
    }

    public void DeactivateSlowMotion()
    {
        _isSlowMotion = false;
        _slowMotionEndTime = null;
    }

    public void ActivateSpeedBoost(int durationSeconds)
    {
        _isSpeedBoost = true;
        _speedBoostEndTime = DateTime.Now.AddSeconds(durationSeconds);
        OnMessage?.Invoke("Speed Boost!");
    }

    public void DeactivateSpeedBoost()
    {
        _isSpeedBoost = false;
        _speedBoostEndTime = null;
    }

    public void ShrinkSnake(int amount)
    {
        _snake.Shrink(amount);
        OnMessage?.Invoke("Shrunk!");
    }

    public void ActivateMagnet(int durationSeconds)
    {
        _isMagnet = true;
        _magnetEndTime = DateTime.Now.AddSeconds(durationSeconds);
        OnMessage?.Invoke("Magnet!");
    }

    public void DeactivateMagnet()
    {
        _isMagnet = false;
        _magnetEndTime = null;
    }

    public int GetGameTickInterval()
    {
        int baseSpeed = _difficultyManager.GetBaseSpeed();
        float levelMultiplier = _levelManager.GetSpeedMultiplier();

        if (_isSlowMotion)
        {
            baseSpeed *= 2; // Slower
        }

        if (_isSpeedBoost)
        {
            baseSpeed /= 2; // Faster
        }

        return (int)(baseSpeed * levelMultiplier);
    }

    public int GetRemainingTimeSeconds()
    {
        if (_gameMode != GameMode.TimeAttack)
        {
            return 0;
        }

        int elapsed = (int)_survivalTime.TotalSeconds;
        return Math.Max(0, _timeAttackDuration - elapsed);
    }

    public void ChangeDirection(Direction direction)
    {
        _snake.ChangeDirection(direction);
    }

    public void ApplyMagnetEffect()
    {
        if (!_isMagnet) return;

        Point snakeHead = _snake.Head;
        int magnetRange = _cellSize * 5;

        foreach (var food in _foods.Where(f => f.IsActive).ToList())
        {
            int distance = Math.Abs(food.Position.X - snakeHead.X) + Math.Abs(food.Position.Y - snakeHead.Y);
            if (distance < magnetRange && distance > 0)
            {
                // Move food towards snake by one cell
                int moveX = Math.Sign(snakeHead.X - food.Position.X) * _cellSize;
                int moveY = Math.Sign(snakeHead.Y - food.Position.Y) * _cellSize;
                
                // Calculate new position
                int newX = food.Position.X + moveX;
                int newY = food.Position.Y + moveY;
                
                // Ensure new position is within bounds
                newX = Math.Max(_gameBoardBounds.Left, Math.Min(newX, _gameBoardBounds.Right - _cellSize));
                newY = Math.Max(_gameBoardBounds.Top, Math.Min(newY, _gameBoardBounds.Bottom - _cellSize));
                
                // Create new food at the moved position (since Food position is read-only)
                var newFood = CreateFoodAtPosition(new Point(newX, newY), food);
                _foods.Remove(food);
                _foods.Add(newFood);
            }
        }
    }

    private Food CreateFoodAtPosition(Point position, Food originalFood)
    {
        return originalFood switch
        {
            RareFood rare => new RareFood(position, _cellSize, 10),
            SpeedFood speed => new SpeedFood(position, _cellSize),
            BonusFood bonus => new BonusFood(position, _cellSize),
            _ => new NormalFood(position, _cellSize)
        };
    }
}
