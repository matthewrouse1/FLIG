using System;
using System.Dynamic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
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

        public static string EmailAddress
        {
            get { return SettingsProvider.Get(nameof(EmailAddress)); }
            set { SettingsProvider.Set(nameof(EmailAddress), value); }
        }

        private static byte[] NewEntropy()
        {
            var Entropy = new byte[16];
            SettingsProvider.Set("Entropy", Convert.ToBase64String(Entropy));
            return Entropy;
        }

        private static byte[] CheckEntropy()
        {
            var entropy = new byte[16];
            if (string.IsNullOrEmpty(SettingsProvider.Get("Entropy")))
            {
                entropy = NewEntropy();
            }
            else
            {
                entropy = Convert.FromBase64String(SettingsProvider.Get("Entropy"));
            }

            return entropy;
        }


        public static SecureString Password
        {
            get
            {
                var entropy = CheckEntropy();

                var secure = new SecureString();

                if (string.IsNullOrEmpty(SettingsProvider.Get("Password")))
                    return secure;

                foreach (var c in Encoding.UTF8.GetString((ProtectedData.Unprotect(Convert.FromBase64String(SettingsProvider.Get("Password")), entropy, DataProtectionScope.CurrentUser))))
                {
                    secure.AppendChar(c);
                }
                return secure;
            }
        }

        public static void SetPassword(string value)
        {
            var entropy = CheckEntropy();

            var byteString = Encoding.UTF8.GetBytes(value);

            SettingsProvider.Set("Password", Convert.ToBase64String(ProtectedData.Protect(byteString, entropy, DataProtectionScope.CurrentUser)));
        }

        public static string RepoUrl
        {
            get { return SettingsProvider.Get(nameof(RepoUrl)); }
            set { SettingsProvider.Set(nameof(RepoUrl), value); }
        }
    }
}