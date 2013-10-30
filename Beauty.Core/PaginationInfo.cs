using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Core
{
    public class PaginationInfo
    {
        public int Size { get; set; }

        public int Current { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }
    }
}
