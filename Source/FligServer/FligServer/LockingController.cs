using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using Microsoft.SqlServer.Server;

namespace FligServer
{
    [RoutePrefix("flig")]
    public class LockingController : ApiController
    {
        private ILockingService _lockingService;

        public LockingController() : this(new FileLockingService())
        { }

        public LockingController(ILockingService lockingService)
        {
            _lockingService = lockingService;
        }

        [Route("override/{user}/{file}")]
        [HttpGet]
        public IHttpActionResult Override(string user, string file)
        {
            _lockingService.CreateLockOverride(file, user);
            var info = _lockingService.RetrieveLockInfo(file);
            var response = string.Empty;
            info.ForEach(
                delegate(LockObject x) { response += string.Format("{0}:{1}\n", x.Username, x.LockedDateTime); });
            return Ok(response);
        }

        [Route("lock/{user}/{file}")]
        [HttpGet]
        public IHttpActionResult Lock(string user, string file)
        {
            if (_lockingService.DoesLockExist(file))
            {
                return BadRequest("The file is already locked");
            }
            _lockingService.CreateLock(file, new List<string>() { user, DateTime.Now.ToString() });
            return Ok(string.Format("Locked: {0}", file));
        }

        [Route("check/{file}")]
        [HttpGet]
        public IHttpActionResult Check(string file)
        {
            if (_lockingService.DoesLockExist(file))
            {
                return Ok(_lockingService.RetrieveLockInfo(file));
            }
            return BadRequest("File isn't locked");
        }

        [Route("unlock/{user}/{file}")]
        [HttpGet]
        public IHttpActionResult Unlock(string file, string user)
        {
            _lockingService.RemoveLock(file, user);
            return Ok(string.Format("Unlocked: {0}", file));
        }
    }
}