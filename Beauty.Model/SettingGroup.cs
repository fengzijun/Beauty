using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class SettingGroup
    {
        public string Category { get; set; }
        public IList<Setting> settings { get; set; }

    }
}
