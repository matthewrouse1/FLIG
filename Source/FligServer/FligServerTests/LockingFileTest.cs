using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private string fakeUser;
        private LockingController lockingController;
        private IHttpActionResult result;
        private OkNegotiatedContentResult<string> content;
        private ILockingService _fakeLockingService;

        public GivenTheFilenameIsValid()
        {
            fakeFile = "aFakeFile.txt";
            fakeUser = "fakeUser";
            _fakeLockingService = new FakeLockingService();
            lockingController = new LockingController(_fakeLockingService);
            result = lockingController.Lock(fakeUser, fakeFile);
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
            Assert.True(_fakeLockingService.DoesLockExist(fakeFile));
        }

        [Fact]
        public void ThenTheSameFileCantBeLockedAgain()
        {
            var secondResult = new LockingController(_fakeLockingService).Lock("anotherUser", fakeFile);
            var secondResponse = Assert.IsType<BadRequestErrorMessageResult>(secondResult);
            Assert.Equal("The file is already locked", secondResponse.Message);
        }

        [Fact]
        public void ThenTheFileCanBeUnlocked()
        {
            lockingController.Unlock(fakeFile, fakeUser);
            Assert.False(_fakeLockingService.DoesLockExist(fakeFile));
        }
    }

    public class FakeLockingService : ILockingService
    {
        private Dictionary<string, List<LockObject>> lockedFileDictionary = new Dictionary<string, List<LockObject>>();

        public void CreateLock(string filename, List<string> content)
        {
            lockedFileDictionary.Add(
                filename, new List<LockObject>()
                    { new LockObject()
                        { Username = content[0], LockedDateTime = DateTime.Parse(content[1]) }
                    }
                );
        }

        public bool DoesLockExist(string filename)
        {
            return lockedFileDictionary.Keys.Contains(filename);
        }

        public bool RemoveLock(string filename, string user)
        {
            lockedFileDictionary.Remove(filename);
            return DoesLockExist(filename);
        }

        public List<LockObject> RetrieveLockInfo(string filename)
        {
            var lockObjects = new List<LockObject>();
            lockedFileDictionary.TryGetValue(filename, out lockObjects);
            return lockObjects;
        }
    }
}
