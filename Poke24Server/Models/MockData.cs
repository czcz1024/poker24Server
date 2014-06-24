using System;
using System.Collections.Generic;
using Poke24Server.Database;

namespace Poke24Server.Models
{
    public class MockData:IDisposable
    {
        public List<Tabs> Tabs
        {
            get
            {
                return new List<Tabs>
            {
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Player=1
                },
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Player=2
                },
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Player=3
                },
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Player=4
                }
            };
            }
        }

        public List<Users> Users
        {
            get
            {
                return new List<Users>
            {
                new Users
            {
                Id=Guid.Parse("10000000-0000-0000-0000-000000000000"),
                UserName="a",
                Password="a",
                NickName="a"
            },
            new Users
            {
                Id=Guid.Parse("20000000-0000-0000-0000-000000000000"),
                UserName="b",
                Password="b",
                NickName="b"
            },
            new Users
            {
                Id=Guid.Parse("30000000-0000-0000-0000-000000000000"),
                UserName="c",
                Password="c",
                NickName="c"
            },
            new Users
            {
                Id=Guid.Parse("40000000-0000-0000-0000-000000000000"),
                UserName="d",
                Password="d",
                NickName="d"
            }
            };
            }
        }

        public void Dispose()
        {
            
        }
    }
}