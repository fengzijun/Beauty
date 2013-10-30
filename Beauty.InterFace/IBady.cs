using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IBady
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Bady
        /// </summary>
        /// <param name="newBady">new Bady</param>
        /// <returns>new Bady id</returns>
        Guid Create(Bady newBady);

        /// <summary>
        /// Update an existing Bady
        /// </summary>
        /// <param name="thisBady">Bady</param>
        /// <returns>bool</returns>
        bool Update(Bady thisBady);

        /// <summary>
        /// Delete an existing Bady
        /// </summary>
        /// <param name="thisBady">Bady</param>
        /// <returns>bool</returns>
        bool Delete(Bady thisBady);


        /// <summary>
        /// Check if a Bady already exists
        /// </summary>
        /// <param name="name">Bady Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Bady by id
        /// </summary>
        /// <param name="id">Bady id</param>
        /// <returns>Bady</returns>
        Bady Get(Guid id);

        /// <summary>
        /// Get all Badys
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Bady">current Bady</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Bady> Get(Guid? id, string username,string badyname, int? statues, int Page, string sortKey,
            out PaginationInfo paing,bool isfuzzy = false );

        IList<Bady> GetNeedToShare(Guid? id,string username, string badyname, int? statues, int Page, string sortKey,
            out PaginationInfo paing, bool isfuzzy = false);

        IList<Bady> GetNotNeedToShare(Guid? id, string username, string badyname, int? statues, int Page, string sortKey,
           out PaginationInfo paing, bool isfuzzy = false);

        #endregion
    }
}
