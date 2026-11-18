
namespace JC.MVI.UserCtrl.YieldCtrl
{
    partial class YieldDayNight
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbxAllDay = new StdLib.WindowsControls.HTRadioButton();
            this.cbxNightYield = new StdLib.WindowsControls.HTRadioButton();
            this.cbxDayYield = new StdLib.WindowsControls.HTRadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.dtPickerDate1 = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnQuery = new StdLib.WindowsControls.HTButton();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnQuery);
            this.panel4.Controls.Add(this.cbxAllDay);
            this.panel4.Controls.Add(this.cbxNightYield);
            this.panel4.Controls.Add(this.cbxDayYield);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.dtPickerDate1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(961, 43);
            this.panel4.TabIndex = 7;
            // 
            // cbxAllDay
            // 
            this.cbxAllDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxAllDay.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cbxAllDay.Location = new System.Drawing.Point(314, 9);
            this.cbxAllDay.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxAllDay.Name = "cbxAllDay";
            this.cbxAllDay.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxAllDay.Size = new System.Drawing.Size(58, 29);
            this.cbxAllDay.TabIndex = 7;
            this.cbxAllDay.Text = "全天";
            this.cbxAllDay.ValueChanged += new StdLib.WindowsControls.HTRadioButton.OnValueChanged(this.cbxDayYield_ValueChanged);
            // 
            // cbxNightYield
            // 
            this.cbxNightYield.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxNightYield.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cbxNightYield.Location = new System.Drawing.Point(250, 9);
            this.cbxNightYield.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxNightYield.Name = "cbxNightYield";
            this.cbxNightYield.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxNightYield.Size = new System.Drawing.Size(58, 29);
            this.cbxNightYield.TabIndex = 7;
            this.cbxNightYield.Text = "夜班";
            this.cbxNightYield.ValueChanged += new StdLib.WindowsControls.HTRadioButton.OnValueChanged(this.cbxDayYield_ValueChanged);
            // 
            // cbxDayYield
            // 
            this.cbxDayYield.Checked = true;
            this.cbxDayYield.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxDayYield.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cbxDayYield.Location = new System.Drawing.Point(186, 9);
            this.cbxDayYield.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxDayYield.Name = "cbxDayYield";
            this.cbxDayYield.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxDayYield.Size = new System.Drawing.Size(58, 29);
            this.cbxDayYield.TabIndex = 7;
            this.cbxDayYield.Text = "白班";
            this.cbxDayYield.ValueChanged += new StdLib.WindowsControls.HTRadioButton.OnValueChanged(this.cbxDayYield_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(9, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "日期:";
            // 
            // dtPickerDate1
            // 
            this.dtPickerDate1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtPickerDate1.Location = new System.Drawing.Point(48, 10);
            this.dtPickerDate1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtPickerDate1.Name = "dtPickerDate1";
            this.dtPickerDate1.Size = new System.Drawing.Size(132, 23);
            this.dtPickerDate1.TabIndex = 2;
            this.dtPickerDate1.ValueChanged += new System.EventHandler(this.dtPickerDate1_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.53245F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.46755F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 43);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(961, 677);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Lavender;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(851, 1);
            this.panel2.Margin = new System.Windows.Forms.Padding(1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(109, 675);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.LightCyan;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(848, 675);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.ChartCtrl_Paint);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuery.FillColor = System.Drawing.Color.White;
            this.btnQuery.FillColor2 = System.Drawing.Color.Empty;
            this.btnQuery.ForeColor = System.Drawing.Color.Black;
            this.btnQuery.Location = new System.Drawing.Point(856, 11);
            this.btnQuery.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.RectSize = 2;
            this.btnQuery.Size = new System.Drawing.Size(100, 28);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // YieldDayNight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "YieldDayNight";
            this.Size = new System.Drawing.Size(961, 720);
            this.Load += new System.EventHandler(this.YieldDayNight_Load);
            this.SizeChanged += new System.EventHandler(this.ChartCtrl_SizeChanged);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel4;
        private StdLib.WindowsControls.HTRadioButton cbxNightYield;
        private StdLib.WindowsControls.HTRadioButton cbxDayYield;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtPickerDate1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private StdLib.WindowsControls.HTRadioButton cbxAllDay;
        private StdLib.WindowsControls.HTButton btnQuery;
    }
}
