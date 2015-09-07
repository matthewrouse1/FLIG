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
using System.Windows.Documents;
using System.Windows.Input;
using FligClient.Annotations;
using FligClient.FileBrowsing;
using FligClient.Git;

namespace FligClient.MasterViewModel
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        private LockedFilesViewModel _lockedFilesViewModel;
        private FileAndFolderBrowserViewModel _fileAndFolderBrowserViewModel;
        private GitViewModel _gitViewModel;

        public MasterViewModel() : this(new FileAndFolderBrowserViewModel(), new LockedFilesViewModel(), new GitViewModel())
        { }

        public MasterViewModel(FileAndFolderBrowserViewModel fileAndFolderBrowserViewModel, LockedFilesViewModel lockedFilesViewModel, GitViewModel gitViewModel)
        {
            _lockedFilesViewModel = lockedFilesViewModel;
            _fileAndFolderBrowserViewModel = fileAndFolderBrowserViewModel;
            _gitViewModel = gitViewModel;

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

        public ICommand UndoCheckoutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    
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

        public ICommand CheckinCommand
        {
             get
            {
                return new DelegateCommand(() =>
                {
                    _gitViewModel.FilesToStage = (from object file in SelectedItemsList select ((File)file).Name).ToList();
                    _gitViewModel.Add();
                    _gitViewModel.CommitMessage = "Test Commit Message";
                    _gitViewModel.Commit();
                    _gitViewModel.Pull();
                    _gitViewModel.Push();

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
