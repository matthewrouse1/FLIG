using FligClient;
using Xunit;

namespace FLIGTests.GivenAFileNeedsToBeChanges
{
    public class WhenTheFileIsntLocked
    {
        [Fact]
        public void ThenFileIsCheckedOut()
        {
            var fakeFileName = "testfile.txt";
            var lockedFilesViewModel = new LockedFilesModel();
            Assert.True(lockedFilesViewModel.LockFile(fakeFileName));
        }
    }

    public class FakeLockedFilesModel : ILockedfilesModel
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
