using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class UserStore:BaseModel<Guid>
    {
        public Bady Bady { get; set; }
        public string Username { get; set; }
        public int Rank { get; set; }
        public string Type { get; set; }
        public int Page { get; set; }
        public int Liked { get; set; }
        public int Record { get; set; }
        public int Comment { get; set; }
        public string Link { get; set; }
        public string mtype { get; set; }
        //post data
        public int? taskstatus { get; set; }
        public int num { get; set; }
        public int needliked { get; set; }//100 150  5
        public int needcommment { get; set; }//20 30 1
        public int needrecord { get; set; }//20 30 1
        public int needlike { get; set; }//10  15 1
        public int needsuper { get; set; }//10  15 1
        public string msg { get; set; }
        public Like Like { get; set; }

       
    }
}
