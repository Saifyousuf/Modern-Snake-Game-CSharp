using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Managers;
using ModernSnakeGame.UI;

namespace ModernSnakeGame.UI;

/// <summary>
/// Main menu form for the Modern Snake game.
/// </summary>
public class MainMenuForm : Form
{
    private readonly SaveManager _saveManager;
    private readonly SoundManager _soundManager;
    private GameSettings _settings;

    // UI Controls
    private Label _titleLabel = null!;
    private Button _startButton = null!;
    private Button _howToPlayButton = null!;
    private Button _leaderboardButton = null!;
    private Button _statisticsButton = null!;
    private Button _settingsButton = null!;
    private Button _exitButton = null!;

    public MainMenuForm()
    {
        _saveManager = new SaveManager();
        _soundManager = new SoundManager();
        _settings = _saveManager.LoadSettings();
        
        _soundManager.SoundEnabled = _settings.SoundEnabled;
        _soundManager.MusicEnabled = _settings.MusicEnabled;

        InitializeComponent();
        ApplyTheme();
        
        // Start background music
        _soundManager.PlayMusic();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // Form settings
        this.Text = "Modern Snake";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // Title Label
        _titleLabel = new Label
        {
            Text = "🐍 MODERN SNAKE",
            Font = new Font("Segoe UI", 48F, FontStyle.Bold, GraphicsUnit.Point),
            AutoSize = true,
            Location = new Point(150, 50)
        };

        // Start Button
        _startButton = CreateStyledButton("Start Game", 275, 180);
        _startButton.Click += StartButton_Click;

        // How to Play Button
        _howToPlayButton = CreateStyledButton("How to Play", 275, 240);
        _howToPlayButton.Click += HowToPlayButton_Click;

        // Leaderboard Button
        _leaderboardButton = CreateStyledButton("Leaderboard", 275, 300);
        _leaderboardButton.Click += LeaderboardButton_Click;

        // Statistics Button
        _statisticsButton = CreateStyledButton("Statistics", 275, 360);
        _statisticsButton.Click += StatisticsButton_Click;

        // Settings Button
        _settingsButton = CreateStyledButton("Settings", 275, 420);
        _settingsButton.Click += SettingsButton_Click;

        // Exit Button
        _exitButton = CreateStyledButton("Exit", 275, 480);
        _exitButton.Click += ExitButton_Click;

        // Add controls
        this.Controls.Add(_titleLabel);
        this.Controls.Add(_startButton);
        this.Controls.Add(_howToPlayButton);
        this.Controls.Add(_leaderboardButton);
        this.Controls.Add(_statisticsButton);
        this.Controls.Add(_settingsButton);
        this.Controls.Add(_exitButton);

        this.ResumeLayout(false);
    }

    private Button CreateStyledButton(string text, int x, int y)
    {
        return new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
            Size = new Size(250, 50),
            Location = new Point(x, y),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
    }

    private void ApplyTheme()
    {
        Color bgColor, textColor, buttonColor;

        switch (_settings.Theme)
        {
            case Theme.Light:
                bgColor = Color.White;
                textColor = Color.Black;
                buttonColor = Color.FromArgb(70, 130, 180);
                break;
            case Theme.Neon:
                bgColor = Color.Black;
                textColor = Color.Cyan;
                buttonColor = Color.FromArgb(0, 100, 100);
                break;
            default: // Dark
                bgColor = Color.FromArgb(30, 30, 30);
                textColor = Color.White;
                buttonColor = Color.FromArgb(70, 130, 180);
                break;
        }

        this.BackColor = bgColor;
        _titleLabel.ForeColor = textColor;
        _howToPlayButton.ForeColor = textColor;
        _leaderboardButton.ForeColor = textColor;
        _statisticsButton.ForeColor = textColor;
        _settingsButton.ForeColor = textColor;
        _exitButton.ForeColor = textColor;

        _startButton.BackColor = buttonColor;
        _howToPlayButton.BackColor = buttonColor;
        _leaderboardButton.BackColor = buttonColor;
        _statisticsButton.BackColor = buttonColor;
        _settingsButton.BackColor = buttonColor;
        _exitButton.BackColor = buttonColor;
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        
        // Show player name input
        using var inputForm = new PlayerNameForm();
        if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(inputForm.PlayerName))
        {
            this.Hide();
            
            using var gameForm = new GameForm(inputForm.PlayerName, _saveManager, _soundManager, _settings);
            gameForm.FormClosed += (s, args) => {
                _settings = _saveManager.LoadSettings();
                ApplyTheme();
                this.Show();
            };
            gameForm.ShowDialog();
        }
    }

    private void HowToPlayButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        using var howToPlayForm = new HowToPlayForm();
        howToPlayForm.ShowDialog();
    }

    private void LeaderboardButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        using var leaderboardForm = new LeaderboardForm(_saveManager);
        leaderboardForm.ShowDialog();
    }

    private void StatisticsButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        using var statisticsForm = new StatisticsForm(_saveManager);
        statisticsForm.ShowDialog();
    }

    private void SettingsButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        using var settingsForm = new SettingsForm(_saveManager, _soundManager);
        settingsForm.FormClosed += (s, args) => {
            _settings = _saveManager.LoadSettings();
            ApplyTheme();
        };
        settingsForm.ShowDialog();
    }

    private void ExitButton_Click(object? sender, EventArgs e)
    {
        _soundManager.PlayClickSound();
        _soundManager.StopMusic();
        this.Close();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _soundManager.StopMusic();
        base.OnFormClosing(e);
    }
}
