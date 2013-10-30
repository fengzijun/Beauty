using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface INotice
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Notice
        /// </summary>
        /// <param name="newNotice">new Notice</param>
        /// <returns>new Notice id</returns>
        Guid Create(Notice newNotice);

        /// <summary>
        /// Update an existing Notice
        /// </summary>
        /// <param name="thisNotice">Notice</param>
        /// <returns>bool</returns>
        bool Update(Notice thisNotice);

        /// <summary>
        /// Delete an existing Notice
        /// </summary>
        /// <param name="thisNotice">Notice</param>
        /// <returns>bool</returns>
        bool Delete(Notice thisNotice);


        /// <summary>
        /// Check if a Notice already exists
        /// </summary>
        /// <param name="name">Notice Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Notice by id
        /// </summary>
        /// <param name="id">Notice id</param>
        /// <returns>Notice</returns>
        Notice Get(Guid id);

        /// <summary>
        /// Get all Notices
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Notice">current Notice</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Notice> Get(Guid? id, int? type, int? statues, int Page, string sortKey, out PaginationInfo paing);

        IList<Notice> GetOneSystemNoice(Guid? id, string userid, int? type, int? statues, int Page, string sortKey, out PaginationInfo paing);

        Guid ReadNotice(ReadNoitce newNotice);
        #endregion
    }
}
