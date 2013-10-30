using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IPrice
    {
        #region * CRUD *

        /// <summary>
        /// Create a new BeautyPrice
        /// </summary>
        /// <param name="newBeautyPrice">new BeautyPrice</param>
        /// <returns>new BeautyPrice id</returns>
        Guid Create(BeautyPrice newBeautyPrice);

        /// <summary>
        /// Update an existing BeautyPrice
        /// </summary>
        /// <param name="thisBeautyPrice">BeautyPrice</param>
        /// <returns>bool</returns>
        bool Update(BeautyPrice thisBeautyPrice);

        /// <summary>
        /// Delete an existing BeautyPrice
        /// </summary>
        /// <param name="thisBeautyPrice">BeautyPrice</param>
        /// <returns>bool</returns>
        bool Delete(BeautyPrice thisBeautyPrice);


        /// <summary>
        /// Check if a BeautyPrice already exists
        /// </summary>
        /// <param name="name">BeautyPrice Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a BeautyPrice by id
        /// </summary>
        /// <param name="id">BeautyPrice id</param>
        /// <returns>BeautyPrice</returns>
        BeautyPrice Get(Guid id);

        /// <summary>
        /// Get all BeautyPrices
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="BeautyPrice">current BeautyPrice</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<BeautyPrice> Get(Guid? id, string pricename,  int? statues, int Page, string sortKey, out PaginationInfo paing);


        #endregion
    }
}
