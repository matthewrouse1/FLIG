using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FligClient.FileBrowsing
{
    public interface IFileAndFolderBrowserModel
    {
        FligFolder AddFolderRecursive(FligFolder startingFolder);
        Collection<FligFolder> FolderList { get; }
        Collection<FligFile> FileList { get; }
        string CurrentlySelectedPath { get; set; }

        Dictionary<string, bool> ExpandedStates { get; set; }
        Dictionary<string, bool> SelectedStates { get; set; }
    }
}