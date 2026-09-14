using System.Media;

namespace ModernSnakeGame.Managers;

/// <summary>
/// Manages game sounds and music.
/// Uses System.SoundPlayer for WAV files.
/// Gracefully handles missing sound files.
/// </summary>
public class SoundManager
{
    private bool _soundEnabled;
    private bool _musicEnabled;
    private readonly string _soundDirectory;
    private SoundPlayer? _backgroundMusicPlayer;
    private bool _isMusicPlaying;

    public bool SoundEnabled 
    { 
        get => _soundEnabled; 
        set => _soundEnabled = value; 
    }

    public bool MusicEnabled 
    { 
        get => _musicEnabled; 
        set 
        { 
            _musicEnabled = value;
            if (!value)
            {
                StopMusic();
            }
            else if (!_isMusicPlaying)
            {
                PlayMusic();
            }
        } 
    }

    public SoundManager()
    {
        _soundEnabled = true;
        _musicEnabled = true;
        _soundDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");
        _isMusicPlaying = false;
    }

    public void PlaySound(string soundFileName)
    {
        if (!_soundEnabled) return;

        try
        {
            string soundPath = Path.Combine(_soundDirectory, soundFileName);
            
            if (File.Exists(soundPath))
            {
                using var player = new SoundPlayer(soundPath);
                player.PlayAsync();
            }
            else
            {
                // Fallback: use system beep
                Console.Beep(800, 100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound {soundFileName}: {ex.Message}");
            // Fallback to system beep
            Console.Beep(800, 100);
        }
    }

    public void PlayClickSound()
    {
        PlaySound("click.wav");
    }

    public void PlayFoodSound()
    {
        PlaySound("food.wav");
    }

    public void PlayPowerUpSound()
    {
        PlaySound("powerup.wav");
    }

    public void PlayLevelUpSound()
    {
        PlaySound("levelup.wav");
    }

    public void PlayGameOverSound()
    {
        PlaySound("gameover.wav");
    }

    public void PlayMusic()
    {
        if (!_musicEnabled || _isMusicPlaying) return;

        try
        {
            string musicPath = Path.Combine(_soundDirectory, "background.wav");
            
            if (File.Exists(musicPath))
            {
                _backgroundMusicPlayer = new SoundPlayer(musicPath);
                _backgroundMusicPlayer.PlayLooping();
                _isMusicPlaying = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing music: {ex.Message}");
        }
    }

    public void StopMusic()
    {
        if (_backgroundMusicPlayer != null)
        {
            _backgroundMusicPlayer.Stop();
            _isMusicPlaying = false;
        }
    }

    public void ToggleSound()
    {
        _soundEnabled = !_soundEnabled;
    }

    public void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
        if (!_musicEnabled)
        {
            StopMusic();
        }
    }
}
