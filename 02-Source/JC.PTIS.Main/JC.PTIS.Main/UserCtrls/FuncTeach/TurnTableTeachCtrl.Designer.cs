namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class TurnTableTeachCtrl
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
            this.htGroupBox1 = new StdLib.WindowsControls.HTGroupBox();
            this.cmxNozzleList = new StdLib.WindowsControls.HTComboBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.htButton4 = new StdLib.WindowsControls.HTButton();
            this.btn = new StdLib.WindowsControls.HTButton();
            this.btnIRDetectStn = new StdLib.WindowsControls.HTButton();
            this.btnBackDetectStn = new StdLib.WindowsControls.HTButton();
            this.btnPickDieStn = new StdLib.WindowsControls.HTButton();
            this.htGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox1.Controls.Add(this.cmxNozzleList);
            this.htGroupBox1.Controls.Add(this.htLabel4);
            this.htGroupBox1.Controls.Add(this.htButton4);
            this.htGroupBox1.Controls.Add(this.btn);
            this.htGroupBox1.Controls.Add(this.btnIRDetectStn);
            this.htGroupBox1.Controls.Add(this.btnBackDetectStn);
            this.htGroupBox1.Controls.Add(this.btnPickDieStn);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox1.Size = new System.Drawing.Size(461, 319);
            this.htGroupBox1.TabIndex = 1;
            this.htGroupBox1.Text = "上料转塔调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // cmxNozzleList
            // 
            this.cmxNozzleList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxNozzleList.FillColor = System.Drawing.Color.White;
            this.cmxNozzleList.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxNozzleList.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmxNozzleList.Location = new System.Drawing.Point(61, 21);
            this.cmxNozzleList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxNozzleList.Name = "cmxNozzleList";
            this.cmxNozzleList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxNozzleList.Size = new System.Drawing.Size(92, 29);
            this.cmxNozzleList.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.cmxNozzleList.TabIndex = 52;
            this.cmxNozzleList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel4.Location = new System.Drawing.Point(6, 24);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(54, 20);
            this.htLabel4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel4.TabIndex = 51;
            this.htLabel4.Text = "吸嘴号:";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htButton4
            // 
            this.htButton4.BackColor = System.Drawing.Color.White;
            this.htButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.htButton4.FillColor = System.Drawing.Color.White;
            this.htButton4.FillColor2 = System.Drawing.Color.Empty;
            this.htButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htButton4.ForeColor = System.Drawing.Color.Black;
            this.htButton4.Location = new System.Drawing.Point(164, 158);
            this.htButton4.MinimumSize = new System.Drawing.Size(1, 1);
            this.htButton4.Name = "htButton4";
            this.htButton4.Size = new System.Drawing.Size(105, 28);
            this.htButton4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htButton4.TabIndex = 50;
            this.htButton4.Text = "抛料位置";
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.Color.White;
            this.btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn.FillColor = System.Drawing.Color.White;
            this.btn.FillColor2 = System.Drawing.Color.Empty;
            this.btn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn.ForeColor = System.Drawing.Color.Black;
            this.btn.Location = new System.Drawing.Point(164, 124);
            this.btn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(105, 28);
            this.btn.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btn.TabIndex = 50;
            this.btn.Text = "放Die位置";
            // 
            // btnIRDetectStn
            // 
            this.btnIRDetectStn.BackColor = System.Drawing.Color.White;
            this.btnIRDetectStn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIRDetectStn.FillColor = System.Drawing.Color.White;
            this.btnIRDetectStn.FillColor2 = System.Drawing.Color.Empty;
            this.btnIRDetectStn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIRDetectStn.ForeColor = System.Drawing.Color.Black;
            this.btnIRDetectStn.Location = new System.Drawing.Point(164, 90);
            this.btnIRDetectStn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnIRDetectStn.Name = "btnIRDetectStn";
            this.btnIRDetectStn.Size = new System.Drawing.Size(105, 28);
            this.btnIRDetectStn.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnIRDetectStn.TabIndex = 50;
            this.btnIRDetectStn.Text = "隐裂检位置";
            // 
            // btnBackDetectStn
            // 
            this.btnBackDetectStn.BackColor = System.Drawing.Color.White;
            this.btnBackDetectStn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackDetectStn.FillColor = System.Drawing.Color.White;
            this.btnBackDetectStn.FillColor2 = System.Drawing.Color.Empty;
            this.btnBackDetectStn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackDetectStn.ForeColor = System.Drawing.Color.Black;
            this.btnBackDetectStn.Location = new System.Drawing.Point(164, 56);
            this.btnBackDetectStn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnBackDetectStn.Name = "btnBackDetectStn";
            this.btnBackDetectStn.Size = new System.Drawing.Size(105, 28);
            this.btnBackDetectStn.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnBackDetectStn.TabIndex = 50;
            this.btnBackDetectStn.Text = "背面检位置";
            // 
            // btnPickDieStn
            // 
            this.btnPickDieStn.BackColor = System.Drawing.Color.White;
            this.btnPickDieStn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickDieStn.FillColor = System.Drawing.Color.White;
            this.btnPickDieStn.FillColor2 = System.Drawing.Color.Empty;
            this.btnPickDieStn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPickDieStn.ForeColor = System.Drawing.Color.Black;
            this.btnPickDieStn.Location = new System.Drawing.Point(164, 22);
            this.btnPickDieStn.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPickDieStn.Name = "btnPickDieStn";
            this.btnPickDieStn.Size = new System.Drawing.Size(105, 28);
            this.btnPickDieStn.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPickDieStn.TabIndex = 50;
            this.btnPickDieStn.Text = "取Die位置";
            // 
            // TurnTableTeachCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TurnTableTeachCtrl";
            this.Size = new System.Drawing.Size(461, 319);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTComboBox cmxNozzleList;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTButton htButton4;
        private StdLib.WindowsControls.HTButton btn;
        private StdLib.WindowsControls.HTButton btnIRDetectStn;
        private StdLib.WindowsControls.HTButton btnBackDetectStn;
        private StdLib.WindowsControls.HTButton btnPickDieStn;
    }
}
