using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IGroup
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Group
        /// </summary>
        /// <param name="newGroup">new Group</param>
        /// <returns>new Group id</returns>
        string Create(Group newGroup);

        /// <summary>
        /// Update an existing Group
        /// </summary>
        /// <param name="thisGroup">Group</param>
        /// <returns>bool</returns>
        bool Update(Group thisGroup);

        /// <summary>
        /// Delete an existing Group
        /// </summary>
        /// <param name="thisGroup">Group</param>
        /// <returns>bool</returns>
        bool Delete(Group thisGroup);


        /// <summary>
        /// Check if a Group already exists
        /// </summary>
        /// <param name="name">Group Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Group by id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group</returns>
        Group Get(string id);

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Group">current Group</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Group> Get(Guid? id, string username, int? statues, int Page, string sortKey, out PaginationInfo paing);


        #endregion
    }
}
