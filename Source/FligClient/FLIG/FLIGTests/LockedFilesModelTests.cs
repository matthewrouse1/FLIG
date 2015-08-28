﻿using FligClient;
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
        public void ThenCheckTheLockFileResponseIsCompleted()
        {
            moqRestClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse() {ResponseStatus = ResponseStatus.Completed});
            Assert.True(lockedFileModel.LockFile(aFakeFile));
        }
    }
}
