namespace JC.PTIS.Main.SubViews
{
    partial class Frm3_DataQuery
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
            this.yieldQueryCtrl1 = new JC.AutoMating.UserCtrl.YieldQueryCtrl();
            this.htTabControlMenu1 = new StdLib.WindowsControls.HTTabControlMenu();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.yieldDayNight1 = new JC.MVI.UserCtrl.YieldCtrl.YieldDayNight();
            this.htTabControlMenu1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // yieldQueryCtrl1
            // 
            this.yieldQueryCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yieldQueryCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.yieldQueryCtrl1.Location = new System.Drawing.Point(0, 0);
            this.yieldQueryCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.yieldQueryCtrl1.Name = "yieldQueryCtrl1";
            this.yieldQueryCtrl1.Size = new System.Drawing.Size(906, 676);
            this.yieldQueryCtrl1.TabIndex = 1;
            // 
            // htTabControlMenu1
            // 
            this.htTabControlMenu1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.htTabControlMenu1.Controls.Add(this.tabPage1);
            this.htTabControlMenu1.Controls.Add(this.tabPage2);
            this.htTabControlMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTabControlMenu1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.htTabControlMenu1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTabControlMenu1.ItemSize = new System.Drawing.Size(28, 150);
            this.htTabControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.htTabControlMenu1.MenuStyle = StdLib.WindowsControls.HTMenuStyle.Custom;
            this.htTabControlMenu1.Multiline = true;
            this.htTabControlMenu1.Name = "htTabControlMenu1";
            this.htTabControlMenu1.SelectedIndex = 0;
            this.htTabControlMenu1.Size = new System.Drawing.Size(1057, 676);
            this.htTabControlMenu1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.htTabControlMenu1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTabControlMenu1.TabBackColor = System.Drawing.Color.AliceBlue;
            this.htTabControlMenu1.TabIndex = 2;
            this.htTabControlMenu1.TabSelectedColor = System.Drawing.Color.PowderBlue;
            this.htTabControlMenu1.TabSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControlMenu1.TabUnSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControlMenu1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.yieldDayNight1);
            this.tabPage1.Location = new System.Drawing.Point(151, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(906, 676);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "产能";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.yieldQueryCtrl1);
            this.tabPage2.Location = new System.Drawing.Point(151, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(906, 676);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "生产履历";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // yieldDayNight1
            // 
            this.yieldDayNight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yieldDayNight1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.yieldDayNight1.Location = new System.Drawing.Point(0, 0);
            this.yieldDayNight1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.yieldDayNight1.Name = "yieldDayNight1";
            this.yieldDayNight1.Size = new System.Drawing.Size(906, 676);
            this.yieldDayNight1.TabIndex = 0;
            // 
            // Frm3_DataQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 676);
            this.Controls.Add(this.htTabControlMenu1);
            this.Name = "Frm3_DataQuery";
            this.Text = "FrmDataQuery";
            this.Controls.SetChildIndex(this.htTabControlMenu1, 0);
            this.htTabControlMenu1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoMating.UserCtrl.YieldQueryCtrl yieldQueryCtrl1;
        private StdLib.WindowsControls.HTTabControlMenu htTabControlMenu1;
        private System.Windows.Forms.TabPage tabPage1;
        private MVI.UserCtrl.YieldCtrl.YieldDayNight yieldDayNight1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}