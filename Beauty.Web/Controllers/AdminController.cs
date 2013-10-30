using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beauty.Model;
using Beauty.InterFace;
using Nodus.Authentication;
using Beauty.Core;
using Beauty.Web.Page;
using Beauty.Web.Models;
using System.IO;

namespace Beauty.Web.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        private ISetting isetting;
        private IPrice iprice;
        private INotice inotice;
        private IRequestMoney irequestmoney;
        private IHelp ihelp;
        private IUserAccount iuseraccount;

        public AdminController(IUser iuser, ISetting isetting,ILog ilog,IPrice iprice,INotice inotice,IRequestMoney irequestmoney,IMoney imoney,IHelp ihelp,IUserAccount iuseraccount)
        {
            this.iuser = iuser;
            this.isetting = isetting;
            this.ilog = ilog;
            this.iprice = iprice;
            this.inotice = inotice;
            this.irequestmoney = irequestmoney;
            this.imoney = imoney;
            this.ihelp = ihelp;
            this.iuseraccount = iuseraccount;
        }

        public ActionResult UserManager(int? page)
        {

            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                PaginationInfo paging2 = new PaginationInfo();
                IList<User> users = iuser.GetByAdmin(null, null, null, null, null, null, null,null, null,null, 1, page.HasValue ? page.Value : 1, null, out paging);
                foreach (User user in users)
                {
                    user.Accounts = iuseraccount.Get(null, user.Username, null, 1, 0, null, out paging2);
                }
                PagedList<User> models = users.ToPagedList<User>(paging);

                return View(models);
            }

            return View();
        }

        public ActionResult UserEdit(string username)
        {

            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = iuser.GetByAdmin(null, username, null, null, null, null, null, null, null, null, 1,0, null, out paging);
                IList<SelectListItem> rolelist = new List<SelectListItem>();
              
                rolelist.Add(new SelectListItem { Text = "一级代理", Value = "1" });
                rolelist.Add(new SelectListItem { Text = "普通用户", Value = "2" });
                
                ViewBag.Rolelist = rolelist;
                WebUser user = AutoMapper.Mapper.Map<WebUser>(users[0]);

                return View(user);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UserEdit(WebUser user)
        {

            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = iuser.GetByAdmin(null, user.UserName, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                users[0].Role = int.Parse(user.Role);
                users[0].Password = user.Password;
                users[0].Point = user.Point;
                users[0].Province = user.Province;
                users[0].City = user.City;
                users[0].Email = user.Email;
                users[0].QQ = user.QQ;
                users[0].ShopAddress = user.ShopAddress;
                users[0].Mobile = user.Mobile;
                users[0].ZFB = user.ZFB;
                users[0].Rate = user.Rate;
                users[0].MaxPoint = user.MaxPoint;
                users[0].TimePoint = user.TimePoint;
                bool isupdate = iuser.Update(users[0]);
                if (isupdate)
                {
                    AddCrossActionMsg("OK", "保存成功");

                }
                else
                {
                    AddCrossActionMsg("fail", "保存失败");
                }
                WriteLog(users[0].Username + " 修改成功");
                return RedirectToAction("useredit", new { username = user.UserName });
            }

            return View();
        }

        [HttpPost]
        public JsonResult UserDel(string username)
        {

            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = iuser.GetByAdmin(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
              

                bool isupdate = iuser.Delete(users[0]);
                if (isupdate)
                {
                    WriteLog(users[0].Username + " 被删除");
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

     
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Setting()
        {
            IList<SettingGroup> groups = isetting.GetSystemSetting();

            return View(groups);
        }

        [HttpPost]
        public JsonResult Setting(IList<Setting> data)
        {
            try
            {
                foreach (Setting setting in data)
                {

                    isetting.Update(setting);
                }
                WriteLog("系统设置被修改");
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SystemLog(int? page)
        {
            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<Log> logs = ilog.Get(null, 1, page.HasValue ? page.Value : 1, null, out paging);

                PagedList<Log> models = logs.ToPagedList<Log>(paging);

                return View(models);
            }

            return View();
        }

        public ActionResult Pricesetting(int? page)
        {
            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<BeautyPrice> users = iprice.Get(null, null, 1, page.HasValue ? page.Value : 1, null, out paging);

                PagedList<BeautyPrice> models = users.ToPagedList<BeautyPrice>(paging);

                return View(models);
            }

            return View();
        }

        public ActionResult PriceCreate()
        {
            return View();
        }

        public ActionResult PriceEdit(string id)
        {
            BeautyPrice price = iprice.Get(new Guid(id));
            WebPrice webprice = AutoMapper.Mapper.Map<WebPrice>(price);
            return View(webprice);
        }

        [HttpPost]
        public ActionResult PriceEdit(WebPrice webprice)
        {

            BeautyPrice price = AutoMapper.Mapper.Map<BeautyPrice>(webprice);
            price.Statues = 1;
            bool isupdate = iprice.Update(price);
            if (isupdate)
            {
                return RedirectToAction("Pricesetting");
            }
            return View();
        }

        [HttpPost]
        public ActionResult PriceCreate(WebPrice model)
        {
            if (string.IsNullOrEmpty(model.Pricename))
                ModelState.AddModelError("", "产品名称不能空");

            if (ModelState.IsValid)
            {
                BeautyPrice price = AutoMapper.Mapper.Map<BeautyPrice>(model);
                price.Statues = 1;
                price.ID = Guid.NewGuid();
                Guid id = iprice.Create(price);
                if (id == price.ID)
                {
                    return RedirectToAction("Pricesetting");
                }
            }

            return View();
        }

        [HttpPost]
        public JsonResult PriceDelete(string id)
        {
            bool isdelete = iprice.Delete(new BeautyPrice { ID = new Guid(id) });
            return Json(isdelete);
        }


        public ActionResult Notice(int? page)
        {
            if (IsAdmin)
            {
                PaginationInfo paging = new PaginationInfo();
                IList<Notice> users = inotice.Get(null, 0, 1, page.HasValue ? page.Value : 1, null, out paging);

                PagedList<Notice> models = users.ToPagedList<Notice>(paging);

                return View(models);
            }

            return View();
        }

        public ActionResult NoticeAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NoticeCreate(Beauty.Model.Notice model)
        {
            if (string.IsNullOrEmpty(model.Msg))
                ModelState.AddModelError("", "公告内容不能空");

            if (ModelState.IsValid)
            {
                model.Type = 0;
                model.Statues = 1;
                model.ID = Guid.NewGuid();
                Guid id = inotice.Create(model);
                if (id == model.ID)
                {
                    return RedirectToAction("notice");
                }
            }

            return View();
        }

        public ActionResult NoticeEdit(string id)
        {
            Notice price = inotice.Get(new Guid(id));

            return View(price);
        }

        [HttpPost]
        public ActionResult NoticeEdit(Beauty.Model.Notice model)
        {
        

            if (ModelState.IsValid)
            {
                Notice notice = inotice.Get(model.ID);
                notice.Msg = model.Msg;
               bool result = inotice.Update(notice);
               if (result)
                {
                    return RedirectToAction("notice");
                }
            }

            return View();
        }

        [HttpPost]
        public JsonResult Noticedelete(string id)
        {

            bool result = inotice.Delete(new Model.Notice { ID = new Guid(id) });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RequestMoneyNodeal(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            PaginationInfo paging2 = new PaginationInfo();
            IList<RequstMoney> moneys = irequestmoney.Get(null, null, "等待处理", 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (RequstMoney model in moneys)
            {
                model.user = iuser.Get(null, model.Username, null, null, null, null, null, null, null, null, 1, 1, null, out paging2)[0];
            }

            PagedList<RequstMoney> models = moneys.ToPagedList<RequstMoney>(paging);

            return View(models);

        }

        [HttpPost]
        public JsonResult RequestMoneyNodeal(string id,string msg)
        {
            PaginationInfo paging = new PaginationInfo();
            RequstMoney money = irequestmoney.Get(new Guid(id));
            money.Msg = msg;

            ResultMsg result = new ResultMsg { Msg = "操作失败", Result = false };
            bool isupdate = irequestmoney.Update(money);
            if (isupdate)
            {
                if (msg.Contains("提现失败"))
                {
                    ReduceUserPoint4(money.Username,money.Money, "提现失败，返回余额：" + money.Money.ToString());
                }
                result.Msg = "操作成功";
                result.Result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RequestMoneydeal(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<RequstMoney> moneys = irequestmoney.Getdealed(null, null, null, 1, page.HasValue ? page.Value : 1, null, out paging);

            PagedList<RequstMoney> models = moneys.ToPagedList<RequstMoney>(paging);

            return View(models);

        }

        public ActionResult Help()
        {
            try
            {
                PaginationInfo paging = new PaginationInfo();
                IList<Help> helpmodels = ihelp.Get(null, null, 0, null, out paging);
                if (helpmodels != null && helpmodels.Count > 0)
                    return View(helpmodels[0]);

                return View();
            }
            catch (Exception ex)
            {
                AddCrossActionMsg("fail", ex.Message);
                string errormsg = "message:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                WriteLog(errormsg);
                return null;
            }
        }

        [HttpPost]
        public ActionResult HelpPost()
        {
            try
            {
                string content = Request.Form["editorValue"];
                Help model = new Help
                {
                    ID = Guid.NewGuid(),
                    msgcontent = content,
                    Statues = 1
                };

                Guid id = ihelp.Create(model);
                AddCrossActionMsg("OK", "保存成功");

                return RedirectToAction("Help");
            }
            catch(Exception ex)
            {
                AddCrossActionMsg("fail", ex.Message);
                string errormsg = "message:"+ex.Message+" Source:"+ex.Source+" StackTrace:"+ex.StackTrace;
                WriteLog(errormsg);
                return RedirectToAction("Help");
            }
        }

        [HttpPost]
        public ContentResult UploadImg()
        {
            try
            {
                var file = Request.Files[0];
                if (file != null)
                {
                    string path = Server.MapPath("~/upload");

                    string filename = Guid.NewGuid().ToString();
                    string[] temp = file.FileName.Split('.');
                    string ext = "." + temp[temp.Length - 1].ToLower();
                    file.SaveAs(path + "/" + filename + ext);
                    string title = Request["pictitle"];
                    string oriName = Request["fileName"];
                    string url = System.Configuration.ConfigurationManager.AppSettings["domain"] + "/upload/" + filename + ext;
                    return Content("{'url':'" + url + "','title':'" + title + "','original':'" + oriName + "','state':'SUCCESS'}");
                }

                return Content("{'state':'上传失败'}");
            }
            catch
            {
                return Content("{'state':'上传失败'}");
            }
        }

        [HttpPost]
        public ContentResult GetUpLoadImgs()
        {
          

            //string[] paths = { "upload"}; //需要遍历的目录列表，最好使用缩略图地址，否则当网速慢时可能会造成严重的延时
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                //文件允许格式
            string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
            string action = Server.HtmlEncode(Request["action"]);

            if (action == "get")
            {
                String str = String.Empty;


                DirectoryInfo info = new DirectoryInfo(Server.MapPath("~/upload"));

                //目录验证
                if (info.Exists)
                {

                    foreach (FileInfo fi in info.GetFiles())
                    {
                        if (Array.IndexOf(filetype, fi.Extension) != -1)
                        {
                            str += domain+ "/upload/" + fi.Name + "ue_separate_ue";
                        }
                    }

                }


                return Content(str);
            }

            return Content("");
        }
     
    }
}
