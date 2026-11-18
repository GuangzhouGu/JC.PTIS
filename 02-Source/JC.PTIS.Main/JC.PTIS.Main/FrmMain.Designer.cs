namespace JC.PTIS.Main
{
    partial class FrmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.efficencyMonitorCtrl1 = new StdLib.PCBaseFrame.Extension.UserCtrl.EfficencyMonitorCtrl();
            this.cmxRunMode = new StdLib.WindowsControls.HTComboBox();
            this.htLabel5 = new StdLib.WindowsControls.HTLabel();
            this.btnChangeIRNgTrayDone = new StdLib.WindowsControls.HTButton();
            this.uphMonitorCtrl1 = new JC.LidDispenser.UserCtrl.Yield.UphMonitorCtrl();
            this.FORM_PNL_TITLE.SuspendLayout();
            this.SuspendLayout();
            // 
            // FORM_PNL_TITLE
            // 
            this.FORM_PNL_TITLE.Controls.Add(this.btnChangeIRNgTrayDone);
            this.FORM_PNL_TITLE.Controls.Add(this.cmxRunMode);
            this.FORM_PNL_TITLE.Controls.Add(this.htLabel5);
            this.FORM_PNL_TITLE.Controls.SetChildIndex(this.htLabel5, 0);
            this.FORM_PNL_TITLE.Controls.SetChildIndex(this.cmxRunMode, 0);
            this.FORM_PNL_TITLE.Controls.SetChildIndex(this.btnChangeIRNgTrayDone, 0);
            // 
            // efficencyMonitorCtrl1
            // 
            this.efficencyMonitorCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.efficencyMonitorCtrl1.BackColor = System.Drawing.SystemColors.Control;
            this.efficencyMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.efficencyMonitorCtrl1.Location = new System.Drawing.Point(1577, 562);
            this.efficencyMonitorCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.efficencyMonitorCtrl1.Name = "efficencyMonitorCtrl1";
            this.efficencyMonitorCtrl1.Size = new System.Drawing.Size(340, 217);
            this.efficencyMonitorCtrl1.TabIndex = 685;
            // 
            // cmxRunMode
            // 
            this.cmxRunMode.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxRunMode.FillColor = System.Drawing.Color.White;
            this.cmxRunMode.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxRunMode.Location = new System.Drawing.Point(1544, 3);
            this.cmxRunMode.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxRunMode.Name = "cmxRunMode";
            this.cmxRunMode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxRunMode.Size = new System.Drawing.Size(121, 24);
            this.cmxRunMode.TabIndex = 76;
            this.cmxRunMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmxRunMode.SelectedIndexChanged += new System.EventHandler(this.cmxRunMode_SelectedIndexChanged);
            // 
            // htLabel5
            // 
            this.htLabel5.AutoSize = true;
            this.htLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(68)))), ((int)(((byte)(139)))));
            this.htLabel5.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel5.ForeColor = System.Drawing.Color.White;
            this.htLabel5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.htLabel5.Location = new System.Drawing.Point(1475, 4);
            this.htLabel5.Name = "htLabel5";
            this.htLabel5.Size = new System.Drawing.Size(68, 20);
            this.htLabel5.TabIndex = 75;
            this.htLabel5.Text = "运行模式:";
            this.htLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChangeIRNgTrayDone
            // 
            this.btnChangeIRNgTrayDone.BackColor = System.Drawing.Color.White;
            this.btnChangeIRNgTrayDone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangeIRNgTrayDone.FillColor = System.Drawing.Color.White;
            this.btnChangeIRNgTrayDone.FillColor2 = System.Drawing.Color.Empty;
            this.btnChangeIRNgTrayDone.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChangeIRNgTrayDone.ForeColor = System.Drawing.Color.Black;
            this.btnChangeIRNgTrayDone.Location = new System.Drawing.Point(1326, 4);
            this.btnChangeIRNgTrayDone.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnChangeIRNgTrayDone.Name = "btnChangeIRNgTrayDone";
            this.btnChangeIRNgTrayDone.Size = new System.Drawing.Size(143, 23);
            this.btnChangeIRNgTrayDone.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnChangeIRNgTrayDone.TabIndex = 77;
            this.btnChangeIRNgTrayDone.Tag = "4";
            this.btnChangeIRNgTrayDone.Text = "更换IRNgTray完成";
            this.btnChangeIRNgTrayDone.Click += new System.EventHandler(this.btnChangeIRNgTrayDone_Click);
            // 
            // uphMonitorCtrl1
            // 
            this.uphMonitorCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uphMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uphMonitorCtrl1.Location = new System.Drawing.Point(1577, 781);
            this.uphMonitorCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uphMonitorCtrl1.Name = "uphMonitorCtrl1";
            this.uphMonitorCtrl1.Size = new System.Drawing.Size(341, 186);
            this.uphMonitorCtrl1.TabIndex = 686;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.uphMonitorCtrl1);
            this.Controls.Add(this.efficencyMonitorCtrl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "JC.PTIS";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Controls.SetChildIndex(this.FORM_PNL_TITLE, 0);
            this.Controls.SetChildIndex(this.FORM_PNL_BOTTOM, 0);
            this.Controls.SetChildIndex(this.efficencyMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.uphMonitorCtrl1, 0);
            this.FORM_PNL_TITLE.ResumeLayout(false);
            this.FORM_PNL_TITLE.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.PCBaseFrame.Extension.UserCtrl.EfficencyMonitorCtrl efficencyMonitorCtrl1;
        private StdLib.WindowsControls.HTComboBox cmxRunMode;
        private StdLib.WindowsControls.HTLabel htLabel5;
        private StdLib.WindowsControls.HTButton btnChangeIRNgTrayDone;
        private LidDispenser.UserCtrl.Yield.UphMonitorCtrl uphMonitorCtrl1;
    }
}

