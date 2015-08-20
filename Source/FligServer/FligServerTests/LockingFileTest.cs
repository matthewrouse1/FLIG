using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using FligServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace FligServerTests.WhenAFileIsModified
{
    public class GivenTheFilenameIsValid
    {
        private string fakeFile;
        private LockingController lockingController;
        private IHttpActionResult result;
        private OkNegotiatedContentResult<string> content;
        private ILockingService _fakeLockingService;

        public GivenTheFilenameIsValid()
        {
            fakeFile = "aFakeFile.txt";
            _fakeLockingService = new FakeLockingService();
            lockingController = new LockingController(_fakeLockingService);
            result = lockingController.Lock(fakeFile);
            content = Assert.IsType<OkNegotiatedContentResult<string>>(result);
        }

        [Fact]
        public void ThenALockedStatusMessageIsReturned()
        {
            Assert.True(content.Content.Contains(string.Format("Locked: {0}", fakeFile)));
        }

        [Fact]
        public void ThenTheLockExistsAfterwards()
        {
            Assert.True(_fakeLockingService.CheckExists(fakeFile));
        }

        [Fact]
        public void ThenTheFileCanBeUnlocked()
        {
            lockingController.Unlock(fakeFile);
            Assert.False(_fakeLockingService.CheckExists(fakeFile));
        }
    }

    public class FakeLockingService : ILockingService
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
