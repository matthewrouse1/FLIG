using System;
using System.Collections.Generic;
using System.Linq;

namespace FligClient
{
    public class LockedFileInfo
    {
        public bool HasLock => Locks.Any();
    
        public List<LockObject> Locks { get; set; }
    }
}