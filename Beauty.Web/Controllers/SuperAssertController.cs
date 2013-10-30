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

    public class SuperAssertController : BaseController
    {
        private ITask itask;
        private IBady ibady;
        private ILike ilike;
        private IShare ishare;
        private IGroup igroup;
        private IUserStore iuserstore;
        private ISetting isetting;
        private IFirstPageArg ifirstpagearg;
        private IUserSetting iusersetting;

        public SuperAssertController(ITask itask, IBady ibady, ILike ilike, IShare ishare, IGroup igroup, IUserStore iuserstore, IFirstPageArg ifirstpagearg, ILog ilog, ISetting isetting, IUserSetting iusersetting,IUser iuser,IMoney imoney)
        {
            this.itask = itask;
            this.ibady = ibady;
            this.iuser = iuser;
            this.ilike = ilike;
            this.ishare = ishare;
            this.igroup = igroup;
            this.iuserstore = iuserstore;
            this.ifirstpagearg = ifirstpagearg;
            this.ilog = ilog;
            this.isetting = isetting;
            this.iusersetting = iusersetting;
            this.imoney = imoney;
        }

        public ActionResult WaitShare(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            PaginationInfo paging2 = new PaginationInfo();
            IList<Task> tasks = itask.Get(null, UserName, null, "share", null, false, 0, 1, page.HasValue ? page.Value : 1, null, out paging);
            foreach (Task task in tasks)
            {
                Share share = ishare.Get(task.Taskid);
                task.Share = share;
                task.Bady = ibady.Get(share.Bady.ID);

            }
            IList<Group> groups = igroup.Get(null, UserName, 1, 0, null, out paging2);
            IList<SelectListItem> items = new List<SelectListItem>();
            foreach (Group group in groups)
            {
                SelectListItem item = new SelectListItem();
                item.Text = group.Name;
                item.Value = group.ID;
                items.Add(item);
            }
            ViewBag.GroupList = items;
            PagedList<Task> models = tasks.ToPagedList<Task>(paging);

            return View(models);
        }


        [HttpPost]
        public JsonResult PostShare(List<Task> data)
        {

            try
            {
                foreach (Task task in data)
                {
                    //share.Statues = 1;
                    //share.ID = Guid.NewGuid();
                    task.IsAuto = true;
                    task.Statues = 1;
                    task.Autoflag = true;
                    
                    bool isupdate = itask.Update(task);
                    WriteLog(UserName + "手动执行分享任务(超级主编) 任务编号为" + task.ID.ToString());
                }
                AddCrossActionMsg("OK", "分享成功");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                AddCrossActionMsg("Fail", "分享失败");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult WaitRecord(int? page)
        {
            PaginationInfo paging = new PaginationInfo();
            PaginationInfo paging2 = new PaginationInfo();
            IList<Task> tasks = itask.GetUnAutoRecordTask(UserName, 1, page.HasValue ? page.Value : 1, null, out paging);
            //IList<Task> tasks2 = itask.Get(null, UserName, null, "superrecord", null, false, 0, 1, page.HasValue ? page.Value : 1, null, out paging);
            //IList<Task> alltasks = tasks.Concat(tasks2) as IList<Task>;
            foreach (Task task in tasks)
            {
                Like like = ilike.Get(task.Taskid);
                task.Like = like;
                task.Bady = ibady.Get(like.Bady.ID);

            }

            IList<Group> groups = igroup.Get(null, UserName, 1, 0, null, out paging2);
            IList<SelectListItem> items = new List<SelectListItem>();
            foreach (Group group in groups)
            {
                SelectListItem item = new SelectListItem();
                item.Text = group.Name;
                item.Value = group.ID;
                items.Add(item);
            }
            ViewBag.GroupList = items;
          
            PagedList<Task> models = tasks.ToPagedList<Task>(paging);

            return View(models);
        }


        [HttpPost]
        public JsonResult PostRecord(List<Task> data)
        {

            try
            {
                foreach (Task task in data)
                {
                    //share.Statues = 1;
                    //share.ID = Guid.NewGuid();
                    task.IsAuto = true;
                    task.Statues = 1;
                    task.Autoflag = true;
                    WriteLog(UserName + "手动执行收录任务(超级主编) 任务编号为" + task.ID.ToString());
                    bool isupdate = itask.Update(task);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GotoFirst(int? page)
        {
            try
            {
                PaginationInfo paging = new PaginationInfo();
                PaginationInfo paging2 = new PaginationInfo();
                IList<UserStore> userstores = iuserstore.Get(null, UserName, null, 1, page.HasValue ? page.Value : 1, null, out paging);
                IList<SettingGroup> systemsettings = isetting.GetSystemSetting();
                IList<SettingGroup> usersetings = iusersetting.GetByUsername(UserName);
                int likedrate = int.Parse(GetSettingVal(systemsettings, "3C309D27-C774-4E60-8706-17EA2C2B0691"));
                int likerate = int.Parse(GetSettingVal(systemsettings, "6C715AE2-2D77-4827-B4C1-7E5352B7D00B"));

                float superrate = float.Parse(GetSettingVal(systemsettings, "4657EED5-A7C5-4058-8F37-797B6155B2A0"));

                foreach (UserStore userstore in userstores)
                {
                    userstore.Bady = ibady.Get(userstore.Bady.ID);
                    IList<Like> likes = ilike.Get(null, userstore.Bady.ID.ToString(), UserName, null, 1, 1, 0, null, out paging2);
                    if (likes != null && likes.Count > 0)
                    {
                        userstore.taskstatus = likes[0].Runstatues;
                        userstore.Like = likes[0];
                    }
                    else
                    {
                        userstore.taskstatus = null;
                    }


                    IList<FirstPageArg> firstpageargs = ifirstpagearg.Get(null, userstore.Type, "hot", 1, 0, null, out paging2);
                    if (firstpageargs != null && firstpageargs.Count == 1)
                    {
                        FirstPageArg firstpagearg = firstpageargs[0];
                        userstore.needlike = firstpagearg.LikeArg - userstore.Liked > 0 ? firstpagearg.LikeArg - userstore.Liked : 0;
                        userstore.needrecord = firstpagearg.RecordArg - userstore.Record > 0 ? firstpagearg.RecordArg - userstore.Record : 0;
                        userstore.needcommment = firstpagearg.CommentArg - userstore.Comment > 0 ? firstpagearg.CommentArg - userstore.Comment : 0;
                        userstore.needliked = userstore.needlike * likerate / likedrate;
                     
                     
                    }
                    else
                    {
                        int likearg = 0;
                        int recordarg = 0;
                        int commentarg = 0;
                        if (firstpageargs.Count > 1)
                        {
                            foreach (FirstPageArg f in firstpageargs)
                            {
                                likearg += f.LikeArg;
                                recordarg += f.RecordArg;
                                commentarg += f.CommentArg;
                            }
                            likearg = likearg / firstpageargs.Count;
                            recordarg = recordarg / firstpageargs.Count;
                            commentarg = commentarg / firstpageargs.Count;
                            userstore.needlike = likearg - userstore.Liked > 0 ? likearg - userstore.Liked : 0;
                            userstore.needrecord = recordarg - userstore.Record > 0 ? recordarg - userstore.Record : 0;
                            userstore.needcommment = commentarg - userstore.Comment > 0 ? commentarg - userstore.Comment : 0;
                            userstore.needliked = userstore.needlike * likerate / likedrate;
                        }
                        else
                        {
                            //默认值
                            likearg = 500;//
                            recordarg = 50;
                            commentarg = 20;
                            userstore.needlike = likearg - userstore.Liked > 0 ? likearg - userstore.Liked : 0;
                            userstore.needrecord = recordarg - userstore.Record > 0 ? recordarg - userstore.Record : 0;
                            userstore.needcommment = commentarg - userstore.Comment > 0 ? commentarg - userstore.Comment : 0;
                            userstore.needliked = userstore.needlike * likerate / likedrate;
                        }
                    }

                    userstore.needrecord = userstore.needrecord - Convert.ToInt32(userstore.needrecord * superrate);
                    userstore.needsuper = Convert.ToInt32(userstore.needrecord * superrate);
                    //300 60 0 60
                }



                ViewBag.likedprice = decimal.Parse(GetSettingVal(systemsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55"));
                ViewBag.likeprice = decimal.Parse(GetSettingVal(systemsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB"));
                ViewBag.commentprice = decimal.Parse(GetSettingVal(systemsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA"));
                ViewBag.recordprice = decimal.Parse(GetSettingVal(systemsettings, "88841710-5B5F-4581-9557-85103CD65534"));
                ViewBag.supperrecordprice = decimal.Parse(GetSettingVal(systemsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE"));
                ViewBag.pricerate = decimal.Parse(GetSettingVal(systemsettings, "2AADE1C6-59BE-4B0E-927C-7B28BFC8C397"));

                PagedList<UserStore> models = userstores.ToPagedList<UserStore>(paging);
                return View(models);
            }
            catch(Exception ex)
            {
                
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                  "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
               
                WriteLog(errorMsg);
                return View();
            }

            

        }

        [HttpPost]
        public JsonResult PostLike(UserStore data)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Like> likes = ilike.Get(null, data.Bady.ID.ToString(), UserName, 3, 1, 1, 0, null, out paging);
            if (likes != null && likes.Count > 0)
            {
                Like like = likes[0];
                like.Likednum = data.needliked;
                like.Commentnum = data.needcommment;
                like.Recordnum = data.needrecord;
                like.Likenum = data.needlike;
                like.Supernum = data.needsuper;
                like.Comment = data.msg;

                decimal totalpoint = 0;
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string likeprice = GetSettingVal(groupsetting, "65AD5D23-2619-480E-B6A9-84C58AD07DCB");
                string commentprice = GetSettingVal(groupsetting, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA");
                string likedprice = GetSettingVal(groupsetting, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55");
                string recordprice = GetSettingVal(groupsetting, "88841710-5B5F-4581-9557-85103CD65534");
                string supperrecordprice = GetSettingVal(groupsetting, "6FF927CF-5676-43B2-A9B4-F72C346E22DE");
                decimal alivepoint = GetAlivePoint();

                totalpoint += data.needlike * decimal.Parse(likeprice);
                totalpoint += data.needrecord * decimal.Parse(recordprice);
                totalpoint += data.needliked * decimal.Parse(likedprice);
                totalpoint += data.needcommment * decimal.Parse(commentprice);
                totalpoint += data.needsuper * decimal.Parse(supperrecordprice);

                if (alivepoint >= totalpoint)
                {
                    like.Statues = 0;
                    bool isupdate = ilike.Update(like);
                    if (isupdate)
                    {
                        like.ID = Guid.NewGuid();
                        like.Statues = 1;
                        like.Runstatues = 0;
                        Guid iscreate = ilike.Create(like);
                        if (iscreate == like.ID)
                        {
                            WriteLog(UserName + "分派了首页直达任务 编号为" + like.ID.ToString());
                            ResultMsg msg = new ResultMsg { Msg = "开始成功", Result = true };
                            ReduceUserPoint(totalpoint, "发布首页直达任务，冻结积分：" + ((int)totalpoint).ToString());
                            return Json(msg, JsonRequestBehavior.AllowGet);


                        }
                        else
                        {
                            ResultMsg msg = new ResultMsg { Msg = "开始失败", Result = false };
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        ResultMsg msg = new ResultMsg { Msg = "开始失败", Result = false };
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ResultMsg msg = new ResultMsg { Msg = "可用积分不足", Result = false };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                decimal totalpoint = 0;
                IList<SettingGroup> groupsetting = isetting.GetSystemSetting();
                string likeprice = GetSettingVal(groupsetting, "65AD5D23-2619-480E-B6A9-84C58AD07DCB");
                string commentprice = GetSettingVal(groupsetting, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA");
                string likedprice = GetSettingVal(groupsetting, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55");
                string recordprice = GetSettingVal(groupsetting, "88841710-5B5F-4581-9557-85103CD65534");
                string supperrecordprice = GetSettingVal(groupsetting, "6FF927CF-5676-43B2-A9B4-F72C346E22DE");

                decimal alivepoint = GetAlivePoint();

                totalpoint += data.needlike * decimal.Parse(likeprice);
                totalpoint += data.needrecord * decimal.Parse(recordprice);
                totalpoint += data.needliked * decimal.Parse(likedprice);
                totalpoint += data.needcommment * decimal.Parse(commentprice);
                totalpoint += data.needsuper * decimal.Parse(supperrecordprice);
                //totalpoint += like.Supernum * decimal.Parse(superrecordprice);
                if (alivepoint >= totalpoint)
                {
                    Like like = new Like
                    {
                        ID = Guid.NewGuid(),
                        Likednum = data.needliked,
                        Commentnum = data.needcommment,
                        Recordnum = data.needrecord,
                        Likenum = data.needlike,
                        Bady = new Bady { ID = data.Bady.ID },
                        
                        Type = 1,
                        Comment = data.msg,
                        Runstatues = 0,
                        Statues = 1,
                        Supernum = data.needsuper,
                        Username = UserName

                    };

                    Guid id = ilike.Create(like);
                    WriteLog(UserName + "分派了首页直达任务 编号为" + like.ID.ToString());
                    if (id == like.ID)
                    {
                        ResultMsg msg = new ResultMsg { Msg = "开始成功", Result = true };
                        ReduceUserPoint(totalpoint, "发布首页直达任务，冻结积分：" + ((int)totalpoint).ToString());
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ResultMsg msg = new ResultMsg { Msg = "开始失败", Result = false };
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ResultMsg msg = new ResultMsg { Msg = "可用积分不足", Result = false };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

            }
        }


        [HttpPost]
        public JsonResult PostLikeStop(UserStore data)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<Like> likes = ilike.Get(null, data.Bady.ID.ToString(), UserName, null, 1, 1, 0, null, out paging);
            Like like = likes[0];
            like.Runstatues = 3;
            bool isupdate = ilike.Update(like);
            IList<Task> tasks = itask.Get(null, null, like.ID, null, null, null, 2, 1, 0, null, out paging);
            IList<Task> unstarttasks = itask.Get(null, null, like.ID, null, null, null, 0, 1, 0, null, out paging);

            IList<SettingGroup> systemsettings = isetting.GetSystemSetting();
            var likedprice = decimal.Parse(GetSettingVal(systemsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55"));
            var likeprice = decimal.Parse(GetSettingVal(systemsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB"));
            var commentprice = decimal.Parse(GetSettingVal(systemsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA"));
            var recordprice = decimal.Parse(GetSettingVal(systemsettings, "88841710-5B5F-4581-9557-85103CD65534"));
            var superrecordprice = decimal.Parse(GetSettingVal(systemsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE"));

            decimal totalpoint = likeprice * like.Likenum + likedprice * like.Likednum + commentprice * like.Commentnum + recordprice * like.Recordnum + superrecordprice * like.Supernum;

            var likecompletenum = tasks.Where(m => m.TaskType == "like").Count();
            var likedcompletenum = tasks.Where(m => m.TaskType == "liked").Count();
            var commentcompletenum = tasks.Where(m => m.TaskType == "commment").Count();
            var recordcompletenum = tasks.Where(m => m.TaskType == "record").Count();
            var superrecordcompletenum = tasks.Where(m => m.TaskType == "superrecord").Count();
            decimal completepoint = likeprice * likecompletenum + likedprice * likedcompletenum + commentprice * commentcompletenum + recordprice * recordcompletenum + superrecordprice * superrecordcompletenum;

            ResultMsg msg = new ResultMsg { Msg = "停止失败", Result = false };
            if (isupdate)
            {
                foreach (Task t in unstarttasks)
                {
                    t.Statues = 0;
                    itask.Update(t);
                }
                WriteLog(UserName + "停止首页直达任务 编号为" + like.ID.ToString());
                AddUserPoint(totalpoint - completepoint, "停止首页直接任务，返回积分：" + ((totalpoint - completepoint)).ToString());
                msg = new ResultMsg { Msg = "停止成功", Result = true };
                return Json(msg, JsonRequestBehavior.AllowGet);
               
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
