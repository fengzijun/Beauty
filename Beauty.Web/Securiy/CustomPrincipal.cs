using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Threading;

namespace Nodus.Authentication
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal() { }

        public CustomPrincipal(ICustomIdentity identity) 
        {
            this.Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return false;
            }
            return ((ICustomIdentity)Identity).IsInRole(role);
        }


        public void Logout()
        {
             HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
             if (cookie != null)
             {
                 FormsAuthentication.SignOut();
                 HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
             }
            HttpContext.Current.User = 
                new GenericPrincipal(new GenericIdentity(""), new string[] { });
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="rememberMe">True, if authentication should persist between browser sessions
        /// </param>
        /// <returns>True if login succeeds</returns>
        public bool Login(string userName, string id, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, true, FormsAuthentication.FormsCookiePath);
            var identity = CustomIdentity.GetCustomIdentity(userName,id);
            if (identity.IsAuthenticated)
            {
                this.Identity = identity;
                HttpContext.Current.User = new CustomPrincipal(identity);
                Thread.CurrentPrincipal = new CustomPrincipal(identity);
           
                if (rememberMe)
                {
                  
                    double defulttimeout = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["defultretimeout"]) ? 14400 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["defultretimeout"]);
                    FormsAuthenticationTicket ticket =
                           new FormsAuthenticationTicket(
                               1, identity.Name, DateTime.Now, DateTime.Now.AddMinutes(defulttimeout), rememberMe,
                               identity.ToJson(), FormsAuthentication.FormsCookiePath);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    double defulttimeout = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["defulttimeout"]) ? 1440 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["defulttimeout"]);
                    FormsAuthenticationTicket ticket =
                           new FormsAuthenticationTicket(
                               1, identity.Name, DateTime.Now, DateTime.Now.AddMinutes(defulttimeout), rememberMe,
                               identity.ToJson(), FormsAuthentication.FormsCookiePath);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Path = FormsAuthentication.FormsCookiePath;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                
                
                
            }
            return identity.IsAuthenticated;
        }

        public bool Login(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, true);
            return true;
        }

        //public bool Login(string cookieString)
        //{
        //    ICustomIdentity identity = CustomIdentity.FromJson(cookieString);
        //    if (identity.IsAuthenticated)
        //    {
        //        HttpContext.Current.User = new CustomPrincipal(identity);
        //        Thread.CurrentPrincipal = new CustomPrincipal(identity);
        //    }
        //    return identity.IsAuthenticated;
        //}
    }
}