using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Poke24Server.Models;

namespace Poke24Server.Controllers
{
    using System.Web.Http.Cors;

    using Poke24Server.Database;
    using Poke24Server.Logic;

    [EnableCors("*","*","*")]
    public class TabsController : ApiController
    {
        private DataContext db;

        public TabsController()
        {
            //db = new DataContext();
        }
        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet,HttpOptions]
        public IEnumerable<TabViewModel> List()
        {
            //return db.Tabs.ToList().Select(this.ConvertToViewModel);
            var list = AllTab.All.Select(x => new TabViewModel{
                Id=x.Key,
                PlayerCount=x.Value.Info.UserCnt,
                AlReady=x.Value.Users.Count(y=>y.UserId!=Guid.Empty)
            }).ToList();
            return list;
        }

        [HttpPost,HttpOptions]
        public Guid CreateTab()
        {
            return Guid.Empty;
            var tab = new Tabs { 
                Id=Guid.NewGuid(),
                Player=4,
                InPlay=false,
                Creator=Guid.Empty
            };
            db.Tabs.Add(tab);
            db.SaveChanges();
            return tab.Id;
        }
    }
}
