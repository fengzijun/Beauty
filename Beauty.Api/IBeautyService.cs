using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Beauty.API
{
    using Beauty.Api.Model;
    using Beauty.Model;
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBeautyService" in both code and config file together.
    [ServiceContract]
    public interface IBeautyService
    {


        [OperationContract]
        BoolResponse CheckUser(string username, string password);

        [OperationContract]
        BoolResponse Risgter(string username, string password, string email, string mobile, string zfb,string province,string city,string qq, string address,string refer);

        [OperationContract]
        BoolResponse ForgetPassword(string username, string email);

        [OperationContract]
        void UpdateUserInfo(string username, int liked, bool issuper, string userid, string account, int type);
        

        [OperationContract]
        void UpdateGroup(string username, string groupid, string groupname);

        [OperationContract]
        IList<WebSettingGroup> GetUserSetting(string username);

        [OperationContract]
        IList<WebTask> GetUserTask(string username);

        [OperationContract]
        WebShare GetShare(string id);

        [OperationContract]
        WebShare GetShareByBady(string id);

        [OperationContract]
        WebBady GetBady(string id);

        [OperationContract]
        WebGroup GetGroup(string id);

        [OperationContract]
        WebLike GetLike(string id);

        [OperationContract]
        void CompleteShareTask(string taskid, string shareid, string badyid, string goodid, string twriteid, string groupid);

        [OperationContract]
        void CompletelikeTask(string taskid, string likeid);

        [OperationContract]
        bool LoginActive(string username,string ip);

        [OperationContract]
        void Log(string msg);

        [OperationContract]
        WebUser GetUser(string username);

        [OperationContract]
        IList<WebNotice> GetNotices(string userid);

        [OperationContract]
        void ReadBeautyNotice(string userid,string noticeid);
    }
}
