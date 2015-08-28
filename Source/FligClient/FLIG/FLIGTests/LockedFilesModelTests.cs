using FligClient;
using Xunit;
using Moq;
using RestSharp;

namespace GivenWeHaveAServerConnection
{
    public class WhenAFileLockIsRequested
    {
        [Fact]
        public void ThenCheckTheResponseIsValid()
        {
            var aFakeFile = "testFile.txt";
            var moqRestClient = new Mock<IRestClient>();
            moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() {ResponseStatus = ResponseStatus.Completed});
            var lockedFileModel = new LockedFilesModel(moqRestClient.Object);
            Assert.True(lockedFileModel.LockFile(aFakeFile));
        }
    }
}
