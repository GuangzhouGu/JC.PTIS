
namespace JC.AET.SubForms
{
    partial class FrmCodeFailConfirm
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
            this.btnRetry = new StdLib.WindowsControls.HTButton();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.htLabel2 = new StdLib.WindowsControls.HTLabel();
            this.lblRfid = new StdLib.WindowsControls.HTLabel();
            this.htLabel3 = new StdLib.WindowsControls.HTLabel();
            this.btnKeyin = new StdLib.WindowsControls.HTButton();
            this.tbxKeyinID = new StdLib.WindowsControls.HTTextBox();
            this.SuspendLayout();
            // 
            // btnRetry
            // 
            this.btnRetry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetry.FillColor2 = System.Drawing.Color.Empty;
            this.btnRetry.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRetry.Location = new System.Drawing.Point(55, 222);
            this.btnRetry.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRetry.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(146, 38);
            this.btnRetry.TabIndex = 0;
            this.btnRetry.Text = "重新读取";
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel1.ForeColor = System.Drawing.Color.Black;
            this.htLabel1.Location = new System.Drawing.Point(74, 59);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(307, 38);
            this.htLabel1.TabIndex = 1;
            this.htLabel1.Text = "条码读取失败，请确认";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel2
            // 
            this.htLabel2.AutoSize = true;
            this.htLabel2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel2.Location = new System.Drawing.Point(50, 127);
            this.htLabel2.Name = "htLabel2";
            this.htLabel2.Size = new System.Drawing.Size(59, 28);
            this.htLabel2.TabIndex = 1;
            this.htLabel2.Text = "条码:";
            this.htLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRfid
            // 
            this.lblRfid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRfid.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRfid.Location = new System.Drawing.Point(117, 122);
            this.lblRfid.Name = "lblRfid";
            this.lblRfid.Size = new System.Drawing.Size(271, 38);
            this.lblRfid.TabIndex = 1;
            this.lblRfid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel3
            // 
            this.htLabel3.AutoSize = true;
            this.htLabel3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel3.Location = new System.Drawing.Point(10, 174);
            this.htLabel3.Name = "htLabel3";
            this.htLabel3.Size = new System.Drawing.Size(101, 28);
            this.htLabel3.TabIndex = 1;
            this.htLabel3.Text = "手输条码:";
            this.htLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnKeyin
            // 
            this.btnKeyin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyin.FillColor2 = System.Drawing.Color.Empty;
            this.btnKeyin.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnKeyin.Location = new System.Drawing.Point(257, 222);
            this.btnKeyin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnKeyin.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnKeyin.Name = "btnKeyin";
            this.btnKeyin.Size = new System.Drawing.Size(146, 38);
            this.btnKeyin.TabIndex = 0;
            this.btnKeyin.Text = "手动输入";
            this.btnKeyin.Click += new System.EventHandler(this.btnKeyin_Click);
            // 
            // tbxKeyinID
            // 
            this.tbxKeyinID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxKeyinID.FillColor = System.Drawing.Color.White;
            this.tbxKeyinID.Font = new System.Drawing.Font("微软雅黑", 15.75F);
            this.tbxKeyinID.Location = new System.Drawing.Point(117, 174);
            this.tbxKeyinID.Maximum = 2147483647D;
            this.tbxKeyinID.Minimum = -2147483648D;
            this.tbxKeyinID.Name = "tbxKeyinID";
            this.tbxKeyinID.Padding = new System.Windows.Forms.Padding(5);
            this.tbxKeyinID.Size = new System.Drawing.Size(271, 35);
            this.tbxKeyinID.TabIndex = 2;
            // 
            // FrmCodeFailConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 275);
            this.CloseFormTimeLag = 1;
            this.Controls.Add(this.tbxKeyinID);
            this.Controls.Add(this.htLabel3);
            this.Controls.Add(this.lblRfid);
            this.Controls.Add(this.htLabel2);
            this.Controls.Add(this.htLabel1);
            this.Controls.Add(this.btnKeyin);
            this.Controls.Add(this.btnRetry);
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "FrmCodeFailConfirm";
            this.Opacity = 1D;
            this.ShowFormTimelag = 1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "对位失败确认";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAlignFailConfirm_FormClosing);
            this.Load += new System.EventHandler(this.FrmCodeFailConfirm_Load);
            this.Controls.SetChildIndex(this.btnRetry, 0);
            this.Controls.SetChildIndex(this.btnKeyin, 0);
            this.Controls.SetChildIndex(this.htLabel1, 0);
            this.Controls.SetChildIndex(this.htLabel2, 0);
            this.Controls.SetChildIndex(this.lblRfid, 0);
            this.Controls.SetChildIndex(this.htLabel3, 0);
            this.Controls.SetChildIndex(this.tbxKeyinID, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StdLib.WindowsControls.HTButton btnRetry;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.WindowsControls.HTLabel htLabel2;
        private StdLib.WindowsControls.HTLabel lblRfid;
        private StdLib.WindowsControls.HTLabel htLabel3;
        private StdLib.WindowsControls.HTButton btnKeyin;
        private StdLib.WindowsControls.HTTextBox tbxKeyinID;
    }
}