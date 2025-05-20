using System.Globalization;
using System.Windows;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchWindows.Authentication;
using FloppyFinchWindows.Menus;

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

        // Set both UI and regular culture
        var culture = new CultureInfo(AccountManager.CurrentAccount.SelectedLanguage);
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;

        // For WPF, also set these to ensure proper resource loading
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public static void ChangeLanguage(string languageCode)
    {
        var culture = new CultureInfo(languageCode);
        
        // Update cultures
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        
        // Update the culture of the resource manager
        FloppyFinchWindows.Resources.Strings.Culture = culture;
    }
}