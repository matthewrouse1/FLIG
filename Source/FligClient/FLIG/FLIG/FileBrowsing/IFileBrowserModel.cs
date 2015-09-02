namespace FligClient.FileBrowsing
{
    public interface IFileBrowserModel
    {
        Folder AddFolderRecursive(Folder startingFolder);
    }
}