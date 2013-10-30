using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Reflection;
using System.Collections;

namespace Beauty.Common
{
    public class HttpHelper
    {
        // Fields
        //private static string accept = "text/html,application/json,text/javascript,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private static CookieContainer cc = new CookieContainer();
        //private static string contentType = "application/x-www-form-urlencoded";
        //private static int currentTry = 0;
        //private static int delay = 0x3e8;
        //private static Encoding encoding = Encoding.GetEncoding("utf-8");
        //private static int maxTry = 300;
        //private static string userAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";

        // Methods
        public static void Download(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string dir, string filename, ImageFormat format)
        {
            Image.FromStream(GetStream(url,userAgent,accept,contentType, cookieContainer, wp, ed)).Save(dir + filename, format);
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, WebProxy wq, Encoding ed)
        {
            return GetHtml(url,userAgent,accept,contentType, cc, wq, ed);
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if(!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
               
            
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
               

                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer, bool isajax)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
                if (isajax)
                {
                    request.Headers.Set("X-Requested-With", "XMLHttpRequest");
                }
           
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();
           
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream,  ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, WebProxy wp, Encoding ed)
        {
            return GetHtml(url, userAgent, accept, contentType, postData, isPost, cc, wp, ed);
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, CookieContainer cookieContainer, WebProxy wp, Encoding ed)
        {
            if (string.IsNullOrEmpty(postData))
            {
                return GetHtml(url, userAgent, accept, contentType, cookieContainer, wp, ed);
            }
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] bytes = null;
                bytes = ed.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = isPost ? "POST" : "GET";
               
            
                request.ContentLength = bytes.Length;
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
          
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer, bool isajax)
        {
          
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] bytes = null;
                bytes = ed.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = isPost ? "POST" : "GET";
                request.ServicePoint.Expect100Continue = false;
                
                if (isajax)
                {
                    request.Headers.Set("X-Requested-With", "XMLHttpRequest");
                }
             
                request.ContentLength = bytes.Length;
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();

                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetSSLHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, WebProxy wp, Encoding ed)
        {
            return GetSSLHtml(url, userAgent, accept, contentType, postData, isPost, cc, wp, ed);
        }

        public static string GetSSLHtml(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";

                var sp = request.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                prop.SetValue(sp, (byte)0, null);


                request.ServicePoint.Expect100Continue = false;
                request.Credentials = CredentialCache.DefaultCredentials;
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetSSLHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, CookieContainer cookieContainer, WebProxy wp, Encoding ed)
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] bytes = null;
                bytes = ed.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
              
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = isPost ? "POST" : "GET";

                var sp = request.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                prop.SetValue(sp, (byte)0, null);

              
                request.ServicePoint.Expect100Continue = false;
                request.Credentials = CredentialCache.DefaultCredentials;
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                request.ContentLength = bytes.Length;
               
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();

                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static string GetSSLHtml(string url, string userAgent, string accept, string contentType, string postData, bool isPost, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer, bool isajax)
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] bytes = null;
                bytes = ed.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = isPost ? "POST" : "GET";

                var sp = request.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                prop.SetValue(sp, (byte)0, null);

                if (isajax)
                {
                    request.Headers.Set("X-Requested-With", "XMLHttpRequest");
                }
                request.ServicePoint.Expect100Continue = false;
                request.ContentLength = bytes.Length;
                request.Credentials = CredentialCache.DefaultCredentials;
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, ed);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();

                request.Abort();
                response.Close();
                return str;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return string.Empty;
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
      

        public static IList<string> GetProxy(string url)
        {
            IList<string> list = new List<string>();
            WebResponse response = WebRequest.Create(url).GetResponse();
            Stream responseStream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("gb2312");
            string input = new StreamReader(responseStream, encoding).ReadToEnd();
            responseStream.Close();
            response.Close();
            MatchCollection matchs = new Regex(@"(\d+)\.(\d+)\.(\d+)\.(\d+):(\d+)", RegexOptions.IgnoreCase).Matches(input);
            if (matchs.Count > 0)
            {
                foreach (Match match in matchs)
                {
                    string item = match.Value;
                    list.Add(item);
                }
            }
            return list;
        }

        public static Stream GetStream(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = url;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
    
                return responseStream;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return null;
            }
        }

        public static Stream GetStream(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
               
                
                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                return responseStream;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return null;
            }
        }

        public static Stream GetSSLStream(string url, string userAgent, string accept, string contentType, CookieContainer cookieContainer, WebProxy wp, Encoding ed, string refer)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                if (!string.IsNullOrEmpty(contentType))
                {
                    request.ContentType = contentType;
                }
                request.Referer = refer;
                request.Accept = accept;
                request.UserAgent = userAgent;
                request.Method = "GET";
                var sp = request.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                prop.SetValue(sp, (byte)0, null);
                request.ServicePoint.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                if (wp != null)
                {
                    request.Proxy = wp;
                }
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                return responseStream;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                return null;
            }
        }

        // Properties
        public static CookieContainer CookieContainer
        {
            get
            {
                return cc;
            }
        }

        public static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }




    
    }


}
