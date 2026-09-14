using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Models;

/// <summary>
/// Represents the snake in the game.
/// Implements IMovable for movement and ICollidable for collision detection.
/// </summary>
public class Snake : IMovable, ICollidable
{
    private Queue<Point> _body;
    private Direction _currentDirection;
    private Direction _nextDirection;
    private int _cellSize;
    private bool _isGrowing;
    private Color _headColor;
    private Color _bodyColor;

    public Point Head => _body.Count > 0 ? _body.Peek() : new Point(0, 0);
    public int Length => _body.Count;
    public IEnumerable<Point> Body => _body;

    public Snake(int startX, int startY, int cellSize)
    {
        _body = new Queue<Point>();
        _cellSize = cellSize;
        _currentDirection = Direction.Right;
        _nextDirection = Direction.Right;
        _isGrowing = false;

        // Initialize with 3 segments
        for (int i = 0; i < 3; i++)
        {
            _body.Enqueue(new Point(startX - i * cellSize, startY));
        }

        SetSkin(SnakeSkin.ClassicGreen);
    }

    public void SetSkin(SnakeSkin skin)
    {
        switch (skin)
        {
            case SnakeSkin.Blue:
                _headColor = Color.DarkBlue;
                _bodyColor = Color.Blue;
                break;
            case SnakeSkin.Red:
                _headColor = Color.DarkRed;
                _bodyColor = Color.Red;
                break;
            case SnakeSkin.Neon:
                _headColor = Color.Cyan;
                _bodyColor = Color.Magenta;
                break;
            case SnakeSkin.Rainbow:
                _headColor = Color.Gold;
                _bodyColor = Color.Purple;
                break;
            default: // ClassicGreen
                _headColor = Color.DarkGreen;
                _bodyColor = Color.Green;
                break;
        }
    }

    public void Move()
    {
        _currentDirection = _nextDirection;

        Point newHead = _currentDirection switch
        {
            Direction.Up => new Point(Head.X, Head.Y - _cellSize),
            Direction.Down => new Point(Head.X, Head.Y + _cellSize),
            Direction.Left => new Point(Head.X - _cellSize, Head.Y),
            Direction.Right => new Point(Head.X + _cellSize, Head.Y),
            _ => Head
        };

        _body.Enqueue(newHead);

        if (!_isGrowing)
        {
            _body.Dequeue();
        }
        else
        {
            _isGrowing = false;
        }
    }

    public void ChangeDirection(Direction newDirection)
    {
        // Prevent 180-degree turns
        bool isOpposite = (newDirection == Direction.Up && _currentDirection == Direction.Down) ||
                          (newDirection == Direction.Down && _currentDirection == Direction.Up) ||
                          (newDirection == Direction.Left && _currentDirection == Direction.Right) ||
                          (newDirection == Direction.Right && _currentDirection == Direction.Left);

        if (!isOpposite)
        {
            _nextDirection = newDirection;
        }
    }

    public void Grow()
    {
        _isGrowing = true;
    }

    public void Shrink(int amount)
    {
        for (int i = 0; i < amount && _body.Count > 3; i++)
        {
            _body.Dequeue();
        }
    }

    public bool CheckCollision(Point position)
    {
        return _body.Contains(position);
    }

    public Rectangle GetBounds()
    {
        if (_body.Count == 0) return Rectangle.Empty;

        int minX = _body.Min(p => p.X);
        int minY = _body.Min(p => p.Y);
        int maxX = _body.Max(p => p.X);
        int maxY = _body.Max(p => p.Y);

        return new Rectangle(minX, minY, maxX - minX + _cellSize, maxY - minY + _cellSize);
    }

    public void Reset(int startX, int startY)
    {
        _body.Clear();
        _currentDirection = Direction.Right;
        _nextDirection = Direction.Right;
        _isGrowing = false;

        for (int i = 0; i < 3; i++)
        {
            _body.Enqueue(new Point(startX - i * _cellSize, startY));
        }
    }

    public Color GetHeadColor() => _headColor;
    public Color GetBodyColor() => _bodyColor;
}
