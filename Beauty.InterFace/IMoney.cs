using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IMoney
    {
        #region * CRUD *

        /// <summary>
        /// Create a new MoneyRecord
        /// </summary>
        /// <param name="newMoneyRecord">new MoneyRecord</param>
        /// <returns>new MoneyRecord id</returns>
        Guid Create(MoneyRecord newMoneyRecord);

        /// <summary>
        /// Update an existing MoneyRecord
        /// </summary>
        /// <param name="thisMoneyRecord">MoneyRecord</param>
        /// <returns>bool</returns>
        bool Update(MoneyRecord thisMoneyRecord);

        /// <summary>
        /// Delete an existing MoneyRecord
        /// </summary>
        /// <param name="thisMoneyRecord">MoneyRecord</param>
        /// <returns>bool</returns>
        bool Delete(MoneyRecord thisMoneyRecord);


        /// <summary>
        /// Check if a MoneyRecord already exists
        /// </summary>
        /// <param name="name">MoneyRecord Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a MoneyRecord by id
        /// </summary>
        /// <param name="id">MoneyRecord id</param>
        /// <returns>MoneyRecord</returns>
        MoneyRecord Get(Guid id);

        /// <summary>
        /// Get all MoneyRecords
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="MoneyRecord">current MoneyRecord</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<MoneyRecord> Get(Guid? id, string username, Guid? userid,string type, int? statues, int Page, string sortKey,
            out PaginationInfo paing);


        IList<MoneyRecord> GetAvail(Guid? id, string username, Guid? userid, string type, int? statues, int Page, string sortKey,
          out PaginationInfo paing);

        #endregion
    }
}
