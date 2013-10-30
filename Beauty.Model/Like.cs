using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Like:BaseModel<Guid>
    {
        public Bady Bady { get; set; }
        public string Lnk { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public int Likenum { get; set; }
        public int Recordnum { get; set; }
        public int Likednum { get; set; }
        public int Commentnum { get; set; }
        public int Supernum { get; set; }
        public int Runstatues { get; set; }
        public int Type { get; set; }

        public string Keyword { get; set; }
    }
}
