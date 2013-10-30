using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface ITask
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Task
        /// </summary>
        /// <param name="newTask">new Task</param>
        /// <returns>new Task id</returns>
        Guid Create(Task newTask);

        /// <summary>
        /// Update an existing Task
        /// </summary>
        /// <param name="thisTask">Task</param>
        /// <returns>bool</returns>
        bool Update(Task thisTask);

        /// <summary>
        /// Delete an existing Task
        /// </summary>
        /// <param name="thisTask">Task</param>
        /// <returns>bool</returns>
        bool Delete(Task thisTask);


        /// <summary>
        /// Check if a Task already exists
        /// </summary>
        /// <param name="name">Task Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <returns>Task</returns>
        Task Get(Guid id);

        /// <summary>
        /// Get all Tasks
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Task">current Task</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Task> Get(Guid? id, string username, Guid? taskid, string tasktype, string type, bool? isauto, int? runstatues, int? statues,
              int Page, string sortKey, out PaginationInfo paing);

        /// <summary>
        /// 判断是不是完成了LIKE任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskid"></param>
        void CheckTaskComplete(Guid id, Guid taskid);


        /// <summary>
        /// 获取不自动的收录和超级主编收录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="statues"></param>
        /// <param name="Page"></param>
        /// <param name="sortKey"></param>
        /// <param name="paing"></param>
        /// <returns></returns>
        IList<Task> GetUnAutoRecordTask(string username, int? statues, int Page, string sortKey, out PaginationInfo paing);

        #endregion
    }
}
