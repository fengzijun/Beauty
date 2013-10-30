using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public class WebTask : BaseWebEntity<Guid>
    {
        [DataMember]
        public Guid Taskid { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public int Runstatues { get; set; }
        [DataMember]
        public bool IsAuto { get; set; }
        [DataMember]
        public string TaskType { get; set; }
        [DataMember]
        public string NewType { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Keyword { get; set; }
        [DataMember]
        public bool? Autoflag { get; set; }
    }
}