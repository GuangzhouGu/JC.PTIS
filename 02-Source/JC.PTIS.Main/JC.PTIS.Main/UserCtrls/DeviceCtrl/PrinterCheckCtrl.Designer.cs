namespace JC.PTIS.Main.UserCtrls.DeviceCtrl
{
    partial class PrinterCheckCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterCheckCtrl));
            this.htGroupBox1 = new StdLib.WindowsControls.HTGroupBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.htLabel3 = new StdLib.WindowsControls.HTLabel();
            this.htLabel2 = new StdLib.WindowsControls.HTLabel();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPackingStop = new StdLib.WindowsControls.HTButton();
            this.btnPackingStart = new StdLib.WindowsControls.HTButton();
            this.htGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox1.Controls.Add(this.htLabel4);
            this.htGroupBox1.Controls.Add(this.htLabel3);
            this.htGroupBox1.Controls.Add(this.htLabel2);
            this.htGroupBox1.Controls.Add(this.htLabel1);
            this.htGroupBox1.Controls.Add(this.pictureBox4);
            this.htGroupBox1.Controls.Add(this.pictureBox3);
            this.htGroupBox1.Controls.Add(this.pictureBox2);
            this.htGroupBox1.Controls.Add(this.pictureBox1);
            this.htGroupBox1.Controls.Add(this.btnPackingStop);
            this.htGroupBox1.Controls.Add(this.btnPackingStart);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox1.Size = new System.Drawing.Size(323, 96);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "标签机调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.BackColor = System.Drawing.Color.Transparent;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel4.Location = new System.Drawing.Point(113, 53);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(68, 20);
            this.htLabel4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel4.TabIndex = 63;
            this.htLabel4.Text = "打印完成:";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel3
            // 
            this.htLabel3.AutoSize = true;
            this.htLabel3.BackColor = System.Drawing.Color.Transparent;
            this.htLabel3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel3.Location = new System.Drawing.Point(141, 24);
            this.htLabel3.Name = "htLabel3";
            this.htLabel3.Size = new System.Drawing.Size(40, 20);
            this.htLabel3.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel3.TabIndex = 63;
            this.htLabel3.Text = "忙碌:";
            this.htLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel2
            // 
            this.htLabel2.AutoSize = true;
            this.htLabel2.BackColor = System.Drawing.Color.Transparent;
            this.htLabel2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel2.Location = new System.Drawing.Point(33, 51);
            this.htLabel2.Name = "htLabel2";
            this.htLabel2.Size = new System.Drawing.Size(40, 20);
            this.htLabel2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel2.TabIndex = 63;
            this.htLabel2.Text = "报警:";
            this.htLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.BackColor = System.Drawing.Color.Transparent;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel1.Location = new System.Drawing.Point(5, 24);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(68, 20);
            this.htLabel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel1.TabIndex = 63;
            this.htLabel1.Text = "准备完成:";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(181, 51);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(30, 25);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 53;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(181, 23);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(30, 25);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 53;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(73, 49);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 53;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(73, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 53;
            this.pictureBox1.TabStop = false;
            // 
            // btnPackingStop
            // 
            this.btnPackingStop.BackColor = System.Drawing.Color.White;
            this.btnPackingStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPackingStop.FillColor = System.Drawing.Color.White;
            this.btnPackingStop.FillColor2 = System.Drawing.Color.Empty;
            this.btnPackingStop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPackingStop.ForeColor = System.Drawing.Color.Black;
            this.btnPackingStop.Location = new System.Drawing.Point(221, 53);
            this.btnPackingStop.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPackingStop.Name = "btnPackingStop";
            this.btnPackingStop.Size = new System.Drawing.Size(80, 28);
            this.btnPackingStop.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPackingStop.TabIndex = 51;
            this.btnPackingStop.Tag = "PackingStop";
            this.btnPackingStop.Text = "停止打印";
            this.btnPackingStop.Click += new System.EventHandler(this.btnPackingStart_Click);
            // 
            // btnPackingStart
            // 
            this.btnPackingStart.BackColor = System.Drawing.Color.White;
            this.btnPackingStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPackingStart.FillColor = System.Drawing.Color.White;
            this.btnPackingStart.FillColor2 = System.Drawing.Color.Empty;
            this.btnPackingStart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPackingStart.ForeColor = System.Drawing.Color.Black;
            this.btnPackingStart.Location = new System.Drawing.Point(221, 19);
            this.btnPackingStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPackingStart.Name = "btnPackingStart";
            this.btnPackingStart.Size = new System.Drawing.Size(80, 28);
            this.btnPackingStart.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPackingStart.TabIndex = 52;
            this.btnPackingStart.Tag = "PackingStart";
            this.btnPackingStart.Text = "开始打印";
            this.btnPackingStart.Click += new System.EventHandler(this.btnPackingStart_Click);
            // 
            // PrinterCheckCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrinterCheckCtrl";
            this.Size = new System.Drawing.Size(323, 96);
            this.Load += new System.EventHandler(this.PackerCheckCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private StdLib.WindowsControls.HTButton btnPackingStop;
        private StdLib.WindowsControls.HTButton btnPackingStart;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTLabel htLabel3;
        private StdLib.WindowsControls.HTLabel htLabel2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
