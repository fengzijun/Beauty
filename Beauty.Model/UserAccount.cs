using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class UserAccount : BaseModel<Guid>
    {

        public string username { get; set; }
        public string account { get; set; }
        public int type { get; set; }
        public string twitterid { get; set; }
    }
}
