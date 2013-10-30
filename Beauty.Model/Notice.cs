using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class Notice : BaseModel<Guid>
    {
        public string Msg { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
    }
}
