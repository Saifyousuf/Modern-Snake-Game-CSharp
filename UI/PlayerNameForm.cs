namespace ModernSnakeGame.UI;

/// <summary>
/// Form for entering player name.
/// </summary>
public class PlayerNameForm : Form
{
    private TextBox _nameTextBox = null!;
    private Button _startButton = null!;
    private Button _cancelButton = null!;
    private Label _instructionLabel = null!;

    public string PlayerName => _nameTextBox.Text.Trim();

    public PlayerNameForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        // Form settings
        this.Text = "Enter Player Name";
        this.Size = new Size(400, 250);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // Instruction Label
        _instructionLabel = new Label
        {
            Text = "Enter Your Name:",
            AutoSize = true,
            Location = new Point(100, 30)
        };

        // Name TextBox
        _nameTextBox = new TextBox
        {
            Location = new Point(75, 70),
            Size = new Size(250, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point),
            MaxLength = 20
        };

        // Start Button
        _startButton = new Button
        {
            Text = "Start Game",
            Size = new Size(120, 40),
            Location = new Point(80, 130),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(70, 130, 180),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        _startButton.Click += StartButton_Click;

        // Cancel Button
        _cancelButton = new Button
        {
            Text = "Cancel",
            Size = new Size(120, 40),
            Location = new Point(200, 130),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        _cancelButton.Click += CancelButton_Click;

        // Add controls
        this.Controls.Add(_instructionLabel);
        this.Controls.Add(_nameTextBox);
        this.Controls.Add(_startButton);
        this.Controls.Add(_cancelButton);

        this.AcceptButton = _startButton;
        this.CancelButton = _cancelButton;

        this.BackColor = Color.FromArgb(30, 30, 30);
        _instructionLabel.ForeColor = Color.White;

        this.ResumeLayout(false);
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_nameTextBox.Text.Trim()))
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            MessageBox.Show("Please enter a valid name.", "Invalid Name", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void CancelButton_Click(object? sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
