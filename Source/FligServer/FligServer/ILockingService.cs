using System;
using System.Collections.Generic;

namespace FligServer
{
    public interface ILockingService
    {
        void CreateLock(string filename, List<string> content);
        bool DoesLockExist(string filename);
        bool RemoveLock(string filename, string user);
        List<LockObject> RetrieveLockInfo(string filename);
        void CreateLockOverride(string filename, string user);
    }

    public class LockObject
    {
        public string Username { get; set; }
        public DateTime LockedDateTime { get; set; }
    }
}