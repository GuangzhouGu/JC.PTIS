namespace JC.PTIS.Main.UserCtrls
{
    partial class RegisterSignalMonitorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterSignalMonitorCtrl));
            this.dgv_InputInfo = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmxCountPerPage = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.tbxPage = new System.Windows.Forms.TextBox();
            this.btnPre = new System.Windows.Forms.Button();
            this.tbxTotalPages = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxTotalRowCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.InputNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsInputed = new System.Windows.Forms.DataGridViewImageColumn();
            this.ForceOutput = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InputInfo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_InputInfo
            // 
            this.dgv_InputInfo.AllowUserToAddRows = false;
            this.dgv_InputInfo.AllowUserToDeleteRows = false;
            this.dgv_InputInfo.AllowUserToResizeRows = false;
            this.dgv_InputInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_InputInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_InputInfo.BackgroundColor = System.Drawing.Color.White;
            this.dgv_InputInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_InputInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_InputInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_InputInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_InputInfo.ColumnHeadersHeight = 25;
            this.dgv_InputInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_InputInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InputNum,
            this.InputName,
            this.InputModuleName,
            this.IsInputed,
            this.ForceOutput});
            this.dgv_InputInfo.Location = new System.Drawing.Point(0, 0);
            this.dgv_InputInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_InputInfo.Name = "dgv_InputInfo";
            this.dgv_InputInfo.RowHeadersVisible = false;
            this.dgv_InputInfo.RowHeadersWidth = 51;
            this.dgv_InputInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.dgv_InputInfo.RowTemplate.Height = 23;
            this.dgv_InputInfo.Size = new System.Drawing.Size(773, 422);
            this.dgv_InputInfo.TabIndex = 39;
            this.dgv_InputInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OutputInfo_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmxCountPerPage);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.tbxPage);
            this.panel1.Controls.Add(this.btnPre);
            this.panel1.Controls.Add(this.tbxTotalPages);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.tbxTotalRowCount);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 423);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 36);
            this.panel1.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(196, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "每页条数";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmxCountPerPage
            // 
            this.cmxCountPerPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmxCountPerPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmxCountPerPage.Enabled = false;
            this.cmxCountPerPage.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmxCountPerPage.FormattingEnabled = true;
            this.cmxCountPerPage.Items.AddRange(new object[] {
            "10",
            "30"});
            this.cmxCountPerPage.Location = new System.Drawing.Point(258, 5);
            this.cmxCountPerPage.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            this.cmxCountPerPage.Name = "cmxCountPerPage";
            this.cmxCountPerPage.Size = new System.Drawing.Size(66, 25);
            this.cmxCountPerPage.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext.Location = new System.Drawing.Point(480, 1);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 33);
            this.btnNext.TabIndex = 43;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // tbxPage
            // 
            this.tbxPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxPage.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxPage.Location = new System.Drawing.Point(407, 4);
            this.tbxPage.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            this.tbxPage.Name = "tbxPage";
            this.tbxPage.ReadOnly = true;
            this.tbxPage.Size = new System.Drawing.Size(35, 26);
            this.tbxPage.TabIndex = 3;
            this.tbxPage.Text = "1";
            this.tbxPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPre
            // 
            this.btnPre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPre.FlatAppearance.BorderSize = 0;
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnPre.Image = ((System.Drawing.Image)(resources.GetObject("btnPre.Image")));
            this.btnPre.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPre.Location = new System.Drawing.Point(330, 1);
            this.btnPre.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(40, 33);
            this.btnPre.TabIndex = 42;
            this.btnPre.UseVisualStyleBackColor = true;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // tbxTotalPages
            // 
            this.tbxTotalPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxTotalPages.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxTotalPages.Location = new System.Drawing.Point(557, 4);
            this.tbxTotalPages.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            this.tbxTotalPages.Name = "tbxTotalPages";
            this.tbxTotalPages.ReadOnly = true;
            this.tbxTotalPages.Size = new System.Drawing.Size(51, 26);
            this.tbxTotalPages.TabIndex = 3;
            this.tbxTotalPages.Text = "0";
            this.tbxTotalPages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(624, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "页";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(381, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "第";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(650, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "共";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxTotalRowCount
            // 
            this.tbxTotalRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxTotalRowCount.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxTotalRowCount.Location = new System.Drawing.Point(676, 4);
            this.tbxTotalRowCount.Margin = new System.Windows.Forms.Padding(3, 8, 3, 4);
            this.tbxTotalRowCount.Name = "tbxTotalRowCount";
            this.tbxTotalRowCount.ReadOnly = true;
            this.tbxTotalRowCount.Size = new System.Drawing.Size(62, 26);
            this.tbxTotalRowCount.TabIndex = 3;
            this.tbxTotalRowCount.Text = "0";
            this.tbxTotalRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(744, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "条";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(454, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "页";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(531, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "共";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputNum
            // 
            this.InputNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.InputNum.FillWeight = 22.84264F;
            this.InputNum.HeaderText = "序号";
            this.InputNum.MinimumWidth = 50;
            this.InputNum.Name = "InputNum";
            this.InputNum.ReadOnly = true;
            this.InputNum.Width = 57;
            // 
            // InputName
            // 
            this.InputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InputName.FillWeight = 22.84264F;
            this.InputName.HeaderText = "输入名称";
            this.InputName.MinimumWidth = 50;
            this.InputName.Name = "InputName";
            this.InputName.ReadOnly = true;
            // 
            // InputModuleName
            // 
            this.InputModuleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InputModuleName.FillWeight = 22.84264F;
            this.InputModuleName.HeaderText = "描述";
            this.InputModuleName.MinimumWidth = 50;
            this.InputModuleName.Name = "InputModuleName";
            this.InputModuleName.ReadOnly = true;
            // 
            // IsInputed
            // 
            this.IsInputed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.IsInputed.FillWeight = 22.84264F;
            this.IsInputed.HeaderText = "有无信号";
            this.IsInputed.Image = global::JC.PTIS.Main.Properties.Resources.Gray2;
            this.IsInputed.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.IsInputed.MinimumWidth = 50;
            this.IsInputed.Name = "IsInputed";
            this.IsInputed.ReadOnly = true;
            this.IsInputed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsInputed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsInputed.Width = 81;
            // 
            // ForceOutput
            // 
            this.ForceOutput.HeaderText = "强制";
            this.ForceOutput.MinimumWidth = 100;
            this.ForceOutput.Name = "ForceOutput";
            // 
            // RegisterSignalMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgv_InputInfo);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RegisterSignalMonitorCtrl";
            this.Size = new System.Drawing.Size(773, 459);
            this.Load += new System.EventHandler(this.RegisterSignalMonitorCtrl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InputInfo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_InputInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmxCountPerPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox tbxPage;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.TextBox tbxTotalPages;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxTotalRowCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputName;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputModuleName;
        private System.Windows.Forms.DataGridViewImageColumn IsInputed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ForceOutput;
    }
}
