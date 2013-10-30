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

    public class RequestMoneyService : BaseService, IRequestMoney
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<RequstMoney> GetRequstMoneys(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<RequstMoney> Comments = new List<RequstMoney>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RequstMoney Comment = new RequstMoney()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Msg = ParseStr(dr["Msg"]),
                            Money = ParseDecimal(dr["Money"]),
                            Statues = ParseInt(dr["Statues"]),
                            Username = ParseStr(dr["Username"]),
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
        /// Create a new RequstMoney
        /// </summary>
        /// <param name="newRequstMoney">new RequstMoney</param>
        /// <returns>new RequstMoney id</returns>
        public Guid Create(RequstMoney newRequstMoney)
        {
            string sql = string.Format("EXEC sp_requestmoney_c {0},{1},{2},{3},{4},{5},{6},{7},{8}"
                                         , ToQuote(newRequstMoney.ID)
                                         , ToQuote(newRequstMoney.Username)
                                         , ToQuote(newRequstMoney.Money)
                                         , ToQuote(newRequstMoney.Msg)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)

                                         , ToQuote(newRequstMoney.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newRequstMoney.ID;
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
        /// Update an existing RequstMoney
        /// </summary>
        /// <param name="thisRequstMoney">RequstMoney</param>
        /// <returns>bool</returns>
        public bool Update(RequstMoney thisRequstMoney)
        {
            string sql = string.Format("EXEC sp_requestmoney_u {0},{1},{2},{3},{4},{5},{6}"
                                         , ToQuote(thisRequstMoney.ID)
                                         , ToQuote(thisRequstMoney.Username)
                                         , ToQuote(thisRequstMoney.Money)
                                         , ToQuote(thisRequstMoney.Msg)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)


                                         , ToQuote(thisRequstMoney.Statues)
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
        /// Delete an existing RequstMoney
        /// </summary>
        /// <param name="thisRequstMoney">RequstMoney</param>
        /// <returns>bool</returns>
        public bool Delete(RequstMoney thisRequstMoney)
        {
            string sql = string.Format("exec dbo.sp_RequstMoney_d {0} "
                                        , ToQuote(thisRequstMoney.ID)

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
        /// Check if a RequstMoney already exists
        /// </summary>
        /// <param name="name">RequstMoney Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a RequstMoney by id
        /// </summary>
        /// <param name="id">RequstMoney id</param>
        /// <returns>RequstMoney</returns>
        public RequstMoney Get(Guid id)
        {
            string sql = string.Format("EXEC sp_requestmoney_g {0},{1},{2},{3},{4},{5}"
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
                IList<RequstMoney> RequstMoneys = GetRequstMoneys(sql, 0, null, out paing);
                if (RequstMoneys.Count > 0)
                    return RequstMoneys[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all RequstMoneys
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="RequstMoney">current RequstMoney</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<RequstMoney> Get(Guid? id, string username,string msg, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_requestmoney_g {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(username)
                              , ToQuote(msg)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<RequstMoney> RequstMoneys = GetRequstMoneys(sql, Page, null, out paing);
                return RequstMoneys;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<RequstMoney> Getdealed(Guid? id, string username, string msg, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_requestmoney_g2 {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(username)
                              , ToQuote(msg)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<RequstMoney> RequstMoneys = GetRequstMoneys(sql, Page, null, out paing);
                return RequstMoneys;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
