using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Beauty.App
{
    public class UserInfo
    {
        public string Userid { get; set; }
        public string Username { get; set; }
        public CookieContainer Cookie { get; set; }
        public bool islogin { get; set; }

        public string account { get; set; }
    }
}
