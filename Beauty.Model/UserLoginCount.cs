using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class UserLoginCount:BaseModel<Guid>
    {
        public Guid? userid { get; set; }
        public string username { get; set; }
        public int count { get; set; }
        public DateTime? time { get; set; }
    }
}
