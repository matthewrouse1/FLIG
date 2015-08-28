using System;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FligClient
{
    public class LockedFilesViewModel
    {
        private ILockedfilesModel _lockedfilesModel;

        public LockedFilesViewModel() : this(new LockedFilesModel())
        {
        }

        public LockedFilesViewModel(ILockedfilesModel lockedfilesModel)
        {
            this._lockedfilesModel = lockedfilesModel;
        }

        public string CurrentFile;

        public ICommand CheckoutCommand
        {
            get
            {
                if (checkoutFileCommand == null)
                    checkoutFileCommand = new DelegateCommand(new Action(CheckoutFile), new Func<bool>(CanCheckoutFile));

                return checkoutFileCommand;
            }
        }

        private DelegateCommand checkoutFileCommand;

        public bool CanCheckoutFile()
        {
            return _lockedfilesModel.CheckLockOnFile(CurrentFile).HasLock;
        }

        public void CheckoutFile()
        {
            _lockedfilesModel.LockFile(CurrentFile);
        }


    }
}