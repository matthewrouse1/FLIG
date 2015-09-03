using FligClient.Providers;

namespace FligClient
{
    public class UserInfo
    {
        public string Username
        {
            get { return SettingsProvider.Get(nameof(Username)); }
            set { SettingsProvider.Set(nameof(Username), value); }
        }

        public string RepoDir
        {
            get { return SettingsProvider.Get(nameof(RepoDir)); }
            set { SettingsProvider.Set(nameof(RepoDir), value); }
        }

        public string WebApiPath
        {
            get { return SettingsProvider.Get(nameof(WebApiPath)); }
            set { SettingsProvider.Set(nameof(WebApiPath), value); }
        }
    }
}