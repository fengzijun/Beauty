using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IHelp
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Help
        /// </summary>
        /// <param name="newHelp">new Help</param>
        /// <returns>new Help id</returns>
        Guid Create(Help newHelp);

        /// <summary>
        /// Update an existing Help
        /// </summary>
        /// <param name="thisHelp">Help</param>
        /// <returns>bool</returns>
        bool Update(Help thisHelp);

        /// <summary>
        /// Delete an existing Help
        /// </summary>
        /// <param name="thisHelp">Help</param>
        /// <returns>bool</returns>
        bool Delete(Help thisHelp);


        /// <summary>
        /// Check if a Help already exists
        /// </summary>
        /// <param name="name">Help Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Help by id
        /// </summary>
        /// <param name="id">Help id</param>
        /// <returns>Help</returns>
        Help Get(string id);

        /// <summary>
        /// Get all Helps
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Help">current Help</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Help> Get(Guid? id, int? statues, int Page, string sortKey, out PaginationInfo paing);


        #endregion
    }
}
