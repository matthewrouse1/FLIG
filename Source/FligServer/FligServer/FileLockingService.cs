using System;
using System.Collections.Generic;
using System.IO;

namespace FligServer
{
    public class FileLockingService : ILockingService
    {
        public FileLockingService()
        {
            Directory.SetCurrentDirectory(@"C:\flig");
        }

        public void CreateLock(string filename, string content)
        {
            var array = new string[1];
            array[0] = content;
            File.WriteAllLines(filename, array);
        }

        public bool DoesLockExist(string filename)
        {
            return File.Exists(filename);
        }

        public bool RemoveLock(string filename)
        {
            File.Delete(filename);
            return !DoesLockExist(filename);
        }

        public List<LockObject> RetrieveLockInfo(string filename)
        {
            var lockObjects = new List<LockObject>();
            foreach (var line in File.ReadAllLines(filename))
            {
                var split = line.Split(':');
                if (split.Length >= 2)
                    lockObjects.Add(new LockObject() {Username = split[0], LockedDateTime = DateTime.Parse(split[1])} );
                else
                    lockObjects.Add(new LockObject() { Username = line } );
            }
            return lockObjects;
        }
    }
}