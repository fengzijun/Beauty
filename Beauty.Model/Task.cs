using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Task:BaseModel<Guid>
    {
        public Guid Taskid { get; set; }
        public Bady Bady { get; set; }
        public Share Share { get; set; }
        public Like Like { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public int Runstatues { get; set; }
        public bool IsAuto { get; set; }
        public string TaskType { get; set; }
        public string NewType { get; set; }
        public string Comment { get; set; }
        public string Keyword { get; set; }
        public bool? Autoflag { get; set; }
    }
}
