using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    /// <summary>
    /// Response data will be included in each wcf call
    /// </summary>
    [DataContract]
    public abstract class BaseWebEntity<T>
    {
        /// <summary>
        /// Unique identifier 
        /// </summary>
        [DataMember]
        public virtual T ID { get; set; }

        /// <summary>
        /// Scope this entity belong to, this is for OAuth2. Example: Profile
        /// </summary>  
        [DataMember]
        public string Createby { get; set; }

        [DataMember]
        public string Createtime { get; set; }

        [DataMember]
        public string Updateby { get; set; }

        [DataMember]
        public string Updatetime { get; set; }

        [DataMember]
        public int Statues { get; set; }

        /// <summary>
        /// Message sent to client
        /// </summary>
        [DataMember]
        public string Message { get; set; }

    }

    [DataContract]
    public abstract class BaseWebData
    {
        [DataMember]
        public string Message { get; set; }
    }

 
}
