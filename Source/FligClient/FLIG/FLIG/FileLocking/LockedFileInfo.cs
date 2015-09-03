using System;
using System.Collections.Generic;
using System.Linq;

namespace FligClient
{
    public class LockedFileInfo
    {
        public LockedFileInfo()
        {
            Locks = new List<LockObject>();
        }

        public bool HasLock
        {
            get
            {
                if (Locks == null || Locks.Count == 0)
                    return false;
                return true;
            }
        }

        private List<LockObject> locks;

        public List<LockObject> Locks
        {
            get { return locks; }
            set
            {
                if (value != null)
                    locks = value;
            }
        }
    }
}