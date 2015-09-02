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
    public class FileBrowserViewModel : INotifyPropertyChanged
    {
        private Collection<Folder> folderList;
        private IFileBrowserModel _fileModel;

        public IEnumerable<Folder> FolderList => folderList;

        public FileBrowserViewModel() : this(new FileBrowserModel())
        { }

        public FileBrowserViewModel(IFileBrowserModel fileModel)
        {
            _fileModel = fileModel;

            folderList = new Collection<Folder>()
            {
                fileModel.AddFolderRecursive(new Folder() {Name = "Home", Path = @"C:\repos\alb"})
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
}
