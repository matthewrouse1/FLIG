using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using FligServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace FligServerTests.WhenAFileIsLocked
{
    public class GivenTheFilenameIsValid
    {
        private string fakeFile;
        private LockingController lockingController;
        private IHttpActionResult result;
        private OkNegotiatedContentResult<string> content;
        private IFileService fakeFileService;

        public GivenTheFilenameIsValid()
        {
            fakeFile = "aFakeFile.txt";
            fakeFileService = new FakeFileService();
            lockingController = new LockingController(fakeFileService);
            result = lockingController.Lock(fakeFile);
            content = Assert.IsType<OkNegotiatedContentResult<string>>(result);
        }

        [Fact]
        public void ThenALockedStatusMessageIsReturnedAndTheLockingFileIsCreated()
        {
            Assert.True(content.Content.Contains(string.Format("Locked: {0}", fakeFile)));
        }

        [Fact]
        public void ThenTheLockingFileExistsAfterwards()
        {
            Assert.True(fakeFileService.CheckExists(fakeFile));
        }

        [Fact]
        public void ThenTheFileCanBeUnlocked()
        {
            lockingController.Unlock(fakeFile);
            Assert.False(fakeFileService.CheckExists(fakeFile));
        }
    }

    public class FakeFileService : IFileService
    {
        private List<string> lockedFileList = new List<string>();

        public void CreateFile(string filename, string content)
        {
            lockedFileList.Add(filename);
        }

        public bool CheckExists(string filename)
        {
            return lockedFileList.Exists(x => x == filename);
        }

        public bool RemoveLock(string filename)
        {
            lockedFileList.Remove(filename);
            return CheckExists(filename);
        }
    }
}
