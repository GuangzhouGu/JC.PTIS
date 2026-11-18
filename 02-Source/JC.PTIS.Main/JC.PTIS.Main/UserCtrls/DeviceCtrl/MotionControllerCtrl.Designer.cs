namespace JC.PTIS.Main.UserCtrls.DeviceCtrl
{
    partial class MotionControllerCtrl
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
            this.htGroupBox7 = new StdLib.WindowsControls.HTGroupBox();
            this.cmxControllerList = new StdLib.WindowsControls.HTComboBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.btnClearControllerFaults = new StdLib.WindowsControls.HTButton();
            this.htGroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox7
            // 
            this.htGroupBox7.Controls.Add(this.cmxControllerList);
            this.htGroupBox7.Controls.Add(this.htLabel4);
            this.htGroupBox7.Controls.Add(this.btnClearControllerFaults);
            this.htGroupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox7.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox7.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox7.Name = "htGroupBox7";
            this.htGroupBox7.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox7.Size = new System.Drawing.Size(476, 424);
            this.htGroupBox7.TabIndex = 56;
            this.htGroupBox7.Text = "控制器调试";
            this.htGroupBox7.TitleTop = 8;
            // 
            // cmxControllerList
            // 
            this.cmxControllerList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxControllerList.FillColor = System.Drawing.Color.White;
            this.cmxControllerList.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxControllerList.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmxControllerList.Location = new System.Drawing.Point(69, 24);
            this.cmxControllerList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxControllerList.Name = "cmxControllerList";
            this.cmxControllerList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxControllerList.Size = new System.Drawing.Size(165, 22);
            this.cmxControllerList.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.cmxControllerList.TabIndex = 54;
            this.cmxControllerList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel4.Location = new System.Drawing.Point(14, 23);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(54, 20);
            this.htLabel4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel4.TabIndex = 53;
            this.htLabel4.Text = "控制器:";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClearControllerFaults
            // 
            this.btnClearControllerFaults.BackColor = System.Drawing.Color.White;
            this.btnClearControllerFaults.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearControllerFaults.FillColor = System.Drawing.Color.White;
            this.btnClearControllerFaults.FillColor2 = System.Drawing.Color.Empty;
            this.btnClearControllerFaults.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearControllerFaults.ForeColor = System.Drawing.Color.Black;
            this.btnClearControllerFaults.Location = new System.Drawing.Point(240, 21);
            this.btnClearControllerFaults.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearControllerFaults.Name = "btnClearControllerFaults";
            this.btnClearControllerFaults.Size = new System.Drawing.Size(105, 28);
            this.btnClearControllerFaults.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnClearControllerFaults.TabIndex = 50;
            this.btnClearControllerFaults.Tag = "14";
            this.btnClearControllerFaults.Text = "控制器清除错误";
            this.btnClearControllerFaults.Click += new System.EventHandler(this.btnClearControllerFaults_Click);
            // 
            // MotionControllerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox7);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MotionControllerCtrl";
            this.Size = new System.Drawing.Size(476, 424);
            this.Load += new System.EventHandler(this.MotionControllerCtrl_Load);
            this.htGroupBox7.ResumeLayout(false);
            this.htGroupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox7;
        private StdLib.WindowsControls.HTComboBox cmxControllerList;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTButton btnClearControllerFaults;
    }
}
