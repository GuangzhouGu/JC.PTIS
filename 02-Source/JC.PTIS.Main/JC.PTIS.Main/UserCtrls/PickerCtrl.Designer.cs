namespace JC.PTIS.Main.UserCtrls
{
    partial class PickerCtrl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPickNo = new System.Windows.Forms.Label();
            this.lblBinLevel = new System.Windows.Forms.Label();
            this.htPanel1 = new StdLib.WindowsControls.HTPanel();
            this.清除信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.htPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPickNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblBinLevel, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 371);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightCyan;
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 183);
            this.label1.TabIndex = 0;
            this.label1.Text = "No";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.详细信息ToolStripMenuItem,
            this.清除信息ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.详细信息ToolStripMenuItem.Text = "详细信息";
            this.详细信息ToolStripMenuItem.Click += new System.EventHandler(this.详细信息ToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightCyan;
            this.label2.ContextMenuStrip = this.contextMenuStrip1;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(1, 186);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 184);
            this.label2.TabIndex = 0;
            this.label2.Text = "Bin";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPickNo
            // 
            this.lblPickNo.AutoSize = true;
            this.lblPickNo.BackColor = System.Drawing.Color.White;
            this.lblPickNo.ContextMenuStrip = this.contextMenuStrip1;
            this.lblPickNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPickNo.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPickNo.ForeColor = System.Drawing.Color.Black;
            this.lblPickNo.Location = new System.Drawing.Point(213, 1);
            this.lblPickNo.Margin = new System.Windows.Forms.Padding(1);
            this.lblPickNo.Name = "lblPickNo";
            this.lblPickNo.Size = new System.Drawing.Size(210, 183);
            this.lblPickNo.TabIndex = 0;
            this.lblPickNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBinLevel
            // 
            this.lblBinLevel.AutoSize = true;
            this.lblBinLevel.BackColor = System.Drawing.Color.White;
            this.lblBinLevel.ContextMenuStrip = this.contextMenuStrip1;
            this.lblBinLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBinLevel.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBinLevel.ForeColor = System.Drawing.Color.Black;
            this.lblBinLevel.Location = new System.Drawing.Point(213, 186);
            this.lblBinLevel.Margin = new System.Windows.Forms.Padding(1);
            this.lblBinLevel.Name = "lblBinLevel";
            this.lblBinLevel.Size = new System.Drawing.Size(210, 184);
            this.lblBinLevel.TabIndex = 0;
            this.lblBinLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htPanel1
            // 
            this.htPanel1.Controls.Add(this.tableLayoutPanel1);
            this.htPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htPanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htPanel1.Location = new System.Drawing.Point(0, 0);
            this.htPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.htPanel1.Name = "htPanel1";
            this.htPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.htPanel1.Radius = 10;
            this.htPanel1.Size = new System.Drawing.Size(428, 375);
            this.htPanel1.TabIndex = 1;
            this.htPanel1.Text = "htPanel1";
            // 
            // 清除信息ToolStripMenuItem
            // 
            this.清除信息ToolStripMenuItem.Name = "清除信息ToolStripMenuItem";
            this.清除信息ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清除信息ToolStripMenuItem.Text = "清除信息";
            this.清除信息ToolStripMenuItem.Click += new System.EventHandler(this.清除信息ToolStripMenuItem_Click);
            // 
            // PickerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.htPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PickerCtrl";
            this.Size = new System.Drawing.Size(428, 375);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.htPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPickNo;
        private System.Windows.Forms.Label lblBinLevel;
        private StdLib.WindowsControls.HTPanel htPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 详细信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除信息ToolStripMenuItem;
    }
}
