using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.App.Temp
{
    [Serializable]
    public class PostLikedResult
    {
        public int? code { get; set; }
        public PostLikedResultChild data { get; set; }
        public string msg { get; set; }
    }

    [Serializable]
    public class PostLikedResultChild
    {
        public string user_id { get; set; }
        public int? other_id { get; set; }
    }
}
