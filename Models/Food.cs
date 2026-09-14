using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Models;

/// <summary>
/// Abstract base class for all food types.
/// Implements ICollidable for collision detection.
/// </summary>
public abstract class Food : ICollidable
{
    protected Point _position;
    protected int _cellSize;
    protected FoodType _type;
    protected int _points;
    protected Color _color;
    protected bool _isActive;

    public Point Position => _position;
    public int Points => _points;
    public FoodType Type => _type;
    public Color Color => _color;
    public bool IsActive => _isActive;

    public Food(Point position, int cellSize)
    {
        _position = position;
        _cellSize = cellSize;
        _isActive = true;
    }

    public abstract void Consume();

    public bool CheckCollision(Point position)
    {
        return _position == position && _isActive;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(_position.X, _position.Y, _cellSize, _cellSize);
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public virtual void Draw(Graphics g)
    {
        if (!_isActive) return;

        using var brush = new SolidBrush(_color);
        g.FillEllipse(brush, _position.X + 2, _position.Y + 2, _cellSize - 4, _cellSize - 4);
    }
}

/// <summary>
/// Normal food - gives 10 points and grows the snake.
/// </summary>
public class NormalFood : Food
{
    public NormalFood(Point position, int cellSize) : base(position, cellSize)
    {
        _type = FoodType.Normal;
        _points = 10;
        _color = Color.Red;
    }

    public override void Consume()
    {
        _isActive = false;
    }
}

/// <summary>
/// Bonus food - gives 25 points and grows the snake.
/// </summary>
public class BonusFood : Food
{
    public BonusFood(Point position, int cellSize) : base(position, cellSize)
    {
        _type = FoodType.Bonus;
        _points = 25;
        _color = Color.Gold;
    }

    public override void Consume()
    {
        _isActive = false;
    }
}

/// <summary>
/// Rare food - gives 50 points, appears for limited time.
/// </summary>
public class RareFood : Food
{
    private DateTime _expirationTime;

    public RareFood(Point position, int cellSize, int durationSeconds) : base(position, cellSize)
    {
        _type = FoodType.Rare;
        _points = 50;
        _color = Color.Cyan;
        _expirationTime = DateTime.Now.AddSeconds(durationSeconds);
    }

    public override void Consume()
    {
        _isActive = false;
    }

    public bool IsExpired()
    {
        return DateTime.Now > _expirationTime;
    }

    public override void Draw(Graphics g)
    {
        if (!_isActive) return;

        // Diamond shape for rare food
        using var brush = new SolidBrush(_color);
        int centerX = _position.X + _cellSize / 2;
        int centerY = _position.Y + _cellSize / 2;
        int size = _cellSize / 2 - 2;

        var diamondPoints = new[]
        {
            new Point(centerX, centerY - size),
            new Point(centerX + size, centerY),
            new Point(centerX, centerY + size),
            new Point(centerX - size, centerY)
        };

        g.FillPolygon(brush, diamondPoints);
    }
}

/// <summary>
/// Speed food - temporarily increases snake speed.
/// </summary>
public class SpeedFood : Food
{
    public SpeedFood(Point position, int cellSize) : base(position, cellSize)
    {
        _type = FoodType.Speed;
        _points = 15;
        _color = Color.Yellow;
    }

    public override void Consume()
    {
        _isActive = false;
    }
}
