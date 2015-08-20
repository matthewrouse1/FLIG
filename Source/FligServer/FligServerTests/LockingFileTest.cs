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
            var fakeFileService = new FakeFileService();
            lockingController = new LockingController(fakeFileService);
            result = lockingController.Lock(fakeFile);
            content = Assert.IsType<OkNegotiatedContentResult<string>>(result);
        }

        [Fact]
        public void ThenALockedStatusMessageIsReturnedAndTheLockingFileIsCreated()
        {
            Assert.True(content.Content.Contains(string.Format("Locked: {0}", fakeFile)));
        }

    }

    public class FakeFileService : IFileService
    {
        public void CreateFile(string filename, string content)
        {
            
        }
    }
}
