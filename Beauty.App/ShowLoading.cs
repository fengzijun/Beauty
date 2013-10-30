using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beauty.App
{
    public partial class ShowLoading : Form
    {
        private Main parent;
        private int count = 0;
        public ShowLoading(Main parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        private void ShowLoading_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (parent.init)
            {
                this.progressBar1.Value = 100;
                this.Close();
            }
            else
            {
                if (this.progressBar1.Value < 50)
                {
                    this.progressBar1.Value += 3;
                }
                else if (this.progressBar1.Value < 80)
                {
                    this.progressBar1.Value += 2;
                }
                else if (this.progressBar1.Value >=80 && this.progressBar1.Value < 99)
                {
                    this.progressBar1.Value += 1;
                }
                else if (this.progressBar1.Value == 99)
                {
                    parent.webBrowser1.Stop();
                    parent.InitInfoNotLogin();
                    this.Close();
                    

                }
            }
        }
    }
}
