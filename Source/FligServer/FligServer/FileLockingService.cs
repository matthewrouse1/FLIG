using System;
using System.IO;

namespace FligServer
{
    public class FileLockingService : ILockingService
    {
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
    }
}