using ModernSnakeGame.Interfaces;

namespace ModernSnakeGame.Models;

/// <summary>
/// Represents an obstacle on the game board.
/// Implements ICollidable for collision detection.
/// </summary>
public class Obstacle : ICollidable
{
    private Rectangle _bounds;
    private Color _color;

    public Rectangle Bounds => _bounds;
    public Color Color => _color;

    public Obstacle(int x, int y, int width, int height)
    {
        _bounds = new Rectangle(x, y, width, height);
        _color = Color.Gray;
    }

    public bool CheckCollision(Point position)
    {
        return _bounds.Contains(position);
    }

    public Rectangle GetBounds()
    {
        return _bounds;
    }

    public void Draw(Graphics g)
    {
        using var brush = new SolidBrush(_color);
        g.FillRectangle(brush, _bounds);

        // Add border
        using var pen = new Pen(Color.DarkGray, 2);
        g.DrawRectangle(pen, _bounds);
    }
}
