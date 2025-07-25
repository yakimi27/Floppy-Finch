﻿using System.Globalization;
using System.Windows;
using FloppyFinchLogics.AccountManagement;
using FloppyFinchWindows.Authentication;
using FloppyFinchWindows.Menus;
using FloppyFinchWindows.Resources;

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

                var culture = new CultureInfo(AccountManager.CurrentAccount.SelectedLanguage);
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                new MainMenuWindow().Show();
                return;
            }

            SessionManager.ClearSession();
        }

        new SignInWindow().Show();
    }

    public static void ChangeLanguage(string languageCode)
    {
        var culture = new CultureInfo(languageCode);

        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        Strings.Culture = culture;
    }
}