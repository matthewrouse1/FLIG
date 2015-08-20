using System;
using System.Web.Http.Results;
using FligServer;
using Xunit;

namespace FligServerTests.WhenAFileIsLocked
{
    public class GivenTheFilenameIsValid
    {
        [Fact]
        public void ThenALockedStatusMessageIsReturned()
        {
            var fakeFile = "aFileName.txt";
            var lockingController = new LockingController();
            var result = lockingController.Get(fakeFile);
            var response = Assert.IsType<OkNegotiatedContentResult<string>>(result);
            Assert.True(response.Content.Contains(string.Format("Locked: {0}", fakeFile)));
        }
    }
}
