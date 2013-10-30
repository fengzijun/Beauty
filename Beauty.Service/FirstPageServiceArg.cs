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

    public class FirstPageArgService : BaseService, IFirstPageArg
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<FirstPageArg> GetFirstPageArgs(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<FirstPageArg> Comments = new List<FirstPageArg>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FirstPageArg Comment = new FirstPageArg()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            CommentArg = ParseInt(dr["CommentArg"]),
                            LikeArg = ParseInt(dr["LikeArg"]),
                            RecordArg = ParseInt(dr["RecordArg"]),
                            Type = ParseStr(dr["Type"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            mtype = ParseStr(dr["mtype"]),
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
        /// Create a new FirstPageArg
        /// </summary>
        /// <param name="newFirstPageArg">new FirstPageArg</param>
        /// <returns>new FirstPageArg id</returns>
        public Guid Create(FirstPageArg newFirstPageArg)
        {
            string sql = string.Format("EXEC sp_FirstPageArg_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                                         , ToQuote(newFirstPageArg.ID)
                                        
                                         , ToQuote(newFirstPageArg.Type)
                                         , ToQuote(newFirstPageArg.LikeArg)
                                         , ToQuote(newFirstPageArg.CommentArg)
                                         , ToQuote(newFirstPageArg.RecordArg)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newFirstPageArg.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newFirstPageArg.ID;
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
        /// Update an existing FirstPageArg
        /// </summary>
        /// <param name="thisFirstPageArg">FirstPageArg</param>
        /// <returns>bool</returns>
        public bool Update(FirstPageArg thisFirstPageArg)
        {
            string sql = string.Format("EXEC sp_FirstPageArg_u {0},{1},{2},{3},{4},{5},{6},{7}"
                                      , ToQuote(thisFirstPageArg.ID)

                                         , ToQuote(thisFirstPageArg.Type)
                                         , ToQuote(thisFirstPageArg.LikeArg)
                                         , ToQuote(thisFirstPageArg.CommentArg)
                                         , ToQuote(thisFirstPageArg.RecordArg)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisFirstPageArg.Statues)


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
        /// Delete an existing FirstPageArg
        /// </summary>
        /// <param name="thisFirstPageArg">FirstPageArg</param>
        /// <returns>bool</returns>
        public bool Delete(FirstPageArg thisFirstPageArg)
        {
            string sql = string.Format("exec dbo.sp_firstpagearg_d {0} "
                                      , ToQuote(thisFirstPageArg.ID)

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
        /// Check if a FirstPageArg already exists
        /// </summary>
        /// <param name="name">FirstPageArg Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a FirstPageArg by id
        /// </summary>
        /// <param name="id">FirstPageArg id</param>
        /// <returns>FirstPageArg</returns>
        public FirstPageArg Get(Guid id)
        {
            string sql = string.Format("EXEC sp_FirstPageArg_g {0}, {1},{2},{3},{4},{5}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "null" 

                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<FirstPageArg> FirstPageArgs = GetFirstPageArgs(sql, 0, null, out paing);
                if (FirstPageArgs.Count > 0)
                    return FirstPageArgs[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all FirstPageArgs
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="FirstPageArg">current FirstPageArg</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<FirstPageArg> Get(Guid? id,  string type,string mtype, int? statues, int Page, string sortKey,
            out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_FirstPageArg_g {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(type)
                             , ToQuote(mtype)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<FirstPageArg> FirstPageArgs = GetFirstPageArgs(sql, Page, null, out paing);
                return FirstPageArgs;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public void FirstPageArgRecord()
        {
            string sql = string.Format("exec dbo.sp_firstpageavg_record "
                                   

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

        #endregion
    }
}
