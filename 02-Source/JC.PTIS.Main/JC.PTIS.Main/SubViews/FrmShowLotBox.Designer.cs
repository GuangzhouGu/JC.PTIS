namespace JC.PTIS.Main.SubViews
{
    partial class FrmShowLotBox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.htComboBox1 = new StdLib.WindowsControls.HTComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(4, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 283);
            this.panel1.TabIndex = 1;
            // 
            // htComboBox1
            // 
            this.htComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htComboBox1.FillColor = System.Drawing.Color.White;
            this.htComboBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htComboBox1.Location = new System.Drawing.Point(78, 42);
            this.htComboBox1.MinimumSize = new System.Drawing.Size(63, 0);
            this.htComboBox1.Name = "htComboBox1";
            this.htComboBox1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.htComboBox1.Size = new System.Drawing.Size(241, 29);
            this.htComboBox1.TabIndex = 2;
            this.htComboBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.htComboBox1.SelectedIndexChanged += new System.EventHandler(this.htComboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "LotCode:";
            // 
            // FrmShowLotBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 365);
            this.CloseFormTimeLag = 1;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.htComboBox1);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FrmShowLotBox";
            this.Opacity = 1D;
            this.ShowFormTimelag = 1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmModelDetail";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.htComboBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private StdLib.WindowsControls.HTComboBox htComboBox1;
        private System.Windows.Forms.Label label1;
    }
}