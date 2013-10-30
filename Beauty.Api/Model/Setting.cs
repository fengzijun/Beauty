using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebSetting:BaseWebEntity<Guid>
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Category { get; set; }
    }
}