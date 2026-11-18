namespace JC.PTIS.Main.UserCtrls
{
    partial class RedisKeyValueMonitorCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.htGroupBox1 = new StdLib.WindowsControls.HTGroupBox();
            this.btnSave = new StdLib.WindowsControls.HTButton();
            this.tbxUser = new StdLib.WindowsControls.HTTextBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.tbxPassword = new StdLib.WindowsControls.HTTextBox();
            this.htLabel3 = new StdLib.WindowsControls.HTLabel();
            this.tbxPort = new StdLib.WindowsControls.HTTextBox();
            this.htLabel2 = new StdLib.WindowsControls.HTLabel();
            this.tbxAddress = new StdLib.WindowsControls.HTTextBox();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.htGroupBox2 = new StdLib.WindowsControls.HTGroupBox();
            this.btnGetAllKey = new StdLib.WindowsControls.HTButton();
            this.btnConnect = new StdLib.WindowsControls.HTButton();
            this.btnClose = new StdLib.WindowsControls.HTButton();
            this.htGroupBox3 = new StdLib.WindowsControls.HTGroupBox();
            this.htGroupBox4 = new StdLib.WindowsControls.HTGroupBox();
            this.htTreeView1 = new StdLib.WindowsControls.HTTreeView();
            this.btnDeleteKey = new StdLib.WindowsControls.HTButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnDeleteAllKeys = new StdLib.WindowsControls.HTButton();
            this.htGroupBox1.SuspendLayout();
            this.htGroupBox2.SuspendLayout();
            this.htGroupBox3.SuspendLayout();
            this.htGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.Controls.Add(this.btnSave);
            this.htGroupBox1.Controls.Add(this.tbxUser);
            this.htGroupBox1.Controls.Add(this.htLabel4);
            this.htGroupBox1.Controls.Add(this.tbxPassword);
            this.htGroupBox1.Controls.Add(this.htLabel3);
            this.htGroupBox1.Controls.Add(this.tbxPort);
            this.htGroupBox1.Controls.Add(this.htLabel2);
            this.htGroupBox1.Controls.Add(this.tbxAddress);
            this.htGroupBox1.Controls.Add(this.htLabel1);
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(3, 62);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox1.Size = new System.Drawing.Size(587, 97);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "Redis设置";
            this.htGroupBox1.TitleTop = 8;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FillColor = System.Drawing.Color.White;
            this.btnSave.FillColor2 = System.Drawing.Color.Empty;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(492, 57);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 25);
            this.btnSave.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnSave.TabIndex = 51;
            this.btnSave.Tag = "14";
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbxUser
            // 
            this.tbxUser.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxUser.FillColor = System.Drawing.Color.White;
            this.tbxUser.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxUser.Location = new System.Drawing.Point(300, 56);
            this.tbxUser.Maximum = 2147483647D;
            this.tbxUser.Minimum = -2147483648D;
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.Padding = new System.Windows.Forms.Padding(5);
            this.tbxUser.ReadOnly = true;
            this.tbxUser.Size = new System.Drawing.Size(150, 26);
            this.tbxUser.TabIndex = 1;
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel4.Location = new System.Drawing.Point(243, 57);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(65, 20);
            this.htLabel4.TabIndex = 0;
            this.htLabel4.Text = "用户名：";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxPassword
            // 
            this.tbxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxPassword.DoubleValue = 123456D;
            this.tbxPassword.FillColor = System.Drawing.Color.White;
            this.tbxPassword.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxPassword.IntValue = 123456;
            this.tbxPassword.Location = new System.Drawing.Point(300, 24);
            this.tbxPassword.Maximum = 2147483647D;
            this.tbxPassword.Minimum = -2147483648D;
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.Padding = new System.Windows.Forms.Padding(5);
            this.tbxPassword.Size = new System.Drawing.Size(150, 26);
            this.tbxPassword.TabIndex = 1;
            this.tbxPassword.Text = "123456";
            // 
            // htLabel3
            // 
            this.htLabel3.AutoSize = true;
            this.htLabel3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel3.Location = new System.Drawing.Point(257, 25);
            this.htLabel3.Name = "htLabel3";
            this.htLabel3.Size = new System.Drawing.Size(51, 20);
            this.htLabel3.TabIndex = 0;
            this.htLabel3.Text = "密码：";
            this.htLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxPort
            // 
            this.tbxPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxPort.DoubleValue = 6379D;
            this.tbxPort.FillColor = System.Drawing.Color.White;
            this.tbxPort.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxPort.IntValue = 6379;
            this.tbxPort.Location = new System.Drawing.Point(73, 55);
            this.tbxPort.Maximum = 2147483647D;
            this.tbxPort.Minimum = -2147483648D;
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Padding = new System.Windows.Forms.Padding(5);
            this.tbxPort.Size = new System.Drawing.Size(150, 26);
            this.tbxPort.TabIndex = 1;
            this.tbxPort.Text = "6379";
            // 
            // htLabel2
            // 
            this.htLabel2.AutoSize = true;
            this.htLabel2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel2.Location = new System.Drawing.Point(30, 56);
            this.htLabel2.Name = "htLabel2";
            this.htLabel2.Size = new System.Drawing.Size(51, 20);
            this.htLabel2.TabIndex = 0;
            this.htLabel2.Text = "端口：";
            this.htLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxAddress
            // 
            this.tbxAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxAddress.FillColor = System.Drawing.Color.White;
            this.tbxAddress.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxAddress.Location = new System.Drawing.Point(73, 23);
            this.tbxAddress.Maximum = 2147483647D;
            this.tbxAddress.Minimum = -2147483648D;
            this.tbxAddress.Name = "tbxAddress";
            this.tbxAddress.Padding = new System.Windows.Forms.Padding(5);
            this.tbxAddress.Size = new System.Drawing.Size(150, 26);
            this.tbxAddress.TabIndex = 1;
            this.tbxAddress.Text = "127.0.0.1";
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel1.Location = new System.Drawing.Point(30, 24);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(51, 20);
            this.htLabel1.TabIndex = 0;
            this.htLabel1.Text = "地址：";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htGroupBox2
            // 
            this.htGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.htGroupBox2.Controls.Add(this.htTreeView1);
            this.htGroupBox2.Controls.Add(this.btnDeleteAllKeys);
            this.htGroupBox2.Controls.Add(this.btnDeleteKey);
            this.htGroupBox2.Controls.Add(this.btnGetAllKey);
            this.htGroupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox2.Location = new System.Drawing.Point(3, 162);
            this.htGroupBox2.Name = "htGroupBox2";
            this.htGroupBox2.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox2.Size = new System.Drawing.Size(587, 447);
            this.htGroupBox2.TabIndex = 0;
            this.htGroupBox2.Text = "Key列表";
            this.htGroupBox2.TitleTop = 8;
            // 
            // btnGetAllKey
            // 
            this.btnGetAllKey.BackColor = System.Drawing.Color.White;
            this.btnGetAllKey.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetAllKey.FillColor = System.Drawing.Color.White;
            this.btnGetAllKey.FillColor2 = System.Drawing.Color.Empty;
            this.btnGetAllKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetAllKey.ForeColor = System.Drawing.Color.Black;
            this.btnGetAllKey.Location = new System.Drawing.Point(492, 15);
            this.btnGetAllKey.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnGetAllKey.Name = "btnGetAllKey";
            this.btnGetAllKey.Size = new System.Drawing.Size(82, 25);
            this.btnGetAllKey.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnGetAllKey.TabIndex = 51;
            this.btnGetAllKey.Tag = "14";
            this.btnGetAllKey.Text = "获取所有Key";
            this.btnGetAllKey.Click += new System.EventHandler(this.btnGetAllKey_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.White;
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.FillColor = System.Drawing.Color.White;
            this.btnConnect.FillColor2 = System.Drawing.Color.Empty;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnect.Location = new System.Drawing.Point(16, 26);
            this.btnConnect.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(82, 25);
            this.btnConnect.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnConnect.TabIndex = 51;
            this.btnConnect.Tag = "14";
            this.btnConnect.Text = "连接";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.FillColor2 = System.Drawing.Color.Empty;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(107, 27);
            this.btnClose.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 25);
            this.btnClose.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnClose.TabIndex = 51;
            this.btnClose.Tag = "14";
            this.btnClose.Text = "断开";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // htGroupBox3
            // 
            this.htGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htGroupBox3.Controls.Add(this.listBox1);
            this.htGroupBox3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox3.Location = new System.Drawing.Point(596, 162);
            this.htGroupBox3.Name = "htGroupBox3";
            this.htGroupBox3.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox3.Size = new System.Drawing.Size(618, 447);
            this.htGroupBox3.TabIndex = 0;
            this.htGroupBox3.Text = "KeyValue监控";
            this.htGroupBox3.TitleTop = 8;
            // 
            // htGroupBox4
            // 
            this.htGroupBox4.Controls.Add(this.btnClose);
            this.htGroupBox4.Controls.Add(this.btnConnect);
            this.htGroupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox4.Location = new System.Drawing.Point(3, 3);
            this.htGroupBox4.Name = "htGroupBox4";
            this.htGroupBox4.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox4.Size = new System.Drawing.Size(587, 58);
            this.htGroupBox4.TabIndex = 0;
            this.htGroupBox4.Text = "Redis连接";
            this.htGroupBox4.TitleTop = 8;
            // 
            // htTreeView1
            // 
            this.htTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.htTreeView1.BackColor = System.Drawing.Color.Transparent;
            this.htTreeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.htTreeView1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTreeView1.ItemHeight = 28;
            this.htTreeView1.Location = new System.Drawing.Point(6, 20);
            this.htTreeView1.Name = "htTreeView1";
            this.htTreeView1.Size = new System.Drawing.Size(444, 421);
            this.htTreeView1.TabIndex = 52;
            this.htTreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.htTreeView1_AfterSelect);
            // 
            // btnDeleteKey
            // 
            this.btnDeleteKey.BackColor = System.Drawing.Color.White;
            this.btnDeleteKey.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteKey.FillColor = System.Drawing.Color.White;
            this.btnDeleteKey.FillColor2 = System.Drawing.Color.Empty;
            this.btnDeleteKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteKey.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteKey.Location = new System.Drawing.Point(492, 46);
            this.btnDeleteKey.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDeleteKey.Name = "btnDeleteKey";
            this.btnDeleteKey.Size = new System.Drawing.Size(82, 25);
            this.btnDeleteKey.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnDeleteKey.TabIndex = 51;
            this.btnDeleteKey.Tag = "14";
            this.btnDeleteKey.Text = "删除选中Key";
            this.btnDeleteKey.Click += new System.EventHandler(this.btnDeleteKey_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(3, 24);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(612, 420);
            this.listBox1.TabIndex = 0;
            // 
            // btnDeleteAllKeys
            // 
            this.btnDeleteAllKeys.BackColor = System.Drawing.Color.White;
            this.btnDeleteAllKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteAllKeys.FillColor = System.Drawing.Color.White;
            this.btnDeleteAllKeys.FillColor2 = System.Drawing.Color.Empty;
            this.btnDeleteAllKeys.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteAllKeys.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteAllKeys.Location = new System.Drawing.Point(492, 77);
            this.btnDeleteAllKeys.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDeleteAllKeys.Name = "btnDeleteAllKeys";
            this.btnDeleteAllKeys.Size = new System.Drawing.Size(82, 25);
            this.btnDeleteAllKeys.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnDeleteAllKeys.TabIndex = 51;
            this.btnDeleteAllKeys.Tag = "14";
            this.btnDeleteAllKeys.Text = "删除所有Key";
            this.btnDeleteAllKeys.Click += new System.EventHandler(this.btnDeleteAllKeys_Click);
            // 
            // RedisKeyValueMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.htGroupBox3);
            this.Controls.Add(this.htGroupBox2);
            this.Controls.Add(this.htGroupBox4);
            this.Controls.Add(this.htGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RedisKeyValueMonitorCtrl";
            this.Size = new System.Drawing.Size(1217, 612);
            this.Load += new System.EventHandler(this.RedisKeyValueMonitorCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox1.PerformLayout();
            this.htGroupBox2.ResumeLayout(false);
            this.htGroupBox3.ResumeLayout(false);
            this.htGroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTTextBox tbxUser;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTTextBox tbxPassword;
        private StdLib.WindowsControls.HTLabel htLabel3;
        private StdLib.WindowsControls.HTTextBox tbxPort;
        private StdLib.WindowsControls.HTLabel htLabel2;
        private StdLib.WindowsControls.HTTextBox tbxAddress;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.WindowsControls.HTButton btnSave;
        private StdLib.WindowsControls.HTGroupBox htGroupBox2;
        private StdLib.WindowsControls.HTButton btnConnect;
        private StdLib.WindowsControls.HTButton btnClose;
        private StdLib.WindowsControls.HTGroupBox htGroupBox3;
        private StdLib.WindowsControls.HTButton btnGetAllKey;
        private StdLib.WindowsControls.HTGroupBox htGroupBox4;
        private StdLib.WindowsControls.HTTreeView htTreeView1;
        private StdLib.WindowsControls.HTButton btnDeleteKey;
        private System.Windows.Forms.ListBox listBox1;
        private StdLib.WindowsControls.HTButton btnDeleteAllKeys;
    }
}
