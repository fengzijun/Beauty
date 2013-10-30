using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebUser : BaseWebEntity<Guid>
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string QQ { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Province { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ShopAddress { get; set; }
        [DataMember]
        public string Refer { get; set; }
        [DataMember]
        public bool IsLogin { get; set; }
        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public Decimal Point { get; set; }
        [DataMember]
        public Decimal FreezePoint { get; set; }
        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public string ZFB { get; set; }

        [DataMember]
        public string Card { get; set; }
        [DataMember]
        public string Bank { get; set; }

    }
}