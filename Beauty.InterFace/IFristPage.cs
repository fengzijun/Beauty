using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;
using System.Data;

namespace Beauty.InterFace
{
    public interface IFirstPage
    {
        #region * CRUD *

        /// <summary>
        /// Create a new FirstPage
        /// </summary>
        /// <param name="newFirstPage">new FirstPage</param>
        /// <returns>new FirstPage id</returns>
        Guid Create(FirstPage newFirstPage);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        bool Createbatch(DataTable dt, string tablename);
        /// <summary>
        /// Update an existing FirstPage
        /// </summary>
        /// <param name="thisFirstPage">FirstPage</param>
        /// <returns>bool</returns>
        bool Update(FirstPage thisFirstPage);

        /// <summary>
        /// Delete an existing FirstPage
        /// </summary>
        /// <param name="thisFirstPage">FirstPage</param>
        /// <returns>bool</returns>
        bool Delete(string type);

        bool DeleteAll();

        void DeleteByType(string type, string mtype);
        /// <summary>
        /// Check if a FirstPage already exists
        /// </summary>
        /// <param name="name">FirstPage Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a FirstPage by id
        /// </summary>
        /// <param name="id">FirstPage id</param>
        /// <returns>FirstPage</returns>
        FirstPage Get(Guid id);

        /// <summary>
        /// Get all FirstPages
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="FirstPage">current FirstPage</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<FirstPage> Get(Guid? id, string badyid ,string type, int? statues, int Page, string sortKey, 
            out PaginationInfo paing);

        #endregion
    }
}
