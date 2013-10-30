using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebShare:BaseWebEntity<Guid>
    {
        [DataMember]
        public WebBady Bady { get; set; }
        [DataMember]
        public bool IsSuper { get; set; }
        [DataMember]
        public int Liked { get; set; }
        [DataMember]
        public Guid? UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Keyword { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public int Runstatues { get; set; }
    }
}