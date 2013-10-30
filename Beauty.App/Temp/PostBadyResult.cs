using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.App.Temp
{
    [Serializable]
    public class PostBadyResult
    {
        //{"error_code":0,"data":{"gInfo":{"title":"\u7c73\u897f\u679c \u7ae5\u88c5\u513f\u7ae5\u76ae\u8863\u5916\u5957\u7537\u7ae5\u79cb\u88c52013\u65b0\u6b3e\u5939\u514b\u886b\u4f11\u95f2\u5916\u5957\u6f6eSM7A7","goods_url":"http:\/\/detail.tmall.com\/item.htm?id=27015308049","image":"http:\/\/img01.taobaocdn.com\/bao\/uploaded\/i1\/14275026844061217\/T1DcimFcxgXXXXXXXX_!!0-item_pic.jpg","domain":"tmall.com","price":158,"goodsID":64628518}}}
        public int error_code { get; set; }
        public BadyInfotTemp data { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
    }

    [Serializable]
    public class BadyInfotTemp
    {
        public BadyInfo gInfo { get; set; }
    }

    [Serializable]
    public class BadyInfo
    {
        public string title { get; set; }
        public string goods_url { get; set; }
        public string image { get; set; }
        public string domain { get; set; }
        public float price { get; set; }
        public int goodsID { get; set; }
    }

}
