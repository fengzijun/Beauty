using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Beauty.App.BeautyService;
using Beauty.Common;
using Microsoft.Win32;

namespace Beauty.App
{
    public partial class Login : Form
    {
        private BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
        public static Guid id;
        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register register = new Register();
            this.Hide();
            register.ShowDialog();
            this.Show();
        }

        public string GetIp()
        {
            try
            {
                string ipAddress = new WebClient().DownloadString("http://icanhazip.com");
                return ipAddress.Replace("\n","");
            }
            catch
            {
                return string.Empty;
            }
        }

        public void writelog(string msg)
        {
            string path = Application.StartupPath + "\\log";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string filename = DateTime.Now.ToString("yyyyMMdd");
            StreamWriter sw = new StreamWriter(path + "\\" + filename + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString() + "----------" + msg);
            sw.Close();
        }

        private void client_CheckUserCompleted(object sender, CheckUserCompletedEventArgs args)
        {
           
            string ip = GetIp();
            BoolResponse result = args.Result;
            if (result.Result)
            {
                if (this.checkBox1.Checked)
                {
                    Beauty.Common.Helper.RegisterKey("Userinfo", this.textBox1.Text.Trim() + "|" + this.textBox2.Text.Trim() + "|1");
                }
                else
                {
                    Beauty.Common.Helper.RegisterKey("Userinfo", this.textBox1.Text.Trim() + "|" + this.textBox2.Text.Trim() + "|0");
                }

                Beauty.App.BeautyService.WebUser user = client.GetUser(this.textBox1.Text.Trim());
        
                if (!user.IsLogin)
                {
                    //client.LoginActive(this.textBox1.Text.Trim(), ip);

                    if (user != null)
                    {
                        Main mainform = new Main(this.textBox1.Text.Trim(), user.ID.ToString());
                        //Form1 mainform = new Form1();
                        this.Hide();
                        mainform.ShowDialog();
                  
                    }
                    else
                    {
                        MsgSet("用户名密码错误");
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(user.Ip) && !string.IsNullOrEmpty(ip) && ip.Trim() != user.Ip.Replace("\n","").Trim())
                    {
                        //writelog("clientip:" + ip + ",userip:" + user.Ip);
                        //ButtonReset();

                        MsgSet("该帐号正在登录");
                    }
                    else
                    {
                        //client.LoginActive(this.textBox1.Text.Trim(), ip);

                        if (user != null)
                        {
                            Main mainform = new Main(this.textBox1.Text.Trim(), user.ID.ToString());
                            this.Hide();
                            mainform.ShowDialog();
              
                        }
                        else
                        {
                            MsgSet("用户名密码错误");
                        }
                    }
                }
            }
            else
            {
                MsgSet("用户名密码错误");
            }

            ButtonReset();
        }

        public void MsgSet(string msg)
        {
            this.label3.Invoke(new Action(() =>
            {
                this.label3.Text = msg;
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(this.textBox1.Text.Trim()) && !string.IsNullOrEmpty(this.textBox2.Text.Trim()))
            {
                ButtonDisable();
                client.CheckUserCompleted += client_CheckUserCompleted;
                client.CheckUserAsync(this.textBox1.Text.Trim(), this.textBox2.Text.Trim());
              
            }
            else
            {
                MsgSet("请输入用户名密码");
            }
        }

        public int BeautyLogin(string username, string password,out string userid)
        {
            userid = string.Empty; 
            if (client.State == System.ServiceModel.CommunicationState.Closed)
                    client.Open();
                BoolResponse result = client.CheckUser(this.textBox1.Text.Trim(), this.textBox2.Text.Trim());
                string ip = GetIp();
                if (result.Result)
                {
                    
                    if (this.checkBox1.Checked)
                    {
                        Beauty.Common.Helper.RegisterKey("Userinfo", this.textBox1.Text.Trim() + "|" + this.textBox2.Text.Trim() + "|1");
                    }
                    else
                    {
                        Beauty.Common.Helper.RegisterKey("Userinfo", this.textBox1.Text.Trim() + "|" + this.textBox2.Text.Trim() + "|0");
                    }

                    Beauty.App.BeautyService.WebUser user = client.GetUser(this.textBox1.Text.Trim());
                    if (!user.IsLogin)
                    {
                        //client.LoginActive(this.textBox1.Text.Trim(), ip);

                        if (user != null)
                        {
                            //Main mainform = new Main(this.textBox1.Text.Trim(), user.ID.ToString());
                            ////Form1 mainform = new Form1();
                            //this.Hide();
                            //mainform.ShowDialog();
                            userid = user.ID.ToString();
                            return 0;
                        }
                        else
                        {
                            ButtonReset();
                            return 1;
                        }
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(user.Ip) && !string.IsNullOrEmpty(ip) && ip.Trim() != user.Ip.Trim())
                        {
                            writelog("clientip:" + ip + ",userip:" + user.Ip);
                            ButtonReset();

                            return 2;
                        }
                        else
                        {
                            //client.LoginActive(this.textBox1.Text.Trim(), ip);

                            if (user != null)
                            {
                                //Main mainform = new Main(this.textBox1.Text.Trim(), user.ID.ToString());
                                //this.Hide();
                                //mainform.ShowDialog();
                                userid = user.ID.ToString();
                                return 0;
                            }
                            else
                            {
                                ButtonReset();
                                return 1;
                            }
                        }
                    }
                    //Environment.Exit(-1);
                }
                else
                {
                    ButtonReset();
                    return 1;
                }
        }

        public void ButtonDisable()
        {
            if (this.button1.InvokeRequired)
            {
                this.button1.Invoke(new Action(() =>
                {
                    this.button1.Text = "正在登录";
                    this.button1.Enabled = false;
                }));
            }
            else
            {
                this.button1.Text = "正在登录";
                this.button1.Enabled = false;
            }
        }

        public void ButtonReset()
        {
            if (this.button1.InvokeRequired)
            {
                this.button1.Invoke(new Action(() =>
                {
                    this.button1.Text = "登录";
                    this.button1.Enabled = true;
                }));
            }
            else
            {
                this.button1.Text = "登录";
                this.button1.Enabled = true;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                string val = Beauty.Common.Helper.GetRegisterKey("Userinfo");
                if (!string.IsNullOrEmpty(val))
                {
                    try
                    {
                        string username = val.Split(new char[] { '|' })[0];
                        string password = val.Split(new char[] { '|' })[1];
                        string ischeck = val.Split(new char[] { '|' })[2];
                        if (ischeck == "1")
                        {
                            this.textBox1.Text = username;
                            this.textBox2.Text = password;
                            this.checkBox1.Checked = true;
                        }
                    }
                    catch
                    {
                    }
                }
                string appname = System.Configuration.ConfigurationManager.AppSettings["app"];
                string version = Helper.GetIEVersion();
                if (version.Contains("9"))
                {
                    Helper.RegisterIE(appname, 9999);
                }
                else if (version.Contains("10"))
                {
                    Helper.RegisterIE(appname, 10001);
                }
                else
                {
                    Helper.RegisterIE(appname, 8000);
                }

                if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text) && IsStartUp())
                {
                    ButtonDisable();
                    client.CheckUserCompleted += client_CheckUserCompleted;
                    client.CheckUserAsync(this.textBox1.Text.Trim(), this.textBox2.Text.Trim());
                }
            }
            catch(Exception ex)
            {
                writelog(ex.Message);
            }


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgetPassword forgetpassword = new ForgetPassword();
            this.Hide();
            forgetpassword.ShowDialog();
            this.Show();
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {

        }

        public bool IsStartUp()
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                object val = rk.GetValue(Application.ProductName);
                if (val == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
