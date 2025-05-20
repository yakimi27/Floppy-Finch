using System.IO;
using System.Text.Json;
using System.Windows;

namespace FloppyFinchLogics.AccountManagement;

public static class AccountManager
{
    private static readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
    private static readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;

    private static readonly int DefaultWindowWidth = 400;
    private static readonly int DefaultWindowHeight = 600;

    private static readonly int DefaultCenterX = (int)((ScreenWidth - DefaultWindowWidth) / 2);
    private static readonly int DefaultCenterY = (int)((ScreenHeight - DefaultWindowHeight) / 2);

    private static readonly string AppDataRoot = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FloppyFinch");

    private static readonly string AccountsFolder = Path.Combine(AppDataRoot, "Accounts");


    static AccountManager()
    {
        Directory.CreateDirectory(AccountsFolder);
    }

    public static AccountData? CurrentAccount { get; set; }
    public static List<AccountData> Accounts { get; } = new();
    public static bool IsGuest { get; private set; }

    public static void LoginAsGuest()
    {
        CurrentAccount = new AccountData { Username = "Guest" };
        IsGuest = true;
    }

    public static void Login(AccountData account)
    {
        CurrentAccount = account;
        IsGuest = false;
    }

    public static bool Register(string username, string password)
    {
        var path = GetAccountPath(username);
        if (File.Exists(path))
            return false;

        var salt = PasswordHasher.GenerateSalt();
        var hash = PasswordHasher.HashPassword(password, salt);

        var account = new AccountData
        {
            Username = username,
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordHash = Convert.ToBase64String(hash),
            Coins = 0,
            HighScore = 0,
            SelectedSkin = "Classic",
            UnlockedSkins = ["Classic"],
            PowerUpLevels = [1, 1, 1], /*Jetpack, Score, Shield*/
            WindowWidth = DefaultWindowWidth,
            WindowHeight = DefaultWindowHeight,
            WindowPositionX = DefaultCenterX,
            WindowPositionY = DefaultCenterY
        };

        SaveAccount(account);
        CurrentAccount = account;
        return true;
    }

    public static AccountData? Login(string username, string password)
    {
        var path = GetAccountPath(username);
        if (!File.Exists(path))
            return null;

        var account = JsonSerializer.Deserialize<AccountData>(File.ReadAllText(path));
        if (account == null) return null;

        var salt = Convert.FromBase64String(account.PasswordSalt);
        var expectedHash = Convert.FromBase64String(account.PasswordHash);

        return PasswordHasher.VerifyPassword(password, salt, expectedHash) ? account : null;
    }

    public static void SaveAccount(AccountData account)
    {
        var path = GetAccountPath(account.Username);
        var json = JsonSerializer.Serialize(account, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private static string GetAccountPath(string username)
    {
        return Path.Combine(AccountsFolder, $"{username}.json");
    }

    public static AccountData? TryAutoLogin(string username)
    {
        var path = GetAccountPath(username);
        if (!File.Exists(path))
            return null;

        var account = JsonSerializer.Deserialize<AccountData>(File.ReadAllText(path));
        return account;
    }

    public static void ClearAccountData(AccountData account)
    {
        account.Coins = 0;
        account.HighScore = 0;
        account.WindowWidth = DefaultWindowWidth;
        account.WindowHeight = DefaultWindowHeight;
        account.WindowPositionX = DefaultCenterX;
        account.WindowPositionY = DefaultCenterY;
        SaveAccount(account);
    }

    public static void DeleteAccount(AccountData account)
    {
        var path = GetAccountPath(account.Username);
        if (File.Exists(path)) File.Delete(path);
        CurrentAccount = null;
        IsGuest = false;
    }

    public static void LoadAllAccounts()
    {
        Accounts.Clear();

        var folderPath = Path.Combine(AppDataRoot, "Accounts");
        if (!Directory.Exists(folderPath))
            return;

        foreach (var file in Directory.GetFiles(folderPath, "*.json"))
            try
            {
                var json = File.ReadAllText(file);
                var account = JsonSerializer.Deserialize<AccountData>(json);
                if (account != null)
                    Accounts.Add(account);
            }
            catch
            {
                //skip
            }
    }
}