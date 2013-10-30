using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebLike : BaseWebEntity<Guid>
    {
        [DataMember]
        public WebBady Bady { get; set; }
        [DataMember]
        public string Lnk { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public int Likenum { get; set; }
        [DataMember]
        public int Recordnum { get; set; }
        [DataMember]
        public int Likednum { get; set; }
        [DataMember]
        public int Commentnum { get; set; }
        [DataMember]
        public int Supernum { get; set; }
        [DataMember]
        public int Runstatues { get; set; }
        [DataMember]
        public int Type { get; set; }
    }
}