using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface IUserAccount
    {
        void Create(UserAccount newUserSetting);

        IList<UserAccount> Get(Guid? id, string username,int? type, int? statues, int Page, string sortKey,
        out PaginationInfo paing, bool isfuzzy = false);
    }
}
