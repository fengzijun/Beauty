using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class User:BaseModel<Guid>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string Mobile { get; set; }
        public bool IsSuper { get; set; }
        public int Liked { get; set; }
        public string Password { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ShopAddress { get; set; }
        public DateTime? Availtime { get; set; }
        public string BeautyAccount { get; set; }
        public string BeautyPassword { get; set; }
        public string Refer { get; set; }
        public Decimal Point { get; set; }
        public Decimal FreezePoint { get; set; }
        public int Role { get; set; }
        public string ZFB { get; set; }
        public string Card { get; set; }
        public bool IsLogin { get; set; }
        public string Ip { get; set; }
        public DateTime Lastlogintime { get; set; }
        public string Bank { get; set; }
        public string Rate { get; set; }
        public decimal? TimePoint { get; set; }
        public decimal? MaxPoint { get; set; }
        public decimal Balance { get; set; }

        public IList<UserAccount> Accounts { get; set; }
    }
}
