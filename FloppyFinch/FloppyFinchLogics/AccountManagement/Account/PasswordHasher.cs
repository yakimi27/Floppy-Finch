using System.Security.Cryptography;
using System.Text;

namespace FloppyFinchLogics.AccountManagement;

public static class PasswordHasher
{
    public static byte[] GenerateSalt(int length = 16)
    {
        var salt = new byte[length];
        using var rand = RandomNumberGenerator.Create();
        rand.GetBytes(salt);
        return salt;
    }

    public static byte[] HashPassword(string password, byte[] salt, int iterations = 100_000, int hashLength = 32)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            hashLength);
    }

    public static bool VerifyPassword(string password, byte[] salt, byte[] expectedHash)
    {
        var hash = HashPassword(password, salt);
        return CryptographicOperations.FixedTimeEquals(hash, expectedHash);
    }
}