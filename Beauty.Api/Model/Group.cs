using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebGroup:BaseWebEntity<string>
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}