using System.Web;
using System.Web.Optimization;

namespace Beauty.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-1.8.2.js",
                        "~/Scripts/bootstrap.*",
                        "~/Scripts/jquery-ui-1.8.24.js",
                        "~/Scripts/knockout-2.2.0.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/html5.js",
                        "~/Scripts/g.js"));

          
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/themes/base/bootstrap.css",
                        "~/Content/themes/base/jquery-ui-1.10.0.custom.css",
                        "~/Content/site.css"
                     
            ));

          
        }
    }
}