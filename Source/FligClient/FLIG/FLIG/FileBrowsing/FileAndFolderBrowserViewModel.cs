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
        private IEnumerable<FligFolder> folderList;
        private IFileAndFolderBrowserModel _fileAndFolderBrowserModel;

        public ObservableCollection<FligFolder> FolderList 
        {
            get
            {
                if (folderList == null)
                    return new ObservableCollection<FligFolder>();
                return new ObservableCollection<FligFolder>(folderList);
            }
            set
            {
                folderList = value;
                OnPropertyChanged(nameof(FolderList));
            }
        }

        private FligFolder selectedFolder;

        public FligFolder SelectedFolder
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

        private IEnumerable<FligFile> fileList;

        public IEnumerable<FligFile> FileList
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

        public void RefreshFolders()
        {
            if (FolderList == null || FolderList.Count == 0)
                return;

            _fileAndFolderBrowserModel.RefreshFolders(FolderList[0]);
            //foreach (var folder in FolderList)
            //{
            //    if (_fileAndFolderBrowserModel.ExpandedStates.ContainsKey(folder.Path))
            //    {
            //        _fileAndFolderBrowserModel.ExpandedStates[folder.Path] = folder.IsExpanded;
            //    }
            //    else
            //    {
            //        _fileAndFolderBrowserModel.ExpandedStates.Add(folder.Path, folder.IsExpanded);
            //    }

            //    if (_fileAndFolderBrowserModel.SelectedStates.ContainsKey(folder.Path))
            //    {
            //        _fileAndFolderBrowserModel.SelectedStates[folder.Path] = folder.IsSelected;
            //    }
            //    else
            //    {
            //        _fileAndFolderBrowserModel.SelectedStates.Add(folder.Path, folder.IsSelected);
            //    }
            //}
            FolderList = new ObservableCollection<FligFolder>(_fileAndFolderBrowserModel.FolderList);
        }

        public FileAndFolderBrowserViewModel(IFileAndFolderBrowserModel fileAndFolderBrowserModel)
        {
            _fileAndFolderBrowserModel = fileAndFolderBrowserModel;

            FolderList = new ObservableCollection<FligFolder>(_fileAndFolderBrowserModel.FolderList);

            FileList = new Collection<FligFile>();
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
            FolderList = new ObservableCollection<FligFolder>(_fileAndFolderBrowserModel.FolderList);
        }
    }

    public class FligFolder
    {
        public FligFolder()
        {
            Children = new List<FligFolder>();
        }

        public string Path { get; set; }

        public string Name { get; set; }

        public List<FligFolder> Children { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
    }

    public class FligFile
    {
        public string Name { get; set; }

        public string LockedOutBy { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
    }
}
