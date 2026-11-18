namespace JC.PTIS.Main.UserCtrls
{
    partial class FoupCtrl
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
            this.请求上FoupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.请求取走FoupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRfid = new System.Windows.Forms.Label();
            this.lblRemainCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.htTitlePanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.htTitlePanel1.Controls.Add(this.panel1);
            this.htTitlePanel1.Controls.Add(this.label11);
            this.htTitlePanel1.Controls.Add(this.lblTotalCount);
            this.htTitlePanel1.Controls.Add(this.label10);
            this.htTitlePanel1.Controls.Add(this.lblRfid);
            this.htTitlePanel1.Controls.Add(this.lblRemainCount);
            this.htTitlePanel1.Controls.Add(this.label8);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(396, 510);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 2;
            this.htTitlePanel1.Text = "Foup1";
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.请求上FoupToolStripMenuItem,
            this.请求取走FoupToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 48);
            // 
            // 请求上FoupToolStripMenuItem
            // 
            this.请求上FoupToolStripMenuItem.Name = "请求上FoupToolStripMenuItem";
            this.请求上FoupToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.请求上FoupToolStripMenuItem.Text = "请求上Foup";
            this.请求上FoupToolStripMenuItem.Click += new System.EventHandler(this.请求上FoupToolStripMenuItem_Click);
            // 
            // 请求取走FoupToolStripMenuItem
            // 
            this.请求取走FoupToolStripMenuItem.Name = "请求取走FoupToolStripMenuItem";
            this.请求取走FoupToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.请求取走FoupToolStripMenuItem.Text = "请求取走Foup";
            this.请求取走FoupToolStripMenuItem.Click += new System.EventHandler(this.请求取走FoupToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(3, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 440);
            this.panel1.TabIndex = 98;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(325, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 17);
            this.label11.TabIndex = 96;
            this.label11.Text = "剩余数:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lblTotalCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalCount.ForeColor = System.Drawing.Color.Black;
            this.lblTotalCount.Location = new System.Drawing.Point(36, 50);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(22, 17);
            this.lblTotalCount.TabIndex = 97;
            this.lblTotalCount.Text = "10";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.label10.Text = "总数:";
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
            this.lblRfid.Size = new System.Drawing.Size(357, 20);
            this.lblRfid.TabIndex = 97;
            this.lblRfid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRemainCount
            // 
            this.lblRemainCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainCount.AutoSize = true;
            this.lblRemainCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lblRemainCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRemainCount.ForeColor = System.Drawing.Color.Black;
            this.lblRemainCount.Location = new System.Drawing.Point(368, 50);
            this.lblRemainCount.Name = "lblRemainCount";
            this.lblRemainCount.Size = new System.Drawing.Size(15, 17);
            this.lblRemainCount.TabIndex = 97;
            this.lblRemainCount.Text = "0";
            this.lblRemainCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // FoupCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FoupCtrl";
            this.Size = new System.Drawing.Size(396, 510);
            this.Load += new System.EventHandler(this.FoupCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblRfid;
        private System.Windows.Forms.Label lblRemainCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 请求上FoupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 请求取走FoupToolStripMenuItem;
    }
}
