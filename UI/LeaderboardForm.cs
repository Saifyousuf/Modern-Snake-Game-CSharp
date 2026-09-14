using ModernSnakeGame.Managers;

namespace ModernSnakeGame.UI;

/// <summary>
/// Leaderboard form displaying top scores.
/// </summary>
public class LeaderboardForm : Form
{
    private readonly SaveManager _saveManager;
    private DataGridView _dataGridView = null!;

    public LeaderboardForm(SaveManager saveManager)
    {
        _saveManager = saveManager;
        InitializeComponent();
        LoadLeaderboard();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "Leaderboard - Top 10";
        this.Size = new Size(800, 500);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // DataGridView for leaderboard
        _dataGridView = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(740, 380),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.FromArgb(30, 30, 30),
            BorderStyle = BorderStyle.None,
            RowTemplate.Height = 35
        };

        // Add columns
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Rank",
            HeaderText = "Rank",
            Width = 60
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Player",
            HeaderText = "Player",
            Width = 150
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Score",
            HeaderText = "Score",
            Width = 80
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Mode",
            HeaderText = "Mode",
            Width = 120
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Difficulty",
            HeaderText = "Difficulty",
            Width = 100
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Level",
            HeaderText = "Level",
            Width = 60
        });
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Date",
            HeaderText = "Date",
            Width = 170
        });

        // Style the grid
        _dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
        _dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        _dataGridView.RowHeadersVisible = false;
        _dataGridView.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
        _dataGridView.DefaultCellStyle.ForeColor = Color.White;
        _dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
        _dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;

        // Close Button
        var closeButton = new Button
        {
            Text = "Close",
            Size = new Size(120, 40),
            Location = new Point(340, 420),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        closeButton.Click += (s, e) => this.Close();

        this.Controls.Add(_dataGridView);
        this.Controls.Add(closeButton);

        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ResumeLayout(false);
    }

    private void LoadLeaderboard()
    {
        var leaderboardManager = new LeaderboardManager(_saveManager);
        var scores = leaderboardManager.TopScores;

        _dataGridView.Rows.Clear();

        int rank = 1;
        foreach (var score in scores)
        {
            _dataGridView.Rows.Add(
                rank++,
                score.PlayerName,
                score.Score,
                score.GameMode.ToString(),
                score.Difficulty.ToString(),
                score.Level,
                score.Date.ToString("yyyy-MM-dd HH:mm")
            );
        }

        if (scores.Count == 0)
        {
            // Add a message if no scores
            _dataGridView.Rows.Add("No scores yet!", "", "", "", "", "", "");
        }
    }
}
