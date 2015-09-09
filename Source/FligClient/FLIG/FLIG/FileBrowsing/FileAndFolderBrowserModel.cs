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
            get
            {
                return new Collection<FligFolder>()
                {
                    AddFolderRecursive(new FligFolder() {Name = "Home", Path = UserInfo.RepoDir, IsExpanded = true})
                };
            }
        }

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

                if (ExpandedStates.ContainsKey(newFolder.Path))
                    newFolder.IsExpanded = ExpandedStates[newFolder.Path];

                if (SelectedStates.ContainsKey(newFolder.Path))
                    newFolder.IsSelected = SelectedStates[newFolder.Path];

                startingFolder.Children.Add(AddFolders(newFolder));
            }

            return startingFolder;
        }

        public FileAndFolderBrowserModel()
        {
            ExpandedStates = new Dictionary<string, bool>();
            SelectedStates = new Dictionary<string, bool>();
        }
    }
}
