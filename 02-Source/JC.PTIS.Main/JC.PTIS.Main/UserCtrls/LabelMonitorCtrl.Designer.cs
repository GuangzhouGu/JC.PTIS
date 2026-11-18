namespace JC.PTIS.Main.UserCtrls
{
    partial class LabelMonitorCtrl
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
            this.htTitlePanel1 = new StdLib.WindowsControls.HTTitlePanel();
            this.lblCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrintResult = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbxLabel = new System.Windows.Forms.PictureBox();
            this.htTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.BackColor = System.Drawing.Color.Transparent;
            this.htTitlePanel1.Controls.Add(this.lblCode);
            this.htTitlePanel1.Controls.Add(this.label1);
            this.htTitlePanel1.Controls.Add(this.lblPrintResult);
            this.htTitlePanel1.Controls.Add(this.label3);
            this.htTitlePanel1.Controls.Add(this.pbxLabel);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Margin = new System.Windows.Forms.Padding(4);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(180, 205);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 3;
            this.htTitlePanel1.Text = "标签检测";
            this.htTitlePanel1.TitleColor = System.Drawing.Color.SkyBlue;
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // lblCode
            // 
            this.lblCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCode.BackColor = System.Drawing.Color.LightCyan;
            this.lblCode.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCode.ForeColor = System.Drawing.Color.Black;
            this.lblCode.Location = new System.Drawing.Point(57, 44);
            this.lblCode.Margin = new System.Windows.Forms.Padding(1);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(120, 17);
            this.lblCode.TabIndex = 1;
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(25, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "条码:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPrintResult
            // 
            this.lblPrintResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrintResult.BackColor = System.Drawing.Color.LightCyan;
            this.lblPrintResult.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrintResult.ForeColor = System.Drawing.Color.Black;
            this.lblPrintResult.Location = new System.Drawing.Point(57, 25);
            this.lblPrintResult.Margin = new System.Windows.Forms.Padding(1);
            this.lblPrintResult.Name = "lblPrintResult";
            this.lblPrintResult.Size = new System.Drawing.Size(120, 17);
            this.lblPrintResult.TabIndex = 1;
            this.lblPrintResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(1, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "检测结果:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxLabel
            // 
            this.pbxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxLabel.Location = new System.Drawing.Point(4, 62);
            this.pbxLabel.Margin = new System.Windows.Forms.Padding(4);
            this.pbxLabel.Name = "pbxLabel";
            this.pbxLabel.Size = new System.Drawing.Size(173, 141);
            this.pbxLabel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxLabel.TabIndex = 0;
            this.pbxLabel.TabStop = false;
            // 
            // LabelMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LabelMonitorCtrl";
            this.Size = new System.Drawing.Size(180, 205);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private System.Windows.Forms.PictureBox pbxLabel;
        private System.Windows.Forms.Label lblPrintResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label1;
    }
}
