using System;
using System.Collections.Generic;

namespace FligClient
{
    public class LockedFileInfo
    {
        public bool HasLock { get; set; }

        public List<Lock> Locks { get; set; }
    }

    public class Lock
    {
        public string Username { get; set; }
        public DateTime LockedDateTime { get; set; }
    }
}