namespace JC.PTIS.Main.SubViews
{
    partial class Frm4_DataMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.registerSignalMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.RegisterSignalMonitorCtrl();
            this.htTabControlMenu1 = new StdLib.WindowsControls.HTTabControlMenu();
            this.InterfaceSignal = new System.Windows.Forms.TabPage();
            this.StepTime = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.redisKeyValueMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.RedisKeyValueMonitorCtrl();
            this.htTabControlMenu1.SuspendLayout();
            this.InterfaceSignal.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // registerSignalMonitorCtrl1
            // 
            this.registerSignalMonitorCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerSignalMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.registerSignalMonitorCtrl1.Location = new System.Drawing.Point(0, 0);
            this.registerSignalMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.registerSignalMonitorCtrl1.Name = "registerSignalMonitorCtrl1";
            this.registerSignalMonitorCtrl1.Size = new System.Drawing.Size(649, 450);
            this.registerSignalMonitorCtrl1.TabIndex = 1;
            // 
            // htTabControlMenu1
            // 
            this.htTabControlMenu1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.htTabControlMenu1.Controls.Add(this.InterfaceSignal);
            this.htTabControlMenu1.Controls.Add(this.StepTime);
            this.htTabControlMenu1.Controls.Add(this.tabPage1);
            this.htTabControlMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTabControlMenu1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.htTabControlMenu1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTabControlMenu1.ItemSize = new System.Drawing.Size(30, 150);
            this.htTabControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.htTabControlMenu1.MenuStyle = StdLib.WindowsControls.HTMenuStyle.Custom;
            this.htTabControlMenu1.Multiline = true;
            this.htTabControlMenu1.Name = "htTabControlMenu1";
            this.htTabControlMenu1.SelectedIndex = 0;
            this.htTabControlMenu1.Size = new System.Drawing.Size(800, 450);
            this.htTabControlMenu1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.htTabControlMenu1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTabControlMenu1.TabBackColor = System.Drawing.Color.Azure;
            this.htTabControlMenu1.TabIndex = 3;
            this.htTabControlMenu1.TabSelectedColor = System.Drawing.Color.SkyBlue;
            this.htTabControlMenu1.TabSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControlMenu1.TabSelectedHighColor = System.Drawing.Color.SkyBlue;
            this.htTabControlMenu1.TabUnSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControlMenu1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // InterfaceSignal
            // 
            this.InterfaceSignal.Controls.Add(this.registerSignalMonitorCtrl1);
            this.InterfaceSignal.Location = new System.Drawing.Point(151, 0);
            this.InterfaceSignal.Name = "InterfaceSignal";
            this.InterfaceSignal.Size = new System.Drawing.Size(649, 450);
            this.InterfaceSignal.TabIndex = 10;
            this.InterfaceSignal.Text = "交互信号";
            this.InterfaceSignal.UseVisualStyleBackColor = true;
            // 
            // StepTime
            // 
            this.StepTime.Location = new System.Drawing.Point(151, 0);
            this.StepTime.Name = "StepTime";
            this.StepTime.Size = new System.Drawing.Size(649, 450);
            this.StepTime.TabIndex = 2;
            this.StepTime.Text = "耗时监控";
            this.StepTime.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.redisKeyValueMonitorCtrl1);
            this.tabPage1.Location = new System.Drawing.Point(151, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(649, 450);
            this.tabPage1.TabIndex = 11;
            this.tabPage1.Text = "Redis监控";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // redisKeyValueMonitorCtrl1
            // 
            this.redisKeyValueMonitorCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.redisKeyValueMonitorCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redisKeyValueMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.redisKeyValueMonitorCtrl1.Location = new System.Drawing.Point(0, 0);
            this.redisKeyValueMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.redisKeyValueMonitorCtrl1.Name = "redisKeyValueMonitorCtrl1";
            this.redisKeyValueMonitorCtrl1.Size = new System.Drawing.Size(649, 450);
            this.redisKeyValueMonitorCtrl1.TabIndex = 0;
            // 
            // Frm4_DataMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.htTabControlMenu1);
            this.Name = "Frm4_DataMonitor";
            this.Text = "FrmDataQuery";
            this.Load += new System.EventHandler(this.Frm4_DataMonitor_Load);
            this.Controls.SetChildIndex(this.htTabControlMenu1, 0);
            this.htTabControlMenu1.ResumeLayout(false);
            this.InterfaceSignal.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserCtrls.RegisterSignalMonitorCtrl registerSignalMonitorCtrl1;
        private StdLib.WindowsControls.HTTabControlMenu htTabControlMenu1;
        private System.Windows.Forms.TabPage InterfaceSignal;
        private System.Windows.Forms.TabPage StepTime;
        private System.Windows.Forms.TabPage tabPage1;
        private UserCtrls.RedisKeyValueMonitorCtrl redisKeyValueMonitorCtrl1;
    }
}