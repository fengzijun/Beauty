using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IUserSetting
    {
        #region * CRUD *

        /// <summary>
        /// Create a new UserSetting
        /// </summary>
        /// <param name="newUserSetting">new UserSetting</param>
        /// <returns>new UserSetting id</returns>
        Guid Create(UserSetting newUserSetting);

        /// <summary>
        /// Update an existing UserSetting
        /// </summary>
        /// <param name="thisUserSetting">UserSetting</param>
        /// <returns>bool</returns>
        bool Update(UserSetting thisUserSetting);

        /// <summary>
        /// Delete an existing UserSetting
        /// </summary>
        /// <param name="thisUserSetting">UserSetting</param>
        /// <returns>bool</returns>
        bool Delete(UserSetting thisUserSetting);


        /// <summary>
        /// Check if a UserSetting already exists
        /// </summary>
        /// <param name="name">UserSetting Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a UserSetting by id
        /// </summary>
        /// <param name="id">UserSetting id</param>
        /// <returns>UserSetting</returns>
        UserSetting Get(Guid id);

        /// <summary>
        /// Get all UserSettings
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="UserSetting">current UserSetting</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<UserSetting> Get(Guid? id, string username, Guid? settingid, int? statues,
            int Page, string sortKey, out PaginationInfo paing);

        IList<SettingGroup> GetByUsername(string username);

        #endregion
    }
}
