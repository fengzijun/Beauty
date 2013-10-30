using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IShare
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Share
        /// </summary>
        /// <param name="newShare">new Share</param>
        /// <returns>new Share id</returns>
        Guid Create(Share newShare);

        /// <summary>
        /// Update an existing Share
        /// </summary>
        /// <param name="thisShare">Share</param>
        /// <returns>bool</returns>
        bool Update(Share thisShare);

        /// <summary>
        /// Delete an existing Share
        /// </summary>
        /// <param name="thisShare">Share</param>
        /// <returns>bool</returns>
        bool Delete(Share thisShare);


        /// <summary>
        /// Check if a Share already exists
        /// </summary>
        /// <param name="name">Share Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Share by id
        /// </summary>
        /// <param name="id">Share id</param>
        /// <returns>Share</returns>
        Share Get(Guid id);

        /// <summary>
        /// Get all Shares
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Share">current Share</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Share> Get(Guid? id, string username, Guid? userid, string badyid, bool? issuper, int? liked, int? runstatues, int? statues,
          int Page, string sortKey, out PaginationInfo paing);


        IList<Share> GetSharing(Guid? id, string username, Guid? userid, string badyid, bool? issuper, int? liked, int? runstatues, int? statues,
        int Page, string sortKey, out PaginationInfo paing);
        #endregion
    }
}
