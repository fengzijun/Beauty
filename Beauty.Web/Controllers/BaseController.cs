using Nodus.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Beauty.Web.Controllers
{
    using Beauty.InterFace;
    using Microsoft.Practices.Unity;
    using Beauty.Model;
    using Beauty.Web.Attribute;
    
    [Authorize]
    [CustomHandleError]
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public ILog ilog;
        public IUser iuser;
        public IMoney imoney;

        public BaseController()
        {
            //Beauty.UnityDependencyResolver resolve = new UnityDependencyResolver();
           
        }

        public void WriteLog(string msg)
        {
            Log log = new Log
            {
                ID = Guid.NewGuid(),
                Msg = msg,
                Statues = 1
            };

            new System.Threading.Tasks.Task(() => ilog.Create(log)).Start();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.IsAdmin = IsAdmin;

            ViewBag.Domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
        }

        public string GetClientIP()
        {
            string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(IPAddress))
                IPAddress = Request.ServerVariables["REMOTE_ADDR"];

            return IPAddress;
        }

        public string UserId
        {
            get
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (cookie != null)
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                        //{"ID":"35115182-635c-4ee1-9618-3c76e708d1c8","IsAuthenticated":true,"Name":"admin","Roles":"Administrator"}

                        ICustomIdentity customidentity = CustomIdentity.FromJson(ticket.UserData);
                        if (customidentity != null)
                        {
                            return customidentity.Id;
                        }
                    }

                }
                return "32A2FF15-6AE0-4B7A-8982-C85915738549";
            }
        }

        public string UserName
        {
            get
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return System.Web.HttpContext.Current.User.Identity.Name;
                }
                return "fengzijun";
            }
        }

        public bool IsAdmin
        {
            get
            {

                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    ICustomIdentity customidentity = CustomIdentity.FromJson(ticket.UserData);
                    if (customidentity != null && customidentity.Roles != null && customidentity.Roles.Length > 0)
                    {
                        if (customidentity.Roles[0] == "0")
                        {
                            return true;
                        }
                    }
                }

                return false;
            }



        }

        public string RoleName
        {
            get
            {

                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    ICustomIdentity customidentity = CustomIdentity.FromJson(ticket.UserData);
                    if (customidentity != null && customidentity.Roles != null && customidentity.Roles.Length > 0)
                    {
                        return customidentity.Roles[0];
                    }
                }

                return string.Empty;
            }
        }

        public string GetSettingVal(IList<SettingGroup> groupsettings, string settingid)
        {
            foreach (SettingGroup settinggroup in groupsettings)
            {
                foreach (Setting setting in settinggroup.settings)
                {
                    if (setting.ID == Guid.Parse(settingid))
                    {
                        return setting.Value;
                    }
                }
            }

            return null;
        }

        protected void AddCrossActionMsg(string key, string errorMessage)
        {
            if (TempData["proccessingmsg"] == null)
            {
                TempData["proccessingmsg"] = new Dictionary<string, string>();
            }

            Dictionary<string, string> errs = TempData["proccessingmsg"] as Dictionary<string, string>;

            if (errs != null)
            {
                errs.Add(key, errorMessage);
            }

        }

        public decimal GetAlivePoint()
        {
            decimal money = 0;
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                money = users[0].Point - users[0].FreezePoint;
            }

            return money;
        }



        /// <summary>
        /// 提现成功
        /// </summary>
        /// <param name="point"></param>
        /// <param name="type"></param>
        public void ReduceUserPoint3(string usrename,decimal money, string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, usrename, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.Balance -= money;
                iuser.Update(user);

                MoneyRecord moneyrecord = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(moneyrecord);
            }

        }

        /// <summary>
        /// 用户兑换
        /// </summary>
        /// <param name="point"></param>
        /// <param name="type"></param>
        public void ReduceUserPoint2(decimal point,decimal money, string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.Point -= point;
                user.Balance += money;
                iuser.Update(user);

                MoneyRecord moneyrecord = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(moneyrecord);
            }

        }

        //提现
        public void ReduceUserPoint3(decimal money, string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
          
                user.Balance -= money;
                iuser.Update(user);

                MoneyRecord moneyrecord = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(moneyrecord);
            }

        }

        //提现失败返回余额
        public void ReduceUserPoint4(string username ,decimal money, string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];

                user.Balance += money;
                iuser.Update(user);

                MoneyRecord moneyrecord = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(moneyrecord);
            }

        }


        public void ReduceUserPoint(decimal point,string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.FreezePoint += point;
                iuser.Update(user);

                MoneyRecord money = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(money);
            }

           
        }

        public void AddUserPoint(decimal point,string type)
        {
            Beauty.Core.PaginationInfo paging = new Core.PaginationInfo();
            IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.FreezePoint -= point;
                if (user.FreezePoint < 0)
                {
                    user.FreezePoint = 0;
                }
                iuser.Update(user);

                MoneyRecord money = new MoneyRecord
                {
                    ID = Guid.NewGuid(),
                    Money = 0,
                    Statues = 1,
                    Username = user.Username,
                    Balance = user.Point - user.FreezePoint,
                    Type = type
                };

                imoney.Create(money);
            }
        }

    }
}
