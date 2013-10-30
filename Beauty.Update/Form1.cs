using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Beauty.Common;
using ICSharpCode.SharpZipLib.Zip;

namespace Beauty.Update
{
    public partial class Form1 : Form
    {
        private string filename = "update.zip";
        string appname = System.Configuration.ConfigurationManager.AppSettings["killapp"];
        string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
        WebClient webClient = new WebClient();
        private DateTime startime;
        private bool isdownload = false;
        public Form1()
        {
            InitializeComponent();
        }

        private static void KillAppProcess(string name)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName.ToLower().Contains(name.ToLower()))
                {
                    p.Kill();

                }
            }
        }

        private static void UnZipFile(string zipFilePath)
        {

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(theEntry.Name))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.progressBar1.Value < 99)
            {
                this.progressBar1.Value += 1;
            }
            if (DateTime.Now.Subtract(startime).Seconds > 30)
            {
                webClient.CancelAsync();
                this.timer1.Stop();
                if (!isdownload)
                {
                    Downloadfile();
                }
                
            }
        }

        public void Downloadfile()
        {
            string filepath = System.Configuration.ConfigurationManager.AppSettings["Url"] + "/" + filename;
            webClient.DownloadFile(new Uri(filepath), filename);
            this.label1.ForeColor = Color.Blue;
            this.progressBar1.Value = 100;
            this.label1.Text = "更新完成，正在为您重新启动程序";
            for (var i = 0; i < 4; i++)
            {
                System.Threading.Thread.Sleep(500);
                Application.DoEvents();
            }
            string version = HttpHelper.GetHtml(System.Configuration.ConfigurationManager.AppSettings["Url"] + "/home/version", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\version.txt", false);
            sw.WriteLine(version);
            sw.Close();
         

            if (File.Exists(Application.StartupPath + "\\" + appname + ".exe"))
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\" + appname + ".exe");
            }
            else
            {
                MessageBox.Show("找不到安装文件，安装失败");
            }

            System.Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
            //this.timer1.Start();
            try
            {
                KillAppProcess(appname);

                if(File.Exists(Application.StartupPath + "\\" + filename))
                {
                    File.Delete(Application.StartupPath + "\\" + filename);
                }

                string filepath = System.Configuration.ConfigurationManager.AppSettings["Url"] + "/" + filename;
                startime = DateTime.Now;
                webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(filepath),filename);

                
              
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误，无法连接服务器");
                writelog(ex.Message);
                writelog(ex.Source);
                writelog(ex.StackTrace);
                Application.Exit();
            }
        }


        public void writelog(string msg)
        {
            string path = Application.StartupPath + "\\log";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string filename = DateTime.Now.ToString("yyyyMMdd_error");
            StreamWriter sw = new StreamWriter(path + "\\" + filename + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString() + "----------" + msg);
            sw.Close();

        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (this.progressBar1.InvokeRequired)
            {
                this.progressBar1.Invoke(new Action(() =>
                {
                    this.progressBar1.Value = e.ProgressPercentage;
                }));
            }
            else
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

           
            //webClient.DownloadFile(new Uri(filepath), filename);
            //this.progressBar1.Value = 100;
            try
            {
                if (File.Exists(Application.StartupPath + "\\" + filename))
                {
                    UnZipFile(Application.StartupPath + "\\" + filename);

                    string version = HttpHelper.GetHtml(System.Configuration.ConfigurationManager.AppSettings["Url"] + "/home/version", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0", "application/json, text/javascript, */*; q=0.01", null, null, Encoding.UTF8);
                    StreamWriter sw = new StreamWriter(Application.StartupPath + "\\version.txt", false);
                    sw.WriteLine(version);
                    sw.Close();

                    this.label1.ForeColor = Color.Blue;
                    this.label1.Text = "更新完成，正在为您重新启动程序";
                    //System.Threading.Thread.Sleep(2000);
                    for (var i = 0; i < 4; i++)
                    {
                        System.Threading.Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    if (File.Exists(Application.StartupPath + "\\" + appname + ".exe"))
                    {
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\" + appname + ".exe");
                    }
                    else
                    {
                        MessageBox.Show("找不到安装文件，安装失败");
                    }

                    System.Environment.Exit(0);
                }
                else
                {
                    isdownload = true;
                    Downloadfile();
                }
            }
            catch(Exception ex)
            {
                writelog(ex.Message);
                writelog(ex.Source);
                writelog(ex.StackTrace);
            }
        }
    }
}
