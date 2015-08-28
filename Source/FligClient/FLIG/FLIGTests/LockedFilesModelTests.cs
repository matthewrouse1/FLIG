using FligClient;
using Xunit;
using Moq;
using RestSharp;

namespace GivenARequest
{
    public class WhentheRequestIsValid
    {
        private string aFakeFile;
        private Mock<IRestClient> moqRestClient;
        private LockedFilesModel lockedFileModel;

        public WhentheRequestIsValid()
        {
            aFakeFile = "testFile.txt";
            moqRestClient = new Mock<IRestClient>();
            lockedFileModel = new LockedFilesModel(moqRestClient.Object);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Completed });
            Assert.True(lockedFileModel.LockFile(aFakeFile));
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Completed });
            Assert.True(lockedFileModel.OverrideLockOnFile(aFakeFile));            
        }
    }

    public class WhenTheRequestIsInvalid
    {
        private string aFakeFile;
        private Mock<IRestClient> moqRestClient;
        private LockedFilesModel lockedFileModel;

        public WhenTheRequestIsInvalid()
        {
            aFakeFile = "testFile.txt";
            moqRestClient = new Mock<IRestClient>();
            lockedFileModel = new LockedFilesModel(moqRestClient.Object);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsFalse()
        {
             moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() {ResponseStatus = ResponseStatus.Error});
            Assert.False(lockedFileModel.LockFile(aFakeFile));           
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Error });
            Assert.False(lockedFileModel.OverrideLockOnFile(aFakeFile));
        }
    }
}
