using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebBady:BaseWebEntity<Guid>
    {
        [DataMember]
        public string Badyname { get; set; }
        [DataMember]
        public string BadyId { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string Badydescription { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public string Platfrom { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Groupid { get; set; }
        [DataMember]
        public string Twitterid { get; set; }
    }
}