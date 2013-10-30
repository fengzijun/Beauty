using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Beauty.App.Model;
using Beauty.App.Temp;
using Beauty.Common;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;


namespace Beauty.App
{
    public class UserAction
    {
        private UserInfo user;
        private BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
        private string ip;
        private static object lockobject = new object();

        public UserAction(UserInfo user)
        {
            this.user = user;
        }

        public void writelog(string msg)
        {
            string path = Application.StartupPath + "\\log";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            lock (lockobject)
            {
                string filename = DateTime.Now.ToString("yyyyMMdd");
                StreamWriter sw = new StreamWriter(path + "\\" + filename + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString()+"----------"+msg);
                sw.Close();
            }
            
        }
        /// <summary>
        /// 获取用户分组
        /// </summary>
        public void GetGroups(object statues)
        {
            //http://www.meilishuo.com/aj/magazine/user_groups
       
            try
            {
                //[{"group_id":"42454433","name":"fengzijun\u559c\u6b22\u7684\u5b9d\u8d1d","role":"1"},{"group_id":"42454373","name":"fengzijun","role":"1"}]
                string html = HttpHelper.GetHtml("http://www.meilishuo.com/aj/magazine/user_groups", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, "", true, user.Cookie, null, Encoding.GetEncoding("gb2312"), null, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<UserGroup> objs = serializer.Deserialize<List<UserGroup>>(html);
                if (objs != null && objs.Count > 0)
                {
                    foreach (UserGroup usergroup in objs)
                    {
                        //client.UpdateGroup(user.Username, usergroup.group_id, usergroup.name);
                        client.BeginUpdateGroup(user.Username, usergroup.group_id, usergroup.name, null, null);
                        writelog("创建分组，分组名：" + usergroup.name);
                    }
                }
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.Log(errorMsg);
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public void GetUserInfo(object statues)
        {
            try
            {
               
                string id = GetUserid();
                int type = 0;
                //http://www.meilishuo.com/person/u/53558319
                string html = HttpHelper.GetHtml("http://www.meilishuo.com/person/u/" + id, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, user.Cookie, null, Encoding.UTF8);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                //HtmlNode navnode = doc.DocumentNode.SelectSingleNode(@"//ul[@class='nav_list']");
                //HtmlNodeCollection nc = navnode.SelectNodes(@"//li/p/a");
                HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//a[@title='美丽说超级主编']");
                bool issuper = false;
                if (node != null)
                {
                    issuper = true;
                    type = 1;
                }
                node = doc.DocumentNode.SelectSingleNode(@"//span[@title='美丽说特别认证']");
                if (node != null)
                {
                    issuper = true;
                    type = 2;
                }
                int liked = 0;
                node = doc.DocumentNode.SelectSingleNode(@"//span[@class='nums']");
                if (node != null)
                {
                    liked = int.Parse(node.InnerText);
                }
                writelog("更新用户信息");
                client.BeginUpdateUserInfo(user.Username, liked, issuper, id, user.account, type, null, null);
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
            }
        }

        /// <summary>
        /// 获取用户美丽说ID
        /// </summary>
        /// <returns></returns>
        public string GetUserid()
        {
            try
            {
               
                string html = HttpHelper.GetHtml("http://www.meilishuo.com/ihome", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, user.Cookie, null, Encoding.UTF8);
                html = html.Substring(html.IndexOf("user_id") + 1, html.Length - html.IndexOf("user_id") - 1);
                string id = html.Substring(html.IndexOf(":") + 1, html.IndexOf(",") - html.IndexOf(":") - 1).Trim();
                return id;
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.Log(errorMsg);
                return null;
            }
        }

        /// <summary>
        /// 获取用户设置
        /// </summary>
        /// <param name="isautoshare"></param>
        /// <param name="isautolike"></param>
        /// <param name="isautoliked"></param>
        /// <param name="isautocomment"></param>
        /// <param name="isautorecord"></param>
        public void GetUserSetting(out bool isauto)
        {
            isauto = true;
         
            try
            {
               
                Beauty.App.BeautyService.WebSettingGroup[] settinggroups = client.GetUserSetting(user.Username);
                foreach (Beauty.App.BeautyService.WebSettingGroup settinggroup in settinggroups)
                {
                    foreach (Beauty.App.BeautyService.WebSetting setting in settinggroup.settings)
                    {
                        if (setting.ID == Guid.Parse("B8C3C8CE-266D-4AC6-B1A5-EF24348AE8C8"))
                        {
                            if (setting.Value == "true")
                            {
                                isauto = true;
                            }
                            else
                            {
                                isauto = false;
                            }
                        }
                     
                    }
                }
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg,null,null);
            }

        }

        /// <summary>
        /// 获取任务并执行
        /// </summary>
        public void GetTask()
        {
            try
            {
                var isauto = true;
             
                Beauty.App.BeautyService.WebTask[] tasks = client.GetUserTask(user.Username);
                if (tasks != null && tasks.Length > 0)
                {
                    GetUserSetting(out isauto);
                    foreach (Beauty.App.BeautyService.WebTask task in tasks)
                    {
                        if (task.TaskType == "share")
                        {
                            if (task.IsAuto && isauto)
                            {
                                PostShare(task);
                            }
                            else if (task.Autoflag.HasValue && !isauto)
                            {
                                PostShare(task);
                            }
                        }
                        else if (task.TaskType == "like" && isauto)
                        {
                            PostLike(task);
                        }
                        else if (task.TaskType == "liked" && isauto)
                        {
                            PostLiked(task);
                        }
                        else if (task.TaskType == "comment" && isauto)
                        {
                            PostComment(task);
                        }
                        else if (task.TaskType == "record")
                        {
                            if (task.IsAuto && isauto)
                            {
                                PostRecord(task);
                            }
                            else if (task.Autoflag.HasValue && !isauto)
                            {
                                PostRecord(task);
                            }

                        }
                        else if (task.TaskType == "superrecord")
                        {
                            if (task.IsAuto && isauto)
                            {
                                PostRecord(task);
                            }
                            else if (task.Autoflag.HasValue && !isauto)
                            {
                                PostRecord(task);
                            }
                        }
                    }
                }
            }
            catch
            {
            
     
            }
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CreateGroup(string name)
        {
            try
            {
              
                string html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/group/create", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", "name=" + name, true, user.Cookie, null, Encoding.UTF8, null, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                PostGroupResult obj = serializer.Deserialize<PostGroupResult>(html);
                if (obj.code == 0)
                {
                    return obj.data;
                }
                return null;
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
                return null;
            }
        }

        /// <summary>
        /// 提交分享
        /// </summary>
        /// <param name="task"></param>
        public void PostShare(Beauty.App.BeautyService.WebTask task)
        {
            //http://www.meilishuo.com/aj/twitter/fetch
            try
            {
                int count = 0;
            
                Beauty.App.BeautyService.WebShare share = client.GetShare(task.Taskid.ToString());
                if (share != null)
                {
                    Beauty.App.BeautyService.WebBady bady = client.GetBady(share.Bady.ID.ToString());
                    string url = System.Web.HttpUtility.UrlEncode(bady.Link);
                    string html = HttpHelper.GetHtml("http://www.meilishuo.com/aj/twitter/fetch", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", "url=" + url, true, user.Cookie, null, Encoding.UTF8, null, true);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    PostBadyResult obj = serializer.Deserialize<PostBadyResult>(html);
                    //{"error_code":0,"data":{"gInfo":{"title":"\u7c73\u897f\u679c \u7ae5\u88c5\u513f\u7ae5\u76ae\u8863\u5916\u5957\u7537\u7ae5\u79cb\u88c52013\u65b0\u6b3e\u5939\u514b\u886b\u4f11\u95f2\u5916\u5957\u6f6eSM7A7","goods_url":"http:\/\/detail.tmall.com\/item.htm?id=27015308049","image":"http:\/\/img01.taobaocdn.com\/bao\/uploaded\/i1\/14275026844061217\/T1DcimFcxgXXXXXXXX_!!0-item_pic.jpg","domain":"tmall.com","price":158,"goodsID":64628518}}}
                    //string errorcode = html.Substring(html.IndexOf(":") + 1, html.IndexOf(",") - html.IndexOf(":") - 1);
                    if (obj.data == null)
                    {
                        //提交错误时，3次重复提交
                        count++;
                        while (count < 3)
                        {
                            html = HttpHelper.GetHtml("http://www.meilishuo.com/aj/twitter/fetch", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", bady.Link, true, user.Cookie, null, Encoding.UTF8, null, true);

                            obj = serializer.Deserialize<PostBadyResult>(html);

                            if (obj.error_code == 0)
                                break;
                            count++;
                        }
                    }

                    if (obj.error_code != 0)
                    {
                        return;
                    }

                    Beauty.App.BeautyService.WebGroup group = null;

                    ///如果分组编号为空，立马获取该用户的分组
                    if (string.IsNullOrEmpty(task.Type))
                    {
                        ///update group

                        html = HttpHelper.GetHtml("http://www.meilishuo.com/aj/magazine/user_groups", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, "", true, user.Cookie, null, Encoding.GetEncoding("gb2312"), null, true);

                        List<UserGroup> objs = serializer.Deserialize<List<UserGroup>>(html);
                        if (objs != null && objs.Count > 0)
                        {
                            foreach (UserGroup usergroup in objs)
                            {
                                if (usergroup.name.Contains(share.Keyword))
                                {
                                    group = new BeautyService.WebGroup { Name = usergroup.name, ID = usergroup.group_id };
                                }
                                client.BeginUpdateGroup(user.Username, usergroup.group_id, usergroup.name, null, null);
                            }

                            if (group == null && objs.Count > 0)
                            {
                                group = new BeautyService.WebGroup { Name = objs[0].name, ID = objs[0].group_id };
                            }
                        }
                    }
                    else
                    {
                        group = client.GetGroup(task.Type);
                    }

                    //创建新分组
                    if (!string.IsNullOrEmpty(task.NewType))
                    {
                        string groupid = CreateGroup(task.NewType);
                        if (groupid != null)
                        {
                            client.BeginUpdateGroup(user.Username, groupid, task.NewType, null, null);
                            group = new BeautyService.WebGroup { Name = task.NewType, ID = groupid };
                        }
                    }

                    //分组不为空才能提交
                    if (group != null)
                    {
                        if (obj.data != null)
                        {
                            string postdata = "type=7&goodsID=" + obj.data.gInfo.goodsID + "&group_id=" + task.Type + "&name=" + group.Name + "&tContent=" + task.Comment + "&goods_pic_url=" + obj.data.gInfo.image + "&syncToQzone=false&syncToWeibo=false";
                            //string goodid = html.Substring(html.LastIndexOf(":") + 1, html.IndexOf("}") - html.LastIndexOf(":") - 1);
                            //{"code":0,"data":{"twitter_id":1739748392,"group_id":42454433}}
                            html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/twitter/create", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", postdata, true, user.Cookie, null, Encoding.UTF8, null, true);

                            PostShareResult resultobject = serializer.Deserialize<PostShareResult>(html);
                            if (resultobject.code == 0)
                            {
                                writelog("完成分享任务");
                                client.BeginCompleteShareTask(task.ID.ToString(), task.Taskid.ToString(), bady.ID.ToString(), obj.data.gInfo.goodsID.ToString(), resultobject.data.twitter_id.ToString(), resultobject.data.group_id.ToString(), null, null);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
            }
        }

        /// <summary>
        /// 提交喜欢
        /// </summary>
        /// <param name="task"></param>
        public void PostLike(Beauty.App.BeautyService.WebTask task)
        {
            //http://www.meilishuo.com/aw/twitter/like
            //stid=1724688975
            try
            {
              
                Beauty.App.BeautyService.WebLike like = client.GetLike(task.Taskid.ToString());
                if (like != null)
                {
                    Beauty.App.BeautyService.WebBady bady = client.GetBady(like.Bady.ID.ToString());
                    string html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/twitter/like", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", "stid=" + bady.Twitterid, true, user.Cookie, null, Encoding.UTF8, null, true);
                    //{"code":0,"data":1740967646}

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    PostLikeResult obj = serializer.Deserialize<PostLikeResult>(html);
                    if (obj.code.HasValue && obj.code.Value == 0)
                    {
                        writelog("完成喜欢任务");
                        client.BeginCompletelikeTask(task.ID.ToString(), like.ID.ToString(), null, null);
                    }
                }
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
            }
        }

        /// <summary>
        /// 提交收录
        /// </summary>
        /// <param name="task"></param>
        public void PostRecord(Beauty.App.BeautyService.WebTask task)
        {
            try
            {
               
                Beauty.App.BeautyService.WebLike like = client.GetLike(task.Taskid.ToString());
                if (like != null)
                {
                    Beauty.App.BeautyService.WebBady bady = client.GetBady(like.Bady.ID.ToString());
                    Beauty.App.BeautyService.WebShare share = client.GetShareByBady(bady.ID.ToString());
                    if (share != null)
                    {
                        string type = task.Type;
                        ///分组为空时立马获取分组
                        if (string.IsNullOrEmpty(type))
                        {
                            //Beauty.App.BeautyService.WebGroup group = null;

                            if (string.IsNullOrEmpty(task.Type))
                            {
                                ///update group

                                string html = HttpHelper.GetHtml("http://www.meilishuo.com/aj/magazine/user_groups", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, "", true, user.Cookie, null, Encoding.GetEncoding("gb2312"), null, true);
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                List<UserGroup> objs = serializer.Deserialize<List<UserGroup>>(html);
                                if (objs != null && objs.Count > 0)
                                {
                                    foreach (UserGroup usergroup in objs)
                                    {
                                        if (usergroup.name.Contains(share.Keyword))
                                        {
                                            //group = new BeautyService.WebGroup { Name = usergroup.name, ID = usergroup.group_id };
                                            type = usergroup.group_id;
                                        }
                                        client.BeginUpdateGroup(user.Username, usergroup.group_id, usergroup.name, null, null);
                                    }

                                    if (string.IsNullOrEmpty(type) && objs.Count > 0)
                                    {
                                        type = objs[0].group_id;
                                    }
                                }
                            }

                        }

                        if (!string.IsNullOrEmpty(type))
                        {
                            string postdata = "type=8&stid=" + bady.Twitterid + "&tContent=" + task.Comment + "&name=" + user.Username + "&group_id=" + type + "&syncToQzone=false&syncToWeibo=false";
                            string html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/twitter/create", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", postdata, true, user.Cookie, null, Encoding.UTF8, null, true);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            PostShareResult resultobject = serializer.Deserialize<PostShareResult>(html);
                            if (resultobject.code.HasValue && resultobject.code.Value == 0)
                            {
                                //client.CompleteShareTask(task.ID.ToString(), task.Taskid.ToString(), bady.ID.ToString(), obj.data.gInfo.goodsID.ToString(), resultobject.data.twitter_id.ToString(), resultobject.data.group_id.ToString());
                                client.BeginCompletelikeTask(task.ID.ToString(), like.ID.ToString(), null, null);
                                writelog("完成收录任务");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
            }
        }

        /// <summary>
        /// 提交评论
        /// </summary>
        /// <param name="task"></param>
        public void PostComment(Beauty.App.BeautyService.WebTask task)
        {
          
            Beauty.App.BeautyService.WebLike like = client.GetLike(task.Taskid.ToString());
            if (like != null)
            {
                Beauty.App.BeautyService.WebBady bady = client.GetBady(like.Bady.ID.ToString());
                if (!string.IsNullOrEmpty(bady.Twitterid))
                {
                    string postdata = "stid=" + bady.Twitterid + "&type=4&tContent=" + task.Comment;

                    string html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/twitter/create", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", postdata, true, user.Cookie, null, Encoding.UTF8, null, true);
                    try
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        PostShareResult resultobject = serializer.Deserialize<PostShareResult>(html);
                        if (resultobject.code.HasValue && resultobject.code.Value == 0)
                        {
                            //client.CompleteShareTask(task.ID.ToString(), task.Taskid.ToString(), bady.ID.ToString(), obj.data.gInfo.goodsID.ToString(), resultobject.data.twitter_id.ToString(), resultobject.data.group_id.ToString());
                            client.BeginCompletelikeTask(task.ID.ToString(), like.ID.ToString(), null, null);
                            writelog("完成评论任务");
                        }
                    }
                    catch (Exception ex)
                    {

                        string errorMsg = "An application error occurred. Please contact the adminstrator " +
                 "with the following information:/n/n";
                        errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                        client.BeginLog(errorMsg, null, null);
                    }
                }
            }
        }

        /// <summary>
        /// 提交粉丝
        /// </summary>
        /// <param name="task"></param>
        public void PostLiked(Beauty.App.BeautyService.WebTask task)
        {
            try
            {
               
                Beauty.App.BeautyService.WebLike like = client.GetLike(task.Taskid.ToString());
                if (like != null)
                {
                    Beauty.App.BeautyService.WebBady bady = client.GetBady(like.Bady.ID.ToString());
                    string html = HttpHelper.GetHtml("http://www.meilishuo.com/share/" + bady.Twitterid, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, user.Cookie, null, Encoding.UTF8, null);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//span[@class='btn addFollow']");
                    if (node != null)
                    {
                        string id = node.Attributes["fuid"].Value;
                        string postdata = "fuid=" + id;
                        html = HttpHelper.GetHtml("http://www.meilishuo.com/aw/user/follow", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", "application/x-www-form-urlencoded; charset=UTF-8", postdata, true, user.Cookie, null, Encoding.UTF8, null, true);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        PostLikedResult resultobject = serializer.Deserialize<PostLikedResult>(html);
                        if (resultobject.code.HasValue && resultobject.code.Value == 0)
                        {
                            //client.CompleteShareTask(task.ID.ToString(), task.Taskid.ToString(), bady.ID.ToString(), obj.data.gInfo.goodsID.ToString(), resultobject.data.twitter_id.ToString(), resultobject.data.group_id.ToString());
                            client.BeginCompletelikeTask(task.ID.ToString(), like.ID.ToString(), null, null);
                            writelog("完成粉丝任务");
                        }
                    }
                }
            }
            catch(Exception ex)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
                errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
                client.BeginLog(errorMsg, null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActiveUser()
        {
            try
            {
                

                if (string.IsNullOrEmpty(ip))
                    ip = GetIp();
                client.BeginLoginActive(user.Username, ip, null, null);
            }
            catch
            {

            }
        }

        public void ReadNotice(string userid, string noticeid)
        {
            client.BeginReadBeautyNotice(userid, noticeid, null, null);
        }

        public string GetIp()
        {
            try
            {
               
                string ipAddress = new WebClient().DownloadString("http://icanhazip.com");
                return ipAddress;
            }
            catch
            {
                return string.Empty;
            }
        }

        public IList<BeautyService.WebNotice> GetNotices(string userid)
        {
            try
            {

                IList<BeautyService.WebNotice> notices = client.GetNotices(userid).ToList();
                return notices;
            }
            catch
            {
                return null;
            }
        }

        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接
        public extern static bool InternetGetConnectedState(out int conState, int reder);

        public bool CheckInternet()
        {
            int n = 0;
            if (InternetGetConnectedState(out n, 0))
            {
                return true;

            }
            else
            {
                return false;

            }
        }
    }
}
