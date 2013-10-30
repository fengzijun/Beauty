using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beauty.Web.Models
{
    public class WebPrice
    {
        public Guid Id { get; set; }
        
        [Required]
        [Display(Name = "产品价格名称")]
        public string Pricename { get; set; }

        [Required]
        [Display(Name = "产品价格")]
        public decimal Price { get; set; }

   
        [Display(Name = "促销价")]
        public decimal? PromotionPrice { get; set; }
    }
}