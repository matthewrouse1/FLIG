using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FligClient.FileBrowsing
{
    public class FileAndFolderBrowserModel : IFileAndFolderBrowserModel
    { 
        public Collection<FligFolder> FolderList
        {
            get { return folderList; }
        }

        public Collection<FligFolder> folderList { get; set; } 

        public Collection<FligFile> FileList
        {
            get
            {
                var fileCollection = new Collection<FligFile>();

                foreach (var file in Directory.GetFiles(CurrentlySelectedPath ?? UserInfo.RepoDir))
                {
                    fileCollection.Add(new FligFile() { Name = file} );
                }

                return fileCollection;
            }
        }

        public Dictionary<string, bool> ExpandedStates { get; set; }
        public Dictionary<string, bool> SelectedStates { get; set; } 

        public string CurrentlySelectedPath { get; set; }

        public FligFolder AddFolderRecursive(FligFolder startingFolder) => AddFolders(startingFolder);

        private string GetFolderName(string Path)
        {
            return Path.Substring(Path.LastIndexOf(@"\") + 1);
        }

        private FligFolder AddFolders(FligFolder startingFolder)
        {
            if (string.IsNullOrEmpty(startingFolder.Path))
                return startingFolder;

            foreach (var directory in Directory.GetDirectories(startingFolder.Path))
            {
                var newFolder = new FligFolder()
                {
                    Name = GetFolderName(directory),
                    Path = directory,
                };

                startingFolder.Children.Add(AddFolders(newFolder));
            }

            return startingFolder;
        }

        private Collection<FligFolder> InitFolders()
        {
            return new Collection<FligFolder>()
                {
                    AddFolderRecursive(new FligFolder() {Name = "Home", Path = UserInfo.RepoDir, IsExpanded = true, IsSelected = true})
                };
        }

        private bool SearchChildrensPaths(FligFolder FolderToSearch, string PathToSearchFor)
        {
            if (FolderToSearch.Path == PathToSearchFor)
            {
                return true;
            }

            return false;
        }

        public void RefreshFolders(FligFolder startingFolder)
        {
            if (folderList == null)
                return;

            foreach (var folder in Directory.GetDirectories(startingFolder.Path))
            {
                if (SearchChildrensPaths(startingFolder, folder) == false)
                {
                    var newFolder = new FligFolder()
                    {
                        Name = GetFolderName(folder),
                        Path = folder
                    };
                    startingFolder.Children.Add(
                        AddFolders(newFolder));
                    RefreshFolders(newFolder);
                }
            }
        }

        public FileAndFolderBrowserModel()
        {
            ExpandedStates = new Dictionary<string, bool>();
            SelectedStates = new Dictionary<string, bool>();

            folderList = InitFolders();
        }
    }
}
