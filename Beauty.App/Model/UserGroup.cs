using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.App.Model
{
    //[{"group_id":"42454433","name":"fengzijun\u559c\u6b22\u7684\u5b9d\u8d1d","role":"1"},{"group_id":"42454373","name":"fengzijun","role":"1"}]
    public class UserGroup
    {
        public string group_id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
}
