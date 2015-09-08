using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FligClient.Annotations;
using FligClient.Providers;

namespace FligClient.SettingsViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            Username = UserInfo.Username;
            EmailAddress = UserInfo.EmailAddress;
            WebApiPath = UserInfo.WebApiPath;
            RepoDir = UserInfo.RepoDir;
            RepoUrl = UserInfo.RepoUrl;
        }

        private string username { get; set; }
        private string password { get; set; }
        private string emailaddress { get; set; }
        private string webapiurl { get; set; }
        private string repodir { get; set; }
        private string repourl { get; set; }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        // Needs a "has changed" flag
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(Password);
            }
        }

        public string EmailAddress
        {
            get { return emailaddress;  }
            set
            {
                emailaddress = value;
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        public string WebApiPath
        {
            get { return webapiurl; }
            set
            {
                webapiurl = value;
                OnPropertyChanged(nameof(WebApiPath));
            }
        }

        public string RepoDir
        {
            get { return repodir; }
            set
            {
                repodir = value;
                OnPropertyChanged(nameof(RepoDir));
            }
        }

        public string RepoUrl
        {
            get { return repourl; }
            set
            {
                repourl = value;
                OnPropertyChanged(nameof(RepoUrl));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand((() =>
                {
                    UserInfo.Username = Username;
                    if (!string.IsNullOrEmpty(Password))
                        UserInfo.SetPassword(Password);
                    UserInfo.EmailAddress = EmailAddress;
                    UserInfo.RepoDir = RepoDir;
                    UserInfo.RepoUrl = RepoUrl;
                    UserInfo.WebApiPath = WebApiPath;
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
