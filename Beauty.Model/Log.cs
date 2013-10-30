using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Log:BaseModel<Guid>
    {
        public string Msg { get; set; }
    }
}
