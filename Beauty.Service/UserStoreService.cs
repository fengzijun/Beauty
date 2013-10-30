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
    
    public class UserStoreService: BaseService, IUserStore
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<UserStore> GetUserStores(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<UserStore> Comments = new List<UserStore>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserStore Comment = new UserStore()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Bady = ParseNGuid(dr["BadyId"]).HasValue ? new Bady { ID = ParseNGuid(dr["BadyId"]).Value } : null,
                            Comment = ParseInt(dr["Comment"]),
                            Liked = ParseInt(dr["Liked"]),
                            Link = ParseStr(dr["Link"]),
                            Page = ParseInt(dr["Page"]),
                            Record = ParseInt(dr["Record"]),
                            Rank = ParseInt(dr["Rank"]),
                            Username = ParseStr(dr["username"]),
                            Type = ParseStr(dr["Type"]),
                            Statues = ParseInt(dr["Statues"]),
                            mtype = ParseStr(dr["mtype"]),
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
        /// Create a new UserStore
        /// </summary>
        /// <param name="newUserStore">new UserStore</param>
        /// <returns>new UserStore id</returns>
        public Guid Create(UserStore newUserStore)
        {
            string sql = string.Format("EXEC sp_UserStore_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                                         , ToQuote(newUserStore.ID)
                                         , ToQuote(newUserStore.Username)
                                         , ToQuote(newUserStore.Bady.ID)
                                         , ToQuote(newUserStore.Page)
                                         , ToQuote(newUserStore.Type)
                                         , ToQuote(newUserStore.Liked)
                                         , ToQuote(newUserStore.Record)
                                         , ToQuote(newUserStore.Comment)
                                         , ToQuote(newUserStore.Link)
                                         , ToQuote(newUserStore.Rank)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newUserStore.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newUserStore.ID;
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

        public bool Createbatch(DataTable dt, string tablename)
        {
            try
            {
                SqlHelper.SqlBulkCopyInsert(dt, tablename, ConnectStr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Update an existing UserStore
        /// </summary>
        /// <param name="thisUserStore">UserStore</param>
        /// <returns>bool</returns>
        public bool Update(UserStore thisUserStore)
        {
            string sql = string.Format("EXEC sp_UserStore_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                                        , ToQuote(thisUserStore.ID)
                                        , ToQuote(thisUserStore.Username)
                                        , ToQuote(thisUserStore.Bady.ID)
                                        , ToQuote(thisUserStore.Type)
                                        , ToQuote(thisUserStore.Page)
                                        , ToQuote(thisUserStore.Liked)
                                        , ToQuote(thisUserStore.Record)
                                        , ToQuote(thisUserStore.Comment)
                                        , ToQuote(thisUserStore.Link)
                                        , ToQuote(thisUserStore.Rank)
                                        , ToQuote(CurrentUserName)
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(thisUserStore.Statues)

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

        public bool UpdateAll()
        {
            string sql = string.Format("exec dbo.sp_userstore_update  "


                                    );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        public bool DeleteAll()
        {
            string sql = string.Format("exec dbo.sp_userstore_d_all  "


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
        /// <summary>
        /// Delete an existing UserStore
        /// </summary>
        /// <param name="thisUserStore">UserStore</param>
        /// <returns>bool</returns>
        public bool Delete(UserStore thisUserStore)
        {
            string sql = string.Format("exec dbo.sp_UserStore_d {0} "
                                       , ToQuote(thisUserStore.ID)

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
        /// Check if a UserStore already exists
        /// </summary>
        /// <param name="name">UserStore Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a UserStore by id
        /// </summary>
        /// <param name="id">UserStore id</param>
        /// <returns>UserStore</returns>
        public UserStore Get(Guid id)
        {
            string sql = string.Format("EXEC sp_UserStore_g {0}, {1},{2},{3},{4},{5},{6}"
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
                IList<UserStore> UserStores = GetUserStores(sql, 0, null, out paing);
                if (UserStores.Count > 0)
                    return UserStores[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all UserStores
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserStore">current UserStore</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<UserStore> Get(Guid? id,string username,string badyid, int? statues,
            int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_UserStore_g {0}, {1},{2},{3},{4},{5},{6}"
                             , "NULL"
                             , ToQuote(username)
                             , ToQuote(badyid)
                             , "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<UserStore> UserStores = GetUserStores(sql, Page, null, out paing);
                return UserStores;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
