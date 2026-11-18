namespace JC.PTIS.Main.UserCtrls.SizeEdit
{
    partial class WaferSizeEditCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaferSizeEditCtrl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.tbxLayerCount = new StdLib.WindowsControls.HTTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxGap = new StdLib.WindowsControls.HTTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbxLayerCount, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbxGap, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(640, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(264, 125);
            this.tableLayoutPanel1.TabIndex = 32;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Right;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(12, 3);
            this.label17.Margin = new System.Windows.Forms.Padding(3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 25);
            this.label17.TabIndex = 5;
            this.label17.Text = "基准Die行号:";
            // 
            // tbxLayerCount
            // 
            this.tbxLayerCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxLayerCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxLayerCount.FillColor = System.Drawing.Color.White;
            this.tbxLayerCount.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxLayerCount.Location = new System.Drawing.Point(110, 3);
            this.tbxLayerCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbxLayerCount.Maximum = 2147483647D;
            this.tbxLayerCount.Minimum = -2147483648D;
            this.tbxLayerCount.Name = "tbxLayerCount";
            this.tbxLayerCount.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tbxLayerCount.Size = new System.Drawing.Size(98, 26);
            this.tbxLayerCount.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "基准Die列号:";
            // 
            // tbxGap
            // 
            this.tbxGap.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxGap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxGap.FillColor = System.Drawing.Color.White;
            this.tbxGap.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxGap.Location = new System.Drawing.Point(110, 34);
            this.tbxGap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbxGap.Maximum = 2147483647D;
            this.tbxGap.Minimum = -2147483648D;
            this.tbxGap.Name = "tbxGap";
            this.tbxGap.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.tbxGap.Size = new System.Drawing.Size(98, 26);
            this.tbxGap.TabIndex = 22;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(629, 466);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // WaferSizeEditCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "WaferSizeEditCtrl";
            this.Size = new System.Drawing.Size(920, 769);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label17;
        private StdLib.WindowsControls.HTTextBox tbxLayerCount;
        private System.Windows.Forms.Label label1;
        private StdLib.WindowsControls.HTTextBox tbxGap;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
