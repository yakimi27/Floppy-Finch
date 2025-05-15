using System.Windows;
using FloppyFinchGameModes.Menus;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchWindows.Authentication;

namespace FloppyFinchWindows;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var session = SessionManager.LoadSession();

        if (session.RememberMe)
        {
            var account = AccountManager.TryAutoLogin(session.LastUsername!);
            if (account != null)
            {
                AccountManager.CurrentAccount = account;
                new MainMenuWindow().Show();
                return;
            }

            SessionManager.ClearSession();
        }

        new SignInWindow().Show();
    }
}