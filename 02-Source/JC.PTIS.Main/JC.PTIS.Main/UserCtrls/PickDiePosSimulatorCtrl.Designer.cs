namespace JC.PTIS.Main.UserCtrls
{
    partial class PickDiePosSimulatorCtrl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCalcPickPos = new StdLib.WindowsControls.HTButton();
            this.tbxBaseDieColNo = new StdLib.WindowsControls.HTTextBox();
            this.htLabel5 = new StdLib.WindowsControls.HTLabel();
            this.tbxBaseDieRowNo = new StdLib.WindowsControls.HTTextBox();
            this.htLabel2 = new StdLib.WindowsControls.HTLabel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 443);
            this.panel1.TabIndex = 51;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnCalcPickPos
            // 
            this.btnCalcPickPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalcPickPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalcPickPos.FillColor = System.Drawing.Color.White;
            this.btnCalcPickPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnCalcPickPos.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalcPickPos.ForeColor = System.Drawing.Color.Black;
            this.btnCalcPickPos.Location = new System.Drawing.Point(411, 469);
            this.btnCalcPickPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCalcPickPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCalcPickPos.Name = "btnCalcPickPos";
            this.btnCalcPickPos.Size = new System.Drawing.Size(105, 30);
            this.btnCalcPickPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnCalcPickPos.TabIndex = 50;
            this.btnCalcPickPos.Text = "显示取Die点位";
            this.btnCalcPickPos.Click += new System.EventHandler(this.btnCalcPickPos_Click);
            // 
            // tbxBaseDieColNo
            // 
            this.tbxBaseDieColNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbxBaseDieColNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxBaseDieColNo.FillColor = System.Drawing.Color.White;
            this.tbxBaseDieColNo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxBaseDieColNo.Location = new System.Drawing.Point(85, 479);
            this.tbxBaseDieColNo.Maximum = 2147483647D;
            this.tbxBaseDieColNo.Minimum = -2147483648D;
            this.tbxBaseDieColNo.Name = "tbxBaseDieColNo";
            this.tbxBaseDieColNo.Padding = new System.Windows.Forms.Padding(5);
            this.tbxBaseDieColNo.Size = new System.Drawing.Size(179, 23);
            this.tbxBaseDieColNo.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxBaseDieColNo.TabIndex = 56;
            this.tbxBaseDieColNo.Text = "0";
            // 
            // htLabel5
            // 
            this.htLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.htLabel5.AutoSize = true;
            this.htLabel5.BackColor = System.Drawing.Color.Transparent;
            this.htLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel5.Location = new System.Drawing.Point(2, 481);
            this.htLabel5.Name = "htLabel5";
            this.htLabel5.Size = new System.Drawing.Size(78, 17);
            this.htLabel5.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel5.TabIndex = 54;
            this.htLabel5.Text = "基准Die列号:";
            this.htLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxBaseDieRowNo
            // 
            this.tbxBaseDieRowNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbxBaseDieRowNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxBaseDieRowNo.DoubleValue = 3D;
            this.tbxBaseDieRowNo.FillColor = System.Drawing.Color.White;
            this.tbxBaseDieRowNo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxBaseDieRowNo.IntValue = 3;
            this.tbxBaseDieRowNo.Location = new System.Drawing.Point(85, 452);
            this.tbxBaseDieRowNo.Maximum = 2147483647D;
            this.tbxBaseDieRowNo.Minimum = -2147483648D;
            this.tbxBaseDieRowNo.Name = "tbxBaseDieRowNo";
            this.tbxBaseDieRowNo.Padding = new System.Windows.Forms.Padding(5);
            this.tbxBaseDieRowNo.Size = new System.Drawing.Size(179, 23);
            this.tbxBaseDieRowNo.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxBaseDieRowNo.TabIndex = 57;
            this.tbxBaseDieRowNo.Text = "3";
            // 
            // htLabel2
            // 
            this.htLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.htLabel2.AutoSize = true;
            this.htLabel2.BackColor = System.Drawing.Color.Transparent;
            this.htLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel2.Location = new System.Drawing.Point(2, 454);
            this.htLabel2.Name = "htLabel2";
            this.htLabel2.Size = new System.Drawing.Size(78, 17);
            this.htLabel2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel2.TabIndex = 55;
            this.htLabel2.Text = "基准Die行号:";
            this.htLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PickDiePosSimulatorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.tbxBaseDieColNo);
            this.Controls.Add(this.htLabel5);
            this.Controls.Add(this.tbxBaseDieRowNo);
            this.Controls.Add(this.htLabel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCalcPickPos);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PickDiePosSimulatorCtrl";
            this.Size = new System.Drawing.Size(519, 503);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private StdLib.WindowsControls.HTButton btnCalcPickPos;
        private StdLib.WindowsControls.HTTextBox tbxBaseDieColNo;
        private StdLib.WindowsControls.HTLabel htLabel5;
        private StdLib.WindowsControls.HTTextBox tbxBaseDieRowNo;
        private StdLib.WindowsControls.HTLabel htLabel2;
    }
}
