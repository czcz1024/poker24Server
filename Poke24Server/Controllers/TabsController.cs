﻿using System;
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
            var list = new List<TabViewModel>
            {
                new TabViewModel
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    PlayerCount=1
                },
                new TabViewModel
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    PlayerCount=2
                },
                new TabViewModel
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    PlayerCount=3
                },
                new TabViewModel
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    PlayerCount=4
                }
            };
            return list;
        }

        private TabViewModel ConvertToViewModel(Tabs tabs)
        {
            var r = new TabViewModel(){
                Id=tabs.Id,
                PlayerCount=tabs.Player
            };
            return r;
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
