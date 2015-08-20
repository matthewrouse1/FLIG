using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FligServer
{
    [RoutePrefix("lockingcontroller")]
    public class LockingController : ApiController
    {
        [Route("lock/{file}")]
        public IHttpActionResult Lock(string file)
        {
            return Ok(string.Format("Locked: {0}", file));
        }
    }
}