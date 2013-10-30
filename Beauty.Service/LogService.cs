using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Service
{
    using Beauty.Core;
    using Beauty.InterFace;
    using Beauty.Model;
    using System.Data;

    public class LogService : BaseService, ILog
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Log> GetLogs(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Log> Comments = new List<Log>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Log Comment = new Log()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Msg = ParseStr(dr["Msg"]),
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
        
        public Guid Create(Log newLog)
        {
            string sql = string.Format("EXEC sp_log_c {0},{1},{2},{3},{4},{5},{6}"
                                        , ToQuote(newLog.ID)
                                        , ToQuote(newLog.Msg)
                                      
                                        , ToQuote(CurrentUserName)
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(CurrentUserName)
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(newLog.Statues)

                                    );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newLog.ID;
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


        public IList<Log> Get(Guid? id, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Log_g {0}, {1},{2},{3}"
                            , "NULL"
                         
                            , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                            , ToQuote(Page)
                            , ToQuote(sortKey)
                         );

            try
            {

                IList<Log> Logs = GetLogs(sql, Page, null, out paing);
                return Logs;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
    }
}
