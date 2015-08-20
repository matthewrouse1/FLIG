namespace FligServer
{
    public interface ILockingService
    {
        void CreateFile(string filename, string content);
        bool CheckExists(string filename);
        bool RemoveLock(string filename);
    }
}