using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class Help:BaseModel<Guid>
    {
        public string msgcontent { get; set; }
    }
}
