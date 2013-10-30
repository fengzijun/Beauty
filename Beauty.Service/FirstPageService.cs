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


    public class FirstPageService : BaseService, IFirstPage
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<FirstPage> GetFirstPages(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<FirstPage> Comments = new List<FirstPage>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FirstPage Comment = new FirstPage()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            BadyId = ParseStr(dr["BadyId"]),
                            Comment = ParseInt(dr["Comment"]),
                            Liked = ParseInt(dr["Liked"]),
                            Link = ParseStr(dr["Link"]),
                            Page = ParseInt(dr["page"]),
                            Record = ParseInt(dr["Record"]),
                            mtype = ParseStr(dr["mtype"]),
                            Type = ParseStr(dr["Type"]),
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
        /// Create a new FirstPage
        /// </summary>
        /// <param name="newFirstPage">new FirstPage</param>
        /// <returns>new FirstPage id</returns>
        public Guid Create(FirstPage newFirstPage)
        {
     
            string sql = string.Format("EXEC sp_firstpage_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31}"
                                         , ToQuote(newFirstPage.ID)
                                         , ToQuote(newFirstPage.twitter_id)
                                         , ToQuote(newFirstPage.twitter_author_uid)
                                         , ToQuote(newFirstPage.twitter_show_type)
                                         , ToQuote(newFirstPage.twitter_images_id)
                                         , ToQuote(newFirstPage.twitter_source_tid)
                                         , ToQuote(newFirstPage.twitter_htmlcontent)
                                         , ToQuote(newFirstPage.twitter_goods_id)
                                         , ToQuote(newFirstPage.twitter_pic_type)
                                         , ToQuote(newFirstPage.like_twitter_id)
                                         , ToQuote(newFirstPage.like_author_uid)
                                         , ToQuote(newFirstPage.from_act_name)
                                         , ToQuote(newFirstPage.from_act_id)
                                         , ToQuote(newFirstPage.goods_price)
                                         , ToQuote(newFirstPage.goods_title)
                                         , ToQuote(newFirstPage.goods_pic_url)
                                         , ToQuote(newFirstPage.goods_url)
                                         , ToQuote(newFirstPage.goods_picture_id)
                                         , ToQuote(newFirstPage.show_pic)
                                         , ToQuote(newFirstPage.BadyId)
                                         , ToQuote(newFirstPage.Type)
                                         , ToQuote(newFirstPage.Page)
                                         , ToQuote(newFirstPage.Liked)
                                         , ToQuote(newFirstPage.Record)
                                         , ToQuote(newFirstPage.Comment)
                                         , ToQuote(newFirstPage.Link)
                                         , ToQuote(newFirstPage.Rank)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newFirstPage.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newFirstPage.ID;
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
        /// create batch into table 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public bool Createbatch(DataTable dt, string tablename)
        {
            try
            {
                SqlHelper.SqlBulkCopyInsert(dt, tablename, ConnectStr);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Update an existing FirstPage
        /// </summary>
        /// <param name="thisFirstPage">FirstPage</param>
        /// <returns>bool</returns>
        public bool Update(FirstPage thisFirstPage)
        {
            string sql = string.Format("EXEC sp_firstpage_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}"
                                       , ToQuote(thisFirstPage.ID)
                                       , ToQuote(thisFirstPage.BadyId)
                                       , ToQuote(thisFirstPage.Type)
                                       , ToQuote(thisFirstPage.Page)
                                       , ToQuote(thisFirstPage.Liked)
                                       , ToQuote(thisFirstPage.Record)
                                       , ToQuote(thisFirstPage.Comment)
                                       , ToQuote(thisFirstPage.Link)
                                       , ToQuote(CurrentUserName)
                                       , ToQuote(DateTime.Now)
                                       , ToQuote(thisFirstPage.Statues)


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
        /// Delete an existing FirstPage
        /// </summary>
        /// <param name="thisFirstPage">FirstPage</param>
        /// <returns>bool</returns>
        public bool Delete(string type)
        {
            string sql = string.Format("exec dbo.sp_group_d {0} "
                                       , ToQuote(type)

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

        public void DeleteByType(string type,string mtype)
        {
            string sql = string.Format("delete firstpage where type = N'" + type + "' and mtype='" + mtype + "'"


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

        public bool DeleteAll()
        {
            string sql = string.Format("exec dbo.sp_firstpage_d_all  "
                                    

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Check if a FirstPage already exists
        /// </summary>
        /// <param name="name">FirstPage Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a FirstPage by id
        /// </summary>
        /// <param name="id">FirstPage id</param>
        /// <returns>FirstPage</returns>
        public FirstPage Get(Guid id)
        {
            string sql = string.Format("EXEC sp_FirstPage_g {0}, {1},{2},{3},{4},{5},{6}"
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
                IList<FirstPage> firstpages = GetFirstPages(sql, 0, null, out paing);
                if (firstpages.Count > 0)
                    return firstpages[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all FirstPages
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="FirstPage">current FirstPage</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<FirstPage> Get(Guid? id, string badyid, string type, int? statues, int Page, string sortKey,
            out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_FirstPage_g {0}, {1},{2},{3},{4},{5},{6}"
                             , "NULL"
                             , ToQuote(badyid)
                             , ToQuote(type)
                             , "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<FirstPage> firstpages = GetFirstPages(sql, Page, null, out paing);
                return firstpages;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
