namespace JC.PTIS.Main.UserCtrls
{
    partial class ExLotBoxCtrl
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
            this.components = new System.ComponentModel.Container();
            this.htTitlePanel1 = new StdLib.WindowsControls.HTTitlePanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.上料盒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.请求出料盒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLotBoxType = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRfid = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.htTitlePanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.htTitlePanel1.Controls.Add(this.panel1);
            this.htTitlePanel1.Controls.Add(this.lblLotBoxType);
            this.htTitlePanel1.Controls.Add(this.label10);
            this.htTitlePanel1.Controls.Add(this.lblRfid);
            this.htTitlePanel1.Controls.Add(this.label8);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(355, 404);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 3;
            this.htTitlePanel1.Text = "外盒1";
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上料盒ToolStripMenuItem,
            this.请求出料盒ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 48);
            // 
            // 上料盒ToolStripMenuItem
            // 
            this.上料盒ToolStripMenuItem.Name = "上料盒ToolStripMenuItem";
            this.上料盒ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.上料盒ToolStripMenuItem.Text = "请求上料盒";
            this.上料盒ToolStripMenuItem.Click += new System.EventHandler(this.上料盒ToolStripMenuItem_Click);
            // 
            // 请求出料盒ToolStripMenuItem
            // 
            this.请求出料盒ToolStripMenuItem.Name = "请求出料盒ToolStripMenuItem";
            this.请求出料盒ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.请求出料盒ToolStripMenuItem.Text = "请求出料盒";
            this.请求出料盒ToolStripMenuItem.Click += new System.EventHandler(this.请求出料盒ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(3, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 331);
            this.panel1.TabIndex = 98;
            // 
            // lblLotBoxType
            // 
            this.lblLotBoxType.AutoSize = true;
            this.lblLotBoxType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lblLotBoxType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLotBoxType.ForeColor = System.Drawing.Color.Black;
            this.lblLotBoxType.Location = new System.Drawing.Point(36, 50);
            this.lblLotBoxType.Name = "lblLotBoxType";
            this.lblLotBoxType.Size = new System.Drawing.Size(0, 17);
            this.lblLotBoxType.TabIndex = 97;
            this.lblLotBoxType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(5, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 96;
            this.label10.Text = "等级:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRfid
            // 
            this.lblRfid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRfid.BackColor = System.Drawing.Color.White;
            this.lblRfid.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRfid.ForeColor = System.Drawing.Color.Black;
            this.lblRfid.Location = new System.Drawing.Point(36, 28);
            this.lblRfid.Name = "lblRfid";
            this.lblRfid.Size = new System.Drawing.Size(316, 20);
            this.lblRfid.TabIndex = 97;
            this.lblRfid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(2, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 17);
            this.label8.TabIndex = 96;
            this.label8.Text = "RFID:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ExLotBoxCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExLotBoxCtrl";
            this.Size = new System.Drawing.Size(355, 404);
            this.Load += new System.EventHandler(this.ExLotBoxCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblLotBoxType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblRfid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 上料盒ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 请求出料盒ToolStripMenuItem;
    }
}
