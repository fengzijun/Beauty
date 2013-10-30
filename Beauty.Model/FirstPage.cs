using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Model
{
    public class FirstPage:BaseModel<Guid>
    {
        public string BadyId { get; set; }
        public string Type { get; set; }
        public int Page { get; set; }
        public int Liked { get; set; }
        public int Record { get; set; }
        public int Comment { get; set; }
        public string Link { get; set; }
        public int Rank { get; set; }

        public string twitter_id { get; set; }
        public string twitter_author_uid { get; set; }
        public string twitter_show_type { get; set; }
        public string twitter_images_id { get; set; }
        public string twitter_source_tid { get; set; }
        public string twitter_htmlcontent { get; set; }
        public string twitter_goods_id { get; set; }
        public string twitter_pic_type { get; set; }
        public string like_twitter_id { get; set; }
        public string like_author_uid { get; set; }
        public string from_act_name { get; set; }
        public string from_act_id { get; set; }
        public string goods_price { get; set; }
        public string goods_title { get; set; }
        public string goods_pic_url { get; set; }
        public string goods_url { get; set; }
        public string goods_picture_id { get; set; }
        public string show_pic { get; set; }
        public string mtype { get; set; }

    }
}
