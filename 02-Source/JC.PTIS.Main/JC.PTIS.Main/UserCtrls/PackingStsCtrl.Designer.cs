namespace JC.PTIS.Main.UserCtrls
{
    partial class PackingStsCtrl
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
            this.lblBelt2DetectResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBelt1DetectResult = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.htTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.BackColor = System.Drawing.Color.Transparent;
            this.htTitlePanel1.Controls.Add(this.lblBelt2DetectResult);
            this.htTitlePanel1.Controls.Add(this.label2);
            this.htTitlePanel1.Controls.Add(this.lblBelt1DetectResult);
            this.htTitlePanel1.Controls.Add(this.label1);
            this.htTitlePanel1.Controls.Add(this.pictureBox1);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(175, 212);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 3;
            this.htTitlePanel1.Text = "束带平台";
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // lblBelt2DetectResult
            // 
            this.lblBelt2DetectResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBelt2DetectResult.BackColor = System.Drawing.Color.LightCyan;
            this.lblBelt2DetectResult.Location = new System.Drawing.Point(65, 45);
            this.lblBelt2DetectResult.Name = "lblBelt2DetectResult";
            this.lblBelt2DetectResult.Size = new System.Drawing.Size(107, 18);
            this.lblBelt2DetectResult.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "束带2结果:";
            // 
            // lblBelt1DetectResult
            // 
            this.lblBelt1DetectResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBelt1DetectResult.BackColor = System.Drawing.Color.LightCyan;
            this.lblBelt1DetectResult.Location = new System.Drawing.Point(65, 24);
            this.lblBelt1DetectResult.Name = "lblBelt1DetectResult";
            this.lblBelt1DetectResult.Size = new System.Drawing.Size(107, 18);
            this.lblBelt1DetectResult.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "束带1结果:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(169, 146);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // PackingStsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PackingStsCtrl";
            this.Size = new System.Drawing.Size(175, 212);
            this.Load += new System.EventHandler(this.LoadDieStnMonitorCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblBelt1DetectResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBelt2DetectResult;
        private System.Windows.Forms.Label label2;
    }
}
