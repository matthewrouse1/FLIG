using System;
using System.IO;

namespace FligServer
{
    public class FileLockingService : ILockingService
    {
        public void CreateFile(string filename, string content)
        {
            var array = new string[1];
            array[0] = content;
            File.WriteAllLines(filename, array);
        }

        public bool CheckExists(string filename)
        {
            return File.Exists(filename);
        }

        public bool RemoveLock(string filename)
        {
            File.Delete(filename);
            if (CheckExists(filename))
                return false;
            return true;
        }
    }
}