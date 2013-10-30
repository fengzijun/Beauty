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

    public class LIkeService : BaseService, ILike
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Like> GetLikes(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Like> Comments = new List<Like>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Like Comment = new Like()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Bady = ParseNGuid(dr["BadyId"]).HasValue ? new Bady { ID = ParseNGuid(dr["BadyId"]).Value } : null,
                            Comment = ParseStr(dr["Comment"]),
                            Commentnum = ParseInt(dr["Commentnum"]),
                            Likednum = ParseInt(dr["Likednum"]),
                            Likenum = ParseInt(dr["Likenum"]),
                            Lnk = ParseStr(dr["Lnk"]),
                            Recordnum = ParseInt(dr["Recordnum"]),
                            Supernum = ParseInt(dr["Supernum"]),
                            Username = ParseStr(dr["Username"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"]),
                            Runstatues = ParseInt(dr["Runstatues"]),
                            Type = ParseInt(dr["Type"])
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
        /// Create a new Like
        /// </summary>
        /// <param name="newLike">new Like</param>
        /// <returns>new Like id</returns>
        public Guid Create(Like newLike)
        {
            string sql = string.Format("EXEC sp_Like_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}"
                                         , ToQuote(newLike.ID)
                                         , newLike.Bady == null ? "null" : ToQuote(newLike.Bady.ID)
                                         , ToQuote(newLike.Type)
                                         , ToQuote(newLike.Lnk)
                                         , ToQuote(newLike.Comment)
                                         , ToQuote(newLike.Username)
                                         , ToQuote(newLike.Likenum)
                                         , ToQuote(newLike.Recordnum)
                                         , ToQuote(newLike.Likednum)
                                         , ToQuote(newLike.Commentnum)
                                         , ToQuote(newLike.Supernum)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newLike.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newLike.ID;
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
        /// Update an existing Like
        /// </summary>
        /// <param name="thisLike">Like</param>
        /// <returns>bool</returns>
        public bool Update(Like thisLike)
        {
            string sql = string.Format("EXEC sp_Like_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                                       , ToQuote(thisLike.ID)
                                       , thisLike.Bady == null ? "null" : ToQuote(thisLike.Bady.ID)
                                       , ToQuote(thisLike.Type)
                                       , ToQuote(thisLike.Lnk)
                                         , ToQuote(thisLike.Comment)
                                         , ToQuote(thisLike.Username)
                                         , ToQuote(thisLike.Likenum)
                                         , ToQuote(thisLike.Recordnum)
                                         , ToQuote(thisLike.Likednum)
                                         , ToQuote(thisLike.Commentnum)
                                         , ToQuote(thisLike.Supernum)

                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisLike.Statues)

                                         , ToQuote(thisLike.Runstatues)
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
        /// Delete an existing Like
        /// </summary>
        /// <param name="thisLike">Like</param>
        /// <returns>bool</returns>
        public bool Delete(Like thisLike)
        {
            string sql = string.Format("exec dbo.sp_like_d {0} "
                                        , ToQuote(thisLike.ID)

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
        /// Check if a Like already exists
        /// </summary>
        /// <param name="name">Like Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Like by id
        /// </summary>
        /// <param name="id">Like id</param>
        /// <returns>Like</returns>
        public Like Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Like_g {0}, {1},{2},{3},{4},{5},{6},{7}"
                                , ToQuote(id)
                                , "NULL"
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
                IList<Like> likes = GetLikes(sql, 0, null, out paing);
                if (likes.Count > 0)
                    return likes[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Likes
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Like">current Like</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Like> Get(Guid? id, string badyid,string username,int? runstatues,int? type, int? statues, int Page, string sortKey,
            out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Like_g {0}, {1},{2},{3},{4},{5},{6},{7}"
                             , "NULL"
                             , ToQuote(badyid)
                             , ToQuote(username)
                             , runstatues.HasValue ? ToQuote(runstatues.Value) : "NULL"
                             , type.HasValue ? ToQuote(type.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Like> Likes = GetLikes(sql, Page, null, out paing);
                return Likes;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        public IList<Like> GetLiking(Guid? id, string badyid, string username, int? runstatues, int? type, int? statues, int Page, string sortKey,
         out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Like_g_liking {0}, {1},{2},{3},{4},{5},{6},{7}"
                             , "NULL"
                             , ToQuote(badyid)
                             , ToQuote(username)
                             , runstatues.HasValue ? ToQuote(runstatues.Value) : "NULL"
                             , type.HasValue ? ToQuote(type.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Like> Likes = GetLikes(sql, Page, null, out paing);
                return Likes;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
