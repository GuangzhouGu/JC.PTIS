namespace JC.PTIS.Main.UserCtrls
{
    partial class Stn4TurnTableMonitorCtrl
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
            this.lblRealPos = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trayCtrl4 = new JC.PTIS.Main.UserCtrls.TrayCtrl();
            this.trayCtrl3 = new JC.PTIS.Main.UserCtrls.TrayCtrl();
            this.trayCtrl2 = new JC.PTIS.Main.UserCtrls.TrayCtrl();
            this.trayCtrl1 = new JC.PTIS.Main.UserCtrls.TrayCtrl();
            this.circleCtrl1 = new JC.PTIS.Main.UserCtrls.TurnTableCtrl();
            this.htTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.BackColor = System.Drawing.Color.Transparent;
            this.htTitlePanel1.Controls.Add(this.lblRealPos);
            this.htTitlePanel1.Controls.Add(this.label3);
            this.htTitlePanel1.Controls.Add(this.trayCtrl4);
            this.htTitlePanel1.Controls.Add(this.trayCtrl3);
            this.htTitlePanel1.Controls.Add(this.trayCtrl2);
            this.htTitlePanel1.Controls.Add(this.trayCtrl1);
            this.htTitlePanel1.Controls.Add(this.circleCtrl1);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(1, 1);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(399, 400);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 0;
            this.htTitlePanel1.Text = "4工位转塔物流显示";
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // lblRealPos
            // 
            this.lblRealPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRealPos.BackColor = System.Drawing.Color.LightCyan;
            this.lblRealPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRealPos.Location = new System.Drawing.Point(357, 24);
            this.lblRealPos.Name = "lblRealPos";
            this.lblRealPos.Size = new System.Drawing.Size(41, 17);
            this.lblRealPos.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(326, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "位置:";
            // 
            // trayCtrl4
            // 
            this.trayCtrl4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.trayCtrl4.BackColor = System.Drawing.Color.LightBlue;
            this.trayCtrl4.DDStnNo = 3;
            this.trayCtrl4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayCtrl4.Location = new System.Drawing.Point(9, 157);
            this.trayCtrl4.Margin = new System.Windows.Forms.Padding(0);
            this.trayCtrl4.Name = "trayCtrl4";
            this.trayCtrl4.Size = new System.Drawing.Size(180, 112);
            this.trayCtrl4.TabIndex = 6;
            this.trayCtrl4.TrayPos = JC.PTIS.Main.TrayPosEnum.Stn4TurnTable;
            // 
            // trayCtrl3
            // 
            this.trayCtrl3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.trayCtrl3.BackColor = System.Drawing.Color.LightBlue;
            this.trayCtrl3.DDStnNo = 2;
            this.trayCtrl3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayCtrl3.Location = new System.Drawing.Point(109, 269);
            this.trayCtrl3.Margin = new System.Windows.Forms.Padding(0);
            this.trayCtrl3.Name = "trayCtrl3";
            this.trayCtrl3.Size = new System.Drawing.Size(180, 112);
            this.trayCtrl3.TabIndex = 6;
            this.trayCtrl3.TrayPos = JC.PTIS.Main.TrayPosEnum.Stn4TurnTable;
            // 
            // trayCtrl2
            // 
            this.trayCtrl2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trayCtrl2.BackColor = System.Drawing.Color.LightBlue;
            this.trayCtrl2.DDStnNo = 1;
            this.trayCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayCtrl2.Location = new System.Drawing.Point(207, 157);
            this.trayCtrl2.Margin = new System.Windows.Forms.Padding(0);
            this.trayCtrl2.Name = "trayCtrl2";
            this.trayCtrl2.Size = new System.Drawing.Size(180, 112);
            this.trayCtrl2.TabIndex = 6;
            this.trayCtrl2.TrayPos = JC.PTIS.Main.TrayPosEnum.Stn4TurnTable;
            // 
            // trayCtrl1
            // 
            this.trayCtrl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.trayCtrl1.BackColor = System.Drawing.Color.LightBlue;
            this.trayCtrl1.DDStnNo = 0;
            this.trayCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayCtrl1.Location = new System.Drawing.Point(109, 45);
            this.trayCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.trayCtrl1.Name = "trayCtrl1";
            this.trayCtrl1.Size = new System.Drawing.Size(180, 112);
            this.trayCtrl1.TabIndex = 6;
            this.trayCtrl1.TrayPos = JC.PTIS.Main.TrayPosEnum.Stn4TurnTable;
            // 
            // circleCtrl1
            // 
            this.circleCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.circleCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circleCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.circleCtrl1.Location = new System.Drawing.Point(0, 22);
            this.circleCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.circleCtrl1.Name = "circleCtrl1";
            this.circleCtrl1.Size = new System.Drawing.Size(399, 378);
            this.circleCtrl1.StnCount = 4;
            this.circleCtrl1.TabIndex = 3;
            // 
            // Stn4TurnTableMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Stn4TurnTableMonitorCtrl";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(401, 402);
            this.Load += new System.EventHandler(this.LoadDieStnMonitorCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.htTitlePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
        private TurnTableCtrl circleCtrl1;
        private TrayCtrl trayCtrl4;
        private TrayCtrl trayCtrl3;
        private TrayCtrl trayCtrl2;
        private TrayCtrl trayCtrl1;
        private System.Windows.Forms.Label lblRealPos;
        private System.Windows.Forms.Label label3;
    }
}
