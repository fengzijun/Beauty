using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty.Model;
using Beauty.Core;


namespace Beauty.InterFace
{
    public interface IUserLoginCount
    {
        void Create(UserLoginCount newUserSetting);
    }
}
