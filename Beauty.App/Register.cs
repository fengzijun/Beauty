using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Beauty.App.BeautyService;
using System.IO;

namespace Beauty.App
{
    public partial class Register : Form
    {
        private BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
        
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.textBox1.Text.Trim()))
            {
                MessageBox.Show("用户名不能为空");
                return;
            }
            if (string.IsNullOrEmpty(this.textBox2.Text.Trim()))
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(this.textBox4.Text.Trim()))
            {
                MessageBox.Show("邮箱不能为空");
                return;
            }



            if (string.IsNullOrEmpty(this.textBox5.Text))
            {
                MessageBox.Show("手机号码不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.textBox6.Text))
            {
                MessageBox.Show("支付宝不能为空");
                return;
            }

         

            if (this.textBox1.Text.Length > 30)
            {
                MessageBox.Show("用户名太长");
                return;
            }
            if (this.textBox2.Text.Length > 30)
            {
                MessageBox.Show("密码太长");
                return;
            }
            if (this.textBox4.Text.Length > 30)
            {
                MessageBox.Show("邮箱太长");
                return;
            }
            if (this.textBox5.Text.Length > 30)
            {
                MessageBox.Show("手机号码不正确");
                return;
            }
            if (this.textBox6.Text.Length > 30)
            {
                MessageBox.Show("支付宝号太长");
                return;
            }

            string s = @"^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$";
            if (!Regex.IsMatch(this.textBox5.Text, s))
            {
                MessageBox.Show("请填写正确的手机号码");
                return;
            }

            if (this.textBox2.Text.Trim() != this.textBox3.Text.Trim())
            {
                MessageBox.Show("密码不匹配");
                return;
            }

            if (string.IsNullOrEmpty(this.textBox11.Text.Trim()))
            {
                MessageBox.Show("请输入邀请码");
                return;
            }

            BoolResponse result = client.Risgter(this.textBox1.Text, this.textBox2.Text, this.textBox4.Text, this.textBox5.Text, this.textBox6.Text, this.textBox7.Text, this.textBox8.Text, this.textBox9.Text, this.textBox10.Text, this.textBox11.Text.Trim());
            MessageBox.Show(result.Message);
            if (result.Result)
            {
                this.Close();
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\refer.txt"))
            {
                StreamReader sr = new StreamReader(Application.StartupPath + "\\refer.txt");
                string refer = sr.ReadToEnd();
                sr.Close();
                if (!string.IsNullOrEmpty(refer.Trim()))
                {
                    this.textBox11.Text = refer.Trim();
                    //this.textBox11.Enabled = false;
                }
                

            }
        }
    }
}
