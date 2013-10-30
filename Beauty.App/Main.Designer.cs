namespace Beauty.App
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.dfadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tuichuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.jiechubangdingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zhanghaobanndingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.workthread = new System.Windows.Forms.Timer(this.components);
            this.activethread = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.noticelab = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.noticelablink = new System.Windows.Forms.LinkLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.noticethread = new System.Windows.Forms.Timer(this.components);
            this.noticelabel = new System.Windows.Forms.Timer(this.components);
            this.systemnoticethread = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(1, 76);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 22);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(984, 619);
            this.webBrowser1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripSplitButton1});
            this.statusStrip1.Location = new System.Drawing.Point(1, 701);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(987, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dfadsToolStripMenuItem,
            this.tuichuToolStripMenuItem,
            this.toolStripMenuItem3,
            this.jiechubangdingToolStripMenuItem,
            this.zhanghaobanndingToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Margin = new System.Windows.Forms.Padding(800, 2, 0, 0);
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripSplitButton1.Size = new System.Drawing.Size(47, 20);
            this.toolStripSplitButton1.Text = "设置";
            // 
            // dfadsToolStripMenuItem
            // 
            this.dfadsToolStripMenuItem.Name = "dfadsToolStripMenuItem";
            this.dfadsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.dfadsToolStripMenuItem.Text = "绑定帐号";
            this.dfadsToolStripMenuItem.Click += new System.EventHandler(this.dfadsToolStripMenuItem_Click);
            // 
            // tuichuToolStripMenuItem
            // 
            this.tuichuToolStripMenuItem.Name = "tuichuToolStripMenuItem";
            this.tuichuToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.tuichuToolStripMenuItem.Text = "解除帐号";
            this.tuichuToolStripMenuItem.Click += new System.EventHandler(this.tuichuToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem3.Text = "设置开机自启动";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // jiechubangdingToolStripMenuItem
            // 
            this.jiechubangdingToolStripMenuItem.Name = "jiechubangdingToolStripMenuItem";
            this.jiechubangdingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.jiechubangdingToolStripMenuItem.Text = "更新";
            this.jiechubangdingToolStripMenuItem.Click += new System.EventHandler(this.jiechubangdingToolStripMenuItem_Click);
            // 
            // zhanghaobanndingToolStripMenuItem
            // 
            this.zhanghaobanndingToolStripMenuItem.Name = "zhanghaobanndingToolStripMenuItem";
            this.zhanghaobanndingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.zhanghaobanndingToolStripMenuItem.Text = "退出";
            this.zhanghaobanndingToolStripMenuItem.Click += new System.EventHandler(this.zhanghaobanndingToolStripMenuItem_Click);
            // 
            // webBrowser2
            // 
            this.webBrowser2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser2.Location = new System.Drawing.Point(1, 76);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 22);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(984, 619);
            this.webBrowser2.TabIndex = 3;
            // 
            // workthread
            // 
            this.workthread.Interval = 60000;
            this.workthread.Tick += new System.EventHandler(this.workthread_Tick);
            // 
            // activethread
            // 
            this.activethread.Interval = 120000;
            this.activethread.Tick += new System.EventHandler(this.activethread_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "美丽说分享宝";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem1.Text = "显示主界面";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem2.Text = "退出";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(987, 75);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.noticelab);
            this.panel2.Location = new System.Drawing.Point(431, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 44);
            this.panel2.TabIndex = 6;
            // 
            // noticelab
            // 
            this.noticelab.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noticelab.ForeColor = System.Drawing.Color.Blue;
            this.noticelab.Location = new System.Drawing.Point(3, 0);
            this.noticelab.Name = "noticelab";
            this.noticelab.Size = new System.Drawing.Size(408, 37);
            this.noticelab.TabIndex = 4;
            this.noticelab.Paint += new System.Windows.Forms.PaintEventHandler(this.noticelab_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Location = new System.Drawing.Point(922, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(31, 21);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(885, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(31, 21);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // noticelablink
            // 
            this.noticelablink.Location = new System.Drawing.Point(162, 704);
            this.noticelablink.Name = "noticelablink";
            this.noticelablink.Size = new System.Drawing.Size(722, 18);
            this.noticelablink.TabIndex = 5;
            this.noticelablink.TabStop = true;
            this.noticelablink.Text = "linkLabel1";
            this.noticelablink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.noticelablink_LinkClicked);
            // 
            // noticethread
            // 
            this.noticethread.Interval = 1000;
            this.noticethread.Tick += new System.EventHandler(this.noticethread_Tick);
            // 
            // noticelabel
            // 
            this.noticelabel.Tick += new System.EventHandler(this.noticelabel_Tick);
            // 
            // systemnoticethread
            // 
            this.systemnoticethread.Interval = 60000;
            this.systemnoticethread.Tick += new System.EventHandler(this.systemnoticethread_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(989, 724);
            this.Controls.Add(this.noticelablink);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.webBrowser2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.webBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "美丽说分享宝";
            this.Activated += new System.EventHandler(this.Main_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Enter += new System.EventHandler(this.Main_Enter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.WebBrowser webBrowser2;
        private System.Windows.Forms.Timer workthread;
        private System.Windows.Forms.Timer activethread;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem tuichuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jiechubangdingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zhanghaobanndingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dfadsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label noticelab;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer noticethread;
        private System.Windows.Forms.LinkLabel noticelablink;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer noticelabel;
        private System.Windows.Forms.Timer systemnoticethread;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;

    }
}

