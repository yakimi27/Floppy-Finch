using System.Collections.ObjectModel;

namespace FloppyFinchLogics.AccountManagement;

public class LeaderboardManager
{
    public LeaderboardManager()
    {
        var accounts = AccountManager.Accounts
            .OrderByDescending(a => a.HighScore)
            .ToList();
        if (accounts.Count > 0)
        {
            FirstPlacePlayer = accounts[0].Username;
            FirstPlaceScore = accounts[0].HighScore;

            OtherPlaces = new ObservableCollection<LeaderboardData>(
                accounts.Skip(1).Select((a, i) => new LeaderboardData
                {
                    Position = i + 2,
                    PlayerName = a.Username,
                    Score = a.HighScore
                }));
        }
        else
        {
            FirstPlacePlayer = "No players";
            FirstPlaceScore = 0;
            OtherPlaces = new ObservableCollection<LeaderboardData>();
        }
    }

    public string FirstPlacePlayer { get; set; }
    public int FirstPlaceScore { get; set; }
    public ObservableCollection<LeaderboardData> OtherPlaces { get; set; }
}