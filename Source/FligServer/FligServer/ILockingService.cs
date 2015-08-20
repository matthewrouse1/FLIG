namespace FligServer
{
    public interface ILockingService
    {
        void CreateLock(string filename, string content);
        bool DoesLockExist(string filename);
        bool RemoveLock(string filename);
    }
}