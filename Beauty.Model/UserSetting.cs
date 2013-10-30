using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class UserSetting:BaseModel<Guid>
    {
        public Guid Userid { get; set; }
        public Guid Settingid { get; set; }
        public string Value { get; set; }
        public string Username { get; set; }
    }
}
