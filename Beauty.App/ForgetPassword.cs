using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Beauty.App.BeautyService;

namespace Beauty.App
{
    public partial class ForgetPassword : Form
    {
        private BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
        
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text.Trim()) && !string.IsNullOrEmpty(this.textBox2.Text.Trim()))
            {
                BoolResponse result = client.ForgetPassword(this.textBox1.Text.Trim(), this.textBox2.Text.Trim());
                MessageBox.Show(result.Message);
                if (result.Result)
                    this.Close();
            }
            else
            {
                MessageBox.Show("邮箱和用户名都不能为空");
            }
        }
    }
}
