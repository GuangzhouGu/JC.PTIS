namespace JC.PTIS.Main.UserCtrls
{
    partial class LotTrayCtrl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStageNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBinLevel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTrayCount = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.htTitlePanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.htTitlePanel1.Controls.Add(this.panel1);
            this.htTitlePanel1.Controls.Add(this.lblStageNo);
            this.htTitlePanel1.Controls.Add(this.label2);
            this.htTitlePanel1.Controls.Add(this.lblBinLevel);
            this.htTitlePanel1.Controls.Add(this.label1);
            this.htTitlePanel1.Controls.Add(this.lblTrayCount);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.White;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Size = new System.Drawing.Size(411, 541);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 3;
            this.htTitlePanel1.TitleHeight = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 509);
            this.panel1.TabIndex = 98;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDoubleClick);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // lblStageNo
            // 
            this.lblStageNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStageNo.AutoSize = true;
            this.lblStageNo.BackColor = System.Drawing.Color.Transparent;
            this.lblStageNo.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStageNo.ForeColor = System.Drawing.Color.Black;
            this.lblStageNo.Location = new System.Drawing.Point(396, 1);
            this.lblStageNo.Name = "lblStageNo";
            this.lblStageNo.Size = new System.Drawing.Size(13, 16);
            this.lblStageNo.TabIndex = 97;
            this.lblStageNo.Text = "0";
            this.lblStageNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(377, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 16);
            this.label2.TabIndex = 97;
            this.label2.Text = "No:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBinLevel
            // 
            this.lblBinLevel.AutoSize = true;
            this.lblBinLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblBinLevel.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBinLevel.ForeColor = System.Drawing.Color.Black;
            this.lblBinLevel.Location = new System.Drawing.Point(3, 1);
            this.lblBinLevel.Name = "lblBinLevel";
            this.lblBinLevel.Size = new System.Drawing.Size(33, 16);
            this.lblBinLevel.TabIndex = 97;
            this.lblBinLevel.Text = "None";
            this.lblBinLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 524);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 16);
            this.label1.TabIndex = 97;
            this.label1.Text = "数量";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTrayCount
            // 
            this.lblTrayCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrayCount.AutoSize = true;
            this.lblTrayCount.BackColor = System.Drawing.Color.Transparent;
            this.lblTrayCount.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTrayCount.ForeColor = System.Drawing.Color.Black;
            this.lblTrayCount.Location = new System.Drawing.Point(27, 524);
            this.lblTrayCount.Name = "lblTrayCount";
            this.lblTrayCount.Size = new System.Drawing.Size(13, 16);
            this.lblTrayCount.TabIndex = 97;
            this.lblTrayCount.Text = "0";
            this.lblTrayCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.详细信息ToolStripMenuItem,
            this.清空信息ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.详细信息ToolStripMenuItem.Text = "详细信息";
            this.详细信息ToolStripMenuItem.Click += new System.EventHandler(this.详细信息ToolStripMenuItem_Click);
            // 
            // 清空信息ToolStripMenuItem
            // 
            this.清空信息ToolStripMenuItem.Name = "清空信息ToolStripMenuItem";
            this.清空信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清空信息ToolStripMenuItem.Text = "清空信息";
            this.清空信息ToolStripMenuItem.Click += new System.EventHandler(this.清空信息ToolStripMenuItem_Click);
            // 
            // LotTrayCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LotTrayCtrl";
            this.Size = new System.Drawing.Size(411, 541);
            this.Load += new System.EventHandler(this.LotTrayCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTrayCount;
        private System.Windows.Forms.Label lblBinLevel;
        private System.Windows.Forms.Label lblStageNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 详细信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空信息ToolStripMenuItem;
    }
}
