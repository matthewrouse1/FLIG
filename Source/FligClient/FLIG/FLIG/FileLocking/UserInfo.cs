using FligClient.Providers;

namespace FligClient
{
    public static class UserInfo
    {
        public static string Username
        {
            get { return SettingsProvider.Get(nameof(Username)); }
            set { SettingsProvider.Set(nameof(Username), value); }
        }

        public static string RepoDir
        {
            get { return SettingsProvider.Get(nameof(RepoDir)); }
            set { SettingsProvider.Set(nameof(RepoDir), value); }
        }

        public static string WebApiPath
        {
            get { return SettingsProvider.Get(nameof(WebApiPath)); }
            set { SettingsProvider.Set(nameof(WebApiPath), value); }
        }
    }
}