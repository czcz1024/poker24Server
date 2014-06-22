using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Poke24Server.Models;

namespace Poke24Server.Controllers
{
    public class TabsController : ApiController
    {
        [HttpGet,HttpOptions]
        public IEnumerable<TabViewModel> List()
        {
            return null;
        }

        [HttpGet,HttpOptions]
        public Guid CreateTab()
        {
            return Guid.Empty;
        }
    }
}
