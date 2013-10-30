using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IUser
    {
        #region * CRUD *

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="newUser">new User</param>
        /// <returns>new User id</returns>
        Guid Create(User newUser);

        /// <summary>
        /// Update an existing User
        /// </summary>
        /// <param name="thisUser">User</param>
        /// <returns>bool</returns>
        bool Update(User thisUser);

        /// <summary>
        /// 心跳反应
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool LoginActive(string username, string ip);

        /// <summary>
        /// Delete an existing User
        /// </summary>
        /// <param name="thisUser">User</param>
        /// <returns>bool</returns>
        bool Delete(User thisUser);


        /// <summary>
        /// Check if a User already exists
        /// </summary>
        /// <param name="name">User Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a User by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        User Get(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <param name="issuper"></param>
        /// <returns></returns>
        int CheckconditionUser(int like, bool issuper,string username);
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="User">current User</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<User> Get(Guid? userid, string username, string mobile, string QQ, string email, bool? issuper, int? liked,
            int? role, string refer, bool? islogin, int? statues, int Page, string sortKey, out PaginationInfo paing);


        IList<User> GetByAdmin(Guid? userid, string username, string mobile, string QQ, string email, bool? issuper, int? liked,
           int? role, string refer, bool? islogin, int? statues, int Page, string sortKey, out PaginationInfo paing);
        #endregion
    }
}
