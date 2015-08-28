using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using FligClient;
using Xunit;
using Moq;
using RestSharp;

namespace GivenARequest
{
    public class WhentheRequestIsValid
    {
        private string aFakeFile;
        private LockedFilesModel lockedFileModel;
        private UserInfo userInfo;

        public WhentheRequestIsValid()
        {
            userInfo = new UserInfo();
            userInfo.Username = "matt";
            aFakeFile = "testFile.txt";
            lockedFileModel = new LockedFilesModel(TestHelper.SetupIRestClientMock(ResponseStatus.Completed, aFakeFile).Object, userInfo);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsTrue()
        {
            Assert.True(lockedFileModel.LockFile(aFakeFile));
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            Assert.True(lockedFileModel.OverrideLockOnFile(aFakeFile));            
        }

        [Fact]
        public void ThenCheckTheUnlockResponseIsTrue()
        {
            Assert.True(lockedFileModel.UnlockFile(aFakeFile));
        }
    }

    public class WhenTheRequestIsInvalid
    {
        private string aFakeFile;
        private LockedFilesModel lockedFileModel;
        private UserInfo userInfo;

        public WhenTheRequestIsInvalid()
        {
            userInfo = new UserInfo();
            userInfo.Username = "matt";
            aFakeFile = "testFile.txt";
            lockedFileModel = new LockedFilesModel(TestHelper.SetupIRestClientMock(ResponseStatus.Error, aFakeFile).Object, userInfo);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsFalse()
        {
            Assert.False(lockedFileModel.LockFile(aFakeFile));           
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            Assert.False(lockedFileModel.OverrideLockOnFile(aFakeFile));
        }

        [Fact]
        public void ThenCheckTheOverrideUnloockFileResponseIsTrue()
        {
            Assert.False(lockedFileModel.UnlockFile(aFakeFile));
        }
    }

    public static class TestHelper
    {
        public static Mock<IRestClient> SetupIRestClientMock(ResponseStatus responsesStatus, string filename)
        {
            var moqRestClient = new Mock<IRestClient>();
            moqRestClient.Setup(x => x.Execute(It.Is<IRestRequest>
                (p => p.Parameters.Exists(y => y.Name == "user")
                && p.Parameters.Exists(y => y.Name == "file")
                && p.Parameters.Exists(y => y.Value == "matt")
                && p.Parameters.Exists(y => y.Value == filename))
                ))
                .Returns(new RestResponse() { ResponseStatus = responsesStatus });
            return moqRestClient;
        }
    }
}
