using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebSettingGroup
    {
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public IList<WebSetting> settings { get; set; }
    }
}