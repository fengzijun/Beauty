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

    public class UserTokenService : BaseService, IUserToken
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<UserToken> GetUserTokens(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<UserToken> Comments = new List<UserToken>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        UserToken Comment = new UserToken()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Userid = new Guid(dr["Userid"].ToString()),
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
        /// Create a new UserToken
        /// </summary>
        /// <param name="newUserToken">new UserToken</param>
        /// <returns>new UserToken id</returns>
        public Guid Create(UserToken newUserToken)
        {
            string sql = string.Format("EXEC sp_UserToken_c {0},{1},{2},{3},{4},{5},{6}"
                                         , ToQuote(newUserToken.ID)
                                         , ToQuote(newUserToken.Userid)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newUserToken.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newUserToken.ID;
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
        /// Update an existing UserToken
        /// </summary>
        /// <param name="thisUserToken">UserToken</param>
        /// <returns>bool</returns>
        public bool Update(UserToken thisUserToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an existing UserToken
        /// </summary>
        /// <param name="thisUserToken">UserToken</param>
        /// <returns>bool</returns>
        public bool Delete(UserToken thisUserToken)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Check if a UserToken already exists
        /// </summary>
        /// <param name="name">UserToken Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a UserToken by id
        /// </summary>
        /// <param name="id">UserToken id</param>
        /// <returns>UserToken</returns>
        public UserToken Get(Guid id)
        {
            string sql = string.Format("EXEC sp_UserToken_g {0}, {1},{2},{3}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                         
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<UserToken> UserTokens = GetUserTokens(sql, 0, null, out paing);
                if (UserTokens.Count > 0)
                    return UserTokens[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all UserTokens
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserToken">current UserToken</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<UserToken> Get(Guid? id, int? statues,
            int Page, string sortKey, out PaginationInfo paing)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
