using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class BeautyPrice:BaseModel<Guid>
    {
        public string Pricename { get; set; }
        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }

    }
}
