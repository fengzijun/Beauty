using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Beauty.Web.Controllers
{
    using Beauty.Model;
    using Beauty.InterFace;
    using Beauty.Core;
    using Beauty.Web.Models;
    using Beauty.Web.Page;
    using Beauty.Common;
    using System.Collections.Specialized;
    using Nodus.Authentication;

    public class HomeController : BaseController
    {
        private IUserStore iuserstore;
        private IBady ibady;
        private ISetting isetting;
        private IMoney imoney;
        private IUser iuser;
        private IPrice iprice;
        private IHelp ihelp;

        public HomeController(IUserStore iuserstore, IBady ibady, ILog ilog, ISetting isetting, IMoney imoney, IUser iuser,IPrice ipirce,IHelp ihelp)
        {
            this.iuserstore = iuserstore;
            this.ibady = ibady;
            this.ilog = ilog;
            this.isetting = isetting;
            this.imoney = imoney;
            this.iuser = iuser;
            this.iprice = ipirce;
            this.ihelp = ihelp;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            string userid = Request["uid"];
            if(!string.IsNullOrEmpty(userid))
            {
                User user = iuser.Get(Guid.Parse(userid));
                if (user != null)
                {
                    ICustomPrincipal cp = new CustomPrincipal();
                    cp.Login(user.Username, user.ID.ToString(), true);
                    user.Ip = GetClientIP();
                    user.Lastlogintime = DateTime.Now;
                    user.IsLogin = true;
                    bool isupdate = iuser.Update(user);
                }
            }

            return RedirectToAction("index","share");
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ContentResult Version()
        {
            string path = Server.MapPath("~/version.txt");
            StreamReader sr = new StreamReader(path);
            string content = sr.ReadToEnd();
            sr.Close();
            return Content(content);
        }

        public ActionResult Rank(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<UserStore> userstores = iuserstore.Get(null, UserName, null, 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (UserStore userstore in userstores)
            {
                userstore.Bady = ibady.Get(userstore.Bady.ID);
            }
            PagedList<UserStore> models = userstores.ToPagedList<UserStore>(paging);

            return View(models);
        }

        public ActionResult ProcessTrx()
        {
            PaginationInfo paging = new PaginationInfo();
            IList<BeautyPrice> prices = iprice.Get(null, null, 1, 0, null, out paging);
            IList<SelectListItem> paylist = new List<SelectListItem>();
            foreach (BeautyPrice s in prices)
            {
                paylist.Add(new SelectListItem { Text = s.Pricename, Value = s.PromotionPrice.HasValue?s.PromotionPrice.Value.ToString():s.Price.ToString() });
            }
            paylist.Add(new SelectListItem { Text = "自定义", Value = ""});
            ViewBag.Paylist = paylist;
            return View();
        }

        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        public ActionResult Notify()
        {
            //SortedDictionary<string, string> sPara = GetRequestPost();
            //PaginationInfo paging = new PaginationInfo();
            //if (sPara.Count > 0)//判断是否有带返回参数
            //{
            //    Notify aliNotify = new Notify();
            //    bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

            //    if (verifyResult)//验证成功
            //    {
            //        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //        //请在这里加上商户的业务逻辑程序代码


            //        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            //        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

            //        //商户订单号

            //        string out_trade_no = Request.Form["out_trade_no"];

            //        //支付宝交易号

            //        string trade_no = Request.Form["trade_no"];

            //        //交易状态
            //        string trade_status = Request.Form["trade_status"];


            //        if (Request.Form["trade_status"] == "TRADE_FINISHED")
            //        {
            //            //判断该笔订单是否在商户网站中已经做过处理
            //            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //            //如果有做过处理，不执行商户的业务程序

            //            //注意：
            //            //该种交易状态只在两种情况下出现
            //            //1、开通了普通即时到账，买家付款成功后。
            //            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
            //            MoneyRecord moneyrecord = imoney.Get(Guid.Parse(Request.Form["out_trade_no"]));
            //            if (!moneyrecord.Balance.HasValue)
            //            {
            //                Setting setting = isetting.Get(Guid.Parse("2AADE1C6-59BE-4B0E-927C-7B28BFC8C397"));
            //                Setting backsetting = isetting.Get(Guid.Parse("3A1FC752-0700-492B-A6C6-A5460271DC91"));
            //                User user = iuser.Get(null, moneyrecord.Username, null, null, null, null, null, null, null, null, null, 0, null, out paging)[0];
            //                user.Point = user.Point + int.Parse(setting.Value) * moneyrecord.Money;
                    
            //                moneyrecord.Balance = user.Point;
            //                iuser.Update(user);
            //                imoney.Update(moneyrecord);
            //                WriteLog(user.Username + " 充值" + moneyrecord.Money.ToString());
            //                if (!string.IsNullOrEmpty(user.Refer))
            //                {
            //                    IList<User> referusers = iuser.Get(null, user.Refer, null, null, null, null, null, null, null, null, null, 0, null, out paging);
            //                    if (referusers != null && referusers.Count > 0)
            //                    {
            //                        User referuser = referusers[0];
            //                        decimal addpoint = int.Parse(setting.Value) * moneyrecord.Money * decimal.Parse(backsetting.Value);
            //                        referuser.Point = referuser.Point + addpoint;
            //                        WriteLog(referuser.Username + " 获得返利积分" + (int.Parse(setting.Value) * moneyrecord.Money * decimal.Parse(backsetting.Value)).ToString());
            //                        MoneyRecord money = new MoneyRecord
            //                        {
            //                            ID = Guid.NewGuid(),
            //                            Money = 0,
            //                            Statues = 1,
            //                            Username = referuser.Username,
            //                            Balance = referuser.Point,
            //                            Type = "获得返利积分:" + addpoint.ToString()
            //                        };
            //                        iuser.Update(referuser);
            //                        imoney.Create(money);
            //                    }
            //                }

            //                ViewBag.Msg = "成功";
            //                ViewBag.Type = "OK";
            //            }
            //            else
            //            {
            //                ViewBag.Msg = "成功";
            //                ViewBag.Type = "OK";
            //            }
            //        }
            //        else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
            //        {
            //            //判断该笔订单是否在商户网站中已经做过处理
            //            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //            //如果有做过处理，不执行商户的业务程序

            //            //注意：
            //            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
            //            MoneyRecord moneyrecord = imoney.Get(Guid.Parse(Request.Form["out_trade_no"]));
            //            if (!moneyrecord.Balance.HasValue)
            //            {
            //                Setting setting = isetting.Get(Guid.Parse("2AADE1C6-59BE-4B0E-927C-7B28BFC8C397"));
            //                User user = iuser.Get(null, moneyrecord.Username, null, null, null, null, null, null, null, null, null, 0, null, out paging)[0];
            //                user.Point = user.Point + int.Parse(setting.Value) * moneyrecord.Money;
            //                moneyrecord.Balance = user.Point;
            //                iuser.Update(user);
            //                imoney.Update(moneyrecord);

            //                ViewBag.Msg = "成功";
            //                ViewBag.Type = "OK";
            //            }
            //            else
            //            {
            //                ViewBag.Msg = "成功";
            //                ViewBag.Type = "OK";
            //            }
            //        }
            //        else
            //        {

            //        }

            //        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

            //        //Response.Write("success");  //请不要修改或删除
            //        ViewBag.Msg = "成功";
            //        ViewBag.Type = "OK";
            //        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    }
            //    else//验证失败
            //    {
            //        ViewBag.Msg = "非法操作";
            //        ViewBag.Type = "Fail";
            //    }
            //}
            //else
            //{
            //    //Response.Write("无通知参数");
            //    ViewBag.Msg = "非法操作";
            //    ViewBag.Type = "Fail";
            //}

            return View();
        }

        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        [AllowAnonymous]
        public ActionResult ProcessTrxReturn()
        {
            SortedDictionary<string, string> sPara = GetRequestGet();
            PaginationInfo paging = new PaginationInfo();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];


                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        MoneyRecord moneyrecord = imoney.Get(Guid.Parse(out_trade_no));
                        if (moneyrecord!=null && !moneyrecord.Balance.HasValue)
                        {
                            Setting setting = isetting.Get(Guid.Parse("2AADE1C6-59BE-4B0E-927C-7B28BFC8C397"));
                            Setting backsetting = isetting.Get(Guid.Parse("3A1FC752-0700-492B-A6C6-A5460271DC91"));
                            User user = iuser.Get(null, moneyrecord.Username, null, null, null, null, null, null, null, null, null, 0, null, out paging)[0];
                            user.Point = user.Point + decimal.Round(decimal.Parse(setting.Value) * moneyrecord.Money, 2);
                            moneyrecord.Balance = user.Point - user.FreezePoint;
                          

                            iuser.Update(user);
                            imoney.Update(moneyrecord);
                            WriteLog(user.Username + " 充值" + moneyrecord.Money.ToString());
                            if (!string.IsNullOrEmpty(user.Refer))
                            {
                                IList<User> referusers = iuser.Get(null, user.Refer, null, null, null, null, null, null, null, null, null, 0, null, out paging);
                                if (referusers != null && referusers.Count > 0)
                                {
                                    User referuser = referusers[0];
                                    decimal addpoint =  decimal.Parse(setting.Value) * moneyrecord.Money * decimal.Parse(backsetting.Value);
                                    addpoint = decimal.Round(addpoint, 2);
                                    referuser.Point = referuser.Point + addpoint;
                                    WriteLog(referuser.Username + " 获得返利积分" + (decimal.Parse(setting.Value) * moneyrecord.Money * decimal.Parse(backsetting.Value)).ToString());
                                    iuser.Update(referuser);

                                    MoneyRecord money = new MoneyRecord
                                    {
                                        ID = Guid.NewGuid(),
                                        Money = 0,
                                        Statues = 1,
                                        Username = referuser.Username,
                                        Balance = referuser.Point - referuser.FreezePoint,
                                        Type = "获得返利积分:" + addpoint.ToString()
                                    };
                                    
                                    imoney.Create(money);
                                }
                            }

                            ViewBag.Money = moneyrecord.Money.ToString();
                            ViewBag.Msg = "成功";
                            ViewBag.Type = "OK";
                        }
                        else
                        {
                            ViewBag.Msg = "非法操作";
                            ViewBag.Type = "Fail";
                        }
                    }
                    else
                    {
                        ViewBag.Msg = "非法操作";
                        ViewBag.Type = "Fail";
                    }





                    //打印页面
                    //Response.Write("验证成功<br />");

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    //Response.Write("验证失败");

                    ViewBag.Msg = "非法操作";
                    ViewBag.Type = "Fail";
                }
            }
            else
            {
                //Response.Write("无返回参数");
                ViewBag.Msg = "非法操作";
                ViewBag.Type = "Fail";
            }

            return View();
        }

        public ActionResult Process()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ProcessTrx(FormCollection data)
        {

            //return JavaScript("window.location = 'http://www.google.co.uk'");
        
            //return new RedirectResult("http://www.baidu.com",false);

            MoneyRecord money = new MoneyRecord
            {
                ID = Guid.NewGuid(),
                Money = decimal.Parse(Request["moneyhidden"]),
                Statues = 1,
                Username = Request["username"],
                Type = "充值类型:" + Request["type"] + "，充值金额：" + Request["moneyhidden"]
            };

            string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
            Guid id = imoney.Create(money);
            if (id == money.ID)
            {
                string payment_type = "1";
                string notify_url = domain + "/home/notify";
                //需http://格式的完整路径，不能加?id=123这类自定义参数

                //页面跳转同步通知页面路径
                string return_url = domain + "/home/processtrxreturn";
                //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

                //卖家支付宝帐户
                string seller_email = "lunux@126.com";
                //必填

                //商户订单号
                string out_trade_no = id.ToString();
                //商户网站订单系统中唯一订单号，必填

                //订单名称
                string subject = "美丽说分享宝";
                //必填

                //付款金额
                string total_fee = money.Money.ToString();
                //必填

                //订单描述

                string body = "";
                //商品展示地址
                string show_url = "";
                //需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html

                //防钓鱼时间戳
                string anti_phishing_key = Submit.Query_timestamp();
                //若要使用请调用类文件submit中的query_timestamp函数

                //客户端的IP地址
                string exter_invoke_ip = GetClientIP();
                //非局域网的外网IP地址，如：221.0.0.1


                ////////////////////////////////////////////////////////////////////////////////////////////////

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "create_direct_pay_by_user");
                sParaTemp.Add("payment_type", payment_type);
                sParaTemp.Add("notify_url", notify_url);
                sParaTemp.Add("return_url", return_url);
                sParaTemp.Add("seller_email", seller_email);
                sParaTemp.Add("out_trade_no", out_trade_no);
                sParaTemp.Add("subject", subject);
                sParaTemp.Add("total_fee", total_fee);
                sParaTemp.Add("body", body);
                sParaTemp.Add("show_url", show_url);
                sParaTemp.Add("anti_phishing_key", anti_phishing_key);
                sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");



                return Content(sHtmlText);
            }
            else
            {
                AddCrossActionMsg("fail", "下定单失败");
                return Content("下定单失败");
            }

        }


        public ActionResult Refer()
        {
            ViewBag.Username = UserName;
            return View();
        }

        public ActionResult help()
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Help> helpmodels = ihelp.Get(null, null, 0, null, out paging);
            if (helpmodels != null && helpmodels.Count > 0)
                return View(helpmodels[0]);

            return View();
        }
    }
}
