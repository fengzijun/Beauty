using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Group:BaseModel<string>
    {
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
