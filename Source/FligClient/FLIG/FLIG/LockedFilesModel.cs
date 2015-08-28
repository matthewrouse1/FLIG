using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FligClient
{
    public class LockedFilesModel : ILockedfilesModel
    {
        public bool LockFile(string filename)
        {
            return true;
        }

        public bool OverrideLockOnFile(string filename)
        {
            return true;
        }

        public LockedFileInfo CheckLockOnFile(string filename)
        {
            return new LockedFileInfo();
        }

        public bool UnlockFile(string filename)
        {
            return true;
        }
    }
}
