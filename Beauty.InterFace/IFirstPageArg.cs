using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IFirstPageArg
    {
        #region * CRUD *

        /// <summary>
        /// Create a new FirstPageArg
        /// </summary>
        /// <param name="newFirstPageArg">new FirstPageArg</param>
        /// <returns>new FirstPageArg id</returns>
        Guid Create(FirstPageArg newFirstPageArg);

        /// <summary>
        /// Update an existing FirstPageArg
        /// </summary>
        /// <param name="thisFirstPageArg">FirstPageArg</param>
        /// <returns>bool</returns>
        bool Update(FirstPageArg thisFirstPageArg);

        /// <summary>
        /// Delete an existing FirstPageArg
        /// </summary>
        /// <param name="thisFirstPageArg">FirstPageArg</param>
        /// <returns>bool</returns>
        bool Delete(FirstPageArg thisFirstPageArg);

        

        /// <summary>
        /// Check if a FirstPageArg already exists
        /// </summary>
        /// <param name="name">FirstPageArg Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a FirstPageArg by id
        /// </summary>
        /// <param name="id">FirstPageArg id</param>
        /// <returns>FirstPageArg</returns>
        FirstPageArg Get(Guid id);

        /// <summary>
        /// Get all FirstPageArgs
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="FirstPageArg">current FirstPageArg</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<FirstPageArg> Get(Guid? id, string type,string mtype, int? statues, int Page, string sortKey,
            out PaginationInfo paing);

        void FirstPageArgRecord();

        #endregion
    }
}
