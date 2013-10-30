using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace Beauty.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    using Beauty.InterFace;
    using Beauty.Service;
    using System.Reflection;
    using Beauty.Web.Controllers;

    public class MvcApplication : System.Web.HttpApplication
    {
        private void RegisterUnityResolver()
        {
            UnityDependencyResolver unity = new UnityDependencyResolver();
            System.Web.Mvc.DependencyResolver.SetResolver(unity);
            unity.RegisterType<IFirstPage, FirstPageService>();
            unity.RegisterType<ILike, LIkeService>();
            unity.RegisterType<IMoney, MoneyService>();
            unity.RegisterType<IPrice, PriceService>();
            unity.RegisterType<ISetting, SettingService>();
            unity.RegisterType<IShare, ShareService>();
            unity.RegisterType<ITask, TaskService>();
            unity.RegisterType<IUser, UserService>();
            unity.RegisterType<IUserSetting, UserSettingService>();
            unity.RegisterType<IUserStore, UserStoreService>();
            unity.RegisterType<IUserToken, UserTokenService>();
            unity.RegisterType<IBady, BadyService>();
            unity.RegisterType<IGroup, GroupService>();
            unity.RegisterType<IFirstPageArg, FirstPageArgService>();
            unity.RegisterType<ILog, LogService>();
            unity.RegisterType<INotice, NoticeService>();
            unity.RegisterType<IRequestMoney, RequestMoneyService>();
            unity.RegisterType<IHelp, HelpService>();
            unity.RegisterType<IUserAccount, UserAccountService>();
            //
            //  Register all controller type found in current assembly so the Unity container will be able to resolve them
            //
            foreach (Type controllerType in (from t in Assembly.GetExecutingAssembly().GetTypes() where typeof(IController).IsAssignableFrom(t) select t))
            {
                unity.RegisterType(controllerType);
            }

            ILog bomb2 = unity.Resolve<ILog>();  
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            EntityMapper.Self.CreateMap();
            RegisterUnityResolver();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            //var Domain = System.Configuration.ConfigurationManager.AppSettings["PayFabricDomain"] as string;
            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            string msg = ex.GetType().ToString();
            //if (msg == "Nodus.Applications.ServiceProxy.LogonException")
            //{
            //    //httpContext.Response.RedirectToRoute(new RouteValueDictionary { controller ="account", Action="logon"});
            //    httpContext.Response.Redirect(Domain+"/account/LogOff");
            //}
            //else
            //{

            //}
            var controller = new ErrorController();
            var routeData = new RouteData();

            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;

                    case 401:
                        action = "AccessDenied";
                        break;
                    case 403:
                        action = "RePostError";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}