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

    public class TaskService : BaseService, ITask
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for Taskd by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<Task> GetTasks(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<Task> Comments = new List<Task>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Task Comment = new Task()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Taskid = new Guid(dr["Taskid"].ToString()),
                            Type = ParseStr(dr["Type"]),
                            TaskType = ParseStr(dr["TaskType"]),
                            Username = ParseStr(dr["Username"]),
                            Keyword = ParseStr(dr["Keyword"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"]),
                            IsAuto = bool.Parse(dr["IsAuto"].ToString()),
                            NewType = ParseStr(dr["NewType"]),
                            Comment = ParseStr(dr["Comment"]),
                            Autoflag = ParseNBool(dr["Autoflag"]),
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
        /// Create a new Task
        /// </summary>
        /// <param name="newTask">new Task</param>
        /// <returns>new Task id</returns>
        public Guid Create(Task newTask)
        {
            string sql = string.Format("EXEC sp_Task_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
                                         , ToQuote(newTask.ID)
                                         , ToQuote(newTask.Taskid)
                                          , ToQuote(newTask.TaskType)
                                         , ToQuote(newTask.Username)
                                         , ToQuote(newTask.IsAuto)
                                         , ToQuote(newTask.Type)
                                         , ToQuote(newTask.NewType)
                                         , ToQuote(newTask.Comment)
                                         , ToQuote(newTask.Keyword)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newTask.Statues)
                                         , ToQuote(newTask.Runstatues)
                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newTask.ID;
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
        /// Update an existing Task
        /// </summary>
        /// <param name="thisTask">Task</param>
        /// <returns>bool</returns>
        public bool Update(Task thisTask)
        {
            string sql = string.Format("EXEC sp_Task_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                                         , ToQuote(thisTask.ID)
                                         , ToQuote(thisTask.Taskid)
                                         , ToQuote(thisTask.TaskType)
                                         , ToQuote(thisTask.Username)
                                         , ToQuote(thisTask.IsAuto)
                                         , ToQuote(thisTask.Type)
                                         , ToQuote(thisTask.NewType)
                                         , ToQuote(thisTask.Comment)
                                         , ToQuote(thisTask.Keyword)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(thisTask.Statues)
                                         , ToQuote(thisTask.Runstatues)
                                         , thisTask.Autoflag.HasValue?ToQuote(thisTask.Autoflag.Value):"NULL"
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
        /// Delete an existing Task
        /// </summary>
        /// <param name="thisTask">Task</param>
        /// <returns>bool</returns>
        public bool Delete(Task thisTask)
        {
            string sql = string.Format("exec dbo.sp_Task_d {0} "
                                        , ToQuote(thisTask.ID)

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
        /// Check if a Task already exists
        /// </summary>
        /// <param name="name">Task Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <returns>Task</returns>
        public Task Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Task_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
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
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<Task> Tasks = GetTasks(sql, 0, null, out paing);
                if (Tasks.Count > 0)
                    return Tasks[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Tasks
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Task">current Task</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<Task> Get(Guid? id, string username, Guid? taskid, string tasktype, string type, bool? isauto, int? runstatues, int? statues,
           int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Task_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                             , "NULL"
                             , taskid.HasValue ? ToQuote(taskid.Value) : "NULL"
                             , ToQuote(tasktype)
                             , isauto.HasValue ? ToQuote(isauto.Value) : "NULL"
                              , runstatues.HasValue ? ToQuote(runstatues.Value) : "NULL"
                             , ToQuote(username)
                              , ToQuote(type)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Task> Tasks = GetTasks(sql, Page, null, out paing);
                return Tasks;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public void CheckTaskComplete(Guid id, Guid taskid)
        {
            string sql = string.Format("exec dbo.sp_checkliketaskComplete {0} ,{1}"
                                        , ToQuote(id)
                                        , ToQuote(taskid)
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

        public IList<Task> GetUnAutoRecordTask(string username, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Task_g_unautorecord {0},{1},{2},{3}"
                             , ToQuote(username)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<Task> Tasks = GetTasks(sql, Page, null, out paing);
                return Tasks;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        #endregion
    }
}
