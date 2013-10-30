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

    public class BadyService : BaseService, IBady
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Bady> GetBadys(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Bady> Comments = new List<Bady>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Bady Comment = new Bady()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            BadyId = ParseStr(dr["BadyId"]),
                            Username = ParseStr(dr["Username"]),
                            ImageUrl = ParseStr(dr["ImageUrl"]),
                            Price = ParseStr(dr["Price"]),
                            Platfrom = ParseStr(dr["Platfrom"]),
                            Link = ParseStr(dr["Link"]),
                            Badydescription = ParseStr(dr["Badydescription"]),
                            Badyname = ParseStr(dr["Badyname"]),
                            Groupid = ParseStr(dr["Groupid"]),
                            Twitterid = ParseStr(dr["Twitterid"]),
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
        /// Create a new Bady
        /// </summary>
        /// <param name="newBady">new Bady</param>
        /// <returns>new Bady id</returns>
        public Guid Create(Bady newBady)
        {
            string sql = string.Format("EXEC sp_Bady_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}"
                                         , ToQuote(newBady.ID)
                                         , ToQuote(newBady.BadyId)
                                         , ToQuote(newBady.Username)
                                         , ToQuote(newBady.Badyname)
                                         , ToQuote(newBady.Badydescription)
                                         , ToQuote(newBady.Link)
                                         , ToQuote(newBady.Platfrom)
                                         , ToQuote(newBady.ImageUrl)
                                         , ToQuote(newBady.Price)
                                         , ToQuote(newBady.Groupid)
                                         , ToQuote(newBady.Twitterid)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newBady.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newBady.ID;
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
        /// Update an existing Bady
        /// </summary>
        /// <param name="thisBady">Bady</param>
        /// <returns>bool</returns>
        public bool Update(Bady thisBady)
        {
            string sql = string.Format("EXEC sp_Bady_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                                        ,  ToQuote(thisBady.ID)
                                         , ToQuote(thisBady.BadyId)
                                         , ToQuote(thisBady.Username)
                                         , ToQuote(thisBady.Badyname)
                                         , ToQuote(thisBady.Badydescription)
                                         , ToQuote(thisBady.Link)
                                         , ToQuote(thisBady.Platfrom)
                                         , ToQuote(thisBady.ImageUrl)
                                         , ToQuote(thisBady.Price)
                                          , ToQuote(thisBady.Groupid)
                                         , ToQuote(thisBady.Twitterid)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisBady.Statues)

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
        /// Delete an existing Bady
        /// </summary>
        /// <param name="thisBady">Bady</param>
        /// <returns>bool</returns>
        public bool Delete(Bady thisBady)
        {
          
            string sql = string.Format("exec dbo.sp_Bady_d {0} "
                                       , ToQuote(thisBady.ID)

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
        /// Check if a Bady already exists
        /// </summary>
        /// <param name="name">Bady Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Bady by id
        /// </summary>
        /// <param name="id">Bady id</param>
        /// <returns>Bady</returns>
        public Bady Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Bady_g {0}, {1},{2},{3},{4},{5}"
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
                IList<Bady> Badys = GetBadys(sql, 0, null, out paing);
                if (Badys.Count > 0)
                    return Badys[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Badys
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Bady">current Bady</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Bady> Get(Guid? id,string username, string badyname, int? statues, int Page, string sortKey,
            out PaginationInfo paing, bool isfuzzy = false)
        {
            string sql = string.Format("EXEC " + (isfuzzy ? "sp_Bady_fuzzy_g" : " sp_Bady_g") + " {0}, {1},{2},{3},{4},{5}"
                             , "NULL"
                             , ToQuote(username)
                             , ToQuote(badyname)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Bady> Badys = GetBadys(sql, Page, null, out paing);
                return Badys;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<Bady> GetNeedToShare(Guid? id, string username, string badyname, int? statues, int Page, string sortKey,
            out PaginationInfo paing, bool isfuzzy = false)
        {
            string sql = string.Format("EXEC " + (isfuzzy ? "sp_Bady_fuzzy_g_NotInShare" : " sp_Bady_g_NotInShare") + " {0}, {1},{2},{3},{4},{5}"
                            , "NULL"
                            , ToQuote(username)
                            , ToQuote(badyname)
                            , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                            , ToQuote(Page)
                            , ToQuote(sortKey)
                         );

            try
            {

                IList<Bady> Badys = GetBadys(sql, Page, null, out paing);
                return Badys;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<Bady> GetNotNeedToShare(Guid? id, string username, string badyname, int? statues, int Page, string sortKey,
           out PaginationInfo paing, bool isfuzzy = false)
        {
            string sql = string.Format("EXEC " + (isfuzzy ? "sp_Bady_fuzzy_g_InShare" : " sp_Bady_g_InShare") + " {0}, {1},{2},{3},{4},{5}"
                            , "NULL"
                            , ToQuote(username)
                            , ToQuote(badyname)
                            , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                            , ToQuote(Page)
                            , ToQuote(sortKey)
                         );

            try
            {

                IList<Bady> Badys = GetBadys(sql, Page, null, out paing);
                return Badys;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
