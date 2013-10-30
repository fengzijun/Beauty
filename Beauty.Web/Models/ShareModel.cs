using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beauty.Core;
using Beauty.Model;

namespace Beauty.Web.Models
{
    public class ShareModel
    {
        public IList<Share> Records { get; set; }
        public PaginationInfo PageInfo { get; set; }
    }
}