using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Share:BaseModel<Guid>
    {
        public Bady Bady { get; set;}
        public bool IsSuper { get; set; }
        public int Liked { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string Keyword { get; set; }
        public string Link { get; set; }
        public int Runstatues { get; set; }
    }
}
