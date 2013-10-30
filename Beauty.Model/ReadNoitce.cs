using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class ReadNoitce : BaseModel<Guid>
    {
        public string Userid { get; set; }
        public string Noticeid { get; set; }
    }
}
