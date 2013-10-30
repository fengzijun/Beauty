using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IUserToken
    {
        #region * CRUD *

        /// <summary>
        /// Create a new UserToken
        /// </summary>
        /// <param name="newUserToken">new UserToken</param>
        /// <returns>new UserToken id</returns>
        Guid Create(UserToken newUserToken);

        /// <summary>
        /// Update an existing UserToken
        /// </summary>
        /// <param name="thisUserToken">UserToken</param>
        /// <returns>bool</returns>
        bool Update(UserToken thisUserToken);

        /// <summary>
        /// Delete an existing UserToken
        /// </summary>
        /// <param name="thisUserToken">UserToken</param>
        /// <returns>bool</returns>
        bool Delete(UserToken thisUserToken);


        /// <summary>
        /// Check if a UserToken already exists
        /// </summary>
        /// <param name="name">UserToken Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a UserToken by id
        /// </summary>
        /// <param name="id">UserToken id</param>
        /// <returns>UserToken</returns>
        UserToken Get(Guid id);

        /// <summary>
        /// Get all UserTokens
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserToken">current UserToken</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<UserToken> Get(Guid? id, int? statues,
            int Page, string sortKey, out PaginationInfo paing);

        #endregion
    }
}
