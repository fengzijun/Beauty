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

    public class SettingService : BaseService, ISetting
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Setting> GetSettings(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Setting> Comments = new List<Setting>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Setting Comment = new Setting()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Name = ParseStr(dr["Name"]),
                            Type = ParseStr(dr["Type"]),
                            Value = ParseStr(dr["Value"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"]),
                            Category = ParseStr(dr["Category"])
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
        /// Create a new Setting
        /// </summary>
        /// <param name="newSetting">new Setting</param>
        /// <returns>new Setting id</returns>
        public Guid Create(Setting newSetting)
        {
            string sql = string.Format("EXEC sp_Setting_c {0},{1},{2},{3},{4},{5},{6},{7},{8}"
                                         , ToQuote(newSetting.ID)
                                         , ToQuote(newSetting.Name)
                                         , ToQuote(newSetting.Value)
                                         , ToQuote(newSetting.Type)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newSetting.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newSetting.ID;
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
        /// Update an existing Setting
        /// </summary>
        /// <param name="thisSetting">Setting</param>
        /// <returns>bool</returns>
        public bool Update(Setting thisSetting)
        {
            string sql = string.Format("EXEC sp_Setting_u {0},{1},{2},{3},{4},{5},{6} "
                                         , ToQuote(thisSetting.ID)
                                         , ToQuote(thisSetting.Name)
                                         , ToQuote(thisSetting.Value)
                                         , ToQuote(thisSetting.Type)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisSetting.Statues)

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
        /// Delete an existing Setting
        /// </summary>
        /// <param name="thisSetting">Setting</param>
        /// <returns>bool</returns>
        public bool Delete(Setting thisSetting)
        {
            string sql = string.Format("exec dbo.sp_Setting_d {0} "
                                        , ToQuote(thisSetting.ID)

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
        /// Check if a Setting already exists
        /// </summary>
        /// <param name="name">Setting Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Setting by id
        /// </summary>
        /// <param name="id">Setting id</param>
        /// <returns>Setting</returns>
        public Setting Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Setting_g {0},{1},{2},{3},{4},{5},{6}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
       
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<Setting> Settings = GetSettings(sql, 0, null, out paing);
                if (Settings.Count > 0)
                    return Settings[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Settings
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Setting">current Setting</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Setting> Get(Guid? id,string category, string name,string type, int? statues, int Page, string sortKey,
            out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Setting_g {0}, {1},{2},{3},{4},{5},{6}"
                             , "NULL"
                             , ToQuote(category)
                             , ToQuote(name)
                             , ToQuote(type)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Setting> Settings = GetSettings(sql, Page, null, out paing);
                return Settings;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<SettingGroup> GetSystemSetting()
        {
            ISetting settingservice = new SettingService();
            PaginationInfo paging = new PaginationInfo();
            IList<Setting> settings = settingservice.Get(null, null, null, null, null, 0, null, out paging);

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
