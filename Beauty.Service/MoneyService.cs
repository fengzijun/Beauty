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

    public class MoneyService : BaseService, IMoney
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<MoneyRecord> GetMoneyRecords(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<MoneyRecord> Comments = new List<MoneyRecord>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MoneyRecord Comment = new MoneyRecord()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Balance = ParseNDecimal(dr["Balance"]),
                            Money = ParseDecimal(dr["Money"]),
                            Type = ParseStr(dr["Type"]),
                            Userid = ParseNGuid(dr["Userid"].ToString()),
                            Username = ParseStr(dr["Username"]),
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
        /// Create a new MoneyRecord
        /// </summary>
        /// <param name="newMoneyRecord">new MoneyRecord</param>
        /// <returns>new MoneyRecord id</returns>
        public Guid Create(MoneyRecord newMoneyRecord)
        {
            string sql = string.Format("EXEC sp_Money_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                                         , ToQuote(newMoneyRecord.ID)
                                         , ToQuote(newMoneyRecord.Username)
                                         , newMoneyRecord.Userid.HasValue ? ToQuote(newMoneyRecord.Userid.Value) : "NULL"
                                         , ToQuote(newMoneyRecord.Money)
                                         , newMoneyRecord.Balance.HasValue ? ToQuote(newMoneyRecord.Balance.Value) : "NULL"
                                         , ToQuote(newMoneyRecord.Type)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newMoneyRecord.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newMoneyRecord.ID;
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
        /// Update an existing MoneyRecord
        /// </summary>
        /// <param name="thisMoneyRecord">MoneyRecord</param>
        /// <returns>bool</returns>
        public bool Update(MoneyRecord thisMoneyRecord)
        {
            string sql = string.Format("EXEC sp_Money_u {0},{1},{2},{3},{4},{5},{6},{7},{8}"
                                         , ToQuote(thisMoneyRecord.ID)
                                         , ToQuote(thisMoneyRecord.Username)
                                         , thisMoneyRecord.Userid.HasValue ? ToQuote(thisMoneyRecord.Userid.Value) : "NULL"
                                         , ToQuote(thisMoneyRecord.Money)
                                         , thisMoneyRecord.Balance.HasValue ? ToQuote(thisMoneyRecord.Balance.Value) : "NULL"
                                         , ToQuote(thisMoneyRecord.Type)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisMoneyRecord.Statues)
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
        /// Delete an existing MoneyRecord
        /// </summary>
        /// <param name="thisMoneyRecord">MoneyRecord</param>
        /// <returns>bool</returns>
        public bool Delete(MoneyRecord thisMoneyRecord)
        {
            string sql = string.Format("exec dbo.sp_Money_d {0} "
                                        , ToQuote(thisMoneyRecord.ID)

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
        /// Check if a MoneyRecord already exists
        /// </summary>
        /// <param name="name">MoneyRecord Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a MoneyRecord by id
        /// </summary>
        /// <param name="id">MoneyRecord id</param>
        /// <returns>MoneyRecord</returns>
        public MoneyRecord Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Money_g {0}, {1},{2},{3},{4},{5},{6}"
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
                IList<MoneyRecord> MoneyRecords = GetMoneyRecords(sql, 0, null, out paing);
                if (MoneyRecords.Count > 0)
                    return MoneyRecords[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all MoneyRecords
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="MoneyRecord">current MoneyRecord</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<MoneyRecord> Get(Guid? id, string username, Guid? userid, string type, int? statues, int Page, string sortKey,
            out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Money_g {0}, {1},{2},{3},{4},{5},{6}"
                             , "NULL"
                             , ToQuote(username)
                             , userid.HasValue? ToQuote(username):"NULL"
                             , ToQuote(type)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<MoneyRecord> MoneyRecords = GetMoneyRecords(sql, Page, null, out paing);
                return MoneyRecords;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<MoneyRecord> GetAvail(Guid? id, string username, Guid? userid, string type, int? statues, int Page, string sortKey,
          out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Money_g_avail {0}, {1},{2},{3},{4},{5},{6}"
                             , "NULL"
                             , ToQuote(username)
                             , userid.HasValue ? ToQuote(username) : "NULL"
                             , ToQuote(type)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<MoneyRecord> MoneyRecords = GetMoneyRecords(sql, Page, null, out paing);
                return MoneyRecords;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
