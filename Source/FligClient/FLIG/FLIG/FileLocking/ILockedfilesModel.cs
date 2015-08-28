namespace FligClient
{
    public interface ILockedfilesModel
    {
        bool LockFile(string filename);

        bool OverrideLockOnFile(string filename);

        LockedFileInfo CheckLockOnFile(string filename);

        bool UnlockFile(string filename);
    }
}