using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FligClient.Annotations;
using FligClient.FileBrowsing;

namespace FligClient.MasterViewModel
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        private LockedFilesViewModel _lockedFilesViewModel;
        private FileAndFolderBrowserViewModel _fileAndFolderBrowserViewModel;

        public MasterViewModel() : this(new FileAndFolderBrowserViewModel(), new LockedFilesViewModel())
        { }

        public MasterViewModel(FileAndFolderBrowserViewModel fileAndFolderBrowserViewModel, LockedFilesViewModel lockedFilesViewModel)
        {
            _lockedFilesViewModel = lockedFilesViewModel;
            _fileAndFolderBrowserViewModel = fileAndFolderBrowserViewModel;
        }

        public IEnumerable<Folder> FolderList => _fileAndFolderBrowserViewModel.FolderList;

        public IEnumerable<File> FileList => DecorateFileDetails();

        private IEnumerable<File> DecorateFileDetails()
        {
            if (_fileAndFolderBrowserViewModel.FileList == null)
                return new Collection<File>();

            var files = _fileAndFolderBrowserViewModel.FileList;

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
