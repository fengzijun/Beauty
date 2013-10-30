using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Collections;
using System.Web.Script.Serialization;
using System.IO;
using Beauty.Core;
using Beauty.InterFace;
using Beauty.Model;
using Beauty.Service;
using System.Data;
using Beauty.Common;

namespace Beauty.AppTest
{
    class Program
    {
        private static string[] BigCateGoryurls = new string[] { "http://www.meilishuo.com/guang/catalog/dress?nid=11&cata_id=2000000000000", 
            "http://www.meilishuo.com/guang/catalog/shoes?nid=13&cata_id=6000000000000",
         "http://www.meilishuo.com/guang/catalog/bag?nid=15&cata_id=5000000000000",
        "http://www.meilishuo.com/guang/catalog/access?nid=1097&cata_id=7000000000000",
        "http://www.meilishuo.com/guang/catalog/jiaju?nid=1093&cata_id=9000000000000",
        "http://www.meilishuo.com/guang/catalog/beauty?nid=1095&cata_id=8000000000000"

        };
        private static string currenturl = string.Empty;
        private static Twitter currentmodel = null;
        private static string jsonstr = string.Empty;
        private static List<TempClass> templist = new List<TempClass>();
        private static DataTable dt;
        //private static System.Threading.Timer threadtimer = new System.Threading.Timer(Start, null, 0, 14400000);
        //private static System.Threading.Timer threadtimer2 = new System.Threading.Timer(Start2, null, 0, 36000);
        private static bool flag = false;
        private static bool flag2 = false;
        private static Hashtable t = new Hashtable();

        public static void PostData()
        {
            int count = 0;
            Console.WriteLine("正在获取数据请稍后...");
            foreach (TempClass temp in templist)
            {
                try
                {
                    count++;
                    //Console.WriteLine("正在获取 "+url + ",还剩" + (templist.Count - count).ToString());
                    int typecount = 0;
                    foreach (UrlInfo urlinfo in temp.urls)
                    {
                        
                        currenturl = urlinfo.url;
                        jsonstr = GetTwritterDetail(urlinfo.url);
                        if (string.IsNullOrEmpty(jsonstr))
                        {
                            for (int restart = 0; restart < 3; restart++)
                            {
                                jsonstr = GetTwritterDetail(urlinfo.url);
                                if (!string.IsNullOrEmpty(jsonstr))
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(jsonstr))
                        {
                            jsonstr = jsonstr.Substring(jsonstr.IndexOf(":") + 1, jsonstr.LastIndexOf(",") - jsonstr.IndexOf(":") - 1);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            List<Twitter> objs = serializer.Deserialize<List<Twitter>>(jsonstr);
                            if (objs == null || objs.Count == 0)
                                break;
                            //Console.WriteLine("数据："+jsonstr);
                            for (int i = 0; i < objs.Count; i++)
                            {
                                if (objs[i] != null && !string.IsNullOrEmpty(objs[i].twitter_id))
                                {
                                    currentmodel = objs[i];
                                    Beauty.Model.FirstPage model = new FirstPage
                                    {
                                        ID = Guid.NewGuid(),
                                        Rank = 20 * urlinfo.frame + i + 1,
                                        Comment = objs[i].count_reply,
                                        from_act_id = objs[i].from_act_id,
                                        from_act_name = objs[i].from_act_name,
                                        goods_pic_url = objs[i].ginfo.goods_pic_url,
                                        goods_picture_id = objs[i].ginfo.goods_picture_id,
                                        goods_price = objs[i].ginfo.goods_price,
                                        goods_title = objs[i].ginfo.goods_title,
                                        like_author_uid = objs[i].like_author_uid,
                                        like_twitter_id = objs[i].like_twitter_id.ToString(),
                                        Liked = objs[i].count_like,
                                        Page = urlinfo.page + 1,
                                        show_pic = objs[i].show_pic,
                                        Link = objs[i].url,
                                        goods_url = objs[i].ginfo.goods_url,
                                        Record = objs[i].count_forward,
                                        Statues = 1,
                                        twitter_author_uid = objs[i].twitter_author_uid,
                                        twitter_goods_id = objs[i].twitter_goods_id,
                                        twitter_htmlcontent = objs[i].twitter_htmlcontent,
                                        twitter_id = objs[i].twitter_id,
                                        twitter_images_id = objs[i].twitter_images_id,
                                        twitter_pic_type = objs[i].twitter_pic_type,
                                        twitter_show_type = objs[i].twitter_show_type,
                                        twitter_source_tid = objs[i].twitter_source_tid,
                                        Type = temp.type,
                                        Createby = "admin",
                                        Createtime = DateTime.Now.ToString(),
                                        Updateby = "admin",
                                        Updatetime = DateTime.Now.ToString(),
                                        mtype = temp.mtype
                                    };

                                    DataRow dataRow = dt.NewRow();
                                    dataRow[0] = model.ID;
                                    dataRow[1] = model.twitter_id;
                                    dataRow[2] = model.twitter_author_uid;
                                    dataRow[3] = model.twitter_show_type;
                                    dataRow[4] = model.twitter_images_id;
                                    dataRow[5] = model.twitter_source_tid;
                                    dataRow[6] = model.twitter_htmlcontent;
                                    dataRow[7] = model.twitter_goods_id;
                                    dataRow[8] = model.twitter_pic_type;
                                    dataRow[9] = model.like_twitter_id;
                                    dataRow[10] = model.like_author_uid;
                                    dataRow[11] = model.from_act_name;
                                    dataRow[12] = model.from_act_id;
                                    dataRow[13] = model.goods_price;
                                    dataRow[14] = model.goods_title;
                                    dataRow[15] = model.goods_pic_url;
                                    dataRow[16] = model.goods_url;
                                    dataRow[17] = model.goods_picture_id;
                                    dataRow[18] = model.show_pic;
                                    dataRow[19] = DBNull.Value;
                                    dataRow[20] = model.Type;
                                    dataRow[21] = model.Page;
                                    dataRow[22] = model.Liked;
                                    dataRow[23] = model.Record;
                                    dataRow[24] = model.Comment;
                                    dataRow[25] = model.Link;
                                    dataRow[26] = model.Rank;
                                    dataRow[27] = model.Createby;
                                    dataRow[28] = model.Createtime;
                                    dataRow[29] = model.Updateby;
                                    dataRow[30] = model.Updatetime;
                                    dataRow[31] = model.Statues;
                                    dataRow[32] = model.mtype;
                                    dt.Rows.Add(dataRow);


                                    typecount++;
                                    //Console.WriteLine(url + " 成功,还剩" + (templist.Count - count).ToString());

                                }
                                else
                                {
                                    //Console.WriteLine(url + " 失败,还剩" + (templist.Count - count).ToString());
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(urlinfo.url + " 失败,还剩" + (templist.Count - count).ToString());
                        }
                    }

                    IFirstPage service = new FirstPageService();
                    service.DeleteByType(temp.type,temp.mtype);
                    service.Createbatch(dt, "firstpage");
                    dt.Rows.Clear();

                    Console.WriteLine(temp.type+"|"+temp.mtype + ":" + typecount.ToString() + "----还剩分类：" + (templist.Count-count).ToString());

                }
                catch(Exception ex)
                {
                    WriteLog(ex);
                }
            }
        }

        static void WriteLog(Exception ex)
        {
            StreamWriter sw = new StreamWriter("error.txt", true);
            sw.WriteLine(ex.Message);
            sw.WriteLine(ex.Source);
            sw.WriteLine(ex.StackTrace);
            sw.WriteLine(currenturl);
            sw.Close();
        }

        static void Main(string[] args)
        {
            //foreach (string s in urls)
            //{
            //    string imageurl = string.Empty;
            //    string title = string.Empty;
            //    GetPaiPaiLinkInfo(s,out imageurl,out title);
            //    Console.WriteLine(imageurl);
            //    Console.WriteLine(title);
            //}
            //decimal ddd = decimal.Parse("0.5");
            //Console.ReadLine();
            //http://www.meilishuo.com/u/EJFA_n/1747464717/11111111?refer_type=&expr_alt=b&frm=out_pic
            //string html = HttpHelper.GetHtml("http://www.meilishuo.com/share/1717971023?d_r=0.1.1.2", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
            //HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //HtmlNode node = doc.DocumentNode.SelectSingleNode("//img[@class='twitter_pic']");
            //string imgurl = string.Empty;
            //string title = string.Empty;
            //if (node != null)
            //{
            //    imgurl = node.Attributes["src"].Value;
            //}
            //node = doc.DocumentNode.SelectSingleNode("//div[@class='goods_info']/h1/a");
            //if (node != null)
            //{
            //    title = node.InnerText.Replace("&nbsp;","").Trim();
            //}
            //GetUserBadyInfo();
            //string id = Helper.GetIDFromShareLink("http://www.meilishuo.com/share/1705032883");
            //GetFirstPageInfo();
            //var ddd = (1 - decimal.Parse("0.3"));
            //threadtimer.Change(0, 10800000);
            StreamReader sr = new StreamReader("test.txt");
            string jsonstr = sr.ReadToEnd();
            sr.Close();
            //jsonstr = jsonstr.Substring(jsonstr.IndexOf(":") + 1, jsonstr.LastIndexOf(",") - jsonstr.IndexOf(":") - 1);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Twitter> objs = serializer.Deserialize<List<Twitter>>(jsonstr);
            Console.ReadLine();
        }

        public static void Start(object statues)
        {
            try
            {
                if (!flag)
                {

                    flag = true;
                    GetFirstPageInfo();
                    GetUserBadyInfo();
                    UpdateFirstPageArg();
                    UpdateUserStore();
                    flag = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
            }
        }

        public static void Start2(object statues)
        {
            try
            {
                if (!flag2)
                {

                    flag2 = true;
                  
                    GetUserBadyInfo();
                    flag2 = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
            }
        }

        public static void GetFirstPageInfo()
        {
            DateTime startime = DateTime.Now;
          
            dt = GetTableSchema();
            int tempcount = 0;
            #region get hot
            foreach (string BigCateGoryurl in BigCateGoryurls)
            {
                string cate_id = BigCateGoryurl.Substring(BigCateGoryurl.LastIndexOf("=") + 1, BigCateGoryurl.Length - BigCateGoryurl.LastIndexOf("=") - 1);
                Hashtable categorys = GetCategorys(BigCateGoryurl);
               
                Console.WriteLine(BigCateGoryurl + ":" + categorys.Count.ToString());
                
                foreach (DictionaryEntry category in categorys)
                {

                    string type = category.Key.ToString();
           
                    //string url = "http://www.meilishuo.com" + category.Value.ToString();
                    ///guang/catalog/dress?nid=2083&cata_id=2000000000000
                    ///
                    
                    if (category.Value.ToString().Contains("nid"))
                    {
                        string nid = category.Value.ToString().Substring(category.Value.ToString().IndexOf("=") + 1, category.Value.ToString().IndexOf("&") - category.Value.ToString().IndexOf("=") - 1);
                      
                        for (int page = 0; page < 10; page++)
                        {

                            for (int frame = 0; frame < 8; frame++)
                            {

                                string url = string.Format("http://www.meilishuo.com/aj/getGoods/catalog?frame={2}&page={3}&view=1&word=0&cata_id={0}&section=hot&price=all&nid={1}", cate_id, nid, frame, page);
                                //TempClass temp = new TempClass { type = type, frame = frame, cate_id = cate_id, page = page, url = url };
                                //templist.Add(temp);
                                //Output("正在获取数据" + templist.Count.ToString());
                                tempcount++;
                                TempUrlAdd(type, url, page, frame, "hot");
                            }

                        }
                    }
                    else
                    {
                     
                        var word = category.Value.ToString().Substring(category.Value.ToString().LastIndexOf("/") + 1, category.Value.ToString().Length - category.Value.ToString().LastIndexOf("/") - 1);
                        for (int page = 0; page < 10; page++)
                        {

                            for (int frame = 0; frame < 8; frame++)
                            {

                                string url = string.Format("http://www.meilishuo.com/aj/getGoods/attr?frame={1}&page={2}&view=1&word={0}&section=hot&hi=&price=all", word, frame, page);
                                //TempClass temp = new TempClass { type = type, frame = frame, cate_id = cate_id, page = page, url = url };
                                //templist.Add(temp);
                                //Output("正在获取数据" + templist.Count.ToString());
                                tempcount++;
                                TempUrlAdd(type, url, page, frame, "hot");
                            }

                        }
                    }


                }

                //Console.WriteLine(BigCateGoryurl + ":" + tempcount.ToString());

            }

            #endregion

            #region get new
            foreach (string BigCateGoryurl in BigCateGoryurls)
            {
                string cate_id = BigCateGoryurl.Substring(BigCateGoryurl.LastIndexOf("=") + 1, BigCateGoryurl.Length - BigCateGoryurl.LastIndexOf("=") - 1);
                Hashtable categorys = GetCategorys(BigCateGoryurl);

                Console.WriteLine(BigCateGoryurl + ":" + categorys.Count.ToString());

                foreach (DictionaryEntry category in categorys)
                {

                    string type = category.Key.ToString();

                    //string url = "http://www.meilishuo.com" + category.Value.ToString();
                    ///guang/catalog/dress?nid=2083&cata_id=2000000000000
                    ///

                    if (category.Value.ToString().Contains("nid"))
                    {
                        string nid = category.Value.ToString().Substring(category.Value.ToString().IndexOf("=") + 1, category.Value.ToString().IndexOf("&") - category.Value.ToString().IndexOf("=") - 1);

                        for (int page = 0; page < 10; page++)
                        {

                            for (int frame = 0; frame < 8; frame++)
                            {

                                string url = string.Format("http://www.meilishuo.com/aj/getGoods/catalog?frame={2}&page={3}&view=1&word=0&cata_id={0}&section=new&price=all&nid={1}", cate_id, nid, frame, page);
                                //TempClass temp = new TempClass { type = type, frame = frame, cate_id = cate_id, page = page, url = url };
                                //templist.Add(temp);
                                //Output("正在获取数据" + templist.Count.ToString());
                                tempcount++;
                                TempUrlAdd(type, url, page, frame, "new");
                            }

                        }
                    }
                    else
                    {
                      
                        var word = category.Value.ToString().Substring(category.Value.ToString().LastIndexOf("/") + 1, category.Value.ToString().Length - category.Value.ToString().LastIndexOf("/") - 1);
                        for (int page = 0; page < 10; page++)
                        {

                            for (int frame = 0; frame < 8; frame++)
                            {

                                string url = string.Format("http://www.meilishuo.com/aj/getGoods/attr?frame={1}&page={2}&view=1&word={0}&section=new&hi=&price=all", word, frame, page);
                                //TempClass temp = new TempClass { type = type, frame = frame, cate_id = cate_id, page = page, url = url };
                                //templist.Add(temp);
                                //Output("正在获取数据" + templist.Count.ToString());
                                tempcount++;
                                TempUrlAdd(type, url, page, frame, "new");
                            }

                        }
                    }


                }


            }

            #endregion

            Console.WriteLine("分类总数:" + templist.Count.ToString());
            Console.WriteLine("URL总数:" + tempcount.ToString());

            //Console.WriteLine("重复：" + recount.ToString());
            PostData();
         
            DateTime endtime = DateTime.Now;
            Console.WriteLine("完成 数据量" + dt.Rows.Count.ToString());
            Console.WriteLine(startime.ToString() + "---" + endtime.ToString());
            dt = null;
            templist = new List<TempClass>();
        }

        public static void TempUrlAdd(string type, string url,int page,int frame,string mtype)
        {
            foreach (TempClass temp in templist)
            {
                if (temp.type == type && temp.mtype == mtype)
                {
                    temp.urls.Add(new UrlInfo { url = url, frame = frame, page = page });

                    return;
                }
            }

            TempClass t = new TempClass { type = type,urls = new List<UrlInfo>(), mtype = mtype};
            t.urls.Add(new UrlInfo { url=  url,frame = frame,page = page});
            if (templist == null)
                templist = new List<TempClass>();
            templist.Add(t);

        }

        public static void UpdateFirstPageArg()
        {
            IFirstPageArg firstpageargservice = new FirstPageArgService();
            firstpageargservice.FirstPageArgRecord();
        }

        public static void UpdateUserStore()
        {
            IUserStore firstpageargservice = new UserStoreService();
            firstpageargservice.UpdateAll();
        }

        public static void GetUserBadyInfo()
        {
            
            IBady badyservice = new BadyService();
            PaginationInfo paging = new PaginationInfo();
            IList<Bady> badys = badyservice.Get(null, null, null, 1, 0, null, out paging);
            dt = GetTableSchema2();
            foreach (Bady b in badys)
            {
                if (!string.IsNullOrEmpty(b.Twitterid))
                {
                    string url = "http://www.meilishuo.com/share/" + b.Twitterid;
                    CookieContainer cookie = new CookieContainer();
                    string html = HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var liked = 0;
                    var comment = 0;
                    var record = 0;
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@class='likeNum poster_like_num']");
                    if (node != null)
                    {
                        liked = int.Parse(node.InnerText);
                    }

                    node = doc.DocumentNode.SelectSingleNode("//span[@class='poster_forward_num']");

                    if (node != null)
                    {
                        record = int.Parse(node.InnerText);
                    }

                    node = doc.DocumentNode.SelectSingleNode("//em[@class='twitter_comment_num']");

                    if (node != null)
                    {
                        comment = int.Parse(node.InnerText); 
                    }

                    UserStore model = new UserStore
                    {
                        ID = Guid.NewGuid(),
                        Bady = b,
                        Comment = comment,
                        Username = b.Username,
                        Createby = "admin",
                        Createtime = DateTime.Now.ToString(),
                        Updatetime = DateTime.Now.ToString(),
                        Statues = 1,
                        Updateby = "admin",
                        Page = 0,
                        Rank = 0,
                        Liked = liked,
                        Record = record,
                        mtype = ""
                    };

                    DataRow dataRow = dt.NewRow();
                    dataRow[0] = model.ID;
                    dataRow[1] = model.Username;
                    dataRow[2] = model.Bady.ID.ToString();
                    dataRow[3] = model.Page;
                    dataRow[4] = DBNull.Value;
                    dataRow[5] = model.Liked;
                    dataRow[6] = model.Record;
                    dataRow[7] = model.Comment;
                    dataRow[8] = DBNull.Value;
                    dataRow[9] = model.Rank;
                    dataRow[10] = model.Createby;
                    dataRow[11] = model.Createtime;
                    dataRow[12] = model.Updateby;
                    dataRow[13] = model.Updatetime;
                    dataRow[14] = model.Statues;
                    dataRow[15] = model.mtype;
                    dt.Rows.Add(dataRow);

                }
            }



            IUserStore service = new UserStoreService();
            service.DeleteAll();
            service.Createbatch(dt, "userstore");

            Console.WriteLine("完成 宝贝数据量" + dt.Rows.Count.ToString());
            dt = null;
        }

        public static void Output(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
        }


        public static string GetTwritterDetail(string url)
        {
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
           "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);

            return result;

        }

        public static DataTable GetTableSchema()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { 
                new DataColumn("id",typeof(Guid)), 
                new DataColumn("twitter_id", typeof(string)),
                new DataColumn("twitter_author_uid", typeof(string)),
                new DataColumn("twitter_show_type", typeof(string)),
                new DataColumn("twitter_images_id", typeof(string)),
                new DataColumn("twitter_source_tid", typeof(string)),
                new DataColumn("twitter_htmlcontent", typeof(string)),
                new DataColumn("twitter_goods_id", typeof(string)),
                new DataColumn("twitter_pic_type", typeof(string)),
                new DataColumn("like_twitter_id", typeof(string)),
                new DataColumn("like_author_uid", typeof(string)),
                new DataColumn("from_act_name", typeof(string)),
                new DataColumn("from_act_id", typeof(string)),
                new DataColumn("goods_price", typeof(string)),
                new DataColumn("goods_title", typeof(string)),
                new DataColumn("goods_pic_url", typeof(string)),
                new DataColumn("goods_url", typeof(string)),
                new DataColumn("goods_picture_id", typeof(string)),
                new DataColumn("show_pic", typeof(string)),
                new DataColumn("badyid", typeof(string)),
                new DataColumn("type", typeof(string)),
                new DataColumn("page", typeof(int)),
                new DataColumn("liked", typeof(int)),
                new DataColumn("record", typeof(int)),
                new DataColumn("comment", typeof(int)),
                new DataColumn("link", typeof(string)),
                new DataColumn("rank", typeof(int)),
                new DataColumn("createby", typeof(string)),
                new DataColumn("createtime", typeof(DateTime)),
                new DataColumn("updateby", typeof(string)),
                new DataColumn("updatetime", typeof(DateTime)),
                new DataColumn("statues", typeof(int)),
                new DataColumn("mtype", typeof(string))
            }

         );
            return dataTable;
        }

        public static DataTable GetTableSchema2()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { 
                new DataColumn("id",typeof(Guid)), 
                new DataColumn("username", typeof(string)),
                new DataColumn("badyid", typeof(string)),
                new DataColumn("page", typeof(int)),
                new DataColumn("type", typeof(string)),
                new DataColumn("liked", typeof(int)),
                new DataColumn("record", typeof(int)),
                new DataColumn("comment", typeof(int)),
                new DataColumn("link", typeof(string)),
                new DataColumn("rank", typeof(int)),
                new DataColumn("createby", typeof(string)),
                new DataColumn("createtime", typeof(DateTime)),
                new DataColumn("updateby", typeof(string)),
                new DataColumn("updatetime", typeof(DateTime)),
                new DataColumn("statues", typeof(int)),
                new DataColumn("mtype", typeof(string))
            }

         );
            return dataTable;
        }

        public static void SetTableVale(DataTable dt,FirstPage model)
        {
            DataRow dataRow = dt.NewRow();
            dataRow[0] = model.ID;
            dataRow[1] = model.twitter_id;
            dataRow[2] = model.twitter_author_uid;
            dataRow[3] = model.twitter_show_type;
            dataRow[4] = model.twitter_images_id;
            dataRow[5] = model.twitter_source_tid;
            dataRow[6] = model.twitter_htmlcontent;
            dataRow[7] = model.twitter_goods_id;
            dataRow[8] = model.twitter_pic_type;
            dataRow[9] = model.like_twitter_id;
            dataRow[10] = model.like_author_uid;
            dataRow[11] = model.from_act_name;
            dataRow[12] = model.from_act_id;
            dataRow[13] = model.goods_price;
            dataRow[14] = model.goods_title;
            dataRow[15] = model.goods_pic_url;
            dataRow[16] = model.goods_url;
            dataRow[17] = model.goods_picture_id;
            dataRow[18] = model.show_pic;
            dataRow[19] = DBNull.Value;
            dataRow[20] = model.Type;
            dataRow[21] = model.Page;
            dataRow[22] = model.Liked;
            dataRow[23] = model.Record;
            dataRow[24] = model.Comment;
            dataRow[25] = model.Link;
            dataRow[26] = model.Rank;
            dataRow[27] = model.Createby;
            dataRow[28] = model.Createtime;
            dataRow[29] = model.Updateby;
            dataRow[30] = model.Updatetime;
            dataRow[31] = model.Statues;

            dt.Rows.Add(dataRow);

        }

        public static void SetTableVale2(DataTable dt, UserStore model)
        {
            DataRow dataRow = dt.NewRow();
            dataRow[0] = model.ID;
            dataRow[1] = model.Username;
            dataRow[2] = model.Bady.ID.ToString();
            dataRow[3] = model.Page;
            dataRow[4] = model.Type;
            dataRow[5] = model.Liked;
            dataRow[6] = model.Record;
            dataRow[7] = model.Comment;
            dataRow[8] = model.Link;
            dataRow[9] = model.Rank;
            dataRow[10] = model.Createby;
            dataRow[11] = model.Createtime;
            dataRow[12] = model.Updateby;
            dataRow[13] = model.Updatetime;
            dataRow[14] = model.Statues;

            dt.Rows.Add(dataRow);

        }

        public static Hashtable GetCategorys(string url)
        {
            Hashtable table = new Hashtable();
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
            "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode navnode = doc.DocumentNode.SelectSingleNode(@"//ul[@class='nav_list']");
            if (navnode != null)
            {
                HtmlNodeCollection nc = navnode.SelectNodes(@"//li/p/a");
                if (nc != null && nc.Count > 0)
                {
                    foreach (HtmlNode anode in nc)
                    {
                        if (anode.InnerText != "所有")
                        {
                            if (!table.ContainsKey(anode.InnerText))
                            {
                                table.Add(anode.InnerText, anode.Attributes["href"].Value);
                            }
                        }
                    }
                }
            }

            return table;
        }

        public static void GetTaoBaoLinkInfo(string url, out string imageurl, out string title)
        {
            imageurl = string.Empty;
            title = string.Empty;
            CookieContainer cookie = new CookieContainer();
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
                "application/json, text/javascript, */*; q=0.01", null, null, Encoding.GetEncoding("gb2312"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//span[@id='J_ImgBooth']");
            if (node != null)
            {
                imageurl = node.Attributes["data-src"] != null ? node.Attributes["data-src"].Value : node.Attributes["src"].Value;
            }
            else
            {
                node = doc.DocumentNode.SelectSingleNode(@"//img[@id='J_ImgBooth']");
                if (node != null)
                {
                    imageurl = node.Attributes["data-src"] != null ? node.Attributes["data-src"].Value : node.Attributes["src"].Value;
                }
            }
            node = doc.DocumentNode.SelectSingleNode(@"//div[@class='tb-detail-hd']");
            if (node != null)
            {
                title = node.InnerText.Replace("\r\n", "").Replace("\t", "").Trim();
            }


        }

        public static void GetPaiPaiLinkInfo(string url, out string imageurl, out string title)
        {
            imageurl = string.Empty;
            title = string.Empty;
            CookieContainer cookie = new CookieContainer();
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
                "application/json, text/javascript, */*; q=0.01", null, null, Encoding.GetEncoding("gb2312"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//img[@alt='商品主图']");
            if (node != null)
            {
                imageurl = node.Attributes["src"].Value;
            }

            node = doc.DocumentNode.SelectSingleNode(@"//div[@class='title']");
            if (node != null)
            {
                title = node.InnerText.Replace("\r\n", "").Replace("\t", "").Trim().Replace("举报此商品", "");
            }
        }
    }
}
