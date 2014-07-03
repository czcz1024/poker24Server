using System;
using System.Collections.Generic;
using Poke24Server.Database;

namespace Poke24Server.Models
{
    public class MockData : IDisposable
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
                },
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    Player=5
                },
                new Tabs
                {
                    Id=Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    Player=6
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
                        Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                        UserName = "a",
                        Password = "a",
                        NickName = "a"
                    },
                    new Users
                    {
                        Id = Guid.Parse("20000000-0000-0000-0000-000000000000"),
                        UserName = "b",
                        Password = "b",
                        NickName = "b"
                    },
                    new Users
                    {
                        Id = Guid.Parse("30000000-0000-0000-0000-000000000000"),
                        UserName = "c",
                        Password = "c",
                        NickName = "c"
                    },
                    new Users
                    {
                        Id = Guid.Parse("40000000-0000-0000-0000-000000000000"),
                        UserName = "d",
                        Password = "d",
                        NickName = "d"
                    }
                    ,
                    new Users
                    {
                        Id = Guid.Parse("50000000-0000-0000-0000-000000000000"),
                        UserName = "e",
                        Password = "e",
                        NickName = "e"
                    }
                    ,
                    new Users
                    {
                        Id = Guid.Parse("60000000-0000-0000-0000-000000000000"),
                        UserName = "f",
                        Password = "f",
                        NickName = "f"
                    }
                    ,
                    new Users
                    {
                        Id = Guid.Parse("70000000-0000-0000-0000-000000000000"),
                        UserName = "g",
                        Password = "g",
                        NickName = "g"
                    }
                    ,
                    new Users
                    {
                        Id = Guid.Parse("80000000-0000-0000-0000-000000000000"),
                        UserName = "h",
                        Password = "h",
                        NickName = "h"
                    },
                    new Users
                    {
                        Id = Guid.Parse("90000000-0000-0000-0000-000000000000"),
                        UserName = "i",
                        Password = "i",
                        NickName = "i"
                    },
                    new Users
                    {
                        Id = Guid.Parse("11000000-0000-0000-0000-000000000000"),
                        UserName = "cz",
                        Password = "123456",
                        NickName = "陈重"
                    },
                    new Users
                    {
                        Id = Guid.Parse("12000000-0000-0000-0000-000000000000"),
                        UserName = "sjf",
                        Password = "123456",
                        NickName = "剑锋"
                    },
                    new Users
                    {
                        Id = Guid.Parse("13000000-0000-0000-0000-000000000000"),
                        UserName = "zy",
                        Password = "123456",
                        NickName = "赵勇"
                    },
                    new Users
                    {
                        Id = Guid.Parse("14000000-0000-0000-0000-000000000000"),
                        UserName = "wxz",
                        Password = "123456",
                        NickName = "欣卓"
                    },
                    new Users
                    {
                        Id = Guid.Parse("15000000-0000-0000-0000-000000000000"),
                        UserName = "lw",
                        Password = "123456",
                        NickName = "刘伟"
                    },
                    new Users
                    {
                        Id = Guid.Parse("16000000-0000-0000-0000-000000000000"),
                        UserName = "lzq",
                        Password = "123456",
                        NickName = "振强"
                    },
                    new Users
                    {
                        Id = Guid.Parse("17000000-0000-0000-0000-000000000000"),
                        UserName = "lwh",
                        Password = "123456",
                        NickName = "华仔"
                    },
                    new Users
                    {
                        Id = Guid.Parse("18000000-0000-0000-0000-000000000000"),
                        UserName = "jxd",
                        Password = "123456",
                        NickName = "晓东"
                    },
                    new Users
                    {
                        Id = Guid.Parse("19000000-0000-0000-0000-000000000000"),
                        UserName = "myw",
                        Password = "123456",
                        NickName = "小马"
                    },
                    new Users
                    {
                        Id = Guid.Parse("20000000-0000-0000-0000-000000000000"),
                        UserName = "bxj",
                        Password = "123456",
                        NickName = "白哥"
                    },
                    new Users
                    {
                        Id = Guid.Parse("21000000-0000-0000-0000-000000000000"),
                        UserName = "yzq",
                        Password = "123456",
                        NickName = "占强"
                    },
                    new Users
                    {
                        Id = Guid.Parse("22000000-0000-0000-0000-000000000000"),
                        UserName = "zp",
                        Password = "123456",
                        NickName = "张鹏"
                    },
                    new Users
                    {
                        Id = Guid.Parse("23000000-0000-0000-0000-000000000000"),
                        UserName = "weh",
                        Password = "123456",
                        NickName = "小王"
                    }
                };
            }
        }

        public void Dispose()
        {

        }
    }
}