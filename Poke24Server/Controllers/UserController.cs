using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Poke24Server.Database;
using Poke24Server.Models;

namespace Poke24Server.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost,HttpOptions]
        public Guid Register([FromBody]Users user)
        {
            return Guid.Empty;
        }

        [HttpPost,HttpOptions]
        public Guid Login([FromBody] LoginViewModel info)
        {
            return Guid.Empty;
        }
    }
}
