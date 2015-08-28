using System.Linq.Expressions;
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
        private UserInfo userInfo;

        public WhentheRequestIsValid()
        {
            userInfo = new UserInfo();
            userInfo.Username = "matt";
            aFakeFile = "testFile.txt";
            moqRestClient = new Mock<IRestClient>();
            lockedFileModel = new LockedFilesModel(moqRestClient.Object, userInfo);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.Is<IRestRequest>
                (p => p.Parameters.Exists(y => y.Name == "user")
                && p.Parameters.Exists(y => y.Name == "file")
                && p.Parameters.Exists(y => y.Value == "matt")
                && p.Parameters.Exists(y => y.Value == aFakeFile))
                ))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Completed });
            Assert.True(lockedFileModel.LockFile(aFakeFile));
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.Is<IRestRequest>
                (p => p.Parameters.Exists(y => y.Name == "user")
                && p.Parameters.Exists(y => y.Name == "file")
                && p.Parameters.Exists(y => y.Value == "matt")
                && p.Parameters.Exists(y => y.Value == aFakeFile))
                ))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Completed });
            Assert.True(lockedFileModel.OverrideLockOnFile(aFakeFile));            
        }
    }

    public class WhenTheRequestIsInvalid
    {
        private string aFakeFile;
        private Mock<IRestClient> moqRestClient;
        private LockedFilesModel lockedFileModel;
        private UserInfo userInfo;

        public WhenTheRequestIsInvalid()
        {
            userInfo = new UserInfo();
            userInfo.Username = "matt";
            aFakeFile = "testFile.txt";
            moqRestClient = new Mock<IRestClient>();
            lockedFileModel = new LockedFilesModel(moqRestClient.Object, userInfo);
        }

        [Fact]
        public void ThenCheckTheLockFileResponseIsFalse()
        {
            moqRestClient.Setup(x => x.Execute(It.Is<IRestRequest>
                (p => p.Parameters.Exists(y => y.Name == "user")
                && p.Parameters.Exists(y => y.Name == "file")
                && p.Parameters.Exists(y => y.Value == "matt")
                && p.Parameters.Exists(y => y.Value == aFakeFile))
                ))
               .Returns(new RestResponse() {ResponseStatus = ResponseStatus.Error});
            Assert.False(lockedFileModel.LockFile(aFakeFile));           
        }

        [Fact]
        public void ThenCheckTheOverrideLockFileResponseIsTrue()
        {
            moqRestClient.Setup(x => x.Execute(It.Is<IRestRequest>
                (p => p.Parameters.Exists(y => y.Name == "user")
                && p.Parameters.Exists(y => y.Name == "file")
                && p.Parameters.Exists(y => y.Value == "matt")
                && p.Parameters.Exists(y => y.Value == aFakeFile))
                ))
                .Returns(new RestResponse() { ResponseStatus = ResponseStatus.Error });
            Assert.False(lockedFileModel.OverrideLockOnFile(aFakeFile));
        }
    }
}
