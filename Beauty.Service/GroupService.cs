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

    public class GroupService : BaseService, IGroup
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Group> GetGroups(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Group> Comments = new List<Group>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Group Comment = new Group()
                        {
                            ID = dr["id"].ToString(),
                            Name = ParseStr(dr["Name"]),
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
        /// Create a new Group
        /// </summary>
        /// <param name="newGroup">new Group</param>
        /// <returns>new Group id</returns>
        public string Create(Group newGroup)
        {
            string sql = string.Format("EXEC sp_Group_c {0},{1},{2},{3},{4},{5},{6},{7}"
                                         , ToQuote(newGroup.ID)
                                         , ToQuote(newGroup.Username)
                                         , ToQuote(newGroup.Name)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newGroup.Statues)


                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newGroup.ID;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
        /// <summary>
        /// Update an existing Group
        /// </summary>
        /// <param name="thisGroup">Group</param>
        /// <returns>bool</returns>
        public bool Update(Group thisGroup)
        {
            string sql = string.Format("EXEC sp_Group_u {0},{1},{2},{3},{4},{5}"
                                         , ToQuote(thisGroup.ID)
                                         , ToQuote(thisGroup.Username)
                                         , ToQuote(thisGroup.Name)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisGroup.Statues)

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
        /// Delete an existing Group
        /// </summary>
        /// <param name="thisGroup">Group</param>
        /// <returns>bool</returns>
        public bool Delete(Group thisGroup)
        {
            string sql = string.Format("exec dbo.sp_group_d {0} "
                                       , ToQuote(thisGroup.Username)

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
        /// Check if a Group already exists
        /// </summary>
        /// <param name="name">Group Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Group by id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        public Group Get(string id)
        {
            string sql = string.Format("EXEC sp_Group_g {0}, {1},{2},{3},{4}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"


                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<Group> Groups = GetGroups(sql, 0, null, out paing);
                if (Groups.Count > 0)
                    return Groups[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Group">current Group</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Group> Get(Guid? id, string username, int? statues, int Page, string sortKey,out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Group_g {0}, {1},{2},{3},{4}"
                             , "NULL"
                             , ToQuote(username)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Group> Groups = GetGroups(sql, Page, null, out paing);
                return Groups;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

     
        #endregion
    }
}
