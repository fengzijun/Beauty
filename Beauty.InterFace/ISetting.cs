using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface ISetting
    {
        #region * CRUD *

        /// <summary>
        /// Create a new Setting
        /// </summary>
        /// <param name="newSetting">new Setting</param>
        /// <returns>new Setting id</returns>
        Guid Create(Setting newSetting);

        /// <summary>
        /// Update an existing Setting
        /// </summary>
        /// <param name="thisSetting">Setting</param>
        /// <returns>bool</returns>
        bool Update(Setting thisSetting);

        /// <summary>
        /// Delete an existing Setting
        /// </summary>
        /// <param name="thisSetting">Setting</param>
        /// <returns>bool</returns>
        bool Delete(Setting thisSetting);


        /// <summary>
        /// Check if a Setting already exists
        /// </summary>
        /// <param name="name">Setting Name</param>
        /// <returns>bool</returns>
        bool Exists(string name);

        /// <summary>
        /// Get a Setting by id
        /// </summary>
        /// <param name="id">Setting id</param>
        /// <returns>Setting</returns>
        Setting Get(Guid id);

        /// <summary>
        /// Get all Settings
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="Setting">current Setting</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        IList<Setting> Get(Guid? id, string category, string name, string type, int? statues, int Page, string sortKey,
            out PaginationInfo paing);

        IList<SettingGroup> GetSystemSetting();
        #endregion
    }
}
