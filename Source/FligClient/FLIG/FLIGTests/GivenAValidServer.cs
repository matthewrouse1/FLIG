using Xunit;

namespace FLIGTests.GivenAValidServer
{
    public class WhenYouCheckoutAFile
    {
        [Fact]
        public void ThenFileIsCheckedOut()
        {
            var fakeFileName = "testfile.txt";
            var fligServer = new FligServer();
            fligServer.Checkout(fakeFileName);
            fligServer.CheckLockStatus(fakeFileName);
        }
    }
}
