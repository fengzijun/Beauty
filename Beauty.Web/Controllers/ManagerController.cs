using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty.Web.Controllers
{
    using Beauty.Model;
    using Beauty.InterFace;
    using Beauty.Core;
    using Beauty.Web.Models;
    using Beauty.Web.Page;
    
    public class ManagerController : BaseController
    {
        //
        // GET: /Manager/

        private IUserSetting iusersetting;
        private IRequestMoney irequestmoney;
        private ISetting isetting;

        public ManagerController(IUserSetting iusersetting, ILog ilog,IMoney imoney,IRequestMoney irequestmoney,IUser iuser,ISetting isetting)
        {
            this.iusersetting = iusersetting;
            this.ilog = ilog;
            this.imoney = imoney;
            this.irequestmoney = irequestmoney;
            this.iuser = iuser;
            this.isetting = isetting;
        }

        public ActionResult UserSetting()
        {
            IList<SettingGroup> groups = iusersetting.GetByUsername(UserName);

            return View(groups);
        }

        [HttpPost]
        public JsonResult UserSetting(IList<UserSetting> data)
        {
            try
            {
                foreach (UserSetting usersetting in data)
                {
                    usersetting.Username = UserName;
                    usersetting.Statues = 1;
                    iusersetting.Update(usersetting);
                }
                WriteLog(UserName + " 更新了自己的系统设置");
                return Json(true, JsonRequestBehavior.AllowGet);
               
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UserMoney(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<MoneyRecord> moneys = imoney.GetAvail(null, UserName, null, null, 1, page.HasValue ? page.Value : 1, null, out paging);

            PagedList<MoneyRecord> models = moneys.ToPagedList<MoneyRecord>(paging);

            return View(models);
        }

        public ActionResult RequestMoney(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<RequstMoney> moneys = irequestmoney.Get(null, UserName,null, 1, page.HasValue ? page.Value : 1, null, out paging);

            PagedList<RequstMoney> models = moneys.ToPagedList<RequstMoney>(paging);
            PaginationInfo paging2 = new PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging2);
            User user = users[0];
            IList<SettingGroup> settings = isetting.GetSystemSetting();
            string ratestr = GetSettingVal(settings, "3396287A-DCF3-4701-8D44-9B3C515D5DEF");
            string moneyratestr = GetSettingVal(settings, "2AADE1C6-59BE-4B0E-927C-7B28BFC8C397");
            decimal rate = decimal.Parse(ratestr);
            decimal moneyrate = decimal.Parse(moneyratestr);
            decimal availmoney = (1 - rate) * (user.Point - user.FreezePoint) / moneyrate;
            availmoney = decimal.Round(availmoney, 2);
            ViewBag.Msg = "您当前的可用积分为" + (user.Point - user.FreezePoint).ToString() + ",可兑换金额为" + availmoney.ToString() + "元";
            decimal point = user.Balance * decimal.Parse(moneyratestr);
            ViewBag.Msg2 = "可购买积分" + point.ToString()+"分";
            ViewBag.point = point.ToString();
            ViewBag.Msg3 = "您当前余额" + user.Balance.ToString() + "元，请按100元的整数倍提取";
            return View(models);
        }

        public ActionResult RequestMoneyAdd()
        {
          
            return View();
        }

        //兑现
        [HttpPost]
        public JsonResult RequestMoneyAdd(string money)
        {
            
                PaginationInfo paging2 = new PaginationInfo();
                IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging2);
                User user = users[0];
                IList<SettingGroup> settings = isetting.GetSystemSetting();
                string ratestr = GetSettingVal(settings, "3396287A-DCF3-4701-8D44-9B3C515D5DEF");
                string moneyratestr = GetSettingVal(settings, "2AADE1C6-59BE-4B0E-927C-7B28BFC8C397");
                decimal rate = decimal.Parse(ratestr);
                decimal moneyrate = decimal.Parse(moneyratestr);
                decimal availmoney = (1 - rate) * (user.Point - user.FreezePoint) / moneyrate;
                availmoney = decimal.Round(availmoney, 2);
                decimal requestmoney = decimal.Parse(money);

              
                ResultMsg msg = new ResultMsg();
                try
                {
                    if (requestmoney < 100)
                    {

                        msg.Msg = "兑现金额必须大于等于100元";
                        msg.Result = false;
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    if (requestmoney > availmoney)
                    {
                        //ModelState.AddModelError("", "可用积分不够，您最多只能兑换" + ((int)availmoney).ToString() + "元");
                        msg.Msg = "可用积分不够";
                        msg.Result = false;
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    decimal point2 = requestmoney * moneyrate / (1 - rate);
                    point2 = decimal.Round(point2, 2);
                    ReduceUserPoint2(point2, requestmoney, "兑换扣去积分：" + point2.ToString() + ",余额增加" + requestmoney.ToString());
                    msg.Msg = "兑换成功";
                    msg.Result = true;
                    return Json(msg, JsonRequestBehavior.AllowGet);
               
                }
                catch
                {
                    ModelState.AddModelError("", "提现失败");
                    msg.Msg = "提现失败，系统忙";
                    msg.Result = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

        }


        //提现
        [HttpPost]
        public JsonResult RequestMoneyToServer(string money)
        {

            PaginationInfo paging2 = new PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging2);
            User user = users[0];
            //IList<SettingGroup> settings = isetting.GetSystemSetting();
            //string ratestr = GetSettingVal(settings, "3396287A-DCF3-4701-8D44-9B3C515D5DEF");
            //string moneyratestr = GetSettingVal(settings, "2AADE1C6-59BE-4B0E-927C-7B28BFC8C397");
            //decimal rate = decimal.Parse(ratestr);
            //decimal moneyrate = decimal.Parse(moneyratestr);
            //decimal availmoney = (1 - rate) * (user.Point - user.FreezePoint) / moneyrate;
            decimal balance = user.Balance;
            decimal requestmoney = decimal.Parse(money);


            ResultMsg msg = new ResultMsg();
            try
            {
                if (requestmoney < 100)
                {
                  
                    msg.Msg = "提现金额必须大于等于100元";
                    msg.Result = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                if (requestmoney > balance)
                {
                    //ModelState.AddModelError("", "可用积分不够，您最多只能兑换" + ((int)availmoney).ToString() + "元");
                    msg.Msg = "可用余额不够";
                    msg.Result = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                //decimal point2 = requestmoney * moneyrate / (1 - rate);
                //ReduceUserPoint2(point2, requestmoney, "提现扣去积分：" + point2.ToString() + ",余额增加" + requestmoney.ToString());
                //msg.Msg = "兑换成功";
                //msg.Result = true;
                //return Json(msg, JsonRequestBehavior.AllowGet);
                Beauty.Model.RequstMoney model = new RequstMoney
                {
                    ID = Guid.NewGuid(),
                    Msg = "等待处理",
                    Money = requestmoney,
                    Username = UserName,
                    Statues = 1
                };

                Guid id = irequestmoney.Create(model);
                if (id == model.ID)
                {
                    //decimal point2 = requestmoney * moneyrate / (1 - rate);
                   // ReduceUserPoint2(point2, requestmoney, "提现扣去积分：" + point2.ToString() + ",余额增加" + requestmoney.ToString());
                    msg.Msg = "提现成功";

                    msg.Result = true;
                    ReduceUserPoint3(requestmoney, "提现" + requestmoney.ToString() + "元,扣除余额" + requestmoney.ToString() + "元");
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", "提现失败，系统忙");
                    msg.Msg = "提现失败，系统忙";
                    msg.Result = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                ModelState.AddModelError("", "提现失败");
                msg.Msg = "提现失败，系统忙";
                msg.Result = false;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult BugPoint()
        {
            return View();
        }

        [HttpPost]
        public JsonResult BugPoint(string point)
        {
            IList<SettingGroup> settings = isetting.GetSystemSetting();
            string moneyratestr = GetSettingVal(settings, "2AADE1C6-59BE-4B0E-927C-7B28BFC8C397");

            ResultMsg msg = new ResultMsg();
            try
            {
                decimal money = decimal.Parse(point) / decimal.Parse(moneyratestr);
                money = decimal.Round(money, 2);
                PaginationInfo paging = new PaginationInfo();
                User user = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 1, null, out paging)[0];
                if (money > user.Balance)
                {
                    ModelState.AddModelError("", "余额不足");
                    msg.Msg = "余额不足";
                    msg.Result = false;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                user.Point += decimal.Parse(point);
                user.Balance -= money;
                iuser.Update(user);

                MoneyRecord moneyrecord = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = "购买积分：" + point.ToString() + ",扣除余额：" + money.ToString()
                };

                imoney.Create(moneyrecord);
                AddCrossActionMsg("OK", "购买积分成功");
                msg.Msg = "购买积分成功";
                msg.Result = true;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                msg.Msg = "购买失败，系统忙";
                msg.Result = false;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
