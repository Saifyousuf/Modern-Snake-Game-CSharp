namespace ModernSnakeGame.UI;

/// <summary>
/// Game Over form displaying final results.
/// </summary>
public class GameOverForm : Form
{
    private readonly int _score;
    private readonly int _highScore;
    private readonly int _level;
    private readonly int _foodCollected;
    private readonly TimeSpan _survivalTime;
    private readonly bool _isNewHighScore;

    public event EventHandler? PlayAgainClicked;
    public event EventHandler? MainMenuClicked;

    public GameOverForm(int score, int highScore, int level, int foodCollected, TimeSpan survivalTime, bool isNewHighScore)
    {
        _score = score;
        _highScore = highScore;
        _level = level;
        _foodCollected = foodCollected;
        _survivalTime = survivalTime;
        _isNewHighScore = isNewHighScore;

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Game Over";
        this.Size = new Size(500, 500);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // New High Score Label (if applicable)
        if (_isNewHighScore)
        {
            var highScoreLabel = new Label
            {
                Text = "🏆 NEW HIGH SCORE! 🏆",
                AutoSize = true,
                Location = new Point(70, 20),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Gold
            };
            this.Controls.Add(highScoreLabel);
        }

        // Game Over Title
        var gameOverTitle = new Label
        {
            Text = "GAME OVER",
            AutoSize = true,
            Location = new Point(150, _isNewHighScore ? 70 : 20),
            Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.Red
        };
        this.Controls.Add(gameOverTitle);

        // Stats
        int startY = _isNewHighScore ? 130 : 80;
        var stats = new[]
        {
            $"Score: {_score}",
            $"High Score: {_highScore}",
            $"Level: {_level}",
            $"Food Collected: {_foodCollected}",
            $"Survival Time: {(int)_survivalTime.TotalMinutes:D2}:{_survivalTime.Seconds:D2}"
        };

        for (int i = 0; i < stats.Length; i++)
        {
            var statLabel = new Label
            {
                Text = stats[i],
                AutoSize = true,
                Location = new Point(125, startY + i * 35),
                Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White
            };
            this.Controls.Add(statLabel);
        }

        // Play Again Button
        var playAgainButton = new Button
        {
            Text = "Play Again",
            Size = new Size(150, 45),
            Location = new Point(80, 380),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };
        playAgainButton.Click += (s, e) => {
            PlayAgainClicked?.Invoke(this, EventArgs.Empty);
        };
        this.Controls.Add(playAgainButton);

        // Main Menu Button
        var mainMenuButton = new Button
        {
            Text = "Main Menu",
            Size = new Size(150, 45),
            Location = new Point(270, 380),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };
        mainMenuButton.Click += (s, e) => {
            MainMenuClicked?.Invoke(this, EventArgs.Empty);
        };
        this.Controls.Add(mainMenuButton);

        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ResumeLayout(false);
    }
}
