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
    public partial class ShowLoadingBand : Form
    {
        private Main parent;
        public ShowLoadingBand(Main parent)
        {
            InitializeComponent();

             this.parent = parent;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (parent.user.islogin)
            {
                this.progressBar1.Value = 100;
                this.Close();
            }
            else
            {
                if (this.progressBar1.Value < 50)
                {
                    this.progressBar1.Value += 5;
                }
                else if (this.progressBar1.Value < 80)
                {
                    this.progressBar1.Value += 3;
                }
                else if (this.progressBar1.Value < 99)
                {
                    this.progressBar1.Value += 1;
                }
            }
        }

        private void ShowLoadingBand_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
