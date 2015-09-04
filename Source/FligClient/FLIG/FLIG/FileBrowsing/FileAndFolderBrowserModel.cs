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
        public Collection<Folder> FolderList
        {
            get
            {
                return new Collection<Folder>()
                {
                    AddFolderRecursive(new Folder() {Name = "Home", Path = UserInfo.RepoDir})
                };
            }
        }

        public Collection<File> FileList
        {
            get
            {
                var fileCollection = new Collection<File>();

                foreach (var file in Directory.GetFiles(CurrentlySelectedPath ?? UserInfo.RepoDir))
                {
                    fileCollection.Add(new File() { Name = file} );
                }

                return fileCollection;
            }
        }

        public string CurrentlySelectedPath { get; set; }

        public Folder AddFolderRecursive(Folder startingFolder) => AddFolders(startingFolder);

        private Folder AddFolders(Folder startingFolder)
        {
            if (string.IsNullOrEmpty(startingFolder.Path))
                return startingFolder;

            foreach (var directory in Directory.GetDirectories(startingFolder.Path))
            {
                var newFolder = new Folder()
                {
                    Name = directory.Substring(directory.LastIndexOf(@"\") + 1),
                    Path = directory
                };
                startingFolder.Children.Add(AddFolders(newFolder));
            }

            return startingFolder;
        }
    }
}
