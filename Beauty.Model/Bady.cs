using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class Bady : BaseModel<Guid>
    {
        public string Badyname { get; set; }
        public string BadyId { get; set; }
        public string ImageUrl { get; set; }
        public string Price { get; set; }
        public string Badydescription { get; set; }
        public string Link { get; set; }
        public string Platfrom { get; set; }
        public string Username { get; set; }
        public string Groupid { get; set; }
        public string Twitterid { get; set; }
    }
}
