namespace ModernSnakeGame.UI;

/// <summary>
/// How to Play form with game instructions.
/// </summary>
public class HowToPlayForm : Form
{
    public HowToPlayForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();

        this.Text = "How to Play";
        this.Size = new Size(700, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);

        // Title
        var titleLabel = new Label
        {
            Text = "🎮 How to Play",
            AutoSize = true,
            Location = new Point(230, 15),
            Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.Gold
        };
        this.Controls.Add(titleLabel);

        // Instructions
        var instructions = new[]
        {
            "🐍 MOVEMENT:",
            "   • Use Arrow Keys (↑ ↓ ← →) or WASD to move",
            "   • On-screen buttons for touch devices",
            "",
            "🍎 FOOD:",
            "   • Red Apple (🍎): +10 points, snake grows",
            "   • Gold Star (⭐): +25 points, snake grows",
            "   • Blue Diamond (💎): +50 points, limited time!",
            "   • Yellow Lightning (⚡): Speed boost",
            "",
            "⚡ POWER-UPS:",
            "   • Shield (🔵): Protects from one collision",
            "   • Slow Motion (🔷): Slows down temporarily",
            "   • Speed Boost (🟠): Faster movement",
            "   • Shrink (🟣): Reduces snake length",
            "   • Magnet (🟪): Attracts nearby food",
            "",
            "📊 LEVELS:",
            "   • Score 0-50: Level 1",
            "   • Score increases → Level increases",
            "   • Higher levels = faster snake + more obstacles",
            "",
            "❌ GAME OVER WHEN:",
            "   • Snake hits the wall",
            "   • Snake hits itself",
            "   • Snake hits an obstacle",
            "",
            "⏸️ CONTROLS:",
            "   • P: Pause/Resume game",
            "   • R: Restart game"
        };

        int startY = 60;
        int lineHeight = 25;
        
        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            bool isHeading = instruction.EndsWith(":") || instruction.StartsWith("🐍") || 
                            instruction.StartsWith("🍎") || instruction.StartsWith("⚡") ||
                            instruction.StartsWith("📊") || instruction.StartsWith("❌") ||
                            instruction.StartsWith("⏸️");
            
            var label = new Label
            {
                Text = instruction,
                AutoSize = true,
                Location = new Point(40, startY + i * lineHeight),
                Font = new Font("Segoe UI", isHeading ? 14F : 12F, 
                    isHeading ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = isHeading ? Color.Gold : Color.White
            };
            this.Controls.Add(label);
        }

        // Keyboard visual
        var keyboardBox = new GroupBox
        {
            Text = "Keyboard Controls",
            Location = new Point(450, 60),
            Size = new Size(200, 180),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        // Draw key boxes
        var keys = new[]
        {
            ("W / ↑", "Up"),
            ("S / ↓", "Down"),
            ("A / ←", "Left"),
            ("D / →", "Right"),
            ("P", "Pause"),
            ("R", "Restart")
        };

        for (int i = 0; i < keys.Length; i++)
        {
            var keyLabel = new Label
            {
                Text = keys[i].Item1,
                AutoSize = true,
                Location = new Point(15, 25 + i * 25),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                Padding = new Padding(5, 2, 5, 2)
            };
            keyboardBox.Controls.Add(keyLabel);

            var descLabel = new Label
            {
                Text = keys[i].Item2,
                AutoSize = true,
                Location = new Point(80, 28 + i * 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.LightGray
            };
            keyboardBox.Controls.Add(descLabel);
        }

        this.Controls.Add(keyboardBox);

        // Close Button
        var closeButton = new Button
        {
            Text = "Close",
            Size = new Size(120, 45),
            Location = new Point(290, 520),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };
        closeButton.Click += (s, e) => this.Close();
        this.Controls.Add(closeButton);

        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ResumeLayout(false);
    }
}
