using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Core;

namespace Beauty.InterFace
{
    public interface ILog
    {
        #region * CRUD *

       
        Guid Create(Log newLog);

    
        IList<Log> Get(Guid? id, int? statues, int Page, string sortKey, out PaginationInfo paing);


        #endregion
    }
}
