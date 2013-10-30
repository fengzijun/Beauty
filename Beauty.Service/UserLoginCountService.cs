using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Beauty.Service
{
    using Beauty.Core;
    using Beauty.InterFace;
    using Beauty.Model;
    
    public class UserLoginCountService : BaseService, IUserLoginCount
    {
        public void Create(UserLoginCount newUserSetting)
        {
            string sql = string.Format("EXEC sp_userlogincount_c {0},{1},{2},{3},{4},{5},{6},{7}"
                                        , ToQuote(newUserSetting.ID)
                                        , newUserSetting.userid.HasValue?ToQuote(newUserSetting.userid.Value):"NULL"
                                        , ToQuote(newUserSetting.username)
                                        
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(CurrentUserName)
                                       
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(CurrentUserName)
                                        , ToQuote(newUserSetting.Statues)

                                    );

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

            

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
    }
}
