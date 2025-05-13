using System.IO;
using System.Text.Json;

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
            HighScore = 0
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
}