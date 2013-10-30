using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class FirstPageArg : BaseModel<Guid>
    {
        public string Type { get; set; }
        public int LikeArg { get; set; }
        public int CommentArg { get; set; }
        public int RecordArg { get; set; }
        public string mtype { get; set; }
    }
}
