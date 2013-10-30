using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using Beauty.Model;
using Beauty.InterFace;
using Nodus.Authentication;
using Beauty.Core;
using Beauty.Web.Models;

namespace Beauty.Web.Controllers
{
    
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login
        private IUser iuser;
        private IUserToken iusertoken;
        public AccountController(IUser iuser,IUserToken iusertoken,ILog ilog)
        {
            this.iuser = iuser;
            this.iusertoken = iusertoken;
            this.ilog = ilog;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("index", "share");
            }
        }

        
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            //ViewBag.Token = token;
            //if (string.IsNullOrEmpty(token))
            //{
            //    return new HttpNotFoundResult();
            //}
            return View();
        }

        [AllowAnonymous]
        public ActionResult Resetpassword(string token)
        {
            ViewBag.Token = token;
            if (string.IsNullOrEmpty(token))
            {
                return new HttpNotFoundResult();
            }
            UserToken usertoken = iusertoken.Get(Guid.Parse(token));
            string tokentimeout = System.Configuration.ConfigurationManager.AppSettings["tokentimeout"];
            if (usertoken == null || DateTime.Now.Subtract(DateTime.Parse(usertoken.Createtime)).TotalMinutes > int.Parse(tokentimeout))
            {
                return RedirectToAction("TokenTimeOut");
            }

            ViewBag.Userid = usertoken.Userid.ToString();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Resetpassword(string userid,ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                User user = iuser.Get(new Guid(userid));
                user.Password = model.NewPassword;
                bool isupdate = iuser.Update(user);
                if (isupdate)
                {
                   WriteLog(user.Username + " 密码修改成功");
                   AddCrossActionMsg("OK", "密码修改成功！");
                }
                else
                {
                    AddCrossActionMsg("OK", "密码修改失败！");
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult TokenTimeOut()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            //ViewBag.Token = token;
            //if (string.IsNullOrEmpty(token))
            //{
            //    return new HttpNotFoundResult();
            //}
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "邮箱不能为空");
                return View();
            }

            PaginationInfo paging = new PaginationInfo();
            IList<User> users = iuser.Get(null, null, null, null, email, null, null, null, null, null, 1, 0, null, out paging);
            if (users == null || users.Count == 0)
            {
                ModelState.AddModelError("", "系统不存在该邮箱");
                return View();
            }

            string msgcontent = "<html>  <head><base target='_blank'></head>  <body style='margin-bottom: 0px; margin-top: 0px; padding-bottom: 0px; padding-top: 0px; '>          <p>         请点击以下链接来重置你的密码:          </p>            <p>          <a href='{0}/account/resetpassword?token={1}' target='_blank'>{0}/account/resetpassword?token={1}</a>          </p>  </body></html>";
            string emailaccount = System.Configuration.ConfigurationManager.AppSettings["emailaccount"];
            string emailpassword = System.Configuration.ConfigurationManager.AppSettings["emailpassword"];
            string smtp = System.Configuration.ConfigurationManager.AppSettings["smtp"];
            string port = System.Configuration.ConfigurationManager.AppSettings["port"];
            string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
            Guid token = Guid.NewGuid();
            bool issuc = Beauty.Common.Helper.SendEmail(emailaccount,emailpassword,smtp,port,users[0].Email,"美丽说分享宝重置密码",string.Format(msgcontent,domain,token.ToString()));
            if (issuc)
            {
                Guid id = iusertoken.Create(new UserToken { ID = token, Userid = users[0].ID , Statues =1});
                if (id == token)
                {
                    WriteLog(users[0].Username + " 激活邮件发送成功");
                    AddCrossActionMsg("OK", "激活邮件发送成功！");
                }
                else
                {
                    AddCrossActionMsg("Fail", "激活邮件发送失败！");
                }
            }
            else
            {
                AddCrossActionMsg("Fail", "激活邮件发送失败！");
            }

            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            //// If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "The user name or password provided is incorrect.");
            if (ModelState.IsValid)
            {
                string code = Request["code"];
                if (Session["__VCode"] == null)
                {
                    ModelState.AddModelError("", "验证码不正确");
                    return View(model);
                }
                if (code != Session["__VCode"].ToString())
                {
                    ModelState.AddModelError("", "验证码不正确");
                    return View(model);
                }
                Core.PaginationInfo paging = new Core.PaginationInfo();
                IList<User> users = iuser.Get(null, model.UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users.Count > 0)
                {
                    User user = users[0];
                    if (user.Password == model.Password)
                    {
                        ICustomPrincipal cp = new CustomPrincipal();
                        cp.Login(model.UserName, user.ID.ToString(), true);
                        user.Ip = null;
                        user.Lastlogintime = DateTime.Now;
                        //user.IsLogin = true;
                        bool isupdate = iuser.Update(user);

                       
                        if (user.Role != 0)
                        {
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            WriteLog(user.Username + " 登录系统");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            WriteLog(user.Username + " 登录系统");
                            return RedirectToAction("usermanager", "admin");
                        }

                    }
                }

                ModelState.AddModelError("", "用户名密码错误");
            }
            return View(model);
        }

        //
        // POST: /Account/LogOff

       
      
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register(string refer)
        {
            ViewBag.refer = refer;
            return View();
        }

       
        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model,string refer)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                string code = Request["code"];
                if (Session["__VCode"] == null)
                {
                    return View(model);
                }
                if (code != Session["__VCode"].ToString())
                {
                    ModelState.AddModelError("", "验证码不正确");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Refer.Trim()))
                {
                    ModelState.AddModelError("", "邀请码不正确");
                    return View(model);
                }
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = iuser.Get(null, model.Refer.Trim(), null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users == null || users.Count == 0)
                {
                    ModelState.AddModelError("", "邀请码不正确");
                    return View(model);
                }

                User newuser = AutoMapper.Mapper.Map<User>(model);
                newuser.Role = 2;
                newuser.Statues = 1;
                newuser.IsSuper = false;
                newuser.Liked = 0;
                newuser.Ip = null;
                //newuser.IsLogin = true;
                newuser.ID = Guid.NewGuid();
                newuser.Lastlogintime = DateTime.Now;
                newuser.Point = 0;
                newuser.Refer = model.Refer;
                newuser.Balance = 0;

                Guid userid = iuser.Create(newuser);
                if(userid == newuser.ID)
                {
                    ICustomPrincipal cp = new CustomPrincipal();
                    cp.Login(model.UserName, userid.ToString(), true);
                    WriteLog(model.UserName + " 注册网站");
                    return RedirectToAction("Index", "Home");
                }
                else{
                    ModelState.AddModelError("", "注册失败");
                    return View(model);
                }
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                 
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        /// <summary>
        /// 验证码
        /// </summary>
        [AllowAnonymous]
        public void ValidateCode()
        {
            // 在此处放置用户代码以初始化页面
            string vnum;
            vnum = GetByRndNum(4);
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            Response.ContentType = "image/jpeg";

            CreateValidateCode(vnum);

        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="vnum"></param>
        private void CreateValidateCode(string vnum)
        {
            Bitmap Img = null;
            Graphics g = null;
            Random random = new Random();
            int gheight = vnum.Length * 15;
            Img = new Bitmap(gheight, 26);
            g = Graphics.FromImage(Img);
            Font f = new Font("微软雅黑", 16, FontStyle.Bold);
            //Font f = new Font("宋体", 9, FontStyle.Bold);

            g.Clear(Color.White);//设定背景色
            Pen blackPen = new Pen(ColorTranslator.FromHtml("#e1e8f3"), 18);

            for (int i = 0; i < 128; i++)// 随机产生干扰线，使图象中的认证码不易被其它程序探测到
            {
                int x = random.Next(gheight);
                int y = random.Next(20);
                int xl = random.Next(6);
                int yl = random.Next(2);
                g.DrawLine(blackPen, x, y, x + xl, y + yl);
            }

            SolidBrush s = new SolidBrush(ColorTranslator.FromHtml("#411464"));
            g.DrawString(vnum, f, s, 1, 1);

            //画边框
            blackPen.Width = 1;
            g.DrawRectangle(blackPen, 0, 0, Img.Width - 1, Img.Height - 1);
            Img.Save(Response.OutputStream, ImageFormat.Jpeg);
            s.Dispose();
            f.Dispose();
            blackPen.Dispose();
            g.Dispose();
            Img.Dispose();

            //Response.End();
        }

        //-----------------给定范围获得随机颜色
        Color GetByRandColor(int fc, int bc)
        {
            Random random = new Random();

            if (fc > 255)
                fc = 255;
            if (bc > 255)
                bc = 255;
            int r = fc + random.Next(bc - fc);
            int g = fc + random.Next(bc - fc);
            int b = fc + random.Next(bc - bc);
            Color rs = Color.FromArgb(r, g, b);
            return rs;
        }

        private static readonly string[] VcArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        //取随机产生的认证码(数字)
        public string GetByRndNum(int VcodeNum)
        {

            string VNum = "";

            Random rand = new Random();

            for (int i = 0; i < VcodeNum; i++)
            {
                VNum += VcArray[rand.Next(0, 9)];
            }
            Session["__VCode"] = VNum;
            return VNum;
        }

        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(UpdatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = iuser.Get(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    if (user.Password == model.OldPassword)
                    {
                        user.Password = model.NewPassword;
                        bool isupdate = iuser.Update(user);
                        if (isupdate)
                        {
                            AddCrossActionMsg("OK", "更改密码成功");
                            WriteLog(user.Username + " 更改密码成功");
                            return RedirectToAction("UpdatePassword");
                        }
                        else
                        {
                            AddCrossActionMsg("fail", "更改密码失败");
                            return RedirectToAction("UpdatePassword");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "旧密码错误");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "找不到该用户，请重新登录");
                }
            }

            return View(model);
        }


        public ActionResult AccountInfo()
        {
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = iuser.GetByAdmin(null, UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            //IList<SelectListItem> rolelist = new List<SelectListItem>();

            //rolelist.Add(new SelectListItem { Text = "一级代理", Value = "1" });
            //rolelist.Add(new SelectListItem { Text = "普通用户", Value = "2" });

            //ViewBag.Rolelist = rolelist;
            WebUser user = AutoMapper.Mapper.Map<WebUser>(users[0]);

            return View(user);
        }

        [HttpPost]
        public ActionResult AccountInfo(Beauty.Web.Models.WebUser user)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = iuser.Get(null, user.UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            User objuser = users[0];
            objuser.Email = user.Email;
            objuser.ZFB = user.ZFB;
            objuser.QQ = user.QQ;
            objuser.Mobile = user.Mobile;
            objuser.Province = user.Province;
            objuser.City = user.City;
            objuser.ShopAddress = user.ShopAddress;

            bool isupdate = iuser.Update(objuser);
            if (isupdate)
            {
                AddCrossActionMsg("OK", "保存成功");
            }
            else
            {
                AddCrossActionMsg("OK", "保存失败");
            }
            return RedirectToAction("AccountInfo");
            
        }
      

    }
}
