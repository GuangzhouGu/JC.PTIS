
namespace HYC.SaturnMain.SubForms
{
    partial class FrmPM
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new StdLib.WindowsControls.HTButton();
            this.safedoorCtrl1 = new StdLib.PCBaseFrame.UserCtrl.SafedoorCtrl();
            this.safedoorCtrl2 = new StdLib.PCBaseFrame.UserCtrl.SafedoorCtrl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Tomato;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 200.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Honeydew;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1920, 1080);
            this.label1.TabIndex = 0;
            this.label1.Text = "PM";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLogin
            // 
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FillColor = System.Drawing.Color.White;
            this.btnLogin.FillColor2 = System.Drawing.Color.Empty;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnLogin.ForeColor = System.Drawing.Color.Blue;
            this.btnLogin.Location = new System.Drawing.Point(1788, 11);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogin.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.RectColor = System.Drawing.Color.White;
            this.btnLogin.Size = new System.Drawing.Size(120, 43);
            this.btnLogin.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "退出PM";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // safedoorCtrl1
            // 
            this.safedoorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.safedoorCtrl1.Location = new System.Drawing.Point(3, 966);
            this.safedoorCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.safedoorCtrl1.Name = "safedoorCtrl1";
            this.safedoorCtrl1.SafeDoorInputName = "SafeDoor1";
            this.safedoorCtrl1.SafeDoorName = "操作侧安全门";
            this.safedoorCtrl1.Size = new System.Drawing.Size(101, 111);
            this.safedoorCtrl1.TabIndex = 2;
            // 
            // safedoorCtrl2
            // 
            this.safedoorCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.safedoorCtrl2.Location = new System.Drawing.Point(110, 966);
            this.safedoorCtrl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.safedoorCtrl2.Name = "safedoorCtrl2";
            this.safedoorCtrl2.SafeDoorInputName = "SafeDoor2";
            this.safedoorCtrl2.SafeDoorName = "非操作侧安全门";
            this.safedoorCtrl2.Size = new System.Drawing.Size(101, 111);
            this.safedoorCtrl2.TabIndex = 3;
            // 
            // FrmPM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.safedoorCtrl1);
            this.Controls.Add(this.safedoorCtrl2);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmPM";
            this.Text = "FrmPM";
            this.Load += new System.EventHandler(this.FrmPM_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private StdLib.WindowsControls.HTButton btnLogin;
        private StdLib.PCBaseFrame.UserCtrl.SafedoorCtrl safedoorCtrl1;
        private StdLib.PCBaseFrame.UserCtrl.SafedoorCtrl safedoorCtrl2;
    }
}