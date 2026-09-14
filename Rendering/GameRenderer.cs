using ModernSnakeGame.Game;
using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Models;

namespace ModernSnakeGame.Rendering;

/// <summary>
/// Handles all rendering operations for the game.
/// </summary>
public class GameRenderer
{
    private readonly int _cellSize;
    private Theme _theme;
    private Color _backgroundColor;
    private Color _gridColor;
    private Color _borderColor;

    public GameRenderer(int cellSize, Theme theme)
    {
        _cellSize = cellSize;
        _theme = theme;
        SetThemeColors(theme);
    }

    private void SetThemeColors(Theme theme)
    {
        switch (theme)
        {
            case Theme.Light:
                _backgroundColor = Color.White;
                _gridColor = Color.LightGray;
                _borderColor = Color.Gray;
                break;
            case Theme.Neon:
                _backgroundColor = Color.Black;
                _gridColor = Color.FromArgb(50, 0, 255, 255);
                _borderColor = Color.Cyan;
                break;
            default: // Dark
                _backgroundColor = Color.FromArgb(30, 30, 30);
                _gridColor = Color.FromArgb(60, 60, 60);
                _borderColor = Color.FromArgb(100, 100, 100);
                break;
        }
    }

    public void UpdateTheme(Theme theme)
    {
        _theme = theme;
        SetThemeColors(theme);
    }

    /// <summary>
    /// Renders the entire game board.
    /// </summary>
    public void Render(Graphics g, Rectangle boardBounds, GameManager gameManager)
    {
        // Clear background
        using var bgBrush = new SolidBrush(_backgroundColor);
        g.FillRectangle(bgBrush, boardBounds);

        // Draw grid
        DrawGrid(g, boardBounds);

        // Draw border
        using var borderPen = new Pen(_borderColor, 3);
        g.DrawRectangle(borderPen, boardBounds);

        // Draw obstacles
        foreach (var obstacle in gameManager.Obstacles)
        {
            obstacle.Draw(g);
        }

        // Draw food
        foreach (var food in gameManager.Foods)
        {
            food.Draw(g);
        }

        // Draw power-ups
        foreach (var powerUp in gameManager.PowerUps)
        {
            powerUp.Draw(g);
        }

        // Draw snake
        DrawSnake(g, gameManager.Snake);

        // Draw shield effect
        if (gameManager.HasShield)
        {
            DrawShieldEffect(g, gameManager.Snake.Head, _cellSize);
        }
    }

    private void DrawGrid(Graphics g, Rectangle bounds)
    {
        using var gridPen = new Pen(_gridColor, 1);

        // Vertical lines
        for (int x = bounds.Left; x <= bounds.Right; x += _cellSize)
        {
            g.DrawLine(gridPen, x, bounds.Top, x, bounds.Bottom);
        }

        // Horizontal lines
        for (int y = bounds.Top; y <= bounds.Bottom; y += _cellSize)
        {
            g.DrawLine(gridPen, bounds.Left, y, bounds.Right, y);
        }
    }

    private void DrawSnake(Graphics g, Snake snake)
    {
        Color headColor = snake.GetHeadColor();
        Color bodyColor = snake.GetBodyColor();

        bool isHead = true;
        foreach (var segment in snake.Body)
        {
            using var brush = new SolidBrush(isHead ? headColor : bodyColor);
            
            // Draw rounded rectangle for segments
            int radius = _cellSize / 4;
            var rect = new Rectangle(segment.X + 1, segment.Y + 1, _cellSize - 2, _cellSize - 2);
            
            g.FillEllipse(brush, rect);

            // Draw eyes on head
            if (isHead)
            {
                DrawSnakeEyes(g, segment, headColor);
            }

            isHead = false;
        }
    }

    private void DrawSnakeEyes(Graphics g, Point position, Color headColor)
    {
        using var eyeBrush = new SolidBrush(Color.White);
        using var pupilBrush = new SolidBrush(Color.Black);

        int eyeSize = _cellSize / 6;
        int pupilSize = eyeSize / 2;

        // Left eye
        int leftEyeX = position.X + _cellSize / 4;
        int leftEyeY = position.Y + _cellSize / 3;
        g.FillEllipse(eyeBrush, leftEyeX, leftEyeY, eyeSize, eyeSize);
        g.FillEllipse(pupilBrush, leftEyeX + 1, leftEyeY + 1, pupilSize, pupilSize);

        // Right eye
        int rightEyeX = position.X + _cellSize * 2 / 4;
        int rightEyeY = position.Y + _cellSize / 3;
        g.FillEllipse(eyeBrush, rightEyeX, leftEyeY, eyeSize, eyeSize);
        g.FillEllipse(pupilBrush, rightEyeX + 1, leftEyeY + 1, pupilSize, pupilSize);
    }

    private void DrawShieldEffect(Graphics g, Point position, int size)
    {
        using var shieldPen = new Pen(Color.FromArgb(150, Color.Blue), 2);
        var rect = new Rectangle(position.X - 2, position.Y - 2, size + 4, size + 4);
        g.DrawEllipse(shieldPen, rect);
    }

    /// <summary>
    /// Draws the pause overlay.
    /// </summary>
    public void DrawPauseOverlay(Graphics g, Rectangle bounds)
    {
        using var overlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
        g.FillRectangle(overlayBrush, bounds);

        using var font = new Font("Arial", 36, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.White);
        
        var textSize = g.MeasureString("GAME PAUSED", font);
        float x = bounds.Left + (bounds.Width - textSize.Width) / 2;
        float y = bounds.Top + (bounds.Height - textSize.Height) / 2;

        g.DrawString("GAME PAUSED", font, textBrush, x, y);
    }

    /// <summary>
    /// Draws the game over screen.
    /// </summary>
    public void DrawGameOver(Graphics g, Rectangle bounds, int score, int level, TimeSpan survivalTime, int foodCollected)
    {
        using var overlayBrush = new SolidBrush(Color.FromArgb(200, 50, 0, 0));
        g.FillRectangle(overlayBrush, bounds);

        using var titleFont = new Font("Arial", 48, FontStyle.Bold);
        using var infoFont = new Font("Arial", 24, FontStyle.Regular);
        using var textBrush = new SolidBrush(Color.White);

        float centerY = bounds.Top + bounds.Height / 2;

        // Title
        var titleSize = g.MeasureString("GAME OVER", titleFont);
        float titleX = bounds.Left + (bounds.Width - titleSize.Width) / 2;
        g.DrawString("GAME OVER", titleFont, textBrush, titleX, centerY - 100);

        // Score
        var scoreText = $"Score: {score}";
        var scoreSize = g.MeasureString(scoreText, infoFont);
        g.DrawString(scoreText, infoFont, textBrush, bounds.Left + (bounds.Width - scoreSize.Width) / 2, centerY - 40);

        // Level
        var levelText = $"Level: {level}";
        var levelSize = g.MeasureString(levelText, infoFont);
        g.DrawString(levelText, infoFont, textBrush, bounds.Left + (bounds.Width - levelSize.Width) / 2, centerY);

        // Food collected
        var foodText = $"Food Collected: {foodCollected}";
        var foodSize = g.MeasureString(foodText, infoFont);
        g.DrawString(foodText, infoFont, textBrush, bounds.Left + (bounds.Width - foodSize.Width) / 2, centerY + 40);

        // Survival time
        var timeText = $"Survival Time: {(int)_survivalTime.TotalMinutes:D2}:{_survivalTime.Seconds:D2}";
        var timeSize = g.MeasureString(timeText, infoFont);
        g.DrawString(timeText, infoFont, textBrush, bounds.Left + (bounds.Width - timeSize.Width) / 2, centerY + 80);
    }

    /// <summary>
    /// Draws a level up notification.
    /// </summary>
    public void DrawLevelUpNotification(Graphics g, Rectangle bounds, int level)
    {
        using var overlayBrush = new SolidBrush(Color.FromArgb(150, 0, 100, 0));
        g.FillRectangle(overlayBrush, bounds);

        using var titleFont = new Font("Arial", 36, FontStyle.Bold);
        using var levelFont = new Font("Arial", 48, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.Gold);

        var titleSize = g.MeasureString("LEVEL UP!", titleFont);
        float titleX = bounds.Left + (bounds.Width - titleSize.Width) / 2;
        g.DrawString("LEVEL UP!", titleFont, textBrush, titleX, bounds.Top + 50);

        var levelText = $"Level {level}";
        var levelSize = g.MeasureString(levelText, levelFont);
        float levelX = bounds.Left + (bounds.Width - levelSize.Width) / 2;
        g.DrawString(levelText, levelFont, textBrush, levelX, bounds.Top + 100);
    }

    /// <summary>
    /// Draws a message popup.
    /// </summary>
    public void DrawMessage(Graphics g, Point position, string message)
    {
        using var font = new Font("Arial", 16, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.Yellow);
        g.DrawString(message, font, textBrush, position.X, position.Y - 30);
    }
}
