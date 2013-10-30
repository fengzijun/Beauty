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
    using Beauty.Common;
    
    public class LikeController : BaseController
    {
        //
        // GET: /Like/
        private ILike ilike;
        private IBady ibady;
        private IShare ishare;
        private IUserSetting iusersetting;
        private ISetting isetting;
        private ITask itask;
        public LikeController(ILike ilike, IShare ishare, IBady ibady, ILog ilog, IUserSetting iusersetting,ISetting isetting,IUser iuser,IMoney imoney,ITask itask)
        {
            this.ilike = ilike;
            this.ishare = ishare;
            this.imoney = imoney;
            this.ibady = ibady;
            this.ilog = ilog;
            this.iuser = iuser;
            this.iusersetting = iusersetting;
            this.isetting = isetting;
            this.itask = itask;
        }

        public ActionResult Index(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, 2, 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Share share in shares)
            {
                share.Bady = ibady.Get(share.Bady.ID);
            }
            PagedList<Share> models = shares.ToPagedList<Share>(paging);
            //ShareModel sharemodel = new ShareModel { PageInfo = paging , Records = shares };
            //IList<Bady> badys = ibady.Get(null, UserName, null, 1, page.HasValue ? page.Value : 1, null, out paging);
            //PagedList<Bady> models = badys.ToPagedList<Bady>(paging);
            IList<SettingGroup> groupsettings = iusersetting.GetByUsername(UserName);

            int likerate = int.Parse(GetSettingVal(groupsettings, "3C309D27-C774-4E60-8706-17EA2C2B0691"));
            int recordrate = int.Parse(GetSettingVal(groupsettings, "E038EAB0-9F3D-4967-BE16-06E04633D6C8"));
            int likedrate = int.Parse(GetSettingVal(groupsettings, "6C715AE2-2D77-4827-B4C1-7E5352B7D00B"));
            int commentrate = int.Parse(GetSettingVal(groupsettings, "4E8E13CA-0726-4610-8CC5-1A5A3191A7BD"));
            ViewBag.likerate = likerate;
            ViewBag.recordrate = recordrate;
            ViewBag.likedrate = likedrate;
            ViewBag.commentrate = commentrate;

            return View(models);
        }

        public ActionResult liking(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Like> likes = ilike.GetLiking(null,null, UserName, null, 0,  1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Like like in likes)
            {
                like.Bady = ibady.Get(like.Bady.ID);
            }
            PagedList<Like> models = likes.ToPagedList<Like>(paging);
            //ShareModel sharemodel = new ShareModel { PageInfo = paging , Records = shares };
            //IList<Bady> badys = ibady.Get(null, UserName, null, 1, page.HasValue ? page.Value : 1, null, out paging);
            //PagedList<Bady> models = badys.ToPagedList<Bady>(paging);
          

            return View(models);
        }

        public ActionResult hasliked(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Like> likes = ilike.Get(null, null, UserName, 2, 0, 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Like like in likes)
            {
                like.Bady = ibady.Get(like.Bady.ID);
            }
            PagedList<Like> models = likes.ToPagedList<Like>(paging);
            //ShareModel sharemodel = new ShareModel { PageInfo = paging , Records = shares };
            //IList<Bady> badys = ibady.Get(null, UserName, null, 1, page.HasValue ? page.Value : 1, null, out paging);
            //PagedList<Bady> models = badys.ToPagedList<Bady>(paging);


            return View(models);
        }

        [HttpPost]
        public JsonResult PostLike(List<Like> data)
        {

            try
            {
                IList<SettingGroup> groupsettings = iusersetting.GetByUsername(UserName);

                int likerate = int.Parse(GetSettingVal(groupsettings, "3C309D27-C774-4E60-8706-17EA2C2B0691"));
                int recordrate = int.Parse(GetSettingVal(groupsettings, "E038EAB0-9F3D-4967-BE16-06E04633D6C8"));
                int likedrate = int.Parse(GetSettingVal(groupsettings, "6C715AE2-2D77-4827-B4C1-7E5352B7D00B"));
                int commentrate = int.Parse(GetSettingVal(groupsettings, "4E8E13CA-0726-4610-8CC5-1A5A3191A7BD"));

                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string likeprice = GetSettingVal(groupsetting, "65AD5D23-2619-480E-B6A9-84C58AD07DCB");
                string commentprice = GetSettingVal(groupsetting, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA");
                string likedprice = GetSettingVal(groupsetting, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55");
                string recordprice = GetSettingVal(groupsetting, "88841710-5B5F-4581-9557-85103CD65534");
                string superrecordprice = GetSettingVal(groupsetting, "6FF927CF-5676-43B2-A9B4-F72C346E22DE");

                decimal alivepoint = GetAlivePoint();

                decimal totalpoint = 0;
                foreach (Like like in data)
                {
                    
                    like.Recordnum = like.Likenum * recordrate / likerate;
                    like.Likednum = like.Likenum * likedrate / likerate;
                    like.Commentnum = like.Likenum * commentrate / likerate;
                    like.Supernum = like.Supernum > like.Likednum ? like.Likednum : like.Supernum;
                    if (like.Commentnum == 0 && !string.IsNullOrEmpty(like.Comment))
                    {
                        like.Commentnum = 1;
                    }
                    totalpoint += like.Likenum * decimal.Parse(likeprice);
                    totalpoint += like.Recordnum * decimal.Parse(recordprice);
                    totalpoint += like.Likednum * decimal.Parse(likedprice);
                    totalpoint += like.Commentnum * decimal.Parse(commentprice);
                    totalpoint += like.Supernum * decimal.Parse(superrecordprice);
                }

                if (alivepoint >= totalpoint)
                {

                    foreach (Like like in data)
                    {
                        if (!string.IsNullOrEmpty(like.Lnk))
                        {
                            var imgurl = string.Empty;
                            var title = string.Empty;
                            Helper.GetShareLinkInfo(like.Lnk, out imgurl, out title);
                            if (!string.IsNullOrEmpty(imgurl) && !string.IsNullOrEmpty(title))
                            {
                                Bady bady = new Bady
                                {
                                    ID = Guid.NewGuid(),

                                    ImageUrl = imgurl,

                                    Badydescription = title,
                                    Link = like.Lnk,
                                    Statues = 1,
                                    Twitterid = Helper.GetIDFromShareLink(like.Lnk),
                                    Username = UserName
                                };
                                Guid id = ibady.Create(bady);

                                Share share = new Share
                                {
                                    ID = Guid.NewGuid(),
                                    Bady = bady,
                                    Comment = like.Comment.Contains("###") ? like.Comment.Split(new string[] { "###" }, StringSplitOptions.None)[0] : like.Comment,
                                    IsSuper = false,
                                    Liked = 0,
                                    Keyword = like.Keyword,
                                    Username = UserName,
                                    Statues = 1,
                                    Runstatues = 2
                                };

                                id = ishare.Create(share);

                                like.Statues = 1;
                                like.ID = Guid.NewGuid();
                                like.Recordnum = like.Likenum * recordrate / likerate;
                                like.Likednum = like.Likenum * likedrate / likerate;
                                like.Commentnum = like.Likenum * commentrate / likerate;
                                like.Supernum = like.Supernum > like.Likednum ? like.Likednum : like.Supernum;
                                like.Bady = bady;
                               
                                if (string.IsNullOrEmpty(like.Username))
                                {
                                    like.Username = UserName;
                                }
                                like.Type = 0;
                                if (like.Commentnum == 0 && !string.IsNullOrEmpty(like.Comment))
                                {
                                    like.Commentnum = 1;
                                }

                                id = ilike.Create(like);
                            }
                            else
                            {
                                ResultMsg msg = new ResultMsg { Result = false, Msg = "请上传正确的美丽说分享连接" };
                                
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            like.Statues = 1;
                            like.ID = Guid.NewGuid();
                            like.Recordnum = like.Likenum * recordrate / likerate;
                            like.Likednum = like.Likenum * likedrate / likerate;
                            like.Commentnum = like.Likenum * commentrate / likerate;
                            like.Supernum = like.Supernum > like.Likednum ? like.Likednum : like.Supernum;
                            if (string.IsNullOrEmpty(like.Username))
                            {
                                like.Username = UserName;
                            }
                            like.Type = 0;
                            if (like.Commentnum == 0 && !string.IsNullOrEmpty(like.Comment))
                            {
                                like.Commentnum = 1;
                            }

                            Guid id = ilike.Create(like);
                            WriteLog(UserName + "分派喜欢任务 编号为" + id.ToString());
                        }
                    }

                    AddCrossActionMsg("OK", "发起喜欢任务成功");
                    ResultMsg msg2 = new ResultMsg { Result = true, Msg = "发起喜欢任务成功" };
                    ReduceUserPoint(totalpoint, "发布喜欢任务，冻结积分：" + ((int)totalpoint).ToString());
                    return Json(msg2, JsonRequestBehavior.AllowGet);
               
                }
                else
                {
                    ResultMsg msg = new ResultMsg { Result = false, Msg = "可用积分不足" };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                AddCrossActionMsg("Fail", "喜欢失败");
                ResultMsg msg = new ResultMsg { Result = false, Msg = "喜欢失败" };
                return Json(msg, JsonRequestBehavior.AllowGet);
             
            }
        }

        [HttpPost]
        public JsonResult Usersetting(string likerate, string recordrate, string likedrate, string commentrate)
        {
            //IList<SettingGroup> groupsettings = iusersetting.GetByUsername(UserName);
            try
            {
                PaginationInfo paging = new PaginationInfo();
                IList<UserSetting> settings = iusersetting.Get(null, UserName, new Guid("3C309D27-C774-4E60-8706-17EA2C2B0691"), 1, 0, null, out paging);
                if (settings == null || settings.Count == 0)
                {
                    UserSetting usersetting = new UserSetting
                    {
                        ID = Guid.NewGuid(),
                        Statues = 1,
                        Value = likedrate,
                        Settingid = new Guid("3C309D27-C774-4E60-8706-17EA2C2B0691"),
                        Username = UserName
                    };
                    iusersetting.Create(usersetting);
                }
                else
                {
                    UserSetting setting = settings[0];
                    setting.Value = likerate;
                    iusersetting.Update(setting);
                }

                settings = iusersetting.Get(null, UserName, new Guid("E038EAB0-9F3D-4967-BE16-06E04633D6C8"), 1, 0, null, out paging);
                if (settings == null || settings.Count == 0)
                {
                    UserSetting usersetting = new UserSetting
                    {
                        ID = Guid.NewGuid(),
                        Statues = 1,
                        Value = recordrate,
                        Settingid = new Guid("E038EAB0-9F3D-4967-BE16-06E04633D6C8"),
                        Username = UserName
                    };
                    iusersetting.Create(usersetting);
                }
                else
                {
                    UserSetting setting = settings[0];
                    setting.Value = recordrate;
                    iusersetting.Update(setting);
                }

                settings = iusersetting.Get(null, UserName, new Guid("6C715AE2-2D77-4827-B4C1-7E5352B7D00B"), 1, 0, null, out paging);
                if (settings == null || settings.Count == 0)
                {
                    UserSetting usersetting = new UserSetting
                    {
                        ID = Guid.NewGuid(),
                        Statues = 1,
                        Value = likedrate,
                        Settingid = new Guid("6C715AE2-2D77-4827-B4C1-7E5352B7D00B"),
                        Username = UserName
                    };
                    iusersetting.Create(usersetting);
                }
                else
                {
                    UserSetting setting = settings[0];
                    setting.Value = likedrate;
                    iusersetting.Update(setting);
                }

                settings = iusersetting.Get(null, UserName, new Guid("4E8E13CA-0726-4610-8CC5-1A5A3191A7BD"), 1, 0, null, out paging);
                if (settings == null || settings.Count == 0)
                {
                    UserSetting usersetting = new UserSetting
                    {
                        ID = Guid.NewGuid(),
                        Statues = 1,
                        Value = commentrate,
                        Settingid = new Guid("4E8E13CA-0726-4610-8CC5-1A5A3191A7BD"),
                        Username = UserName
                    };
                    iusersetting.Create(usersetting);
                }
                else
                {
                    UserSetting setting = settings[0];
                    setting.Value = commentrate;
                    iusersetting.Update(setting);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public JsonResult DeleteNoLike(List<Share> data)
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
        public JsonResult DeleteLiking(List<Like> data)
        {
            PaginationInfo paging = new PaginationInfo();

            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> systemsettings = isetting.GetSystemSetting();
                var likedprice = decimal.Parse(GetSettingVal(systemsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55"));
                var likeprice = decimal.Parse(GetSettingVal(systemsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB"));
                var commentprice = decimal.Parse(GetSettingVal(systemsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA"));
                var recordprice = decimal.Parse(GetSettingVal(systemsettings, "88841710-5B5F-4581-9557-85103CD65534"));
                var superrecordprice = decimal.Parse(GetSettingVal(systemsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE"));

                foreach (Like s in data)
                {

                    Like like = ilike.Get(s.ID);
                    bool isdelete = ilike.Delete(like);

                    decimal totalpoint = likeprice * like.Likenum + likedprice * like.Likednum + commentprice * like.Commentnum + recordprice * like.Recordnum + superrecordprice * like.Supernum;
                    decimal completepoint = 0;

                    IList<Task> starttasks = itask.Get(null, null, like.ID, null, null, null, null, 1, 0, null, out paging);

                    var likecompletenum = starttasks.Where(m => m.TaskType == "like" && m.Runstatues == 3).Count();
                    var likedcompletenum = starttasks.Where(m => m.TaskType == "liked" && m.Runstatues == 3).Count();
                    var commentcompletenum = starttasks.Where(m => m.TaskType == "commment" && m.Runstatues == 3).Count();
                    var recordcompletenum = starttasks.Where(m => m.TaskType == "record" && m.Runstatues == 3).Count();
                    var superrecordcompletenum = starttasks.Where(m => m.TaskType == "superrecord" && m.Runstatues == 3).Count();
                    completepoint = likeprice * likecompletenum + likedprice * likedcompletenum + commentprice * commentcompletenum + recordprice * recordcompletenum + superrecordprice * superrecordcompletenum;
                    foreach (Task t in starttasks)
                    {
                        if (t.Runstatues != 2)
                        {
                            itask.Delete(t);
                        }
                    }
                   

                    if (isdelete)
                    {
                        WriteLog(UserName + "删除喜欢任务 编号为" + like.ID.ToString());
                        if (like.Runstatues == 0 || like.Runstatues == 1)
                        {
                            AddUserPoint(totalpoint - completepoint, "删除喜欢任务,编号为" + like.ID.ToString() + ",返回积分：" + (totalpoint - completepoint).ToString());
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
        public JsonResult StopLiking(List<Like> data)
        {
            PaginationInfo paging = new PaginationInfo();

            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> systemsettings = isetting.GetSystemSetting();
                var likedprice = decimal.Parse(GetSettingVal(systemsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55"));
                var likeprice = decimal.Parse(GetSettingVal(systemsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB"));
                var commentprice = decimal.Parse(GetSettingVal(systemsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA"));
                var recordprice = decimal.Parse(GetSettingVal(systemsettings, "88841710-5B5F-4581-9557-85103CD65534"));
                var superrecordprice = decimal.Parse(GetSettingVal(systemsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE"));

                foreach (Like s in data)
                {

                    Like like = ilike.Get(s.ID);
                    like.Runstatues = 3;
                    bool isupdate = ilike.Update(like);

                    decimal totalpoint = likeprice * like.Likenum + likedprice * like.Likednum + commentprice * like.Commentnum + recordprice * like.Recordnum + superrecordprice * like.Supernum;
                    decimal completepoint = 0;

                    IList<Task> starttasks = itask.Get(null, null, like.ID, null, null, null, null, 1, 0, null, out paging);

                    var likecompletenum = starttasks.Where(m => m.TaskType == "like" && m.Runstatues == 2).Count();
                    var likedcompletenum = starttasks.Where(m => m.TaskType == "liked" && m.Runstatues == 2).Count();
                    var commentcompletenum = starttasks.Where(m => m.TaskType == "commment" && m.Runstatues == 2).Count();
                    var recordcompletenum = starttasks.Where(m => m.TaskType == "record" && m.Runstatues == 2).Count();
                    var superrecordcompletenum = starttasks.Where(m => m.TaskType == "superrecord" && m.Runstatues == 2).Count();
                    completepoint = likeprice * likecompletenum + likedprice * likedcompletenum + commentprice * commentcompletenum + recordprice * recordcompletenum + superrecordprice * superrecordcompletenum;
                    foreach (Task t in starttasks)
                    {
                        if (t.Runstatues != 2)
                        {
                            t.Runstatues = 3;
                            t.Statues = 0;
                            itask.Update(t);
                        }
                    }


                    if (isupdate)
                    {
                        WriteLog(UserName + "停止喜欢任务 编号为" + like.ID.ToString());
                        if (totalpoint - completepoint == 0)
                        {
                            AddUserPoint(totalpoint - completepoint, "停止喜欢任务,但是该任务已经被人完成,编号为" + like.ID.ToString() + ",返回积分：" + (totalpoint - completepoint).ToString());
                        }
                        else
                        {
                            AddUserPoint(totalpoint - completepoint, "停止喜欢任务,编号为" + like.ID.ToString() + ",返回积分：" + (totalpoint - completepoint).ToString());
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
        public JsonResult ReStartLike(List<Like> data)
        {
            PaginationInfo paging = new PaginationInfo();

            //IList<Share> shares = ishare.Get(null, UserName, null, null, null, null, null, 1, 0, null, out paging);
            //Share share = shares[0];
            //bool isdelete = ishare.Delete(share);
            try
            {
                IList<SettingGroup> systemsettings = isetting.GetSystemSetting();
                var likedprice = decimal.Parse(GetSettingVal(systemsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55"));
                var likeprice = decimal.Parse(GetSettingVal(systemsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB"));
                var commentprice = decimal.Parse(GetSettingVal(systemsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA"));
                var recordprice = decimal.Parse(GetSettingVal(systemsettings, "88841710-5B5F-4581-9557-85103CD65534"));
                var superrecordprice = decimal.Parse(GetSettingVal(systemsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE"));
                decimal totalpoint = 0;
                decimal completepoint = 0;
                foreach (Like l in data)
                {

                    Like like = ilike.Get(l.ID);
                    if (like.Runstatues == 3)
                    {

                        totalpoint += likeprice * like.Likenum + likedprice * like.Likednum + commentprice * like.Commentnum + recordprice * like.Recordnum + superrecordprice * like.Supernum;
                        IList<Task> starttasks = itask.Get(null, null, like.ID, null, null, null, null, 1, 0, null, out paging);
                        var likecompletenum = starttasks.Where(m => m.TaskType == "like" && m.Runstatues == 2).Count();
                        var likedcompletenum = starttasks.Where(m => m.TaskType == "liked" && m.Runstatues == 2).Count();
                        var commentcompletenum = starttasks.Where(m => m.TaskType == "commment" && m.Runstatues == 2).Count();
                        var recordcompletenum = starttasks.Where(m => m.TaskType == "record" && m.Runstatues == 2).Count();
                        var superrecordcompletenum = starttasks.Where(m => m.TaskType == "superrecord" && m.Runstatues == 2).Count();
                        completepoint = likeprice * likecompletenum + likedprice * likedcompletenum + commentprice * commentcompletenum + recordprice * recordcompletenum + superrecordprice * superrecordcompletenum;
                    }
                }

                decimal alivepoint = GetAlivePoint();

                if ((totalpoint - completepoint) > alivepoint)
                {
                    ResultMsg msg = new ResultMsg { Result = false, Msg = "可用积分不足" };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

                foreach (Like l in data)
                {

                    Like like = ilike.Get(l.ID);
                    if (like.Runstatues == 3)
                    {
                        like.Runstatues = 0;
                        bool isupdate = ilike.Update(like);

                        IList<Task> starttasks = itask.Get(null, null, like.ID, null, null, null, null, null, 0, null, out paging);
                        foreach (Task t in starttasks)
                        {
                            if (t.Runstatues != 2)
                            {
                                t.Runstatues = 0;
                                t.Statues = 1;
                                itask.Update(t);
                            }
                        }

                        if (isupdate)
                        {
                            WriteLog(UserName + "重启喜欢任务 编号为" + like.ID.ToString());


                        }
                    }
                }

                ReduceUserPoint((totalpoint - completepoint), "重启喜欢任务，冻结积分：" + (totalpoint - completepoint).ToString());
                return Json(new ResultMsg { Msg = "重新开始成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "重新开始失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Deleteliked(List<Like> data)
        {
            try
            {
                foreach (Like l in data)
                {
                    ilike.Delete(l);
                }
                return Json(new ResultMsg { Msg = "删除成功", Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new ResultMsg { Msg = "删除失败", Result = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
