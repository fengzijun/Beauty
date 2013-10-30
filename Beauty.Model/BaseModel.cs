using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class BaseModel<T>
    {
        public T ID { get; set; }
        public string Createby { get; set; }
        public string Createtime { get; set; }
        public string Updateby { get; set; }
        public string Updatetime { get; set; }
        public int Statues { get; set; }

    }
}
