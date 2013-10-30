using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class RequstMoney:BaseModel<Guid>
    {
        public string Username { get; set; }
        public decimal Money { get; set; }
        public string Msg { get; set; }

        public User user { get; set; }
    }
}
