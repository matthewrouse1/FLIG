using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using FligClient.Annotations;
using FligClient.CommitViewModel;
using FligClient.FileBrowsing;
using FligClient.Git;
using FligClient.SettingsViewModel;

namespace FligClient.MasterViewModel
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        private LockedFilesViewModel _lockedFilesViewModel;
        private FileAndFolderBrowserViewModel _fileAndFolderBrowserViewModel;
        private GitViewModel _gitViewModel;

        public MasterViewModel() : this(new GitViewModel(), new FileAndFolderBrowserViewModel(), new LockedFilesViewModel())
        { }

        public MasterViewModel(GitViewModel gitViewModel, FileAndFolderBrowserViewModel fileAndFolderBrowserViewModel, LockedFilesViewModel lockedFilesViewModel)
        {
            _gitViewModel = gitViewModel;
            _lockedFilesViewModel = lockedFilesViewModel;
            _fileAndFolderBrowserViewModel = fileAndFolderBrowserViewModel;

            // Used to refresh the files/folders that exist on the serevr after git has pulled/pushed new ones
            //new Task(() =>
            //{
            //    while (true)
            //    {
            //        var selectedFolder = SelectedFolder;
            //        _fileAndFolderBrowserViewModel.UpdateFilesAndFolders();
            //        SelectedFolder = selectedFolder;
            //        OnPropertyChanged(nameof(FolderList));
            //        System.Threading.Thread.Sleep(10000);
            //    }
            //}).Start();
        }

        public ICommand EditCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedItemsList == null || SelectedItemsList.Count == 0)
                        return;
                    new Task(() =>
                    {
                        Process.Start(((File) SelectedItemsList[0]).Name);
                    }).Start();
                });
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _fileAndFolderBrowserViewModel.RefreshFolders();
                    OnPropertyChanged(nameof(FolderList));
                    SelectedFolder = SelectedFolder;
                });
            }
        }

        public ICommand UndoCheckoutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var result = MessageBox.Show("Are sure you want to discard changes to all selected files",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        _gitViewModel.ResetFiles(SelectedItemsList);
                    }
                });
            }
        }

        public ICommand PullCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _gitViewModel.Pull();
                });
            }
        }

        public ICommand CheckoutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _gitViewModel.Pull();

                    foreach (var file in SelectedItemsList)
                    {
                        _lockedFilesViewModel.CurrentFile = ((File)file).Name;
                        _lockedFilesViewModel.CheckoutCommand.Execute(null);
                    }
                    OnPropertyChanged(nameof(FileList));
                });
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var settingsView = new SettingsView();
                    settingsView.ShowDialog();
                });
            }
        }

        public ICommand CheckinCommand
        {
             get
            {
                return new DelegateCommand(() =>
                {
                    var commitView = new CommitView(_gitViewModel, SelectedItemsList);
                    commitView.ShowDialog();

                    foreach (var file in SelectedItemsList)
                    {
                        _lockedFilesViewModel.CurrentFile = ((File)file).Name;
                        _lockedFilesViewModel.UnlockFileCommand.Execute(null);
                    }
                    OnPropertyChanged(nameof(FileList));
                });
            }           
        }

        public IEnumerable<Folder> FolderList => _fileAndFolderBrowserViewModel.FolderList;

        public IEnumerable<File> FileList => DecorateFileDetails();

        private IEnumerable<File> DecorateFileDetails()
        {
            var files = _fileAndFolderBrowserViewModel.FileList.ToList();

            foreach (var file in files)
            {
                _lockedFilesViewModel.CurrentFile = file.Name;
                _lockedFilesViewModel.GetStatus();

                var lockedBy = string.Empty;
                lockedBy = _lockedFilesViewModel.CurrentFileLockInfo.Locks.Aggregate(lockedBy, (current, lockUser) => string.Format("{0} {1}", current, lockUser.Username));

                file.LockedOutBy = lockedBy;
            }

            return files;
        }

        private IList _selectedItems = new ArrayList();

        public IList SelectedItemsList
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItemsList));
            }
        }

        public Folder SelectedFolder
        {
            get { return _fileAndFolderBrowserViewModel.SelectedFolder; }
            set
            {
                _fileAndFolderBrowserViewModel.SelectedFolder = value;
                OnPropertyChanged(nameof(FileList));
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
