using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebNotice : BaseWebEntity<Guid>
    {
        [DataMember]
        public string Msg { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}