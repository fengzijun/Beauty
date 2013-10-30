using Beauty.Core;
using Beauty.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Beauty.Model;
using Beauty.Api.Model;

namespace Beauty.API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BeautyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BeautyService.svc or BeautyService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BeautyService : IBeautyService
    {
        public BoolResponse CheckUser(string username, string password)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                if (user.Password == password)
                {
                    return new BoolResponse { Message = "", Result = true };
                }
            }

            return new BoolResponse { Message = "", Result = false };
        }

        public BoolResponse Risgter(string username, string password, string email, string mobile, string zfb,string province,string city,string qq, string address,string refer)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            PaginationInfo paging = new PaginationInfo();

            IList<User> referusers = userds.Get(null, refer, null, null, null, null, null, null, null, null, 1, 0, null, out paging);

            if (referusers == null || referusers.Count == 0)
            {
                return new BoolResponse { Message = "邀请码不正确", Result = false };
            }

            IList<User> users = userds.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);

            if (users != null && users.Count > 0)
            {
                return new BoolResponse { Message = "用户名已经存在", Result = false };
            }

            users = userds.Get(null, null, null, null, email, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                return new BoolResponse { Message = "邮箱已经存在", Result = false };
            }

            User newuser = new User();
            newuser.Role = 2;
            newuser.Statues = 1;
            newuser.IsSuper = false;
            newuser.Liked = 0;
            newuser.Username = username;
            newuser.Password = password;
            newuser.Email = email;
            newuser.Mobile = mobile;
            newuser.ZFB = zfb;
            newuser.IsLogin = true;
            newuser.QQ = qq;
            newuser.Province = province;
            newuser.City = city;
            newuser.ShopAddress = address;
            newuser.ID = Guid.NewGuid();
            newuser.Lastlogintime = DateTime.Now;
            newuser.Point = 0;
            newuser.Balance = 0;
            newuser.Refer = refer;

            Guid userid = userds.Create(newuser);

            if (userid == newuser.ID)
            {
                return new BoolResponse { Message = "注册成功", Result = true };
            }

            return new BoolResponse { Message = "注册失败", Result = true };
        }

        public BoolResponse ForgetPassword(string username, string email)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            var usertokends = DataServiceContainer.Self.GetService<IUserToken>();
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, username, null, null, email, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                string msgcontent = "<html>  <head><base target='_blank'></head>  <body style='margin-bottom: 0px; margin-top: 0px; padding-bottom: 0px; padding-top: 0px; '>          <p>         请点击以下链接来重置你的密码:          </p>            <p>          <a href='{0}/account/resetpassword?token={1}' target='_blank'>{0}/account/resetpassword?token={1}</a>          </p>  </body></html>";
                string emailaccount = System.Configuration.ConfigurationManager.AppSettings["emailaccount"];
                string emailpassword = System.Configuration.ConfigurationManager.AppSettings["emailpassword"];
                string smtp = System.Configuration.ConfigurationManager.AppSettings["smtp"];
                string port = System.Configuration.ConfigurationManager.AppSettings["port"];
                string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];

                Guid token = Guid.NewGuid();
                bool issuc = Beauty.Common.Helper.SendEmail(emailaccount, emailpassword, smtp, port, users[0].Email, "美丽说分享宝重置密码", string.Format(msgcontent, domain, token.ToString()));
                if (issuc)
                {
                    Guid id = usertokends.Create(new UserToken { ID = token, Userid = users[0].ID, Statues = 1 });
                    if (id == token)
                    {
                        return new BoolResponse { Result = true, Message = "邮件发送成功" };
                    }
                }

                return new BoolResponse { Result = false, Message = "邮件发送失败" };
            }

            return new BoolResponse { Result = false, Message = "用户名或者邮箱填写错误" };
        }

        public void UpdateGroup(string username, string groupid, string groupname)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            var groupds = DataServiceContainer.Self.GetService<IGroup>();
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                groupds.Create(new Group { Name = groupname, Statues = 1, ID = groupid, Username = username });
            }
        }

        public void UpdateUserInfo(string username, int liked, bool issuper,string userid,string account,int type)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            var useraccountds = DataServiceContainer.Self.GetService<IUserAccount>();
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.Liked = liked;
                user.IsSuper = issuper;
                userds.Update(user);
            }

            useraccountds.Create(new UserAccount { ID = Guid.NewGuid(), twitterid = userid, type = type, account = account, username = username, Statues =1 });
        }

        public IList<WebSettingGroup> GetUserSetting(string username)
        {
            var usersettingds = DataServiceContainer.Self.GetService<IUserSetting>();
            IList<SettingGroup> settings = usersettingds.GetByUsername(username);
            IList<WebSettingGroup> webgroups = new List<WebSettingGroup>();
            webgroups = AutoMapper.Mapper.Map<IList<WebSettingGroup>>(settings);
            //foreach (SettingGroup settinggroup in settings)
            //{
            //    webgroups.Add(new WebSettingGroup { Category = settinggroup.Category, settings = AutoMapper.Mapper.Map<IList<WebSetting>>(settinggroup.settings) });
            //}
            return webgroups;
        }

        public WebShare GetShare(string id)
        {
            var shareds = DataServiceContainer.Self.GetService<IShare>();
            Share share = shareds.Get(new Guid(id));
            return AutoMapper.Mapper.Map<WebShare>(share);
        }

        public WebShare GetShareByBady(string id)
        {
            var shareds = DataServiceContainer.Self.GetService<IShare>();
            PaginationInfo paging = new PaginationInfo();
            IList<Share> shares = shareds.Get(null,null,null,id,null,null,2,1,0,null,out paging);
            if (shares != null && shares.Count > 0)
            {
                WebShare webshare = AutoMapper.Mapper.Map<WebShare>(shares[0]);
                return webshare;
            }

            return null;
        }

        public WebBady GetBady(string id)
        {
            var badyds = DataServiceContainer.Self.GetService<IBady>();
            Bady bady = badyds.Get(new Guid(id));
            return AutoMapper.Mapper.Map<WebBady>(bady);
        }


        public IList<WebTask> GetUserTask(string username)
        {
            var taskds = DataServiceContainer.Self.GetService<ITask>();
            PaginationInfo paging = new PaginationInfo();
            IList<Task> tasks = taskds.Get(null, username, null, null, null, true, 0, 1, 0, null, out paging);

            return AutoMapper.Mapper.Map<IList<WebTask>>(tasks);
        }

        public WebGroup GetGroup(string id)
        {
            var groupds = DataServiceContainer.Self.GetService<IGroup>();
            Group group = groupds.Get(id);
            return AutoMapper.Mapper.Map<WebGroup>(group);
        }

        public WebLike GetLike(string id)
        {
            var likeds = DataServiceContainer.Self.GetService<ILike>();
            Like like = likeds.Get(new Guid(id));
            return AutoMapper.Mapper.Map<WebLike>(like);
        }

        public void CompleteShareTask(string taskid, string shareid, string badyid, string goodid, string twriteid, string groupid)
        {
            var taskds = DataServiceContainer.Self.GetService<ITask>();
            var shareds = DataServiceContainer.Self.GetService<IShare>();
            var badyds = DataServiceContainer.Self.GetService<IBady>();
            var settingds = DataServiceContainer.Self.GetService<ISetting>();
            var userds = DataServiceContainer.Self.GetService<IUser>();
            Task t = taskds.Get(new Guid(taskid));
            t.Runstatues = 2;
            taskds.Update(t);

            Share s = shareds.Get(new Guid(shareid));
            s.Runstatues = 2;
            shareds.Update(s);

            Bady b = badyds.Get(new Guid(badyid));
            b.BadyId = goodid;
            b.Twitterid = twriteid;
            b.Groupid = groupid;
            badyds.Update(b);


            IList<SettingGroup> groupsetting = settingds.GetSystemSetting();
            //string val = GetSettingVal(groupsettings, "BBA6A74C-85F1-4612-AF9E-525599396A0A");
            string serviceval = GetSettingVal(groupsetting, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");//

            string val = GetSettingVal(groupsetting, "11BE7BA7-C738-41C1-B78B-FFE54E9A8FFF");
            string val2 = GetSettingVal(groupsetting, "A39E4869-B383-43ED-92F0-5B2EA18B0BBA");
            string val3 = GetSettingVal(groupsetting, "09008380-8EE3-4D16-8858-F956044DE5E9");
            string val4 = GetSettingVal(groupsetting, "22F0B30D-8B72-4C6A-A193-0D1E26E74F02");
            string val5 = GetSettingVal(groupsetting, "5C16F422-14CE-4285-AC0B-1132471C32DA");
            string val6 = GetSettingVal(groupsetting, "DC2A7CED-31F2-42BE-A566-4DF62C163A93");
            string supperval = GetSettingVal(groupsetting, "E798DAD0-754B-450A-94DD-EF136AAAB991");

            //decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
            decimal addpoint = 0;
            if (s.Liked == 5000 && s.IsSuper)
            {
                addpoint += decimal.Parse(val) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 5000 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 10000 && s.IsSuper)
            {
                addpoint += decimal.Parse(val2) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 10000 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val2) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 20000 && s.IsSuper)
            {
                addpoint += decimal.Parse(val3) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 20000 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val3) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 30000 && s.IsSuper)
            {
                addpoint += decimal.Parse(val4) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 30000 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val4) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 50000 && s.IsSuper)
            {
                addpoint += decimal.Parse(val5) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 50000 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val5) * (1 - decimal.Parse(serviceval)) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 50001 && s.IsSuper)
            {
                addpoint += decimal.Parse(val6) * decimal.Parse(supperval) * (1 - decimal.Parse(serviceval));
            }
            else if (s.Liked == 50001 && !s.IsSuper)
            {
                addpoint += decimal.Parse(val6) * (1 - decimal.Parse(serviceval));
            }

            addpoint = decimal.Round(addpoint, 2);
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.Point += addpoint;
                userds.Update(user);
                MoneyRecord("完成分享任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
            }

            users = userds.Get(null, s.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                User user = users[0];
                user.Point -= decimal.Parse(val);
                user.FreezePoint -= decimal.Parse(val);
                userds.Update(user);
                MoneyRecord("分享任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
            }
        }

        public void MoneyRecord(string type,string username,decimal balance)
        {
            var moneyds = DataServiceContainer.Self.GetService<IMoney>();
            
            MoneyRecord money = new MoneyRecord
            {
                ID = Guid.NewGuid(),
                Money = 0,
                Statues = 1,
                Username = username,
                Balance = balance,
                Type = type
            };

            moneyds.Create(money);
        }

        public void CompletelikeTask(string taskid, string likeid)
        {
            var taskds = DataServiceContainer.Self.GetService<ITask>();
            var settingds = DataServiceContainer.Self.GetService<ISetting>();
            var userds = DataServiceContainer.Self.GetService<IUser>();
            var likeds = DataServiceContainer.Self.GetService<ILike>();
            Task t = taskds.Get(new Guid(taskid));
            t.Runstatues = 2;
            taskds.Update(t);
            Like l = likeds.Get(t.Taskid);
            if (t.TaskType == "like")
            {
                IList<SettingGroup> groupsettings = settingds.GetSystemSetting();
                string val = GetSettingVal(groupsettings, "65AD5D23-2619-480E-B6A9-84C58AD07DCB");
                string serviceval = GetSettingVal(groupsettings, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");

                decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
                addpoint = decimal.Round(addpoint, 2);
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point += addpoint;
                    userds.Update(user);
                    MoneyRecord("完成喜欢任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
                }

                users = userds.Get(null, l.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point -= decimal.Parse(val);
                    user.FreezePoint -= decimal.Parse(val);
                    userds.Update(user);
                    MoneyRecord("喜欢任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
                }
            }
            else if (t.TaskType == "comment")
            {
                IList<SettingGroup> groupsettings = settingds.GetSystemSetting();
                string val = GetSettingVal(groupsettings, "4D2E7311-BA9F-468B-BC3B-3F938C9CC3EA");
                string serviceval = GetSettingVal(groupsettings, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");
                decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
                addpoint = decimal.Round(addpoint, 2);
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point += addpoint;
                    userds.Update(user);
                    MoneyRecord("完成评论任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
                }

                users = userds.Get(null, l.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point -= decimal.Parse(val);
                    user.FreezePoint -= decimal.Parse(val);
                    userds.Update(user);
                    MoneyRecord("评论任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
                }
            }
            else if (t.TaskType == "record")
            {
                IList<SettingGroup> groupsettings = settingds.GetSystemSetting();
                string val = GetSettingVal(groupsettings, "88841710-5B5F-4581-9557-85103CD65534");

                string serviceval = GetSettingVal(groupsettings, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");
                decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
                addpoint = decimal.Round(addpoint, 2);
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point += addpoint;
                    userds.Update(user);
                    MoneyRecord("完成收录任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
                }

                users = userds.Get(null, l.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point -= decimal.Parse(val);
                    user.FreezePoint -= decimal.Parse(val);
                    userds.Update(user);
                    MoneyRecord("收录任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
                }
            }
            else if (t.TaskType == "superrecord")
            {
                IList<SettingGroup> groupsettings = settingds.GetSystemSetting();
                string val = GetSettingVal(groupsettings, "6FF927CF-5676-43B2-A9B4-F72C346E22DE");

                string serviceval = GetSettingVal(groupsettings, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");
                decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
                addpoint = decimal.Round(addpoint, 2);
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point += addpoint;
                    userds.Update(user);
                    MoneyRecord("完成收录任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
                }

                users = userds.Get(null, l.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point -= decimal.Parse(val);
                    user.FreezePoint -= decimal.Parse(val);
                    userds.Update(user);
                    MoneyRecord("收录任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
                }
            }
            else if (t.TaskType == "liked")
            {
                IList<SettingGroup> groupsettings = settingds.GetSystemSetting();
                string val = GetSettingVal(groupsettings, "FB8E6B56-DEA6-4D70-9C52-E451E7423F55");

                string serviceval = GetSettingVal(groupsettings, "93D5A072-4318-49D5-A1B8-9AAE4C752F26");
                decimal addpoint = decimal.Parse(val) * (1 - decimal.Parse(serviceval));
                addpoint = decimal.Round(addpoint, 2);
                PaginationInfo paging = new PaginationInfo();
                IList<User> users = userds.Get(null, t.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point += addpoint;
                    userds.Update(user);
                    MoneyRecord("完成粉丝任务，增加积分：" + addpoint.ToString(), user.Username, user.Point - user.FreezePoint);
                }

                users = userds.Get(null, l.Username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
                if (users != null && users.Count > 0)
                {
                    User user = users[0];
                    user.Point -= decimal.Parse(val);
                    user.FreezePoint -= decimal.Parse(val);
                    userds.Update(user);
                    MoneyRecord("粉丝任务被完成，扣除积分：" + val, user.Username, user.Point - user.FreezePoint);
                }
            }

            taskds.CheckTaskComplete(new Guid(taskid), new Guid(likeid));
        }

        public bool LoginActive(string username,string ip)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            var usercountds = DataServiceContainer.Self.GetService<IUserLoginCount>();
            usercountds.Create(new UserLoginCount { ID = Guid.NewGuid(), username = username, Statues = 1 });
            return userds.LoginActive(username,ip);
        }

        public void Log(string msg)
        {
            Log log = new Log
            {
                ID = Guid.NewGuid(),
                Msg = msg,
                Statues = 1
            };

            var logds = DataServiceContainer.Self.GetService<ILog>();
            logds.Create(log);
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

        public WebUser GetUser(string username)
        {
            var userds = DataServiceContainer.Self.GetService<IUser>();
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = userds.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                return AutoMapper.Mapper.Map<WebUser>(users[0]);
            }

            return null;
        }


        public IList<WebNotice> GetNotices(string userid)
        {
            var noticeds = DataServiceContainer.Self.GetService<INotice>();
            PaginationInfo paging = new PaginationInfo();
            IList<Notice> notices = noticeds.GetOneSystemNoice(null, userid, 0, 1, 1, null, out paging);

            IList<Notice> notices2 = noticeds.Get(null, 1, 1, 1, null, out paging);

            notices = notices.Concat(notices2).ToList();

            return AutoMapper.Mapper.Map<IList<WebNotice>>(notices);
        }

        public void ReadBeautyNotice(string userid, string noticeid)
        {
            var noticeds = DataServiceContainer.Self.GetService<INotice>();
            ReadNoitce model = new ReadNoitce
            {
                ID = Guid.NewGuid(),
                Userid = userid,
                Noticeid = noticeid
            };

            noticeds.ReadNotice(model);
        }
    }
}
