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

    public class ShareController : BaseController
    {
        private IShare ishare;
        private IBady ibady;
        private ISetting isetting;
        private ITask itask;
        public ShareController(IShare ishare,IBady ibady,ILog ilog,ISetting isetting,IUser iuser,IMoney imoney,ITask itask)
        {
            this.ishare = ishare;
            this.ibady = ibady;
            this.imoney = imoney;
            this.ilog = ilog;
            this.isetting = isetting;
            this.iuser = iuser;
            this.itask = itask;
        }

        public ActionResult Index(int? page,string key)
        {

            
            PaginationInfo paging = new PaginationInfo();
            IList<Bady> badys = ibady.GetNeedToShare(null,UserName, key , 1, page.HasValue ? page.Value : 1, null, out paging ,true);
            PagedList<Bady> models = badys.ToPagedList<Bady>(paging);
          
            return View(models);
        }

        public ActionResult HasShared(int? page, string key)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Share> shares = ishare.Get(null, UserName,null,null,null,null,2,1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Share share in shares)
            {
                share.Bady = ibady.Get(share.Bady.ID);
            }
            PagedList<Share> models = shares.ToPagedList<Share>(paging);

            return View(models);
        }

        public ActionResult Sharing(int? page, string key)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Share> shares = ishare.GetSharing(null, UserName, null, null, null, null, null, 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Share share in shares)
            {
                share.Bady = ibady.Get(share.Bady.ID);
            }
            PagedList<Share> models = shares.ToPagedList<Share>(paging);

            return View(models);
        }

        [HttpPost]
        public JsonResult Update(string link)
        {
            string imageurl = string.Empty;
            string title = string.Empty;
            string plat = string.Empty;

            ResultMsg msg = new ResultMsg { Result = false, Msg = "上传失败" };

            if (link.ToLower().Contains("taobao") || link.ToLower().Contains("tmall"))
            {
                Beauty.Common.Helper.GetTaoBaoLinkInfo(link, out imageurl, out title);
                plat = "淘宝";
            }
            else if (link.ToLower().Contains("wanggou"))
            {
                Beauty.Common.Helper.GetPaiPaiLinkInfo(link, out imageurl, out title);
                plat = "拍拍";
            }

            if (!string.IsNullOrEmpty(imageurl) && !string.IsNullOrEmpty(link))
            {
                Bady bady = new Bady
                {
                    ID = Guid.NewGuid(),

                    ImageUrl = imageurl,

                    Badydescription = title,
                    Link = link,
                    Statues = 1,
                    Platfrom = plat,
                    Username = UserName
                };
                Guid id = ibady.Create(bady);
                if (id == bady.ID)
                {
                    WriteLog(UserName + "上传了宝贝 编号为" + id.ToString());
                    msg = new ResultMsg { Result = true, Msg = "上传成功" };

                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckUser(List<Share> data)
        {
            int result = 0;

            foreach (Share share in data)
            {
                int tempresult = iuser.CheckconditionUser(share.Liked, share.IsSuper,UserName);
                if (tempresult > result)
                    result = tempresult;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostShare(List<Share> data)
        {

            try
            {
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string val = GetSettingVal(groupsetting, "11BE7BA7-C738-41C1-B78B-FFE54E9A8FFF");
                string val2 = GetSettingVal(groupsetting, "A39E4869-B383-43ED-92F0-5B2EA18B0BBA");
                string val3 = GetSettingVal(groupsetting, "09008380-8EE3-4D16-8858-F956044DE5E9");
                string val4 = GetSettingVal(groupsetting, "22F0B30D-8B72-4C6A-A193-0D1E26E74F02");
                string val5 = GetSettingVal(groupsetting, "5C16F422-14CE-4285-AC0B-1132471C32DA");
                string val6 = GetSettingVal(groupsetting, "DC2A7CED-31F2-42BE-A566-4DF62C163A93");
                string supperval = GetSettingVal(groupsetting, "E798DAD0-754B-450A-94DD-EF136AAAB991");
                decimal alivepoint = GetAlivePoint();

                decimal totalpoint = 0;
                foreach (Share s in data)
                {
                    if (s.Liked == 5000 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 5000 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val);
                    }
                    else if (s.Liked == 10000 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val2) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 10000 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val2);
                    }
                    else if (s.Liked == 20000 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val3) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 20000 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val3);
                    }
                    else if (s.Liked == 30000 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val4) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 30000 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val4);
                    }
                    else if (s.Liked == 50000 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val5) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 50000 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val5);
                    }
                    else if (s.Liked == 50001 && s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val6) * decimal.Parse(supperval);
                    }
                    else if (s.Liked == 50001 && !s.IsSuper)
                    {
                        totalpoint += decimal.Parse(val6);
                    }

                }

                totalpoint = decimal.Round(totalpoint, 2);

                if (totalpoint <= alivepoint)
                {
                    foreach (Share share in data)
                    {
                        share.Statues = 1;
                        share.ID = Guid.NewGuid();

                        Guid id = ishare.Create(share);
                        WriteLog(UserName + "分派了分享任务 编号为" + id.ToString());
                    }
                    AddCrossActionMsg("OK", "发起分享任务成功");
                    ReduceUserPoint(totalpoint, "发起分享任务，冻结积分：" + (totalpoint).ToString());
                   
                    ResultMsg msg = new ResultMsg { Result = true, Msg = "发起分享任务成功" };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ResultMsg msg = new ResultMsg { Result = false, Msg = "可用积分不足" };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                AddCrossActionMsg("Fail", "分享失败");
                ResultMsg msg = new ResultMsg { Result = false, Msg = "发起分享任务失败" };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteNoShare(List<Bady> data)
        {
            try
            {
                foreach (Bady s in data)
                {
                    ibady.Delete(s);
                }
                return Json(new ResultMsg { Msg = "删除成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "删除失败", Result = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult DeleteShared(List<Share> data)
        {
            try
            {
                foreach (Share s in data)
                {
                    ishare.Delete(s);
                }
                return Json(new ResultMsg { Msg = "删除成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "删除失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteSharing(List<Share> data)
        {
            PaginationInfo paging = new PaginationInfo();
            
            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string val = GetSettingVal(groupsetting, "11BE7BA7-C738-41C1-B78B-FFE54E9A8FFF");
                string val2 = GetSettingVal(groupsetting, "A39E4869-B383-43ED-92F0-5B2EA18B0BBA");
                string val3 = GetSettingVal(groupsetting, "09008380-8EE3-4D16-8858-F956044DE5E9");
                string val4 = GetSettingVal(groupsetting, "22F0B30D-8B72-4C6A-A193-0D1E26E74F02");
                string val5 = GetSettingVal(groupsetting, "5C16F422-14CE-4285-AC0B-1132471C32DA");
                string val6 = GetSettingVal(groupsetting, "DC2A7CED-31F2-42BE-A566-4DF62C163A93");
                string supperval = GetSettingVal(groupsetting, "E798DAD0-754B-450A-94DD-EF136AAAB991");

                foreach (Share s in data)
                {

                    Share share = ishare.Get(s.ID);
                    bool isdelete = ishare.Delete(s);
                    decimal shareprice = 0;

                    if (share.Liked == 5000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 5000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val);
                    }
                    else if (share.Liked == 10000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val2) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 10000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val2);
                    }
                    else if (share.Liked == 20000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val3) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 20000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val3);
                    }
                    else if (share.Liked == 30000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val4) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 30000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val4);
                    }
                    else if (share.Liked == 50000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val5) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 50000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val5);
                    }
                    else if (share.Liked == 50001 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val6) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 50001 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val6);
                    }

                    decimal totalppint = shareprice * 1;
                    decimal completepoint = 0;

                    IList<Task> starttasks = itask.Get(null, null, share.ID, null, null, null, null, 1, 0, null, out paging);
                    if (starttasks == null || starttasks.Count == 0)
                    {
                        completepoint = 0;
                        
                    }
                    else
                    {
                        
                        Task t = starttasks[0];
                        if (t.Runstatues == 0)
                        {
                            completepoint = 0;
                        }
                        else if (t.Runstatues == 1)
                        {
                            completepoint = 0;
                        }
                        else if (t.Runstatues == 2)
                        {
                            completepoint = shareprice * 1;
                        }
                        itask.Delete(t);
                        //return Json(new ResultMsg { Msg = "删除失败,任务已经被人执行", Result = false }, JsonRequestBehavior.AllowGet);
                    }

                    if (isdelete)
                    {
                        WriteLog(UserName + "删除分享任务 编号为" + share.ID.ToString());
                        if (share.Runstatues == 0)
                        {
                            AddUserPoint(totalppint - completepoint, "删除分享任务,编号为" + share.ID.ToString() + ",返回积分：" + (totalppint - completepoint).ToString());
                        }
                    }
                }

                return Json(new ResultMsg { Msg = "删除成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "删除失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
            
            
         
        }

        [HttpPost]
        public JsonResult StopSharing(List<Share> data)
        {
            PaginationInfo paging = new PaginationInfo();

            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string val = GetSettingVal(groupsetting, "11BE7BA7-C738-41C1-B78B-FFE54E9A8FFF");
                string val2 = GetSettingVal(groupsetting, "A39E4869-B383-43ED-92F0-5B2EA18B0BBA");
                string val3 = GetSettingVal(groupsetting, "09008380-8EE3-4D16-8858-F956044DE5E9");
                string val4 = GetSettingVal(groupsetting, "22F0B30D-8B72-4C6A-A193-0D1E26E74F02");
                string val5 = GetSettingVal(groupsetting, "5C16F422-14CE-4285-AC0B-1132471C32DA");
                string val6 = GetSettingVal(groupsetting, "DC2A7CED-31F2-42BE-A566-4DF62C163A93");
                string supperval = GetSettingVal(groupsetting, "E798DAD0-754B-450A-94DD-EF136AAAB991");

                foreach (Share s in data)
                {

                    Share share = ishare.Get(s.ID);
                    share.Runstatues = 3;
                    bool isupdate = ishare.Update(share);
                    decimal shareprice = 0;

                    if (share.Liked == 5000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 5000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val);
                    }
                    else if (share.Liked == 10000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val2) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 10000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val2);
                    }
                    else if (share.Liked == 20000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val3) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 20000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val3);
                    }
                    else if (share.Liked == 30000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val4) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 30000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val4);
                    }
                    else if (share.Liked == 50000 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val5) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 50000 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val5);
                    }
                    else if (share.Liked == 50001 && share.IsSuper)
                    {
                        shareprice = decimal.Parse(val6) * decimal.Parse(supperval);
                    }
                    else if (share.Liked == 50001 && !share.IsSuper)
                    {
                        shareprice = decimal.Parse(val6);
                    }

                    decimal totalppint = shareprice * 1;
                    decimal completepoint = 0;

                    IList<Task> starttasks = itask.Get(null, null, share.ID, null, null, null, null, 1, 0, null, out paging);
                    if (starttasks == null || starttasks.Count == 0)
                    {
                        completepoint = 0;

                    }
                    else
                    {

                        Task t = starttasks[0];
                        if (t.Runstatues == 0)
                        {
                            completepoint = 0;
                        }
                        else if (t.Runstatues == 1)
                        {
                            completepoint = 0;
                        }
                        else if (t.Runstatues == 2)
                        {
                            completepoint = shareprice * 1;
                        }
                        t.Runstatues = 3;
                        t.Statues = 0;
                        itask.Update(t);
                        //return Json(new ResultMsg { Msg = "删除失败,任务已经被人执行", Result = false }, JsonRequestBehavior.AllowGet);
                    }

                    if (isupdate)
                    {
                        WriteLog(UserName + "停止分享任务 编号为" + share.ID.ToString());
                        if (totalppint - completepoint == 0)
                        {
                            AddUserPoint(totalppint - completepoint, "停止分享任务,但是该任务已经被人完成,编号为" + share.ID.ToString() + ",返回积分：" + (totalppint - completepoint).ToString());
                        }
                        else
                        {
                            AddUserPoint(totalppint - completepoint, "停止分享任务,编号为" + share.ID.ToString() + ",返回积分：" + (totalppint - completepoint).ToString());
                        } 

                    }
                }

                return Json(new ResultMsg { Msg = "停止成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "停止失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReStartShare(List<Share> data)
        {
            PaginationInfo paging = new PaginationInfo();

            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string val = GetSettingVal(groupsetting, "11BE7BA7-C738-41C1-B78B-FFE54E9A8FFF");
                string val2 = GetSettingVal(groupsetting, "A39E4869-B383-43ED-92F0-5B2EA18B0BBA");
                string val3 = GetSettingVal(groupsetting, "09008380-8EE3-4D16-8858-F956044DE5E9");
                string val4 = GetSettingVal(groupsetting, "22F0B30D-8B72-4C6A-A193-0D1E26E74F02");
                string val5 = GetSettingVal(groupsetting, "5C16F422-14CE-4285-AC0B-1132471C32DA");
                string val6 = GetSettingVal(groupsetting, "DC2A7CED-31F2-42BE-A566-4DF62C163A93");
                string supperval = GetSettingVal(groupsetting, "E798DAD0-754B-450A-94DD-EF136AAAB991");
                decimal totalpoint = 0;

                foreach (Share s in data)
                {

                    Share share = ishare.Get(s.ID);
                    if (share.Runstatues == 3)
                    {
                     
                        if (share.Liked == 5000 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 5000 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val);
                        }
                        else if (share.Liked == 10000 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val2) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 10000 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val2);
                        }
                        else if (share.Liked == 20000 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val3) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 20000 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val3);
                        }
                        else if (share.Liked == 30000 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val4) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 30000 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val4);
                        }
                        else if (share.Liked == 50000 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val5) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 50000 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val5);
                        }
                        else if (share.Liked == 50001 && share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val6) * decimal.Parse(supperval);
                        }
                        else if (share.Liked == 50001 && !share.IsSuper)
                        {
                            totalpoint += decimal.Parse(val6);
                        }

                     
                    }
                }
                decimal alivepoint = GetAlivePoint();

                if (totalpoint > alivepoint)
                {
                    ResultMsg msg = new ResultMsg { Result = false, Msg = "可用积分不足" };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
              
                foreach (Share s in data)
                {

                    Share share = ishare.Get(s.ID);
                    if (share.Runstatues == 3)
                    {
                        share.Runstatues = 0;
                        bool isupdate = ishare.Update(share);

                        IList<Task> starttasks = itask.Get(null, null, share.ID, null, null, null, null, null, 0, null, out paging);
                        if (starttasks == null || starttasks.Count == 0)
                        {


                        }
                        else
                        {

                            Task t = starttasks[0];
                            t.Runstatues = 0;
                            t.Statues = 1;
                            itask.Update(t);
                            //return Json(new ResultMsg { Msg = "删除失败,任务已经被人执行", Result = false }, JsonRequestBehavior.AllowGet);
                        }

                        if (isupdate)
                        {
                            WriteLog(UserName + "重启分享任务 编号为" + share.ID.ToString());
                           

                        }
                    }
                }

                ReduceUserPoint(totalpoint, "重启分享任务，冻结积分：" + (totalpoint).ToString());
                return Json(new ResultMsg { Msg = "重新开始成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "重新开始失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
