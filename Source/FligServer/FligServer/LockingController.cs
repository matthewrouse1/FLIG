using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;

namespace FligServer
{
    [RoutePrefix("lockingcontroller")]
    public class LockingController : ApiController
    {
        private IFileService fileService;

        public LockingController() : this(new FileService())
        { }

        public LockingController(IFileService _fileService)
        {
            fileService = _fileService;
        }

        [Route("lock/{file}")]
        public IHttpActionResult Lock(string file)
        {
            return Ok(string.Format("Locked: {0}", file));
        }
    }
}