using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;
using System.Data;

namespace Beauty.InterFace
{
    public interface IUserStore
    {
        #region * CRUD *

        /// <summary>
        /// Create a new UserStore
        /// </summary>
        /// <param name="newUserStore">new UserStore</param>
        /// <returns>new UserStore id</returns>
        Guid Create(UserStore newUserStore);

        bool Createbatch(DataTable dt, string tablename);
        /// <summary>
        /// Update an existing UserStore
        /// </summary>
        /// <param name="thisUserStore">UserStore</param>
        /// <returns>bool</returns>
        bool Update(UserStore thisUserStore);

        /// <summary>
        /// Delete an existing UserStore
        /// </summary>
        /// <param name="thisUserStore">UserStore</param>
        /// <returns>bool</returns>
        bool Delete(UserStore thisUserStore);


        /// <summary>
        /// Check if a UserStore already exists
        /// </summary>
        /// <param name="name">UserStore Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        bool DeleteAll();
        /// <summary>
        /// Get a UserStore by id
        /// </summary>
        /// <param name="id">UserStore id</param>
        /// <returns>UserStore</returns>
        UserStore Get(Guid id);

        /// <summary>
        /// Get all UserStores
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserStore">current UserStore</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<UserStore> Get(Guid? id,string username,string badyid, int? statues,
            int Page, string sortKey, out PaginationInfo paing);

        bool UpdateAll();

        #endregion
    }
}
