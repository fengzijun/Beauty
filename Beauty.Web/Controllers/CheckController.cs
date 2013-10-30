using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Beauty.Web.Controllers
{
    using Beauty.InterFace;
    using Beauty.Model;
    using Beauty.Core;

    public class CheckController : Controller
    {
       
        // GET: /Check/
        private IUser iuser;

        public CheckController(IUser iuser)
        {
            this.iuser = iuser;
        }

        public JsonResult IsExistUsername(string username)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = iuser.Get(null, username, null, null, null, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsxistEmail(string email)
        {
            PaginationInfo paging = new PaginationInfo();
            IList<User> users = iuser.Get(null, null, null, null, email, null, null, null, null, null, 1, 0, null, out paging);
            if (users != null && users.Count > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
