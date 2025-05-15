using System.IO;
using System.Text.Json;
using System.Windows;

namespace FloppyFinchLogics.AccountManagement;

public static class AccountManager
{
    private static readonly string AppDataRoot = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FloppyFinch");

    private static readonly string AccountsFolder = Path.Combine(AppDataRoot, "Accounts");


    static AccountManager()
    {
        Directory.CreateDirectory(AccountsFolder);
    }

    public static AccountData? CurrentAccount { get; set; }
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

        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;

        var windowWidth = 600;
        var windowHeight = 400;

        var centerX = (int)((screenWidth - windowWidth) / 2);
        var centerY = (int)((screenHeight - windowHeight) / 2);

        var account = new AccountData
        {
            Username = username,
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordHash = Convert.ToBase64String(hash),
            Coins = 0,
            HighScore = 0,
            WindowWidth = windowWidth,
            WindowHeight = windowHeight,
            WindowPositionX = centerX,
            WindowPositionY = centerY
        };

        SaveAccount(account);
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
}