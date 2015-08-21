using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FligServer
{
    public class FileLockingService : ILockingService
    {
        public FileLockingService()
        {
            Directory.SetCurrentDirectory(@"C:\flig");
        }

        public void CreateLock(string filename, List<string> content)
        {
            File.WriteAllLines(filename, content.ToArray());
        }

        public bool DoesLockExist(string filename)
        {
            return File.Exists(filename);
        }

        public bool RemoveLock(string filename, string user)
        {
            var fileContents = File.ReadAllLines(filename).ToList();
            fileContents.RemoveAll(x => GetLockObjectFromString(x).Username == user);

            File.Delete(filename);

            if (fileContents.Count > 0)
            {
                CreateLock(filename, fileContents);
            }

            return !DoesLockExist(filename);
        }

        private LockObject GetLockObjectFromString(string lineToParse)
        {
            var split = lineToParse.Split('|');
            if (split.Length >= 2)
            {
                return new LockObject() {Username = split[0], LockedDateTime = DateTime.Parse(split[1])};
            }

            return new LockObject() {Username = lineToParse};
        }

        public List<LockObject> RetrieveLockInfo(string filename)
        {
            var lockObjects = new List<LockObject>();
            foreach (var line in File.ReadAllLines(filename))
            {
                lockObjects.Add(GetLockObjectFromString(line));
            }
            return lockObjects;
        }

        public void CreateLockOverride(string filename, string user)
        {
            var fileContents = new List<string>() {string.Format("{0}|{1}", user, DateTime.Now.ToString())};
            if (!DoesLockExist(filename))
                CreateLock(filename, fileContents);
            else
            {
                UpdateLock(filename, user);
            }
        }

        private void UpdateLock(string filename, string user)
        {
            var info = RetrieveLockInfo(filename);
            var newData = new string[info.Count() + 1];
            for (int i = 0; i < 255 && i < info.Count(); i++)
            {
                newData[i] = string.Format("{0}|{1}\n", info[i].Username, info[i].LockedDateTime);
            }
            newData[newData.Length - 1] = string.Format("{0}|{1}", user, DateTime.Now.ToString());
            File.WriteAllLines(filename, newData);
        }
    }
}