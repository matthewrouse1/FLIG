using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using Microsoft.SqlServer.Server;

namespace FligServer
{
    [RoutePrefix("lockingcontroller")]
    public class LockingController : ApiController
    {
        private ILockingService _lockingService;

        public LockingController() : this(new FileLockingService())
        { }

        public LockingController(ILockingService lockingService)
        {
            _lockingService = lockingService;
        }

        [Route("lock/{file}")]
        public IHttpActionResult Lock(string file)
        {
            _lockingService.CreateFile(file, "test");
            return Ok(string.Format("Locked: {0}", file));
        }

        [Route("unlock/{file}")]
        public IHttpActionResult Unlock(string file)
        {
            return Ok(string.Format("Unlocked: {0}", file));
        }
    }
}