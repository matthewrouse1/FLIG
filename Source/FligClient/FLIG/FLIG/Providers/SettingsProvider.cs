using System.Reflection;

namespace FligClient.Providers
{
    public static class SettingsProvider
    {
        public static string Get(string SettingsKey)
        {
            var prop = Properties.Settings.Default.GetType().GetProperty(SettingsKey, BindingFlags.Instance | BindingFlags.Public);
            if (prop != null && prop.CanRead)
                return prop.GetValue(Properties.Settings.Default).ToString();

            return null;
        }

        public static void Set(string SettingsKey, string SettingsValue)
        {
            var prop = Properties.Settings.Default.GetType().GetProperty(SettingsKey, BindingFlags.Instance | BindingFlags.Public);
            if (prop != null && prop.CanWrite)
                prop.SetValue(Properties.Settings.Default, SettingsValue);
            Properties.Settings.Default.Save();
        }
    }
}
