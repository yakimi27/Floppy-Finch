namespace FloppyFinchLogics.AccountManagement;

public class AccountData
{
    public AccountData()
    {
        UnlockedSkins = new List<string>();
        PowerUpLevels = new List<int>();
    }

    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    public int Coins { get; set; }
    public int HighScore { get; set; }
    public List<string> UnlockedSkins { get; set; }
    public List<int> PowerUpLevels { get; set; }

    public bool DarkMode { get; set; }
    public bool MaximizedWindow { get; set; }
    public int WindowPositionX { get; set; }
    public int WindowPositionY { get; set; }
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }
}