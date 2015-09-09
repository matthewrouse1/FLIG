using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FligClient.Annotations;
using FligClient.FileBrowsing;
using FligClient.Git;

namespace FligClient.CommitViewModel
{
    public class CommitViewModel : INotifyPropertyChanged
    {
        public GitViewModel _gitViewModel { get; set; }
        public IList _selectedFiles { get; set; }

        public CommitViewModel()
        {
            commitMessage = "Enter Commit Message here:";
        }

        public Action CloseAction { get; set; }

        public ICommand CommitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _gitViewModel.FilesToStage = (from object file in _selectedFiles select ((FligFile)file).Name).ToList();
                    _gitViewModel.Add();
                    _gitViewModel.CommitMessage = commitMessage;
                    _gitViewModel.Commit();
                    _gitViewModel.Pull();
                    _gitViewModel.Push();
                    CloseAction();
                });
            }
        }

        private string commitMessage { get; set; }
        public string CommitMessage {
            get { return commitMessage; }
            set
            {
                commitMessage = value;
                OnPropertyChanged(nameof(commitMessage));
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
