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
    
    public class UserAccountService : BaseService, IUserAccount
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<UserAccount> GetUserAccounts(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<UserAccount> Comments = new List<UserAccount>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserAccount Comment = new UserAccount()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            account = ParseStr(dr["account"]),
                            username = ParseStr(dr["username"]),
                            twitterid = ParseStr(dr["twitterid"]),
                            type = ParseInt(dr["type"]),
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
        /// Create a new UserAccount
        /// </summary>
        /// <param name="newUserAccount">new UserAccount</param>
        /// <returns>new UserAccount id</returns>
        public void Create(UserAccount newUserAccount)
        {
            string sql = string.Format("EXEC sp_useraccount_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                                         , ToQuote(newUserAccount.ID)
                                         , ToQuote(newUserAccount.username)
                                         , ToQuote(newUserAccount.account)
                                         , ToQuote(newUserAccount.type)
                                         , ToQuote(newUserAccount.twitterid)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(newUserAccount.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

            

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
        /// <summary>
        /// Update an existing UserAccount
        /// </summary>
        /// <param name="thisUserAccount">UserAccount</param>
        /// <returns>bool</returns>
   
        /// <summary>
        /// Get a UserAccount by id
        /// </summary>
        /// <param name="id">UserAccount id</param>
        /// <returns>UserAccount</returns>

        /// <summary>
        /// Get all UserAccounts
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserAccount">current UserAccount</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public  IList<UserAccount> Get(Guid? id, string username,int? type, int? statues, int Page, string sortKey,
        out PaginationInfo paing, bool isfuzzy = false)
        {
            string sql = string.Format("EXEC sp_useraccount_g {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(username)
                             , type.HasValue ? ToQuote(type.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<UserAccount> UserAccounts = GetUserAccounts(sql, Page, null, out paing);
                return UserAccounts;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
