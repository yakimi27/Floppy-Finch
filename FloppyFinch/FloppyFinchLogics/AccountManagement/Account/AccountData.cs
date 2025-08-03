namespace FloppyFinchLogics.AccountManagement.Account;

public class AccountData
{
    public AccountData()
    {
        SelectedLanguage = "en";
        SelectedBackground = "Light";
        UnlockedSkins = ["Classic"];
        SelectedSkin = "Classic";
        PowerUpLevels = [1, 1, 1];
        Coins = 0;
        HighScore = 0;
        Sounds = true;
    }

    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int Coins { get; set; }
    public int HighScore { get; set; }
    public List<string> UnlockedSkins { get; set; }
    public List<int> PowerUpLevels { get; set; }
    public bool MaximizedWindow { get; set; }
    public int WindowPositionX { get; set; }
    public int WindowPositionY { get; set; }
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }
    public string SelectedSkin { get; set; }
    public string SelectedLanguage { get; set; }
    public string SelectedBackground { get; set; }
    public bool Sounds { get; set; }
}