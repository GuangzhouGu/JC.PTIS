namespace JC.PTIS.Main.UserCtrls
{
    partial class LotCellCtrl
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
            this.htPanel1 = new StdLib.WindowsControls.HTPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTrayCount = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.清空信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空信息ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.htPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.htPanel1.Size = new System.Drawing.Size(201, 165);
            this.htPanel1.TabIndex = 0;
            this.htPanel1.Text = "htPanel1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.01677F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.98323F));
            this.tableLayoutPanel1.Controls.Add(this.lblTrayCount, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblBin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(197, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTrayCount
            // 
            this.lblTrayCount.BackColor = System.Drawing.Color.White;
            this.lblTrayCount.ContextMenuStrip = this.contextMenuStrip1;
            this.lblTrayCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTrayCount.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTrayCount.ForeColor = System.Drawing.Color.Black;
            this.lblTrayCount.Location = new System.Drawing.Point(121, 81);
            this.lblTrayCount.Margin = new System.Windows.Forms.Padding(1);
            this.lblTrayCount.Name = "lblTrayCount";
            this.lblTrayCount.Size = new System.Drawing.Size(75, 79);
            this.lblTrayCount.TabIndex = 7;
            this.lblTrayCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.详细信息ToolStripMenuItem,
            this.清空信息ToolStripMenuItem,
            this.清空信息ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.详细信息ToolStripMenuItem.Text = "详细信息";
            this.详细信息ToolStripMenuItem.Click += new System.EventHandler(this.详细信息ToolStripMenuItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightCyan;
            this.label4.ContextMenuStrip = this.contextMenuStrip1;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1, 81);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 79);
            this.label4.TabIndex = 6;
            this.label4.Text = "Lot";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBin
            // 
            this.lblBin.BackColor = System.Drawing.Color.White;
            this.lblBin.ContextMenuStrip = this.contextMenuStrip1;
            this.lblBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBin.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBin.ForeColor = System.Drawing.Color.Black;
            this.lblBin.Location = new System.Drawing.Point(121, 1);
            this.lblBin.Margin = new System.Windows.Forms.Padding(1);
            this.lblBin.Name = "lblBin";
            this.lblBin.Size = new System.Drawing.Size(75, 78);
            this.lblBin.TabIndex = 2;
            this.lblBin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightCyan;
            this.label2.ContextMenuStrip = this.contextMenuStrip1;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(1, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 78);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bin";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 清空信息ToolStripMenuItem
            // 
            this.清空信息ToolStripMenuItem.Name = "清空信息ToolStripMenuItem";
            this.清空信息ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清空信息ToolStripMenuItem.Text = "修改信息";
            // 
            // 清空信息ToolStripMenuItem1
            // 
            this.清空信息ToolStripMenuItem1.Name = "清空信息ToolStripMenuItem1";
            this.清空信息ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.清空信息ToolStripMenuItem1.Text = "清空信息";
            this.清空信息ToolStripMenuItem1.Click += new System.EventHandler(this.清空信息ToolStripMenuItem1_Click);
            // 
            // LotCellCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.htPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LotCellCtrl";
            this.Size = new System.Drawing.Size(201, 165);
            this.Load += new System.EventHandler(this.LotCellCtrl_Load);
            this.htPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTPanel htPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBin;
        private System.Windows.Forms.Label lblTrayCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 详细信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空信息ToolStripMenuItem1;
    }
}
