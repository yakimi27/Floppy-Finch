using System.IO;
using System.Text.Json;

namespace FloppyFinchLogics.AccountManagement;

public static class SessionManager
{
    private static readonly string SessionFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FloppyFinch", "session.json");

    public static void SaveSession(SessionData session)
    {
        var json = JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SessionFilePath, json);
    }

    public static SessionData LoadSession()
    {
        if (!File.Exists(SessionFilePath))
            return new SessionData();

        var json = File.ReadAllText(SessionFilePath);
        return JsonSerializer.Deserialize<SessionData>(json) ?? new SessionData();
    }

    public static void ClearSession()
    {
        if (File.Exists(SessionFilePath))
            File.Delete(SessionFilePath);
    }
}