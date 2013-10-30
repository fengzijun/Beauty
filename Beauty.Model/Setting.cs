using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Setting:BaseModel<Guid>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
    }
}
