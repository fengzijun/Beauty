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


    public class ShareService: BaseService, IShare
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Share> GetShares(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Share> Comments = new List<Share>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Share Comment = new Share()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Bady = new Bady{ ID = new Guid(dr["badyid"].ToString())},
                            Comment = ParseStr(dr["Comment"]),
                            IsSuper = bool.Parse(dr["IsSuper"].ToString()),
                            Keyword = ParseStr(dr["Keyword"]),
                            Liked = ParseInt(dr["Liked"]),
                            Link = ParseStr(dr["Link"]),
                            UserId = ParseNGuid(dr["UserId"].ToString()),
                            Username = ParseStr(dr["Username"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"]),
                            Runstatues = ParseInt(dr["Runstatues"])
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
        /// Create a new Share
        /// </summary>
        /// <param name="newShare">new Share</param>
        /// <returns>new Share id</returns>
        public Guid Create(Share newShare)
        {
            string sql = string.Format("EXEC sp_Share_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}"
                                         , ToQuote(newShare.ID)
                                         , ToQuote(newShare.Bady.ID)
                                         , "NULL"
                                         , "NULL"
                                         , "NULL"
                                         , ToQuote(newShare.IsSuper)
                                         , ToQuote(newShare.Liked)
                                         , ToQuote(newShare.UserId)
                                         , ToQuote(newShare.Username)
                                         , ToQuote(newShare.Comment)
                                         , ToQuote(newShare.Keyword)
                                         , ToQuote(newShare.Link)
                                         , ToQuote(newShare.Runstatues)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newShare.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newShare.ID;
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
        /// Update an existing Share
        /// </summary>
        /// <param name="thisShare">Share</param>
        /// <returns>bool</returns>
        public bool Update(Share thisShare)
        {
            string sql = string.Format("EXEC sp_Share_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}"
                                         , ToQuote(thisShare.ID)
                                         , ToQuote(thisShare.Bady.ID)
                                         , "NULL"
                                         , "NULL"
                                         , "NULL"
                                         , ToQuote(thisShare.IsSuper)
                                         , ToQuote(thisShare.Liked)
                                         , ToQuote(thisShare.UserId)
                                         , ToQuote(thisShare.Username)
                                         , ToQuote(thisShare.Comment)
                                         , ToQuote(thisShare.Keyword)
                                         , ToQuote(thisShare.Link)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisShare.Statues)

                                         , ToQuote(thisShare.Runstatues)
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
        /// Delete an existing Share
        /// </summary>
        /// <param name="thisShare">Share</param>
        /// <returns>bool</returns>
        public bool Delete(Share thisShare)
        {
            string sql = string.Format("exec dbo.sp_Share_d {0} "
                                        , ToQuote(thisShare.ID)

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
        /// Check if a Share already exists
        /// </summary>
        /// <param name="name">Share Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Share by id
        /// </summary>
        /// <param name="id">Share id</param>
        /// <returns>Share</returns>
        public Share Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Share_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
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
                IList<Share> Shares = GetShares(sql, 0, null, out paing);
                if (Shares.Count > 0)
                    return Shares[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Shares
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Share">current Share</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Share> Get(Guid? id, string username, Guid? userid, string badyid, bool? issuper, int? liked,int? runstatues, int? statues,
        int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Share_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                             , "NULL"
                             , ToQuote(badyid)
                             , "NULL"
                             , issuper.HasValue?ToQuote(issuper.Value):"NULL"
                             , liked.HasValue ? ToQuote(liked.Value) : "NULL"
                             , userid.HasValue ? ToQuote(userid.Value) : "NULL"
                             , ToQuote(username)
                             , "NULL"
                             , "NULL"
                             , runstatues.HasValue ? ToQuote(runstatues.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Share> Shares = GetShares(sql, Page, null, out paing);
                return Shares;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public IList<Share> GetSharing(Guid? id, string username, Guid? userid, string badyid, bool? issuper, int? liked, int? runstatues, int? statues,
        int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Share_g_sharing {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                             , "NULL"
                             , ToQuote(badyid)
                             , "NULL"
                             , issuper.HasValue ? ToQuote(issuper.Value) : "NULL"
                             , liked.HasValue ? ToQuote(liked.Value) : "NULL"
                             , userid.HasValue ? ToQuote(userid.Value) : "NULL"
                             , ToQuote(username)
                             , "NULL"
                             , "NULL"
                             , runstatues.HasValue ? ToQuote(runstatues.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Share> Shares = GetShares(sql, Page, null, out paing);
                return Shares;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
