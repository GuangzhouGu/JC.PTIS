
namespace JC.PTIS.Main.SubForms
{
    partial class FrmSizeEdit
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
            this.htTabControl1 = new StdLib.WindowsControls.HTTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.foupSizeEditCtrl1 = new JC.PTIS.Main.UserCtrls.SizeEdit.FoupSizeEditCtrl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.traySizeEditCtrl1 = new JC.PTIS.Main.UserCtrls.SizeEdit.TraySizeEditCtrl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.bufferSizeEditCtrl1 = new JC.PTIS.Main.UserCtrls.SizeEdit.BufferSizeEditCtrl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lotBoxSizeCtrl1 = new JC.PTIS.Main.UserCtrls.SizeEdit.LotBoxSizeCtrl();
            this.htTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // htTabControl1
            // 
            this.htTabControl1.Controls.Add(this.tabPage1);
            this.htTabControl1.Controls.Add(this.tabPage3);
            this.htTabControl1.Controls.Add(this.tabPage4);
            this.htTabControl1.Controls.Add(this.tabPage5);
            this.htTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.htTabControl1.FillColor = System.Drawing.Color.White;
            this.htTabControl1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTabControl1.ItemSize = new System.Drawing.Size(150, 25);
            this.htTabControl1.Location = new System.Drawing.Point(0, 3);
            this.htTabControl1.MenuStyle = StdLib.WindowsControls.HTMenuStyle.Custom;
            this.htTabControl1.Name = "htTabControl1";
            this.htTabControl1.SelectedIndex = 0;
            this.htTabControl1.Size = new System.Drawing.Size(1043, 652);
            this.htTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.htTabControl1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTabControl1.TabBackColor = System.Drawing.Color.LightCyan;
            this.htTabControl1.TabIndex = 1;
            this.htTabControl1.TabSelectedColor = System.Drawing.Color.SkyBlue;
            this.htTabControl1.TabSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControl1.TabSelectedHighColor = System.Drawing.Color.SkyBlue;
            this.htTabControl1.TabUnSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControl1.SelectedIndexChanged += new System.EventHandler(this.htTabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.foupSizeEditCtrl1);
            this.tabPage1.Location = new System.Drawing.Point(0, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1043, 627);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Foup尺寸参数";
            // 
            // foupSizeEditCtrl1
            // 
            this.foupSizeEditCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.foupSizeEditCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.foupSizeEditCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.foupSizeEditCtrl1.Location = new System.Drawing.Point(0, 0);
            this.foupSizeEditCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.foupSizeEditCtrl1.Name = "foupSizeEditCtrl1";
            this.foupSizeEditCtrl1.Size = new System.Drawing.Size(1043, 627);
            this.foupSizeEditCtrl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.traySizeEditCtrl1);
            this.tabPage3.Location = new System.Drawing.Point(0, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1043, 627);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tray尺寸参数";
            // 
            // traySizeEditCtrl1
            // 
            this.traySizeEditCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.traySizeEditCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traySizeEditCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.traySizeEditCtrl1.Location = new System.Drawing.Point(0, 0);
            this.traySizeEditCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.traySizeEditCtrl1.Name = "traySizeEditCtrl1";
            this.traySizeEditCtrl1.Size = new System.Drawing.Size(1043, 627);
            this.traySizeEditCtrl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.bufferSizeEditCtrl1);
            this.tabPage4.Location = new System.Drawing.Point(0, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1043, 627);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "半Tray缓存尺寸参数";
            // 
            // bufferSizeEditCtrl1
            // 
            this.bufferSizeEditCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.bufferSizeEditCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bufferSizeEditCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bufferSizeEditCtrl1.Location = new System.Drawing.Point(0, 0);
            this.bufferSizeEditCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.bufferSizeEditCtrl1.Name = "bufferSizeEditCtrl1";
            this.bufferSizeEditCtrl1.Size = new System.Drawing.Size(1043, 627);
            this.bufferSizeEditCtrl1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Controls.Add(this.lotBoxSizeCtrl1);
            this.tabPage5.Location = new System.Drawing.Point(0, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1043, 627);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "LotBox尺寸参数";
            // 
            // lotBoxSizeCtrl1
            // 
            this.lotBoxSizeCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.lotBoxSizeCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotBoxSizeCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxSizeCtrl1.Location = new System.Drawing.Point(0, 0);
            this.lotBoxSizeCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.lotBoxSizeCtrl1.Name = "lotBoxSizeCtrl1";
            this.lotBoxSizeCtrl1.Size = new System.Drawing.Size(1043, 627);
            this.lotBoxSizeCtrl1.TabIndex = 0;
            // 
            // FrmSizeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 655);
            this.Controls.Add(this.htTabControl1);
            this.Name = "FrmSizeEdit";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Text = "FrmMagSize";
            this.Load += new System.EventHandler(this.FrmSizeEdit_Load);
            this.htTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTTabControl htTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private UserCtrls.SizeEdit.TraySizeEditCtrl traySizeEditCtrl1;
        private System.Windows.Forms.TabPage tabPage5;
        private UserCtrls.SizeEdit.LotBoxSizeCtrl lotBoxSizeCtrl1;
        private UserCtrls.SizeEdit.FoupSizeEditCtrl foupSizeEditCtrl1;
        private UserCtrls.SizeEdit.BufferSizeEditCtrl bufferSizeEditCtrl1;
    }
}