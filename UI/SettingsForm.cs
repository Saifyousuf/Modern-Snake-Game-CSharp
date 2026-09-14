using ModernSnakeGame.Interfaces;
using ModernSnakeGame.Managers;

namespace ModernSnakeGame.UI;

/// <summary>
/// Settings form for configuring game options.
/// </summary>
public class SettingsForm : Form
{
    private readonly SaveManager _saveManager;
    private readonly SoundManager _soundManager;
    private GameSettings _settings;

    private ComboBox _themeComboBox = null!;
    private ComboBox _skinComboBox = null!;
    private ComboBox _difficultyComboBox = null!;
    private ComboBox _gameModeComboBox = null!;
    private CheckBox _soundCheckBox = null!;
    private CheckBox _musicCheckBox = null!;

    public SettingsForm(SaveManager saveManager, SoundManager soundManager)
    {
        _saveManager = saveManager;
        _soundManager = soundManager;
        _settings = _saveManager.LoadSettings();
        InitializeComponent();
        LoadSettings();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Settings";
        this.Size = new Size(500, 550);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // Title
        var titleLabel = new Label
        {
            Text = "⚙️ Settings",
            AutoSize = true,
            Location = new Point(180, 20),
            Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.Gold
        };
        this.Controls.Add(titleLabel);

        int startY = 70;
        int labelX = 50;
        int controlX = 250;
        int rowHeight = 50;

        // Theme
        this.Controls.Add(CreateLabel("Theme:", labelX, startY));
        _themeComboBox = CreateComboBox(new[] { "Dark", "Light", "Neon" }, controlX, startY);
        this.Controls.Add(_themeComboBox);

        // Snake Skin
        this.Controls.Add(CreateLabel("Snake Skin:", labelX, startY + rowHeight));
        _skinComboBox = CreateComboBox(new[] { "Classic Green", "Blue", "Red", "Neon", "Rainbow" }, controlX, startY + rowHeight);
        this.Controls.Add(_skinComboBox);

        // Difficulty
        this.Controls.Add(CreateLabel("Difficulty:", labelX, startY + rowHeight * 2));
        _difficultyComboBox = CreateComboBox(new[] { "Easy", "Medium", "Hard", "Extreme" }, controlX, startY + rowHeight * 2);
        this.Controls.Add(_difficultyComboBox);

        // Game Mode
        this.Controls.Add(CreateLabel("Game Mode:", labelX, startY + rowHeight * 3));
        _gameModeComboBox = CreateComboBox(new[] { "Classic", "Time Attack", "Obstacle", "Endless" }, controlX, startY + rowHeight * 3);
        this.Controls.Add(_gameModeComboBox);

        // Sound
        _soundCheckBox = new CheckBox
        {
            Text = "Sound Effects",
            Location = new Point(labelX, startY + rowHeight * 4),
            Size = new Size(200, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.White
        };
        this.Controls.Add(_soundCheckBox);

        // Music
        _musicCheckBox = new CheckBox
        {
            Text = "Background Music",
            Location = new Point(labelX, startY + rowHeight * 4 + 35),
            Size = new Size(200, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.White
        };
        this.Controls.Add(_musicCheckBox);

        // Save Button
        var saveButton = new Button
        {
            Text = "Save Settings",
            Size = new Size(150, 45),
            Location = new Point(80, 450),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };
        saveButton.Click += SaveButton_Click;
        this.Controls.Add(saveButton);

        // Reset Button
        var resetButton = new Button
        {
            Text = "Reset to Default",
            Size = new Size(150, 45),
            Location = new Point(270, 450),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };
        resetButton.Click += ResetButton_Click;
        this.Controls.Add(resetButton);

        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ResumeLayout(false);
    }

    private Label CreateLabel(string text, int x, int y)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            Location = new Point(x, y + 10),
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
            ForeColor = Color.White
        };
    }

    private ComboBox CreateComboBox(string[] items, int x, int y)
    {
        return new ComboBox
        {
            Items = { items },
            Location = new Point(x, y),
            Size = new Size(200, 32),
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(50, 50, 50),
            ForeColor = Color.White
        };
    }

    private void LoadSettings()
    {
        _themeComboBox.SelectedIndex = (int)_settings.Theme;
        _skinComboBox.SelectedIndex = (int)_settings.SnakeSkin;
        _difficultyComboBox.SelectedIndex = (int)_settings.Difficulty;
        _gameModeComboBox.SelectedIndex = (int)_settings.GameMode;
        _soundCheckBox.Checked = _settings.SoundEnabled;
        _musicCheckBox.Checked = _settings.MusicEnabled;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        _settings.Theme = (Theme)_themeComboBox.SelectedIndex;
        _settings.SnakeSkin = (SnakeSkin)_skinComboBox.SelectedIndex;
        _settings.Difficulty = (Difficulty)_difficultyComboBox.SelectedIndex;
        _settings.GameMode = (GameMode)_gameModeComboBox.SelectedIndex;
        _settings.SoundEnabled = _soundCheckBox.Checked;
        _settings.MusicEnabled = _musicCheckBox.Checked;

        _saveManager.SaveSettings(_settings);
        
        _soundManager.SoundEnabled = _settings.SoundEnabled;
        _soundManager.MusicEnabled = _settings.MusicEnabled;

        MessageBox.Show("Settings saved successfully!", "Settings Saved", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ResetButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to reset settings to default?",
            "Reset Settings",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _settings = new GameSettings();
            _saveManager.SaveSettings(_settings);
            LoadSettings();
            
            _soundManager.SoundEnabled = _settings.SoundEnabled;
            _soundManager.MusicEnabled = _settings.MusicEnabled;
        }
    }
}
