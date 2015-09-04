using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FligClient.Annotations;

namespace FligClient.FileBrowsing
{
    public class FileAndFolderBrowserViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Folder> folderList;
        private IFileAndFolderBrowserModel _fileAndFolderBrowserModel;

        public IEnumerable<Folder> FolderList 
        {
            get
            {
                return folderList;
            }
            set
            {
                folderList = value;
                OnPropertyChanged(nameof(FolderList));
            }
        }

        private Folder selectedFolder;

        public Folder SelectedFolder
        {
            get
            {
                return selectedFolder;
            }
            set
            {
                if (value == null)
                    return;

                selectedFolder = value;
                _fileAndFolderBrowserModel.CurrentlySelectedPath = value.Path;
                FileList = _fileAndFolderBrowserModel.FileList;
            }
        }

        private IEnumerable<File> fileList;

        public IEnumerable<File> FileList
        {
            get
            {
                return fileList;
            }
            set
            {
                fileList = value;
                OnPropertyChanged(nameof(FileList));
            }
        }

        public FileAndFolderBrowserViewModel() : this(new FileAndFolderBrowserModel())
        {
        }

        public FileAndFolderBrowserViewModel(IFileAndFolderBrowserModel fileAndFolderBrowserModel)
        {
            _fileAndFolderBrowserModel = fileAndFolderBrowserModel;

            folderList = _fileAndFolderBrowserModel.FolderList;

            FileList = new Collection<File>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateFilesAndFolders()
        {
            FileList = _fileAndFolderBrowserModel.FileList;
            FolderList = _fileAndFolderBrowserModel.FolderList;
        }
    }

    public class Folder
    {
        public Folder()
        {
            Children = new List<Folder>();
        }

        public string Path { get; set; }

        public string Name { get; set; }

        public List<Folder> Children { get; set; }
    }

    public class File
    {
        public string Name { get; set; }

        public string LockedOutBy { get; set; }
    }
}
