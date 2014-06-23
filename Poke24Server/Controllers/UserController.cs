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
    using System.Web.Http.Cors;

    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private DataContext db;

        public UserController()
        {
            db = new DataContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost,HttpOptions]
        public Guid Register([FromBody]Users user)
        {
            if (db.Users.Any(x => x.UserName == user.UserName))
            {
                return Guid.Empty;
            }
            user.Id = Guid.NewGuid();
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        [HttpPost,HttpOptions]
        public Guid Login([FromBody] LoginViewModel info)
        {
            var obj = db.Users.FirstOrDefault(x => x.UserName == info.Username && x.Password == info.Password);
            if (obj == null)
            {
                return Guid.Empty;
            }
            return obj.Id;
        }
    }
}
