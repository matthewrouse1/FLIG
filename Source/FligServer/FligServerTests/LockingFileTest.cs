using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using FligServer;
using Xunit;

namespace FligServerTests.WhenAFileIsLocked
{
    public class GivenTheFilenameIsValid
    {
        private string fakeFile;
        private LockingController lockingController;
        private IHttpActionResult result;
        private OkNegotiatedContentResult<string> content;

        public GivenTheFilenameIsValid()
        {
            fakeFile = "aFakeFile.txt";
            lockingController = new LockingController();
            result = lockingController.Lock(fakeFile);
            content = Assert.IsType<OkNegotiatedContentResult<string>>(result);
        }

        [Fact]
        public void ThenALockedStatusMessageIsReturned()
        {
            Assert.True(content.Content.Contains(string.Format("Locked: {0}", fakeFile)));
        }

        [Fact]
        public void ThenALockingFileIsCreated()
        {
            
        }
    }
}
