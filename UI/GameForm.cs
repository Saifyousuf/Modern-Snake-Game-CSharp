using ModernSnakeGame.Game;
using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Managers;
using ModernSnakeGame.Models;
using ModernSnakeGame.Rendering;

namespace ModernSnakeGame.UI;

/// <summary>
/// Main game form containing the game board and controls.
/// </summary>
public class GameForm : Form
{
    private readonly string _playerName;
    private readonly SaveManager _saveManager;
    private readonly SoundManager _soundManager;
    private readonly GameSettings _settings;
    
    private GameManager? _gameManager;
    private GameRenderer? _renderer;
    private Timer _gameTimer = null!;
    private bool _isLevelUpAnimating;
    private DateTime _levelUpAnimationStartTime;
    
    // UI Controls
    private Panel _gamePanel = null!;
    private Label _scoreLabel = null!;
    private Label _highScoreLabel = null!;
    private Label _levelLabel = null!;
    private Label _difficultyLabel = null!;
    private Label _timeLabel = null!;
    private Button _pauseButton = null!;
    private Button _restartButton = null!;
    private Button _menuButton = null!;
    
    // Touch controls
    private Button _upButton = null!;
    private Button _downButton = null!;
    private Button _leftButton = null!;
    private Button _rightButton = null!;
    
    private const int CellSize = 20;
    private const int BoardWidth = 600;
    private const int BoardHeight = 400;

    public GameForm(string playerName, SaveManager saveManager, SoundManager soundManager, GameSettings settings)
    {
        _playerName = playerName;
        _saveManager = saveManager;
        _soundManager = soundManager;
        _settings = settings;
        
        InitializeComponent();
        InitializeGame();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Modern Snake - Game";
        this.Size = new Size(1000, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.KeyPreview = true;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // Game Panel (for rendering)
        _gamePanel = new DoubleBufferedPanel
        {
            Location = new Point(20, 80),
            Size = new Size(BoardWidth + 4, BoardHeight + 4),
            BackColor = Color.FromArgb(30, 30, 30)
        };
        _gamePanel.Paint += GamePanel_Paint;

        // Score Label
        _scoreLabel = CreateInfoLabel("Score: 0", 20, 20);
        
        // High Score Label
        _highScoreLabel = CreateInfoLabel("High Score: 0", 200, 20);
        
        // Level Label
        _levelLabel = CreateInfoLabel("Level: 1", 400, 20);
        
        // Difficulty Label
        _difficultyLabel = CreateInfoLabel($"Difficulty: {_settings.Difficulty}", 550, 20);
        
        // Time Label (for Time Attack mode)
        _timeLabel = CreateInfoLabel("", 750, 20);
        if (_settings.GameMode == GameMode.TimeAttack)
        {
            _timeLabel.Visible = true;
        }

        // Pause Button
        _pauseButton = CreateControlButton("Pause (P)", 20, BoardHeight + 100);
        _pauseButton.Click += PauseButton_Click;

        // Restart Button
        _restartButton = CreateControlButton("Restart (R)", 150, BoardHeight + 100);
        _restartButton.Click += RestartButton_Click;

        // Menu Button
        _menuButton = CreateControlButton("Main Menu", 280, BoardHeight + 100);
        _menuButton.Click += MenuButton_Click;

        // Touch Controls
        CreateTouchControls();

        // Add controls
        this.Controls.Add(_scoreLabel);
        this.Controls.Add(_highScoreLabel);
        this.Controls.Add(_levelLabel);
        this.Controls.Add(_difficultyLabel);
        this.Controls.Add(_timeLabel);
        this.Controls.Add(_gamePanel);
        this.Controls.Add(_pauseButton);
        this.Controls.Add(_restartButton);
        this.Controls.Add(_menuButton);

        // Key events
        this.KeyDown += GameForm_KeyDown;
        this.KeyUp += GameForm_KeyUp;

        this.ResumeLayout(false);
    }

    private Label CreateInfoLabel(string text, int x, int y)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Location = new Point(x, y),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.White
        };
    }

    private Button CreateControlButton(string text, int x, int y)
    {
        return new Button
        {
            Text = text,
            Size = new Size(110, 40),
            Location = new Point(x, y),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
    }

    private void CreateTouchControls()
    {
        int buttonSize = 50;
        int startX = BoardWidth + 50;
        int startY = 150;

        // Up Button
        _upButton = CreateTouchButton("▲", startX + buttonSize, startY - buttonSize * 2 - 10);
        _upButton.Click += (s, e) => ChangeDirection(Direction.Up);

        // Down Button
        _downButton = CreateTouchButton("▼", startX + buttonSize, startY + buttonSize * 2 + 10);
        _downButton.Click += (s, e) => ChangeDirection(Direction.Down);

        // Left Button
        _leftButton = CreateTouchButton("◀", startX - buttonSize * 2 - 10, startY);
        _leftButton.Click += (s, e) => ChangeDirection(Direction.Left);

        // Right Button
        _rightButton = CreateTouchButton("▶", startX + buttonSize * 2 + 10, startY);
        _rightButton.Click += (s, e) => ChangeDirection(Direction.Right);

        this.Controls.Add(_upButton);
        this.Controls.Add(_downButton);
        this.Controls.Add(_leftButton);
        this.Controls.Add(_rightButton);
    }

    private Button CreateTouchButton(string text, int x, int y)
    {
        return new Button
        {
            Text = text,
            Size = new Size(60, 60),
            Location = new Point(x, y),
            Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
    }

    private void InitializeGame()
    {
        var snake = new Snake(BoardWidth / 2, BoardHeight / 2, CellSize);
        snake.SetSkin(_settings.SnakeSkin);
        
        var boardBounds = new Rectangle(2, 2, BoardWidth, BoardHeight);
        
        _gameManager = new GameManager(
            snake, 
            boardBounds, 
            CellSize, 
            _settings.GameMode, 
            _settings.Difficulty);
        
        _renderer = new GameRenderer(CellSize, _settings.Theme);
        
        // Load persistent high score from statistics
        var savedStats = _saveManager.LoadStatistics();
        _gameManager.ScoreManager.SetHighScore(savedStats.HighestScore);
        
        // Subscribe to events
        _gameManager.OnGameStateChanged += () => UpdateUI();
        _gameManager.OnPauseStateChanged += () => UpdateUI();
        _gameManager.OnGameOver += HandleGameOver;
        _gameManager.OnFoodEaten += (pos) => _soundManager.PlayFoodSound();
        _gameManager.OnPowerUpCollected += (pos) => _soundManager.PlayPowerUpSound();
        _gameManager.OnMessage += ShowMessage;
        _gameManager.LevelManager.OnLevelUp += LevelManager_OnLevelUp;
        
        // Start game
        _gameManager.Start();
        
        // Setup timer
        _gameTimer = new Timer();
        _gameTimer.Tick += GameTimer_Tick;
        _gameTimer.Interval = _gameManager.GetGameTickInterval();
        _gameTimer.Start();
    }

    private void LevelManager_OnLevelUp(int level)
    {
        _isLevelUpAnimating = true;
        _levelUpAnimationStartTime = DateTime.Now;
        _soundManager.PlayLevelUpSound();
        _gamePanel.Invalidate();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        if (_gameManager != null && !_gameManager.IsPaused && !_gameManager.IsGameOver)
        {
            _gameManager.Update();
            
            // Update timer interval based on current speed
            _gameTimer.Interval = _gameManager.GetGameTickInterval();
            
            // Check for level up animation
            if (_isLevelUpAnimating && (DateTime.Now - _levelUpAnimationStartTime).TotalSeconds > 2)
            {
                _isLevelUpAnimating = false;
            }
            
            _gamePanel.Invalidate();
        }
    }

    private void GamePanel_Paint(object? sender, PaintEventArgs e)
    {
        if (_renderer != null && _gameManager != null)
        {
            var boardBounds = new Rectangle(0, 0, BoardWidth, BoardHeight);
            _renderer.Render(e.Graphics, boardBounds, _gameManager);
            
            // Draw pause overlay
            if (_gameManager.IsPaused)
            {
                _renderer.DrawPauseOverlay(e.Graphics, boardBounds);
            }
            
            // Draw level up notification
            if (_isLevelUpAnimating)
            {
                _renderer.DrawLevelUpNotification(e.Graphics, boardBounds, _gameManager.LevelManager.CurrentLevel);
            }
        }
    }

    private void GameForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (_gameManager == null || _gameManager.IsGameOver) return;
        
        // Prevent default behavior for game keys
        if (e.KeyCode == Keys.P || e.KeyCode == Keys.R)
        {
            e.SuppressKeyPress = true;
        }

        switch (e.KeyCode)
        {
            case Keys.Up:
            case Keys.W:
                ChangeDirection(Direction.Up);
                break;
            case Keys.Down:
            case Keys.S:
                ChangeDirection(Direction.Down);
                break;
            case Keys.Left:
            case Keys.A:
                ChangeDirection(Direction.Left);
                break;
            case Keys.Right:
            case Keys.D:
                ChangeDirection(Direction.Right);
                break;
            case Keys.P:
                TogglePause();
                break;
            case Keys.R:
                RestartGame();
                break;
        }
    }

    private void GameForm_KeyUp(object? sender, KeyEventArgs e)
    {
        // Can be used for future key-up handling
    }

    private void ChangeDirection(Direction direction)
    {
        if (_gameManager != null && !_gameManager.IsPaused && !_gameManager.IsGameOver)
        {
            _gameManager.ChangeDirection(direction);
        }
    }

    private void TogglePause()
    {
        if (_gameManager != null)
        {
            _gameManager.Pause();
            _gamePanel.Invalidate();
        }
    }

    private void RestartGame()
    {
        if (_gameManager != null)
        {
            _gameManager.Restart();
            _gamePanel.Invalidate();
        }
    }

    private void UpdateUI()
    {
        if (_gameManager == null) return;
        
        _scoreLabel.Text = $"Score: {_gameManager.ScoreManager.CurrentScore}";
        _highScoreLabel.Text = $"High Score: {_gameManager.ScoreManager.HighScore}";
        _levelLabel.Text = $"Level: {_gameManager.LevelManager.CurrentLevel}";
        
        if (_settings.GameMode == GameMode.TimeAttack)
        {
            int remainingTime = _gameManager.GetRemainingTimeSeconds();
            _timeLabel.Text = $"Time: {remainingTime}s";
            if (remainingTime <= 10)
            {
                _timeLabel.ForeColor = Color.Red;
            }
            else
            {
                _timeLabel.ForeColor = Color.White;
            }
        }
    }

    private void HandleGameOver()
    {
        _gameTimer.Stop();
        _soundManager.PlayGameOverSound();
        
        // Record statistics
        var statsManager = new StatisticsManager(_saveManager);
        statsManager.RecordGameEnd(
            _gameManager!.ScoreManager.CurrentScore,
            _gameManager.LevelManager.CurrentLevel,
            _gameManager.SurvivalTime,
            _gameManager.ScoreManager.FoodCollected,
            false);
        
        // Add to leaderboard
        var leaderboardManager = new LeaderboardManager(_saveManager);
        leaderboardManager.AddScore(
            _playerName,
            _gameManager.ScoreManager.CurrentScore,
            _settings.GameMode,
            _settings.Difficulty,
            _gameManager.LevelManager.CurrentLevel);
        
        // Update high score in ScoreManager from persistent data
        var savedStats = _saveManager.LoadStatistics();
        _gameManager.ScoreManager.SetHighScore(savedStats.HighestScore);
        
        // Show game over
        _gamePanel.Invalidate();
        
        // Show dialog after a short delay
        Task.Delay(500).ContinueWith(_ => {
            this.Invoke(new Action(() => {
                using var gameOverForm = new GameOverForm(
                    _gameManager.ScoreManager.CurrentScore,
                    savedStats.HighestScore,
                    _gameManager.LevelManager.CurrentLevel,
                    _gameManager.ScoreManager.FoodCollected,
                    _gameManager.SurvivalTime,
                    leaderboardManager.IsHighScore(_gameManager.ScoreManager.CurrentScore));
                
                gameOverForm.PlayAgainClicked += (s, e) => {
                    gameOverForm.Close();
                    RestartGame();
                    _gameTimer.Start();
                };
                
                gameOverForm.MainMenuClicked += (s, e) => {
                    gameOverForm.Close();
                    this.Close();
                };
                
                gameOverForm.ShowDialog(this);
            }));
        });
    }

    private void ShowMessage(string message)
    {
        // Could show floating message animation
        Console.WriteLine($"Message: {message}");
    }

    private void PauseButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        TogglePause();
    }

    private void RestartButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        
        var result = MessageBox.Show(
            "Are you sure you want to restart?",
            "Restart Game",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            RestartGame();
        }
    }

    private void MenuButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        this.Close();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _gameTimer?.Stop();
        _gameTimer?.Dispose();
        base.OnFormClosing(e);
    }
}

/// <summary>
/// Double-buffered panel for smooth rendering.
/// </summary>
public class DoubleBufferedPanel : Panel
{
    public DoubleBufferedPanel()
    {
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.ResizeRedraw, true);
    }
}
