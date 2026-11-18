namespace JC.PTIS.Main.UserCtrls
{
    partial class LotMonitorCtrl
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
            this.lotTrayCtrl1 = new JC.PTIS.Main.UserCtrls.LotTrayCtrl();
            this.htTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.BackColor = System.Drawing.Color.Transparent;
            this.htTitlePanel1.Controls.Add(this.lotTrayCtrl1);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(212, 192);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 2;
            this.htTitlePanel1.Text = "Lot物流显示";
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // lotTrayCtrl1
            // 
            this.lotTrayCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.lotTrayCtrl1.DDStnNo = null;
            this.lotTrayCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotTrayCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotTrayCtrl1.Location = new System.Drawing.Point(0, 22);
            this.lotTrayCtrl1.LotPos = JC.PTIS.Main.LotPosEnum.PackingStage;
            this.lotTrayCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.lotTrayCtrl1.Name = "lotTrayCtrl1";
            this.lotTrayCtrl1.Size = new System.Drawing.Size(212, 170);
            this.lotTrayCtrl1.TabIndex = 0;
            // 
            // LotMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LotMonitorCtrl";
            this.Size = new System.Drawing.Size(212, 192);
            this.Load += new System.EventHandler(this.LoadDieStnMonitorCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private LotTrayCtrl lotTrayCtrl1;
    }
}
