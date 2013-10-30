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
    
    public class NoticeService : BaseService, INotice
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Notice> GetNotices(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Notice> Comments = new List<Notice>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Notice Comment = new Notice()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Msg = ParseStr(dr["Msg"]),
                            Type = ParseInt(dr["Type"]),
                            Statues = ParseInt(dr["Statues"]),
                            Url = ParseStr(dr["Url"]),
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
        /// Create a new Notice
        /// </summary>
        /// <param name="newNotice">new Notice</param>
        /// <returns>new Notice id</returns>
        public Guid Create(Notice newNotice)
        {
            string sql = string.Format("EXEC sp_notice_c {0},{1},{2},{3},{4},{5},{6},{7}"
                                         , ToQuote(newNotice.ID)
                                         , ToQuote(newNotice.Msg)
                                         , ToQuote(newNotice.Type)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         
                                         , ToQuote(newNotice.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newNotice.ID;
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


        public Guid ReadNotice(ReadNoitce newNotice)
        {
            string sql = string.Format("EXEC sp_readnotice_c {0},{1},{2},{3},{4},{5},{6},{7}"
                                         , ToQuote(newNotice.ID)
                                         , ToQuote(newNotice.Noticeid)
                                         , ToQuote(newNotice.Userid)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)

                                         , ToQuote(newNotice.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newNotice.ID;
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
        /// Update an existing Notice
        /// </summary>
        /// <param name="thisNotice">Notice</param>
        /// <returns>bool</returns>
        public bool Update(Notice thisNotice)
        {
            string sql = string.Format("EXEC sp_notice_u {0},{1},{2},{3},{4},{5}"
                                         , ToQuote(thisNotice.ID)
                                         , ToQuote(thisNotice.Msg)
                                         , ToQuote(thisNotice.Type)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         

                                         , ToQuote(thisNotice.Statues)
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
        /// Delete an existing Notice
        /// </summary>
        /// <param name="thisNotice">Notice</param>
        /// <returns>bool</returns>
        public bool Delete(Notice thisNotice)
        {
            string sql = string.Format("exec dbo.sp_notice_d {0} "
                                        , ToQuote(thisNotice.ID)

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
        /// Check if a Notice already exists
        /// </summary>
        /// <param name="name">Notice Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Notice by id
        /// </summary>
        /// <param name="id">Notice id</param>
        /// <returns>Notice</returns>
        public Notice Get(Guid id)
        {
            string sql = string.Format("EXEC sp_notice_g {0},{1},{2},{3},{4}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<Notice> Notices = GetNotices(sql, 0, null, out paing);
                if (Notices.Count > 0)
                    return Notices[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Notices
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Notice">current Notice</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Notice> Get(Guid? id, int? type, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_notice_g {0}, {1},{2},{3},{4}"
                             , "NULL"
                             , type.HasValue ? ToQuote(type.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Notice> Notices = GetNotices(sql, Page, null, out paing);
                return Notices;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<Notice> GetOneSystemNoice(Guid? id, string userid, int? type, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_notice_g_user {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(userid)
                             , type.HasValue ? ToQuote(type.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Notice> Notices = GetNotices(sql, Page, null, out paing);
                return Notices;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
