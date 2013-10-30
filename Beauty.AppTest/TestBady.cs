using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.AppTest
{
    [Serializable]
    public class Twitter
    {
        public string twitter_id { get; set; }
        public string twitter_author_uid { get; set; }
        public string twitter_show_type { get; set; }
        public string twitter_images_id { get; set; }
        public string twitter_source_tid { get; set; }
        public string twitter_htmlcontent { get; set; }
        public string twitter_goods_id { get; set; }
        public string twitter_create_time { get; set; }
        public string twitter_pic_type { get; set; }
        public int isShowTime { get; set; }
        public int like_twitter_id { get; set; }
        public string like_author_uid { get; set; }
        public string from_act_id { get; set; }
        public string from_act_name { get; set; }
        public string url { get; set; }
        public string repin { get; set; }
        public Twitter_Info uinfo { get; set; }
        public Twitter_gInfo ginfo { get; set; }
        public int poster_width { get; set; }
        public int poster_height { get; set; }
        public string show_pic { get; set; }
        public int? is_liked { get; set; }
        public List<Twitter_gInfo_Comment> comments { get; set; }
        public int count_forward { get; set; }
        public int count_reply { get; set; }
        public int count_like { get; set; }
        public int isShowLike { get; set; }
        public int isShowClose { get; set; }
        public int isShowPrice { get; set; }
        public int dm { get; set; }
    }

    [Serializable]
    public class Twitter_Info
    {
        public string nickname { get; set; }
        public string avatar_c { get; set; }
        public string is_taobao_seller { get; set; }
        public string user_id { get; set; }
        public Twitter_Info_Identity identity { get; set; }

    }

    [Serializable]
    public class Twitter_Info_Identity
    {
        public string blueV { get; set; }
        public string pinkV { get; set; }
        public string purpleV { get; set; }
        public string editorV { get; set; }
        public string heart_buyer { get; set; }
        public string diamond_buyer { get; set; }
    }

    [Serializable]
    public class Twitter_gInfo
    {
        public string goods_id { get; set; }
        public string goods_price { get; set; }
        public string goods_title { get; set; }
        public string goods_pic_url { get; set; }
        public string goods_url { get; set; }
        public string goods_picture_id { get; set; }
 
    }

    [Serializable]
    public class Twitter_gInfo_Comment
    {
        public string twitter_id { get; set; }
        public string twitter_author_uid { get; set; }
        public string twitter_source_tid { get; set; }
        public string twitter_htmlcontent { get; set; }
        public Twitter_gInfo_Comment_author author { get; set; }
    }

    [Serializable]
    public class Twitter_gInfo_Comment_author
    {
        public string nickname { get; set; }
        public string avatar_c { get; set; }
        public string is_taobao_seller { get; set; }
        public string user_id { get; set; }
        public Twitter_Info_Identity identity { get; set; }
    }


    public class TempClass
    {
        public List<UrlInfo> urls { get; set; }
      
        public string cate_id { get; set; }
        public string type { get; set; }
        public string mtype { get; set; }
    }

    public class UrlInfo
    {
        public string url { get; set; }
        public int frame { get; set; }
        public int page { get; set; }
    }

}
