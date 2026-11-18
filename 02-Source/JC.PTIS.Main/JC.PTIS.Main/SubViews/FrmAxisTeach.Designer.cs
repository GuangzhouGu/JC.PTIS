namespace JC.PTIS.Main.SubViews
{
    partial class FrmAxisTeach
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
            this.singleAxisTeach1 = new JC.PTIS.Main.UserCtrls.FuncTeach.SingleAxisTeach();
            this.SuspendLayout();
            // 
            // singleAxisTeach1
            // 
            this.singleAxisTeach1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.singleAxisTeach1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.singleAxisTeach1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.singleAxisTeach1.Location = new System.Drawing.Point(1, 36);
            this.singleAxisTeach1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.singleAxisTeach1.Name = "singleAxisTeach1";
            this.singleAxisTeach1.Size = new System.Drawing.Size(360, 346);
            this.singleAxisTeach1.TabIndex = 1;
            // 
            // FrmAxisTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 383);
            this.CloseFormTimeLag = 1;
            this.Controls.Add(this.singleAxisTeach1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmAxisTeach";
            this.Opacity = 1D;
            this.ShowFormTimelag = 1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmModelDetail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAxisTeach_FormClosing);
            this.Controls.SetChildIndex(this.singleAxisTeach1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private UserCtrls.FuncTeach.SingleAxisTeach singleAxisTeach1;
    }
}