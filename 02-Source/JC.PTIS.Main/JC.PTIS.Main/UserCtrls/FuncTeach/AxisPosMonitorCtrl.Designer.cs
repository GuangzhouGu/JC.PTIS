namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class AxisPosMonitorCtrl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.htGroupBox1 = new StdLib.WindowsControls.HTGroupBox();
            this.cmxStationList = new StdLib.WindowsControls.HTComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvAxisList = new System.Windows.Forms.DataGridView();
            this.InputNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.htGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAxisList)).BeginInit();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.Controls.Add(this.dgvAxisList);
            this.htGroupBox1.Controls.Add(this.cmxStationList);
            this.htGroupBox1.Controls.Add(this.label4);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox1.Size = new System.Drawing.Size(366, 345);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "轴位置监控";
            this.htGroupBox1.TitleTop = 8;
            // 
            // cmxStationList
            // 
            this.cmxStationList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxStationList.FillColor = System.Drawing.Color.White;
            this.cmxStationList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmxStationList.Location = new System.Drawing.Point(74, 19);
            this.cmxStationList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxStationList.Name = "cmxStationList";
            this.cmxStationList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxStationList.Size = new System.Drawing.Size(161, 23);
            this.cmxStationList.TabIndex = 41;
            this.cmxStationList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmxStationList.SelectedIndexChanged += new System.EventHandler(this.cmxStationList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(5, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 20);
            this.label4.TabIndex = 40;
            this.label4.Text = "工位名称:";
            // 
            // dgvAxisList
            // 
            this.dgvAxisList.AllowUserToAddRows = false;
            this.dgvAxisList.AllowUserToDeleteRows = false;
            this.dgvAxisList.AllowUserToResizeRows = false;
            this.dgvAxisList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAxisList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAxisList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvAxisList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAxisList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAxisList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAxisList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAxisList.ColumnHeadersHeight = 25;
            this.dgvAxisList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAxisList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InputNum,
            this.InputName});
            this.dgvAxisList.Location = new System.Drawing.Point(6, 46);
            this.dgvAxisList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvAxisList.Name = "dgvAxisList";
            this.dgvAxisList.RowHeadersVisible = false;
            this.dgvAxisList.RowHeadersWidth = 51;
            this.dgvAxisList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.dgvAxisList.RowTemplate.Height = 23;
            this.dgvAxisList.Size = new System.Drawing.Size(354, 297);
            this.dgvAxisList.TabIndex = 42;
            // 
            // InputNum
            // 
            this.InputNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.InputNum.FillWeight = 22.84264F;
            this.InputNum.HeaderText = "轴名称";
            this.InputNum.MinimumWidth = 10;
            this.InputNum.Name = "InputNum";
            this.InputNum.ReadOnly = true;
            this.InputNum.Width = 83;
            // 
            // InputName
            // 
            this.InputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InputName.FillWeight = 22.84264F;
            this.InputName.HeaderText = "位置";
            this.InputName.MinimumWidth = 10;
            this.InputName.Name = "InputName";
            this.InputName.ReadOnly = true;
            // 
            // AxisPosMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.htGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AxisPosMonitorCtrl";
            this.Size = new System.Drawing.Size(366, 345);
            this.Load += new System.EventHandler(this.AxisPosMonitorCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAxisList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTComboBox cmxStationList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvAxisList;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputName;
    }
}
