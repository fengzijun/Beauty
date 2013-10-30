using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Beauty.Api.Model
{
    [DataContract]
    public partial class BoolResponse : BaseWebData
    {
        [DataMember]
        public virtual bool Result { get; set; }
    }

    [DataContract]
    public partial class NullabeBoolResponse : BaseWebData
    {
        [DataMember]
        public virtual bool? Result { get; set; }
    }

    //[DataContract]
    //public class IntResponse : BaseWebData
    //{
    //    [DataMember]
    //    public string Result { get; set; }
    //}

    [DataContract]
    public partial class StringResponse : BaseWebData
    {
        [DataMember]
        public virtual string Result { get; set; }
    }

  

    [DataContract]
    public partial class GuidResponse : BaseWebData
    {
        [DataMember]
        public virtual Guid Result { get; set; }
    }
}
