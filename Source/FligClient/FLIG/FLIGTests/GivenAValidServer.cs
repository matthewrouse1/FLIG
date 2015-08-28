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

    public class WhenTheFileIsLocked
    {
        private LockedFilesViewModel lockedFilesViewModel;
        private string fakeFileName = "testfile.txt";

        public WhenTheFileIsLocked()
        {
            lockedFilesViewModel = new LockedFilesViewModel(new FakeLockedFilesModel());
            lockedFilesViewModel.CurrentFile = fakeFileName;
            lockedFilesViewModel.CheckoutFile();
        }

        [Fact]
        public void ThenTheLockCanBeOverriddenIfUserHasAuthority()
        {
            lockedFilesViewModel.OverrideAuthority = true;
            Assert.True(lockedFilesViewModel.OverrideCheckoutCommand.CanExecute(null));
        }

        [Fact]
        public void ThenTheLockCantBeOverriddenIfUserDoesntHaveAuthority()
        {
            lockedFilesViewModel.OverrideAuthority = false;
            Assert.False(lockedFilesViewModel.OverrideCheckoutCommand.CanExecute(null));
        }
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
