using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class MoneyRecord:BaseModel<Guid>
    {
        public string Username { get; set;}
        public Guid? Userid { get; set; }
        public decimal Money { get; set; }
        public decimal? Balance { get; set; }
        public string Type { get; set; }

    }
}
