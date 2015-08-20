namespace FligServer
{
    public interface IFileService
    {
        void CreateFile(string filename, string content);
        bool CheckExists(string filename);
        bool RemoveLock(string filename);
    }
}