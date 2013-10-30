using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Beauty.App
{
    public partial class noticeform : Form
    {
        public noticeform(string content,string userid,string noticeid)
        {
            InitializeComponent();
            this.content = content;
            this.userid = userid;
            this.noticeid = userid;
        }

        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd); 

        private string content = string.Empty;
        private List<string> arrcontent;
        private int currentindex = 0;
        private string userid;
        private string noticeid;
        private void noticeform_Load(object sender, EventArgs e)
        {
            //this.label1.Text = content;

            SetForegroundWindow(this.Handle);
            var count = Convert.ToInt32(content.Length / 136);
            arrcontent = new List<string>();
            for (var i = 0; i < count+1; i++)
            {
                string tempcontent = string.Empty;
                if (i == count)
                {
                    tempcontent = content.Substring(i * 136, content.Length - i * 136);
                }
                else
                {
                    tempcontent = content.Substring(i * 136, 136);
                }
                if (!string.IsNullOrEmpty(tempcontent))
                {
                    arrcontent.Add(tempcontent);
                }
            }

            if (arrcontent.Count > 0)
            {
                this.label1.Text = arrcontent[currentindex];
                this.label2.Enabled = false;
                this.label3.Enabled = false;
                //setpage();
                if (arrcontent.Count > 1)
                {
                    this.label3.Enabled = true;
                }
            }
            //MessageBox.Show(content.Length.ToString());
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            this.label2.ForeColor = Color.Blue;
            this.label2.Cursor = Cursors.Hand;     
            
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            this.label2.ForeColor = Color.Black;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            this.label3.ForeColor = Color.Black;
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            this.label3.ForeColor = Color.Blue;
            this.label3.Cursor = Cursors.Hand;     
        }

     

        private void label2_Click(object sender, EventArgs e)
        {
            currentindex--;
            this.label1.Text = arrcontent[currentindex];
            if (currentindex == 0)
            {
                this.label2.Enabled = false;
            }

            if (currentindex < arrcontent.Count - 1)
            {
                this.label3.Enabled = true;
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
            currentindex++;
            this.label1.Text = arrcontent[currentindex];
            if (currentindex == arrcontent.Count - 1)
            {
                this.label3.Enabled = false;
            }

            if (currentindex > 0)
            {
                this.label2.Enabled = true;
            }
        }

    }
}
