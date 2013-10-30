using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IRequestMoney
    {
        #region * CRUD *

        /// <summary>
        /// Create a new RequstMoney
        /// </summary>
        /// <param name="newRequstMoney">new RequstMoney</param>
        /// <returns>new RequstMoney id</returns>
        Guid Create(RequstMoney newRequstMoney);

        /// <summary>
        /// Update an existing RequstMoney
        /// </summary>
        /// <param name="thisRequstMoney">RequstMoney</param>
        /// <returns>bool</returns>
        bool Update(RequstMoney thisRequstMoney);

        /// <summary>
        /// Delete an existing RequstMoney
        /// </summary>
        /// <param name="thisRequstMoney">RequstMoney</param>
        /// <returns>bool</returns>
        bool Delete(RequstMoney thisRequstMoney);


        /// <summary>
        /// Check if a RequstMoney already exists
        /// </summary>
        /// <param name="name">RequstMoney Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a RequstMoney by id
        /// </summary>
        /// <param name="id">RequstMoney id</param>
        /// <returns>RequstMoney</returns>
        RequstMoney Get(Guid id);

        /// <summary>
        /// Get all RequstMoneys
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="RequstMoney">current RequstMoney</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<RequstMoney> Get(Guid? id, string username,string msg ,int? statues, int Page, string sortKey, out PaginationInfo paing);

        /// <summary>
        /// get dealed msg
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <param name="statues"></param>
        /// <param name="Page"></param>
        /// <param name="sortKey"></param>
        /// <param name="paing"></param>
        /// <returns></returns>
        IList<RequstMoney> Getdealed(Guid? id, string username, string msg, int? statues, int Page, string sortKey, out PaginationInfo paing);
        #endregion
    }
}
