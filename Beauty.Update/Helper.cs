using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Beauty.Common
{
    public class Helper
    {
        

        public static void RegisterKey(string name, string val)
        {
            try
            {
                RegistryKey rsg = null;
                //if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\MMDonload").SubKeyCount <= 0)
                //{
                //    Registry.LocalMachine.DeleteSubKey("SOFTWARE\\MMDonload");

                //}
                //Registry.LocalMachine.CreateSubKey("SOFTWARE\\meilishuo");
                rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\meilishuo", true);//true表示可以修改
                if (rsg == null)
                {
                    Registry.LocalMachine.CreateSubKey("SOFTWARE\\meilishuo");
                    rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\meilishuo", true);
                }
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
            RegistryKey rsg = null;
            rsg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Version Vector", true);//true表示可以修改
            if (rsg != null)
            {
                string val = rsg.GetValue("IE").ToString();
                return val;
            }
            return null;
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
