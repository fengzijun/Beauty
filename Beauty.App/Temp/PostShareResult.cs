using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.App.Temp
{
    [Serializable]
    public class PostShareResult
    {
        public int? code { get; set; }
        public PostShareResultChild data { get; set; }
        public string msg { get; set; }
    }

    [Serializable]
    public class PostShareResultChild
    {
        public int? twitter_id { get; set; }
        public int? group_id { get; set; }
    }
}
