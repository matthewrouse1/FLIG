using System.Collections.ObjectModel;

namespace FligClient.FileBrowsing
{
    public interface IFileAndFolderBrowserModel
    {
        Folder AddFolderRecursive(Folder startingFolder);
        Collection<Folder> FolderList { get; }
        Collection<File> FileList { get; }
        string CurrentlySelectedPath { get; set; }
    }
}