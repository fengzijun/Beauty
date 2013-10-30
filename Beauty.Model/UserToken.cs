using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class UserToken : BaseModel<Guid>
    {
        public Guid Userid { get; set; }
    }
}
