using JC.PTIS.Main.Properties;
using System.Windows.Forms;

namespace JC.B17AutoMating.SubForms
{
    partial class FrmTips
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "FrmTips";

            this.lblMsg = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.pctStat = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.pctStat).BeginInit();
            base.SuspendLayout();
            this.lblMsg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.Font = new System.Drawing.Font("微软雅黑", 12f);
            this.lblMsg.Location = new System.Drawing.Point(32, 3);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(279, 39);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "提示信息";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Image =Resources.CloseRed;
            this.btnClose.Location = new System.Drawing.Point(316, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 25);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(btnClose_MouseDown);
            this.pctStat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pctStat.BackColor = System.Drawing.Color.Transparent;
            this.pctStat.Image = Resources.warning;
            this.pctStat.Location = new System.Drawing.Point(6, 14);
            this.pctStat.Name = "pctStat";
            this.pctStat.Size = new System.Drawing.Size(20, 20);
            this.pctStat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctStat.TabIndex = 0;
            this.pctStat.TabStop = false;
            this.timer1.Tick += new System.EventHandler(timer1_Tick);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            base.ClientSize = new System.Drawing.Size(344, 47);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.lblMsg);
            base.Controls.Add(this.pctStat);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            base.Name = "FrmTips";
            base.Padding = new System.Windows.Forms.Padding(0, 0, 0, -20);
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmTips";
            base.TopMost = true;
            base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmTips_FormClosing);
            base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FrmTips_FormClosed);
            base.Load += new System.EventHandler(FrmTips_Load);
            ((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.pctStat).EndInit();
            base.ResumeLayout(false);
        }


        private PictureBox pctStat;

        private Label lblMsg;

        private PictureBox btnClose;

        private Timer timer1;

        #endregion
    }
}