using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Models;

/// <summary>
/// Abstract base class for all power-ups.
/// Implements ICollidable for collision detection.
/// </summary>
public abstract class PowerUp : ICollidable
{
    protected Point _position;
    protected int _cellSize;
    protected PowerUpType _type;
    protected int _durationSeconds;
    protected Color _color;
    protected bool _isActive;
    protected DateTime _activationTime;

    public Point Position => _position;
    public PowerUpType Type => _type;
    public int DurationSeconds => _durationSeconds;
    public Color Color => _color;
    public bool IsActive => _isActive;

    public PowerUp(Point position, int cellSize, int durationSeconds)
    {
        _position = position;
        _cellSize = cellSize;
        _durationSeconds = durationSeconds;
        _isActive = true;
        _activationTime = DateTime.MinValue;
    }

    public abstract void Activate(GameManager gameManager);
    public abstract void Deactivate(GameManager gameManager);

    public bool CheckCollision(Point position)
    {
        return _position == position && _isActive;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(_position.X, _position.Y, _cellSize, _cellSize);
    }

    public virtual void Draw(Graphics g)
    {
        if (!_isActive) return;

        using var brush = new SolidBrush(_color);
        
        // Draw a star shape for power-ups
        int centerX = _position.X + _cellSize / 2;
        int centerY = _position.Y + _cellSize / 2;
        int outerRadius = _cellSize / 2 - 2;
        int innerRadius = outerRadius / 2;

        var points = new List<Point>();
        for (int i = 0; i < 10; i++)
        {
            double angle = i * Math.PI / 5 - Math.PI / 2;
            int radius = (i % 2 == 0) ? outerRadius : innerRadius;
            int x = centerX + (int)(radius * Math.Cos(angle));
            int y = centerY + (int)(radius * Math.Sin(angle));
            points.Add(new Point(x, y));
        }

        g.FillPolygon(brush, points.ToArray());
    }

    public bool IsExpired()
    {
        return _activationTime != DateTime.MinValue && 
               DateTime.Now > _activationTime.AddSeconds(_durationSeconds);
    }

    public void ActivatePowerUp()
    {
        _activationTime = DateTime.Now;
        _isActive = false; // Hide from board after collection
    }
}

/// <summary>
/// Shield power-up - protects from one collision.
/// </summary>
public class ShieldPowerUp : PowerUp
{
    public ShieldPowerUp(Point position, int cellSize) : base(position, cellSize, 10)
    {
        _type = PowerUpType.Shield;
        _color = Color.Blue;
    }

    public override void Activate(GameManager gameManager)
    {
        gameManager.ActivateShield(DurationSeconds);
        ActivatePowerUp();
    }

    public override void Deactivate(GameManager gameManager)
    {
        gameManager.DeactivateShield();
    }
}

/// <summary>
/// Slow motion power-up - slows down the snake temporarily.
/// </summary>
public class SlowMotionPowerUp : PowerUp
{
    public SlowMotionPowerUp(Point position, int cellSize) : base(position, cellSize, 8)
    {
        _type = PowerUpType.SlowMotion;
        _color = Color.LightBlue;
    }

    public override void Activate(GameManager gameManager)
    {
        gameManager.ActivateSlowMotion(DurationSeconds);
        ActivatePowerUp();
    }

    public override void Deactivate(GameManager gameManager)
    {
        gameManager.DeactivateSlowMotion();
    }
}

/// <summary>
/// Speed boost power-up - increases snake speed temporarily.
/// </summary>
public class SpeedBoostPowerUp : PowerUp
{
    public SpeedBoostPowerUp(Point position, int cellSize) : base(position, cellSize, 8)
    {
        _type = PowerUpType.SpeedBoost;
        _color = Color.Orange;
    }

    public override void Activate(GameManager gameManager)
    {
        gameManager.ActivateSpeedBoost(DurationSeconds);
        ActivatePowerUp();
    }

    public override void Deactivate(GameManager gameManager)
    {
        gameManager.DeactivateSpeedBoost();
    }
}

/// <summary>
/// Shrink power-up - reduces snake length.
/// </summary>
public class ShrinkPowerUp : PowerUp
{
    public ShrinkPowerUp(Point position, int cellSize) : base(position, cellSize, 0)
    {
        _type = PowerUpType.Shrink;
        _color = Color.Purple;
    }

    public override void Activate(GameManager gameManager)
    {
        gameManager.ShrinkSnake(3);
        ActivatePowerUp();
    }

    public override void Deactivate(GameManager gameManager)
    {
        // No deactivation needed for instant effect
    }
}

/// <summary>
/// Magnet power-up - attracts nearby food.
/// </summary>
public class MagnetPowerUp : PowerUp
{
    public MagnetPowerUp(Point position, int cellSize) : base(position, cellSize, 10)
    {
        _type = PowerUpType.Magnet;
        _color = Color.Magenta;
    }

    public override void Activate(GameManager gameManager)
    {
        gameManager.ActivateMagnet(DurationSeconds);
        ActivatePowerUp();
    }

    public override void Deactivate(GameManager gameManager)
    {
        gameManager.DeactivateMagnet();
    }
}
