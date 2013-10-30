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

    public class HelpService: BaseService, IHelp
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Help> GetHelps(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Help> Comments = new List<Help>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Help Comment = new Help()
                        {
                            ID = new Guid(dr["id"].ToString()),
                         
                            Statues = ParseInt(dr["Statues"]),
                            msgcontent = ParseStr(dr["msgcontent"]),
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
        /// Create a new Help
        /// </summary>
        /// <param name="newHelp">new Help</param>
        /// <returns>new Help id</returns>
        public Guid Create(Help newHelp)
        {
            string sql = string.Format("EXEC sp_Help_c {0},{1},{2},{3},{4},{5},{6}"
                                         , ToQuote(newHelp.ID)
                                         , ToQuote(newHelp.msgcontent)
                                         
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(newHelp.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newHelp.ID;
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
        /// Update an existing Help
        /// </summary>
        /// <param name="thisHelp">Help</param>
        /// <returns>bool</returns>
        public bool Update(Help thisHelp)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Delete an existing Help
        /// </summary>
        /// <param name="thisHelp">Help</param>
        /// <returns>bool</returns>
        public bool Delete(Help thisHelp)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Check if a Help already exists
        /// </summary>
        /// <param name="name">Help Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Help by id
        /// </summary>
        /// <param name="id">Help id</param>
        /// <returns>Help</returns>
        public Help Get(string id)
        {
            string sql = string.Format("EXEC sp_Help_g {0}, {1},{2},{3}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
  


                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<Help> Helps = GetHelps(sql, 0, null, out paing);
                if (Helps.Count > 0)
                    return Helps[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Helps
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Help">current Help</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Help> Get(Guid? id, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Help_g {0}, {1},{2},{3}"
                             , "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Help> Helps = GetHelps(sql, Page, null, out paing);
                return Helps;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        #endregion
    }
}
