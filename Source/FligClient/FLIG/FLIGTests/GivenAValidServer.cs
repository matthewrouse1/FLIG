using FligClient;
using Xunit;

namespace FLIGTests.GivenAFileNeedsToBeChanges
{
    public class WhenTheFileIsntLocked
    {
        private LockedFilesViewModel lockedFilesViewModel;
        private string fakeFileName = "testfile.txt";

        public WhenTheFileIsntLocked()
        {
            lockedFilesViewModel = new LockedFilesViewModel(new FakeLockedFilesModel());
            lockedFilesViewModel.CurrentFile = fakeFileName;
            lockedFilesViewModel.CheckoutFile();
        }

        [Fact]
        public void ThenFileIsCheckedOut()
        {
            // Cannot checkout the file now, because it is locked
            Assert.True(!lockedFilesViewModel.CanCheckoutFile());
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
