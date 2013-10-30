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

    public class UserSettingService : BaseService, IUserSetting
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<UserSetting> GetUserSettings(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<UserSetting> Comments = new List<UserSetting>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserSetting Comment = new UserSetting()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Settingid = new Guid(dr["Settingid"].ToString()),
                            Username = ParseStr(dr["Username"]),
                            Value = ParseStr(dr["Value"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"])
                        };

                        Comments.Add(Comment);
                    }


                    paging = new PaginationInfo()
                    {
                        Current = page,
                        Size = ParseInt(ds.Tables[1].Rows[0]["pagesize"]),
                        TotalRecords = ParseInt(ds.Tables[1].Rows[0]["totalrecords"]),
                        TotalPages = (int)Math.Ceiling(ParseInt(ds.Tables[1].Rows[0]["totalrecords"]) /
                        ParseFloat(ds.Tables[1].Rows[0]["pagesize"]))
                    };

                    return Comments;
                }
            }
        }

        #endregion

        #region * CRUD *

        /// <summary>
        /// Create a new UserSetting
        /// </summary>
        /// <param name="newUserSetting">new UserSetting</param>
        /// <returns>new UserSetting id</returns>
        public Guid Create(UserSetting newUserSetting)
        {
            string sql = string.Format("EXEC sp_UserSetting_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                                         , ToQuote(newUserSetting.ID)
                                         , ToQuote(newUserSetting.Userid)
                                         , ToQuote(newUserSetting.Username)
                                         , ToQuote(newUserSetting.Settingid)
                                         , ToQuote(newUserSetting.Value)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newUserSetting.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newUserSetting.ID;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
        /// <summary>
        /// Update an existing UserSetting
        /// </summary>
        /// <param name="thisUserSetting">UserSetting</param>
        /// <returns>bool</returns>
        public bool Update(UserSetting thisUserSetting)
        {
            string sql = string.Format("EXEC sp_UserSetting_u {0},{1},{2},{3},{4},{5}"

                                         , ToQuote(thisUserSetting.Username)
                                         , ToQuote(thisUserSetting.Settingid)
                                         , ToQuote(thisUserSetting.Value)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisUserSetting.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Delete an existing UserSetting
        /// </summary>
        /// <param name="thisUserSetting">UserSetting</param>
        /// <returns>bool</returns>
        public bool Delete(UserSetting thisUserSetting)
        {
            string sql = string.Format("exec dbo.sp_UserSetting_d {0} "
                                        , ToQuote(thisUserSetting.ID)

                                      );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount >= 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        /// <summary>
        /// Check if a UserSetting already exists
        /// </summary>
        /// <param name="name">UserSetting Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a UserSetting by id
        /// </summary>
        /// <param name="id">UserSetting id</param>
        /// <returns>UserSetting</returns>
        public UserSetting Get(Guid id)
        {
            string sql = string.Format("EXEC sp_UserSetting_g {0},{1},{2},{3},{4},{5}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<UserSetting> UserSettings = GetUserSettings(sql, 0, null, out paing);
                if (UserSettings.Count > 0)
                    return UserSettings[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all UserSettings
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserSetting">current UserSetting</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<UserSetting> Get(Guid? id, string username, Guid? settingid, int? statues,
             int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_UserSetting_g {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(username)
                             , settingid.HasValue ? ToQuote(settingid.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<UserSetting> UserSettings = GetUserSettings(sql, Page, null, out paing);
                return UserSettings;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// GetByUserid
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<SettingGroup> GetByUsername(string username)
        {
            ISetting settingservice = new SettingService();
            PaginationInfo paging = new PaginationInfo();
            IList<Setting> settings = settingservice.Get(null, null,null, null, 1, 0, null, out paging);
            IList<UserSetting> usersettings = Get(null, username, null, null, 0, null, out paging);

            foreach (Setting setting in settings)
            {
                foreach (UserSetting usersetting in usersettings)
                {
                    if (usersetting.Settingid == setting.ID)
                    {
                        setting.Value = usersetting.Value;
                    }
                }
            }

            IList<SettingGroup> settinggroups = new List<SettingGroup>();
            IEnumerable<IGrouping<string, Setting>> groups = settings.GroupBy(m => m.Category).ToList();
            foreach (IGrouping<string, Setting> petGroup in groups)
            {
                settinggroups.Add(new SettingGroup { Category = petGroup.Key, settings = petGroup.ToList() });
            }
            for (int i = 0; i < settinggroups.Count; i++)
            {
                if (settinggroups[i].Category == "admin")
                {
                    settinggroups.RemoveAt(i);
                }
            }
            
            return settinggroups;
        }

        #endregion
    }
}
