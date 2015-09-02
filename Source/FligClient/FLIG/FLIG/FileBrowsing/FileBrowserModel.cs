using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FligClient.FileBrowsing
{
    public class FileBrowserModel : IFileBrowserModel
    {
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
