using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Beauty.Common;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using SHDocVw;

namespace Beauty.App
{
    public partial class Main : Form
    {
        public UserInfo user;
        public UserAction useraction;
        public bool isrunning = false;
        public bool init = false;
        public bool isfirstshow = true;
        public ShowLoading initform;
        public string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
        public string appname = System.Configuration.ConfigurationManager.AppSettings["app"];

        DateTime lastgetnoticetime;
        private Point firstlocationy;
        IList<BeautyService.WebNotice> notices;

        const int WM_SYSCOMMAND = 0x112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;


        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        public bool isonline = true;
        public bool noticehasrunned = false;
        private noticeform beautynoticeform;


        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd); 

        public Main(string username, string userid)
        {

            InitializeComponent();

            user = new UserInfo();
            user.Userid = userid;
            user.Username = username;
            user.islogin = false;
            user.Cookie = new CookieContainer();
            useraction = new UserAction(user);
            this.dfadsToolStripMenuItem.Enabled = false;
            this.tuichuToolStripMenuItem.Enabled = false;
            //this.toolStripStatusLabel1.Text = "正在初始化，请稍等";
            this.webBrowser2.Visible = false;
            this.noticelab.Text = "";
            this.noticelablink.Text = "";
            noticelab.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            noticelablink.BackColor = Color.FromArgb(255, 81, 148);
            this.webBrowser1.Width = this.Width - 2;
            this.webBrowser2.Width = this.Width - 2;
            this.statusStrip1.BackColor = Color.FromArgb(255, 81, 148);
            this.toolStripStatusLabel1.Text = "正在初始化...";
            this.Paint += Main_Paint;

            this.panel1.BackColor = Color.FromArgb(254, 39, 118);
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

            this.noticelablink.Text = "美丽说秋装女装韩版休闲潮韩国秋季新款花边镂空女款针织衫女开衫 成功进入上衣分类首页";
            this.noticelablink.Visible = false;
            firstlocationy = this.noticelab.Location;

            this.noticelab.Visible = false;
        }

        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            isonline = e.IsAvailable;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() == SC_MINIMIZE) //是否点击最小化
                {

                    //这里写操作代码
                    this.Visible = false; //隐藏窗体
                    return;
                }


            }

            base.WndProc(ref m);
        }


        private void Main_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Properties.Resources.ResourceManager.GetObject("min") as Image;
            this.pictureBox1.Width = this.pictureBox1.Image.Width;
            this.pictureBox1.Height = this.pictureBox1.Image.Height;

            this.pictureBox2.Image = Properties.Resources.ResourceManager.GetObject("close") as Image;
            this.pictureBox2.Width = this.pictureBox2.Image.Width;
            this.pictureBox2.Height = this.pictureBox2.Image.Height;
            //this.pictureBox1.MouseHover+=pictureBox1_MouseHover;
            //this.pictureBox1.MouseLeave+=pictureBox1_MouseLeave;
            this.panel1.BackgroundImage = Properties.Resources.ResourceManager.GetObject("title") as Image;
            if (!CheckNewVersion())
            {
                if (MessageBox.Show("检查到有新版本，需要更新吗？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (File.Exists((Application.StartupPath + "\\Beauty.Update.exe")))
                    {
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\Beauty.Update.exe");
                    }
                    else
                    {
                        MessageBox.Show("找不到更新程序文件");
                    }

                }
            }

            this.webBrowser1.Navigate(domain + "/home/index?uid=" + user.Userid);
            this.webBrowser2.Navigate("http://www.meilishuo.com/user/login");
            this.webBrowser2.DocumentCompleted += webBrowser2_DocumentCompleted;
            this.webBrowser2.Navigating += webBrowser2_Navigating;
            SHDocVw.WebBrowser wb = (SHDocVw.WebBrowser)webBrowser2.ActiveXInstance;
            this.webBrowser2.Navigated+=webBrowser2_Navigated;
            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            SetIsStartUp();
            activethread.Start();

            noticethread.Start();
            systemnoticethread.Start();
            this.Load += Main_Load;
            //this.CreateGraphics().DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
            //this.Refresh();
        }

    

        public void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (e.Url.ToString().ToLower().Contains("superassert/waitshare"))
                {
                    if (!user.islogin)
                    {
                        if (MessageBox.Show("您还没绑定美丽说帐号，需要绑定吗？", "提示信息", MessageBoxButtons.OKCancel)
                            == System.Windows.Forms.DialogResult.OK)
                        {
                            this.webBrowser2.Navigate("http://www.meilishuo.com/user/login");
                            this.webBrowser2.Visible = true;
                        }
                        else
                        {
                            this.webBrowser1.GoBack();
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void webBrowser2_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().ToLower().Contains("ihome"))
            {
                InitInfoLogin();

                //ShowLoadingBand form = new ShowLoadingBand(this);
                //form.ShowDialog();
            }
        }

        public void webBrowser2_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                HtmlElement el = this.webBrowser2.Document.All["mlsUser"];
                if (el != null)
                {
                    if(!string.IsNullOrEmpty(el.GetAttribute("value").ToString()))
                    {
                        this.user.account = el.GetAttribute("value").ToString();
                    }
                }
               

             

            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InputCookie()
        {
            string cookieStr = webBrowser2.Document.Cookie;
            string[] cookstr = cookieStr.Split(';');
            foreach (string str in cookstr)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                ck.Domain = "meilishuo.com";
                //myCookieContainer.Add(ck);
                user.Cookie.Add(ck);
            }
        }

        public void InitInfoNotLogin()
        {
            user.islogin = false;
            this.toolStripStatusLabel1.Text = "帐号状态：未绑定";
            //this.zhanghaobingdingToolStripMenuItem.Enabled = true;
            //this.toolStripMenuItem3.Enabled = false;
            init = true;
            this.dfadsToolStripMenuItem.Enabled = true;
            this.tuichuToolStripMenuItem.Enabled = false;
            if (isfirstshow)
            {
                MessageBox.Show("请先绑定美丽说帐号");
                isfirstshow = false;
            }
        }

        public void InitInfoLogin()
        {
            this.webBrowser2.Visible = false;
            this.toolStripStatusLabel1.Text = "帐号状态：正在进行绑定，请稍候";

            if (!user.islogin)
            {
                user.islogin = true;
                this.toolStripStatusLabel1.Text = "帐号状态：绑定";
                this.webBrowser2.Visible = false;
                InputCookie();
                init = true;
                useraction = new UserAction(this.user);
                ThreadPool.QueueUserWorkItem(useraction.GetGroups);
                ThreadPool.QueueUserWorkItem(useraction.GetUserInfo);
                //activethread.Start();
                workthread.Start();

                //this.zhanghaobingdingToolStripMenuItem.Enabled = false;
                //this.toolStripMenuItem3.Enabled = true;
                this.dfadsToolStripMenuItem.Enabled = false;
                this.tuichuToolStripMenuItem.Enabled = true;

                //new Task(() => { useraction.GetTask(); }).Start();
            }
        }

        public void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (e.Url.ToString().ToLower().Contains("user/login"))
                {
                    if (!user.islogin)
                    {
                        InitInfoNotLogin();
                    }

                }

              
          

                if (e.Url.ToString().ToLower().Contains("ihome"))
                {

                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Environment.Exit(-1);
        }

        private void workthread_Tick(object sender, EventArgs e)
        {
            //if (!CheckInternet())
            //{
            //    return;
            //}
            try
            {
                if (!isrunning)
                {
                    isrunning = true;
                    if (user.islogin)
                    {
                        //this.toolStripStatusLabel1.Text = "正在为您执行任务，请稍等";
                        //if (notifyIcon1.Visible == true)
                        //{
                        //    this.notifyIcon1.ShowBalloonTip(500, "提示信息", "正在为您执行任务...", ToolTipIcon.Info);
                        //}
                        var tokensource = new CancellationTokenSource();
                        var token = tokensource.Token;
                        Task t = new Task(() => { useraction.GetTask(); });
                        t.ContinueWith(new Action<Task>((task) => { isrunning = false; }), token);

                        t.Start();
                    }
                    else
                    {
                        //this.notifyIcon1.ShowBalloonTip(500, "提示信息", "请先绑定美丽说帐号", ToolTipIcon.Info);
                    }

                    //this.toolStripStatusLabel1.Text = "任务执行完毕----"+DateTime.Now.ToString();
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                isrunning = false;
                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void activethread_Tick(object sender, EventArgs e)
        {
            try
            {
                if (useraction != null)
                {
                    useraction.ActiveUser();
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                
                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                SetForegroundWindow(this.Handle);
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);

                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //this.notifyIcon1.Visible = false; 
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            //this.notifyIcon1.Visible = false; 
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.notifyIcon1.Visible = false;
            Environment.Exit(-1);
            //this.Close();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            //this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = true;

            //this.notifyIcon1.ShowBalloonTip(500, "提示信息", "程序将会为您自动完成任务", ToolTipIcon.Info);
        }

        #region clear cookie

        //        public enum ShowCommands : int
        //        {
        //            SW_HIDE = 0,
        //            SW_SHOWNOrmAL = 1,
        //            SW_NOrmAL = 1,
        //            SW_SHOWMINIMIZED = 2,
        //            SW_SHOWMAXIMIZED = 3,
        //            SW_MAXIMIZE = 3,
        //            SW_SHOWNOACTIVATE = 4,
        //            SW_SHOW = 5,
        //            SW_MINIMIZE = 6,
        //            SW_SHOWMINNOACTIVE = 7,
        //            SW_SHOWNA = 8,
        //            SW_RESTORE = 9,
        //            SW_SHOWDEFAULT = 10,
        //            SW_FORCEMINIMIZE = 11,
        //            SW_MAX = 11
        //        }

        //        [DllImport("shell32.dll")]
        //        static extern IntPtr ShellExecute( IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);
        ////清除IE临时文件

        //        public bool CleanCookies(string URLStr, int ExType)
        //        {
        //            //定义变量
        //            string CookiesPath, FindDirctroy, OsTypeStr;
        //            string UserProfile;
        //            string XPCookiesPath, VistaCookiesPath;
        //            long OsType;

        //            //获取用户配置路径
        //            UserProfile = Environment.GetEnvironmentVariable("USERPROFILE");
        //            //MessageBox.Show(UserProfile);
        //            //获取操作系统类型
        //            OsType = Environment.OSVersion.Version.Major;

        //            //解析地址
        //            if (URLStr == "")
        //                FindDirctroy = "";
        //            else
        //            {
        //                //用"."分割字符
        //                char[] separator = { '.' };
        //                string[] MyWords;
        //                MyWords = URLStr.Split(separator);
        //                //选取其中的关键字
        //                try
        //                {
        //                    FindDirctroy = MyWords[1];
        //                }
        //                catch
        //                {
        //                    FindDirctroy = "";
        //                    //如果出错提示
        //                    //MessageBox.Show("输入的网址格式不正确。");
        //                }
        //                //测试使用
        //                //MessageBox.Show(FindDirctroy);
        //            }

        //            //判断浏览器类型
        //            if (ExType == 0)
        //            {
        //                //IE浏览器
        //                XPCookiesPath = @"/Cookies/";
        //                VistaCookiesPath = @"\AppData\Local\Microsoft\Windows\Temporary Internet Files";
        //                //C:\Users\Administrator\AppData\Local\Microsoft\Windows\Temporary Internet Files
        //            }
        //            else if (ExType == 1)
        //            {
        //                //FireFox浏览器
        //                XPCookiesPath = @"/Application Data/Mozilla/Firefox/Profiles/";
        //                VistaCookiesPath = @"/AppData/Roaming/Mozilla/Firefox/Profiles/";
        //                FindDirctroy = "cookies";
        //            }
        //            else
        //            {
        //                XPCookiesPath = "";
        //                VistaCookiesPath = "";
        //                return false;
        //            }

        //            //判断操作系统类型
        //            if (OsType == 5)
        //            {
        //                //系统为XP
        //                OsTypeStr = "Microsoft Windows XP";
        //                CookiesPath = UserProfile + XPCookiesPath;
        //                //测试使用
        //                //MessageBox.Show(CookiesPath);
        //            }
        //            else if (OsType == 6)
        //            {
        //                //系统为Vista
        //                OsTypeStr = "Microsoft Windows Vista";
        //                CookiesPath = UserProfile + VistaCookiesPath;
        //                //测试使用
        //                //MessageBox.Show(CookiesPath);
        //            }
        //            else if (OsType == 7)
        //            {
        //                //系统为Win 7
        //                OsTypeStr = "Microsoft Windows 7";
        //                CookiesPath = UserProfile + VistaCookiesPath;
        //                //测试使用
        //                //MessageBox.Show(CookiesPath);
        //            }
        //            else
        //            {
        //                //未识别之操作系统
        //                OsTypeStr = "Other OS Version";
        //                CookiesPath = "";
        //                return false;
        //            }

        //            //删除文件
        //            if (DeleteFileFunction(CookiesPath, FindDirctroy))
        //                return true;
        //            else
        //                return false;
        //        }
        //        //重载函数
        //        public bool CleanCookies()
        //        {
        //            if (CleanCookies("", 0))
        //                return true;
        //            else
        //                return false;
        //        }


        //        public void DoSuff(DirectoryInfo directory)
        //        {

        //            foreach (var file in directory.GetFiles())
        //            {
        //                Console.WriteLine(file.FullName);
        //                StreamWriter sw = new StreamWriter("temp.txt", true);
        //                sw.WriteLine(file.FullName);
        //                sw.Close();
        //            }

        //            foreach (var subDirectory in directory.GetDirectories())
        //            {
        //                DoSuff(subDirectory);
        //            }
        //        }

        //        private bool DeleteFileFunction(string filepath, string FindWords)
        //        {
        //            string Dstr;
        //            //下面这些字串例外
        //            string ExceptStr = "index.dat";
        //            //解析删除关键字
        //            if (FindWords == "")
        //                Dstr = "*.*";
        //            else
        //                Dstr = "*" + FindWords + "*";

        //            //删除cookie文件夹
        //            try
        //            {
        //                string cookiepath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache).ToString();
        //                //System.IO.File.SetAttributes(cookiepath, FileAttributes.Normal);
        //                var dInfo = new DirectoryInfo(cookiepath);

        //                DoSuff(dInfo);

        //                var dd = "";
        //                //string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));

        //                //foreach (string dFileName in files)
        //                //{
        //                //    if (dFileName.ToLower().Contains("meilishuo"))
        //                //    {
        //                //        File.Delete(dFileName);
        //                //    }
        //                //}
        //            }
        //            catch (Exception e)
        //            {
        //                //MessageBox.Show("Cookies删除失败！/n" + e.ToString());
        //                return false;
        //            }

        //            //深层遍历（解决Vista Low权限问题）
        //            string[] LowPath = Directory.GetDirectories(filepath);
        //            foreach (string ThePath in LowPath)
        //            {
        //                try
        //                {
        //                    foreach (string dFileName in Directory.GetFiles(ThePath, Dstr))
        //                    {
        //                        if (dFileName == filepath + ExceptStr)
        //                            continue;
        //                        File.Delete(dFileName);
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    //MessageBox.Show("遍历文件删除失败！/n" + e.ToString());
        //                    return false;
        //                }
        //            }
        //            //测试使用
        //            //MessageBox.Show("删除完成!");
        //            return true;
        //        }

        #endregion

        private void Main_Enter(object sender, EventArgs e)
        {

        }

        private void Main_Activated(object sender, EventArgs e)
        {
            if (!init)
            {
                if (initform == null)
                {
                    initform = new ShowLoading(this);
                    initform.ShowDialog();
                }
            }
        }


        public bool CheckNewVersion()
        {
            try
            {
                string version = HttpHelper.GetHtml(domain + "/home/version", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
                if (File.Exists(Application.StartupPath+ "\\version.txt"))
                {

                    StreamReader sr = new StreamReader(Application.StartupPath + "\\version.txt");
                    string val = sr.ReadToEnd().Replace("\r\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                 
                    sr.Close();

                    if (val == version)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);

                MessageBox.Show(ex.Message.ToString(), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        //退出
        private void zhanghaobanndingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Environment.Exit(-1);
        }


        //更新
        private void jiechubangdingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string version = HttpHelper.GetHtml(domain + "/home/version", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
                if (File.Exists(Application.StartupPath + "\\version.txt"))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\version.txt");
                    string val = sr.ReadToEnd().Replace("\r\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                    sr.Close();
                    if (val == version)
                    {
                        MessageBox.Show("当前已经是最新版本！");
                    }
                    else
                    {
                        if (File.Exists((Application.StartupPath + "\\Beauty.Update.exe")))
                        {
                            System.Diagnostics.Process.Start(Application.StartupPath + "\\Beauty.Update.exe");
                        }
                        else
                        {
                            MessageBox.Show("找不到更新程序文件");
                        }
                    }
                }
                else
                {
                    if (File.Exists((Application.StartupPath + "\\Beauty.Update.exe")))
                    {
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\Beauty.Update.exe");
                    }
                    else
                    {
                        MessageBox.Show("找不到更新程序文件");
                    }
                }
            }
            catch(Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //解除绑定
        private void tuichuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser2.Navigate("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
                user.Cookie = new CookieContainer();
                user.islogin = false;
                workthread.Stop();
                MessageBox.Show("解除绑定成功");
                this.toolStripStatusLabel1.Text = "帐号状态：未绑定";
                this.dfadsToolStripMenuItem.Enabled = true;
                this.tuichuToolStripMenuItem.Enabled = false;

                if (this.webBrowser1.Url.ToString().ToLower().Contains("superassert/waitshare"))
                {
                    if (!user.islogin)
                    {
                        if (MessageBox.Show("您还没绑定美丽说帐号，需要绑定吗？", "提示信息", MessageBoxButtons.OKCancel)
                            == System.Windows.Forms.DialogResult.OK)
                        {
                            this.webBrowser2.Navigate("http://www.meilishuo.com/user/login");
                            this.webBrowser2.Visible = true;
                        }
                        else
                        {
                            this.webBrowser1.GoBack();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dfadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowser2.Navigate("http://www.meilishuo.com/user/login");
            this.webBrowser2.Visible = true;
        }

        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void noticethread_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Subtract(lastgetnoticetime).Minutes > 2 || notices == null)
                {
                    //notices = useraction.GetNotices();
                    Task t = new Task(() => { notices = useraction.GetNotices(this.user.Userid); });
                    //t.ContinueWith(new Action<Task>((task) => { SetNotice(); lastgetnoticetime = DateTime.Now; }));
                    t.Start();
                    lastgetnoticetime = DateTime.Now;
                }
                else
                {
                    if (string.IsNullOrEmpty(this.noticelab.Text) || noticehasrunned)
                    {
                        SetNotice();
                    }
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void InitLabel(string msg)
        {
            this.noticelab.Visible = false;
            int LblNum = msg.Length;   //Label内容长度
            int RowNum = 33;           //每行显示的字数
            float FontWidth = noticelab.Width / RowNum;    //每个字符的宽度
            int RowHeight = 15;           //每行的高度
            int ColNum = (LblNum - (LblNum / RowNum) * RowNum) == 0 ? (LblNum / RowNum) : (LblNum / RowNum) + 1;   //列数

            //label1.Width = (int)(FontWidth * 10.0);          //设置显示宽度
            this.noticelab.Height = RowHeight * ColNum;           //设置显示高度
            this.noticelab.BackColor = System.Drawing.Color.Transparent;
            this.noticelab.Visible = true;
        }

        /// <summary>
        /// 查找打开的FORM
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool FindForm(string formName)
        {
            foreach (Form form in Application.OpenForms)//获得所有打开的窗体
            {
                if (form.Name == formName)
                {
                    return true;
                }
            }
            return false;
        }

        private void systemnoticethread_Tick(object sender, EventArgs e)
        {
            //Task t = new Task(() => { notices = useraction.GetNotices(this.user.Userid); SetSysNotice(); });
            ////t.ContinueWith(new Action<Task>((task) => { SetNotice(); lastgetnoticetime = DateTime.Now; }));
            //t.Start();

            notices = useraction.GetNotices(this.user.Userid);
            SetSysNotice();
        }

        public void SetNotice()
        {
            if (notices != null && notices.Count > 0 && this.noticelab.Parent.IsHandleCreated)
            {

                this.noticelab.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        Random rand = new Random();
                        Random rand2 = new Random();
                        IList<BeautyService.WebNotice> usernotices = notices.Where(m => m.Type == 1).ToList();

                        if (usernotices != null && usernotices.Count > 0)
                        {
                            int index2 = rand.Next(0, usernotices.Count);
                            this.noticelablink.Text = usernotices[index2].Msg;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                        useraction.writelog(errrormsg);
                        MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }));

            }
        }

        public void SetSysNotice()
        {
            if (notices != null && notices.Count > 0)
            {


                try
                {
                   
                    IList<BeautyService.WebNotice> sysnotices = notices.Where(m => m.Type == 0).ToList();

                    if (sysnotices != null && sysnotices.Count > 0)
                    {

                        beautynoticeform = new noticeform(sysnotices[0].Msg, this.user.Userid, sysnotices[0].ID.ToString());
                        beautynoticeform.Name = "beautynoticeform";
                        if (!FindForm("beautynoticeform"))
                        {
                            useraction.ReadNotice(this.user.Userid, sysnotices[0].ID.ToString());

                            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - beautynoticeform.Width, Screen.PrimaryScreen.WorkingArea.Height);
                            beautynoticeform.PointToScreen(p);
                            beautynoticeform.Location = p;
                            beautynoticeform.Show();
                            for (int i = 0; i <= beautynoticeform.Height; i++)
                            {
                                beautynoticeform.Location = new Point(p.X, p.Y - i);
                                Thread.Sleep(5);//将线程沉睡时间调的越小升起的越快
                            }
                        }
                    }
                    else
                    {

                    }


                }
                catch (Exception ex)
                {
                    string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                    useraction.writelog(errrormsg);
                    MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
        }

        private void noticelablink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (BeautyService.WebNotice notice in notices)
            {
                if (notice.Msg == this.noticelablink.Text)
                {
                    if (!string.IsNullOrEmpty(notice.Url))
                    {
                        System.Diagnostics.Process.Start("IEXPLORE.EXE", notice.Url);
                    }
                }
            }
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0,
                                                     this.ClientSize.Width - 1,
                                                     this.ClientSize.Height - 1));
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //base.OnPaint(e);
            //ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
            //                      Color.Black, 2, ButtonBorderStyle.Inset,
            //                      Color.Black, 2, ButtonBorderStyle.Inset,
            //                      Color.Black, 2, ButtonBorderStyle.Inset,
            //                      Color.Black, 2, ButtonBorderStyle.Inset);
        }


        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接
        public extern static bool InternetGetConnectedState(out int conState, int reder);

        public bool CheckInternet()
        {
            return isonline;
        }


        public Point GetControlPosition(Control c)
        {
            int x = c.Location.X;
            int y = c.Location.Y;
            while (!(c is Form))
            {
                if (c.Parent == null)
                    break;
                x += c.Parent.Location.X;
                y += c.Parent.Location.Y;
                c = c.Parent;
            }

            return new Point(x, y);
        }

        //private void noticelabel_Tick(object sender, EventArgs e)
        //{
        //    if (GetControlPosition(this.noticelab).Y +30 > GetControlPosition(this.panel1).Y)
        //    {
        //        this.noticelab.Location = new System.Drawing.Point(this.noticelab.Location.X, this.noticelab.Location.Y - 1);
        //        //if (this.label1.Location.Y + this.label1.Height < this.panel1.Location.Y)
        //        //{
        //        //    this.label1.Location = new System.Drawing.Point(this.label1.Location.X, firstlocationy.Y - this.panel1.Height);
        //        //}
        //    }
        //    else
        //    {
        //        this.noticelab.Location = new System.Drawing.Point(firstlocationy.X, firstlocationy.Y);
        //        this.noticelab.Text = "";
        //        noticehasrunned = true;
        //        noticelabel.Stop();
        //    }
        //}

        private void noticelab_Paint(object sender, PaintEventArgs e)
        {
            //  ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.FromArgb(236, 235, 240), 1,
            //ButtonBorderStyle.Solid, Color.FromArgb(236, 235, 240), 1, ButtonBorderStyle.Solid, Color.FromArgb(236, 235, 240), 1,
            //ButtonBorderStyle.Solid, Color.FromArgb(236, 235, 240), 1, ButtonBorderStyle.Solid);
        }

        private void noticelabel_Tick(object sender, EventArgs e)
        {

        }

        public void SetIsStartUp()
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                object val = rk.GetValue(Application.ProductName);
                if (val == null)
                {
                    this.toolStripMenuItem3.Text = "设置开机自启动";
                    RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Shared Tools\MSConfig\startupreg", true);
                    if (rk2 != null)
                    {
                        string[] subsnames = rk2.GetSubKeyNames();
                        foreach (string name in subsnames)
                        {
                            if (name == Application.ProductName)
                            {

                                rk2.DeleteSubKey(name);
                            }
                        }
                    }
                  
                }
                else
                {
                    this.toolStripMenuItem3.Text = "取消开机自启动";
                }
            }
            catch (Exception ex)
            {
                string errrormsg = "msg:" + ex.Message + " Source:" + ex.Source + " StackTrace:" + ex.StackTrace;
                useraction.writelog(errrormsg);
                MessageBox.Show(ex.Message.ToString(), "错误提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 开机自启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.toolStripMenuItem3.Text == "设置开机自启动")
            {
                RunWhenStart(true, Application.ProductName, Application.StartupPath + "\\" + appname);
            }
            else
            {
                RunWhenStart(false, Application.ProductName, Application.StartupPath + "\\" + appname);
            }
        }

        public void RunWhenStart(bool Started, string name, string path)
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            

            try
            {
                if (Started == true)
                {
                    
                    rk.SetValue(name, path);
                    this.toolStripMenuItem3.Text = "取消开机自启动";
                    MessageBox.Show("设置成功");
                }
                else
                {
                    rk.DeleteValue(name);
                    this.toolStripMenuItem3.Text = "设置开机自启动";

                    RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Shared Tools\MSConfig\startupreg", true);
                    if (rk != null)
                    {
                        string[] subsnames = rk2.GetSubKeyNames();
                        foreach (string subname in subsnames)
                        {
                            if (subname == Application.ProductName)
                            {

                                rk2.DeleteSubKey(subname);
                            }
                        }

                        MessageBox.Show("取消成功");
                    }
                }

                
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message.ToString(), "错误提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                rk.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
