using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface ILike
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Like
        /// </summary>
        /// <param name="newLike">new Like</param>
        /// <returns>new Like id</returns>
        Guid Create(Like newLike);

        /// <summary>
        /// Update an existing Like
        /// </summary>
        /// <param name="thisLike">Like</param>
        /// <returns>bool</returns>
        bool Update(Like thisLike);

        /// <summary>
        /// Delete an existing Like
        /// </summary>
        /// <param name="thisLike">Like</param>
        /// <returns>bool</returns>
        bool Delete(Like thisLike);


        /// <summary>
        /// Check if a Like already exists
        /// </summary>
        /// <param name="name">Like Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Like by id
        /// </summary>
        /// <param name="id">Like id</param>
        /// <returns>Like</returns>
        Like Get(Guid id);

        /// <summary>
        /// Get all Likes
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Like">current Like</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Like> Get(Guid? id, string badyid, string username, int? runstatues, int? type, int? statues, int Page, string sortKey,
           out PaginationInfo paing);
        IList<Like> GetLiking(Guid? id, string badyid, string username, int? runstatues, int? type, int? statues, int Page, string sortKey,
         out PaginationInfo paing);
        #endregion
    }
}
