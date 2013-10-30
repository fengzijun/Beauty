using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Beauty.Web.Helper
{
    public class HtmlHelper
    {
        public static string OutputException(Exception ex)
        {
            string html = "<li>" + ex.Message + "</li>" + Environment.NewLine;

            if (ex.InnerException != null)
            {
                html += OutputException(ex.InnerException);
            }

            return html;
        }
    }
}