using ModernSnakeGame.UI;

namespace ModernSnakeGame;

/// <summary>
/// Main entry point for the Modern Snake Game application.
/// </summary>
static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Enable visual styles for modern look
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(true);
        
        // Set high DPI mode for better scaling
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        try
        {
            // Run the main menu form
            Application.Run(new MainMenuForm());
        }
        catch (Exception ex)
        {
            // Handle any unhandled exceptions gracefully
            MessageBox.Show(
                $"An error occurred: {ex.Message}\n\n" +
                "The application will now close.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
