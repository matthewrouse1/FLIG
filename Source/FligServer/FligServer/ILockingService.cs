using System;
using System.Collections.Generic;

namespace FligServer
{
    public interface ILockingService
    {
        void CreateLock(string filename, string content);
        bool DoesLockExist(string filename);
        bool RemoveLock(string filename);
        List<LockObject> RetrieveLockInfo(string filename);
    }

    public class LockObject
    {
        public string Username { get; set; }
        public DateTime LockedDateTime { get; set; }
    }
}