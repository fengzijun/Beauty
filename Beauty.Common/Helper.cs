using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Win32;

namespace Beauty.Common
{
    public class Helper
    {
        public static bool SendEmail(string emailaccount,string emailpassword,string smtp, string port ,string emailto,string subject,string msgcontent)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtp, Convert.ToInt32(port));
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(emailaccount,
                  emailpassword);

                MailMessage message = new MailMessage(emailaccount, emailto);

                message.IsBodyHtml = true;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;

                message.Subject = subject;
                message.Body = msgcontent;

                message.Attachments.Clear();

                smtpClient.Send(message);

                return true;//成功
            }
            catch (SmtpException se)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void GetTaoBaoLinkInfo(string url, out string imageurl, out string title)
        {
            imageurl = string.Empty;
            title = string.Empty;
            CookieContainer cookie = new CookieContainer();
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
                "application/json, text/javascript, */*; q=0.01", null, null, Encoding.GetEncoding("gb2312"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//span[@id='J_ImgBooth']");
            if (node != null)
            {
                imageurl = node.Attributes["data-src"] != null ? node.Attributes["data-src"].Value : node.Attributes["src"].Value;
            }
            else
            {
                node = doc.DocumentNode.SelectSingleNode(@"//img[@id='J_ImgBooth']");
                if (node != null)
                {
                    imageurl = node.Attributes["data-src"] != null ? node.Attributes["data-src"].Value : node.Attributes["src"].Value;
                }
            }
            node = doc.DocumentNode.SelectSingleNode(@"//div[@class='tb-detail-hd']");
            if (node != null)
            {
                title = node.InnerText.Replace("\r\n", "").Replace("\t", "").Trim();
            }


        }

        public static string GetIDFromShareLink(string url)
        {
            //http://www.meilishuo.com/share/1705032883?d_r=0.1.1.1
            try
            {
                if (url.Contains("?"))
                {
                    return url.Substring(url.LastIndexOf("/") + 1, url.IndexOf("?") - url.LastIndexOf("/") - 1);
                }
                else
                {
                    return url.Substring(url.LastIndexOf("/") + 1, url.Length - url.LastIndexOf("/") - 1);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void GetPaiPaiLinkInfo(string url, out string imageurl, out string title)
        {
            imageurl = string.Empty;
            title = string.Empty;
            CookieContainer cookie = new CookieContainer();
            string result = Beauty.Common.HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0",
                "application/json, text/javascript, */*; q=0.01", null, null, Encoding.GetEncoding("gb2312"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"//img[@alt='商品主图']");
            if (node != null)
            {
                imageurl = node.Attributes["src"].Value;
            }

            node = doc.DocumentNode.SelectSingleNode(@"//div[@class='title']");
            if (node != null)
            {
                title = node.InnerText.Replace("\r\n", "").Replace("\t", "").Trim().Replace("举报此商品", "");
            }
        }

        public static void GetShareLinkInfo(string url, out string imageurl, out string title)
        {
            imageurl = string.Empty;
            title = string.Empty;
            string html = HttpHelper.GetHtml(url, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//img[@class='twitter_pic']");

            if (node != null)
            {
                imageurl = node.Attributes["src"].Value;
            }
            node = doc.DocumentNode.SelectSingleNode("//div[@class='goods_info']/h1/a");
            if (node != null)
            {
                title = node.InnerText.Replace("&nbsp;", "").Trim();
            }
        }

        public static void RegisterKey(string name, string val)
        {
            try
            {
                RegistryKey rsg = null;
                //if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\MMDonload").SubKeyCount <= 0)
                //{
                //    Registry.LocalMachine.DeleteSubKey("SOFTWARE\\MMDonload");

                //}
                Registry.LocalMachine.CreateSubKey("SOFTWARE\\meilishuo");
                rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\meilishuo", true);//true表示可以修改

                rsg.SetValue(name, val);
                rsg.Close();
            }
            catch (Exception ex)
            {
                //this.label2.Text = ex.Message;
            }
        }

        public static string GetRegisterKey(string name)
        {
            try
            {
                RegistryKey rsg = null;
                rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\meilishuo", true);
                if (rsg.GetValue(name) != null) //读取失败返回null
                {

                    return rsg.GetValue(name).ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetIEVersion()
        {
            try
            {
                RegistryKey rsg = null;
                rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Version Vector", true);//true表示可以修改
                if (rsg != null)
                {
                    string val = rsg.GetValue("IE").ToString();
                    return val;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static void RegisterIE(string name, int val)
        {
            try
            {
                RegistryKey rsg = null;
                //if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\MMDonload").SubKeyCount <= 0)
                //{
                //    Registry.LocalMachine.DeleteSubKey("SOFTWARE\\MMDonload");

                //}
                rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);//true表示可以修改

                if (rsg != null)
                {
                    rsg.SetValue(name, val, RegistryValueKind.DWord);
                    rsg.Close();
                }

                rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);//true表示可以修改
                if (rsg != null)
                {
                    rsg.SetValue(name, val, RegistryValueKind.DWord);
                    rsg.Close();
                }
                //if (PlateFormRunMode == 32)
                //{
                //    //Registry.LocalMachine.CreateSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION\Beauty.App.vshost.exe");

                //    rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);//true表示可以修改


                //    rsg.SetValue(name, val, RegistryValueKind.DWord);
                //    rsg.Close();
                //}
                //else
                //{
                //    //Registry.LocalMachine.CreateSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION\Beauty.App.vshost.exe");

                //    rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);//true表示可以修改
                  
                //    rsg.SetValue(name, val, RegistryValueKind.DWord);
                //    rsg.Close();
                //}
            }
            catch (Exception ex)
            {
                //this.label2.Text = ex.Message;
            }
        }

        public static int PlateFormRunMode
        {
            get
            {
                if (IntPtr.Size == 8)
                {
                    return 64;
                }
                return 32;

            }
        }

        public static string UnicodeToChina(string str)
        {
            string outStr = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //將unicode轉為10進制整數，然後轉為char中文
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }

            return outStr;
        }
    }
}
