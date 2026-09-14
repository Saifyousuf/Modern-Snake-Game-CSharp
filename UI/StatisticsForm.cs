using ModernSnakeGame.Managers;

namespace ModernSnakeGame.UI;

/// <summary>
/// Statistics form displaying player statistics.
/// </summary>
public class StatisticsForm : Form
{
    private readonly SaveManager _saveManager;

    public StatisticsForm(SaveManager saveManager)
    {
        _saveManager = saveManager;
        InitializeComponent();
        LoadStatistics();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Statistics";
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
            Text = "📊 Game Statistics",
            AutoSize = true,
            Location = new Point(140, 20),
            Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.Gold
        };
        this.Controls.Add(titleLabel);

        // Stats labels
        var stats = new[]
        {
            ("Total Games Played:", "totalGames"),
            ("Highest Score:", "highestScore"),
            ("Highest Level:", "highestLevel"),
            ("Total Food Collected:", "totalFood"),
            ("Best Survival Time:", "bestTime"),
            ("Total Play Time:", "totalPlayTime"),
            ("Games Won:", "gamesWon"),
            ("Games Lost:", "gamesLost")
        };

        int startY = 80;
        for (int i = 0; i < stats.Length; i++)
        {
            var labelName = stats[i].Item2;
            
            var labelText = new Label
            {
                Text = stats[i].Item1,
                AutoSize = true,
                Location = new Point(50, startY + i * 45),
                Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White
            };
            this.Controls.Add(labelText);

            var labelValue = new Label
            {
                Name = labelName,
                Text = "-",
                AutoSize = true,
                Location = new Point(300, startY + i * 45),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(70, 130, 180)
            };
            this.Controls.Add(labelValue);
        }

        // Reset Button
        var resetButton = new Button
        {
            Text = "Reset Statistics",
            Size = new Size(150, 40),
            Location = new Point(80, 460),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Red,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        resetButton.Click += ResetButton_Click;
        this.Controls.Add(resetButton);

        // Close Button
        var closeButton = new Button
        {
            Text = "Close",
            Size = new Size(150, 40),
            Location = new Point(270, 460),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        closeButton.Click += (s, e) => this.Close();
        this.Controls.Add(closeButton);

        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ResumeLayout(false);
    }

    private void LoadStatistics()
    {
        var statsManager = new StatisticsManager(_saveManager);
        var stats = statsManager.Statistics;

        this.Controls.Find("totalGames", true).FirstOrDefault()?.Text = stats.TotalGamesPlayed.ToString();
        this.Controls.Find("highestScore", true).FirstOrDefault()?.Text = stats.HighestScore.ToString();
        this.Controls.Find("highestLevel", true).FirstOrDefault()?.Text = stats.HighestLevel.ToString();
        this.Controls.Find("totalFood", true).FirstOrDefault()?.Text = stats.TotalFoodCollected.ToString();
        this.Controls.Find("bestTime", true).FirstOrDefault()?.Text = $"{(int)stats.BestSurvivalTime.TotalMinutes:D2}:{stats.BestSurvivalTime.Seconds:D2}";
        this.Controls.Find("totalPlayTime", true).FirstOrDefault()?.Text = $"{(int)stats.TotalPlayTime.TotalHours:D2}:{stats.TotalPlayTime.Minutes:D2}:{stats.TotalPlayTime.Seconds:D2}";
        this.Controls.Find("gamesWon", true).FirstOrDefault()?.Text = stats.GamesWon.ToString();
        this.Controls.Find("gamesLost", true).FirstOrDefault()?.Text = stats.GamesLost.ToString();
    }

    private void ResetButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to reset all statistics?",
            "Reset Statistics",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            var statsManager = new StatisticsManager(_saveManager);
            statsManager.Reset();
            LoadStatistics();
        }
    }
}
