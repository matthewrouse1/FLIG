using System;
using System.Windows;
using FligClient;

namespace FLIG
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (string.IsNullOrEmpty(UserInfo.RepoDir) || string.IsNullOrEmpty(UserInfo.RepoUrl))
            {
                StartupUri = new Uri("/FLIG;component/SettingsViewModel/SettingsView.xaml", UriKind.Relative);
            }
            else
            {
                StartupUri = new Uri("/FLIG;component/MainWindow.xaml", UriKind.Relative);
            }
        }
    }
}
