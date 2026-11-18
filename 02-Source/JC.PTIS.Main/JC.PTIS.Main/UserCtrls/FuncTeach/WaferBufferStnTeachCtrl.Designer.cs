namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class WaferBufferStnTeachCtrl
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
            this.foupCtrl1 = new JC.PTIS.Main.UserCtrls.FoupCtrl();
            this.htGroupBox5 = new StdLib.WindowsControls.HTGroupBox();
            this.vacuumCtrl1 = new StdLib.PCBaseFrame.UserCtrl.VacuumCtrl();
            this.cylinderCtrl2 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.cylinderCtrl1 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.htGroupBox8 = new StdLib.WindowsControls.HTGroupBox();
            this.tbxRFID = new StdLib.WindowsControls.HTTextBox();
            this.htButton2 = new StdLib.WindowsControls.HTButton();
            this.tbxBarcode = new StdLib.WindowsControls.HTTextBox();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.btnSubBarcode = new StdLib.WindowsControls.HTButton();
            this.htLabel6 = new StdLib.WindowsControls.HTLabel();
            this.htGroupBox4 = new StdLib.WindowsControls.HTGroupBox();
            this.btnWaferTableUnloadPos = new StdLib.WindowsControls.HTButton();
            this.btnWaferTableLoadPos = new StdLib.WindowsControls.HTButton();
            this.btnMoveToDestLayer = new StdLib.WindowsControls.HTButton();
            this.btnWaitPos = new StdLib.WindowsControls.HTButton();
            this.htGroupBox3 = new StdLib.WindowsControls.HTGroupBox();
            this.btnResume = new StdLib.WindowsControls.HTButton();
            this.btnAutoPickStop = new StdLib.WindowsControls.HTButton();
            this.btnPause = new StdLib.WindowsControls.HTButton();
            this.htGroupBox7 = new StdLib.WindowsControls.HTGroupBox();
            this.cmxFoupLayerList = new StdLib.WindowsControls.HTComboBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.btnPickWaferFromFoup = new StdLib.WindowsControls.HTButton();
            this.btnPlaceWafer = new StdLib.WindowsControls.HTButton();
            this.htGroupBox2 = new StdLib.WindowsControls.HTGroupBox();
            this.rbtnRight = new StdLib.WindowsControls.HTRadioButton();
            this.rbtnLeft = new StdLib.WindowsControls.HTRadioButton();
            this.htGroupBox1.SuspendLayout();
            this.htGroupBox5.SuspendLayout();
            this.htGroupBox8.SuspendLayout();
            this.htGroupBox4.SuspendLayout();
            this.htGroupBox3.SuspendLayout();
            this.htGroupBox7.SuspendLayout();
            this.htGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox1.Controls.Add(this.foupCtrl1);
            this.htGroupBox1.Controls.Add(this.htGroupBox5);
            this.htGroupBox1.Controls.Add(this.htGroupBox8);
            this.htGroupBox1.Controls.Add(this.htGroupBox4);
            this.htGroupBox1.Controls.Add(this.htGroupBox3);
            this.htGroupBox1.Controls.Add(this.htGroupBox7);
            this.htGroupBox1.Controls.Add(this.htGroupBox2);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(4, 34, 4, 4);
            this.htGroupBox1.Size = new System.Drawing.Size(1057, 820);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "Wafer缓存调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // foupCtrl1
            // 
            this.foupCtrl1.Desc = "Foup1";
            this.foupCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.foupCtrl1.FoupNo = 0;
            this.foupCtrl1.Location = new System.Drawing.Point(640, 82);
            this.foupCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.foupCtrl1.Name = "foupCtrl1";
            this.foupCtrl1.Size = new System.Drawing.Size(226, 263);
            this.foupCtrl1.TabIndex = 48;
            this.foupCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.foupCtrl1.TotalWaferCount = 10;
            // 
            // htGroupBox5
            // 
            this.htGroupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox5.Controls.Add(this.vacuumCtrl1);
            this.htGroupBox5.Controls.Add(this.cylinderCtrl2);
            this.htGroupBox5.Controls.Add(this.cylinderCtrl1);
            this.htGroupBox5.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox5.Location = new System.Drawing.Point(345, 74);
            this.htGroupBox5.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox5.Name = "htGroupBox5";
            this.htGroupBox5.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox5.Size = new System.Drawing.Size(290, 271);
            this.htGroupBox5.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox5.TabIndex = 47;
            this.htGroupBox5.Text = "气缸真空";
            this.htGroupBox5.TitleTop = 8;
            // 
            // vacuumCtrl1
            // 
            this.vacuumCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.vacuumCtrl1.Location = new System.Drawing.Point(5, 145);
            this.vacuumCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.vacuumCtrl1.Name = "vacuumCtrl1";
            this.vacuumCtrl1.Size = new System.Drawing.Size(138, 115);
            this.vacuumCtrl1.StationName = null;
            this.vacuumCtrl1.TabIndex = 50;
            this.vacuumCtrl1.VacuumName = "WaferBufferClawVac";
            this.vacuumCtrl1.VacuumNameChn = "夹爪真空";
            // 
            // cylinderCtrl2
            // 
            this.cylinderCtrl2.CylinderName = "WaferBufferStretchCyl";
            this.cylinderCtrl2.CylinderNameChn = "伸缩气缸";
            this.cylinderCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl2.Location = new System.Drawing.Point(146, 22);
            this.cylinderCtrl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl2.Name = "cylinderCtrl2";
            this.cylinderCtrl2.Size = new System.Drawing.Size(137, 115);
            this.cylinderCtrl2.StationName = null;
            this.cylinderCtrl2.TabIndex = 0;
            // 
            // cylinderCtrl1
            // 
            this.cylinderCtrl1.CylinderName = "WaferBufferClawCyl";
            this.cylinderCtrl1.CylinderNameChn = "夹爪气缸";
            this.cylinderCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl1.Location = new System.Drawing.Point(6, 22);
            this.cylinderCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl1.Name = "cylinderCtrl1";
            this.cylinderCtrl1.Size = new System.Drawing.Size(137, 115);
            this.cylinderCtrl1.StationName = null;
            this.cylinderCtrl1.TabIndex = 0;
            // 
            // htGroupBox8
            // 
            this.htGroupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox8.Controls.Add(this.tbxRFID);
            this.htGroupBox8.Controls.Add(this.htButton2);
            this.htGroupBox8.Controls.Add(this.tbxBarcode);
            this.htGroupBox8.Controls.Add(this.htLabel1);
            this.htGroupBox8.Controls.Add(this.btnSubBarcode);
            this.htGroupBox8.Controls.Add(this.htLabel6);
            this.htGroupBox8.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox8.Location = new System.Drawing.Point(5, 347);
            this.htGroupBox8.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox8.Name = "htGroupBox8";
            this.htGroupBox8.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox8.Size = new System.Drawing.Size(338, 124);
            this.htGroupBox8.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox8.TabIndex = 47;
            this.htGroupBox8.Text = "读码";
            this.htGroupBox8.TitleTop = 8;
            // 
            // tbxRFID
            // 
            this.tbxRFID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxRFID.FillColor = System.Drawing.Color.White;
            this.tbxRFID.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxRFID.Location = new System.Drawing.Point(50, 51);
            this.tbxRFID.Maximum = 2147483647D;
            this.tbxRFID.Minimum = -2147483648D;
            this.tbxRFID.Name = "tbxRFID";
            this.tbxRFID.Padding = new System.Windows.Forms.Padding(5);
            this.tbxRFID.ReadOnly = true;
            this.tbxRFID.Size = new System.Drawing.Size(148, 23);
            this.tbxRFID.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxRFID.TabIndex = 51;
            // 
            // htButton2
            // 
            this.htButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.htButton2.FillColor = System.Drawing.Color.White;
            this.htButton2.FillColor2 = System.Drawing.Color.Empty;
            this.htButton2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htButton2.ForeColor = System.Drawing.Color.Black;
            this.htButton2.Location = new System.Drawing.Point(206, 49);
            this.htButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.htButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.htButton2.Name = "htButton2";
            this.htButton2.Size = new System.Drawing.Size(81, 26);
            this.htButton2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htButton2.TabIndex = 49;
            this.htButton2.Text = "读取Rfid";
            this.htButton2.Click += new System.EventHandler(this.htButton2_Click);
            // 
            // tbxBarcode
            // 
            this.tbxBarcode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxBarcode.FillColor = System.Drawing.Color.White;
            this.tbxBarcode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxBarcode.Location = new System.Drawing.Point(50, 19);
            this.tbxBarcode.Maximum = 2147483647D;
            this.tbxBarcode.Minimum = -2147483648D;
            this.tbxBarcode.Name = "tbxBarcode";
            this.tbxBarcode.Padding = new System.Windows.Forms.Padding(5);
            this.tbxBarcode.ReadOnly = true;
            this.tbxBarcode.Size = new System.Drawing.Size(148, 23);
            this.tbxBarcode.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxBarcode.TabIndex = 51;
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.BackColor = System.Drawing.Color.Transparent;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel1.Location = new System.Drawing.Point(9, 51);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(39, 20);
            this.htLabel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel1.TabIndex = 50;
            this.htLabel1.Text = "Rfid:";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSubBarcode
            // 
            this.btnSubBarcode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubBarcode.FillColor = System.Drawing.Color.White;
            this.btnSubBarcode.FillColor2 = System.Drawing.Color.Empty;
            this.btnSubBarcode.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSubBarcode.ForeColor = System.Drawing.Color.Black;
            this.btnSubBarcode.Location = new System.Drawing.Point(206, 17);
            this.btnSubBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubBarcode.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSubBarcode.Name = "btnSubBarcode";
            this.btnSubBarcode.Size = new System.Drawing.Size(81, 26);
            this.btnSubBarcode.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnSubBarcode.TabIndex = 49;
            this.btnSubBarcode.Text = "扫码";
            this.btnSubBarcode.Click += new System.EventHandler(this.btnSubBarcode_Click);
            // 
            // htLabel6
            // 
            this.htLabel6.AutoSize = true;
            this.htLabel6.BackColor = System.Drawing.Color.Transparent;
            this.htLabel6.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel6.Location = new System.Drawing.Point(9, 19);
            this.htLabel6.Name = "htLabel6";
            this.htLabel6.Size = new System.Drawing.Size(40, 20);
            this.htLabel6.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel6.TabIndex = 50;
            this.htLabel6.Text = "条码:";
            this.htLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htGroupBox4
            // 
            this.htGroupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox4.Controls.Add(this.btnWaferTableUnloadPos);
            this.htGroupBox4.Controls.Add(this.btnWaferTableLoadPos);
            this.htGroupBox4.Controls.Add(this.btnMoveToDestLayer);
            this.htGroupBox4.Controls.Add(this.btnWaitPos);
            this.htGroupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox4.Location = new System.Drawing.Point(185, 74);
            this.htGroupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox4.Name = "htGroupBox4";
            this.htGroupBox4.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox4.Size = new System.Drawing.Size(158, 271);
            this.htGroupBox4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox4.TabIndex = 47;
            this.htGroupBox4.Text = "快速定位";
            this.htGroupBox4.TitleTop = 8;
            // 
            // btnWaferTableUnloadPos
            // 
            this.btnWaferTableUnloadPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWaferTableUnloadPos.FillColor = System.Drawing.Color.White;
            this.btnWaferTableUnloadPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnWaferTableUnloadPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWaferTableUnloadPos.ForeColor = System.Drawing.Color.Black;
            this.btnWaferTableUnloadPos.Location = new System.Drawing.Point(17, 127);
            this.btnWaferTableUnloadPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWaferTableUnloadPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWaferTableUnloadPos.Name = "btnWaferTableUnloadPos";
            this.btnWaferTableUnloadPos.Size = new System.Drawing.Size(125, 28);
            this.btnWaferTableUnloadPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnWaferTableUnloadPos.TabIndex = 48;
            this.btnWaferTableUnloadPos.Text = "Wafer平台下料位置";
            this.btnWaferTableUnloadPos.Click += new System.EventHandler(this.btnWaferTableUnloadPos_Click);
            // 
            // btnWaferTableLoadPos
            // 
            this.btnWaferTableLoadPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWaferTableLoadPos.FillColor = System.Drawing.Color.White;
            this.btnWaferTableLoadPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnWaferTableLoadPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWaferTableLoadPos.ForeColor = System.Drawing.Color.Black;
            this.btnWaferTableLoadPos.Location = new System.Drawing.Point(17, 91);
            this.btnWaferTableLoadPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWaferTableLoadPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWaferTableLoadPos.Name = "btnWaferTableLoadPos";
            this.btnWaferTableLoadPos.Size = new System.Drawing.Size(125, 28);
            this.btnWaferTableLoadPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnWaferTableLoadPos.TabIndex = 48;
            this.btnWaferTableLoadPos.Text = "Wafer平台上料位置";
            this.btnWaferTableLoadPos.Click += new System.EventHandler(this.btnWaferLulPos_Click);
            // 
            // btnMoveToDestLayer
            // 
            this.btnMoveToDestLayer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoveToDestLayer.FillColor = System.Drawing.Color.White;
            this.btnMoveToDestLayer.FillColor2 = System.Drawing.Color.Empty;
            this.btnMoveToDestLayer.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveToDestLayer.ForeColor = System.Drawing.Color.Black;
            this.btnMoveToDestLayer.Location = new System.Drawing.Point(17, 57);
            this.btnMoveToDestLayer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMoveToDestLayer.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMoveToDestLayer.Name = "btnMoveToDestLayer";
            this.btnMoveToDestLayer.Size = new System.Drawing.Size(125, 28);
            this.btnMoveToDestLayer.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnMoveToDestLayer.TabIndex = 48;
            this.btnMoveToDestLayer.Text = "移动到指定层";
            this.btnMoveToDestLayer.Click += new System.EventHandler(this.btnMoveToDefineLayer_Click);
            // 
            // btnWaitPos
            // 
            this.btnWaitPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWaitPos.FillColor = System.Drawing.Color.White;
            this.btnWaitPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnWaitPos.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWaitPos.ForeColor = System.Drawing.Color.Black;
            this.btnWaitPos.Location = new System.Drawing.Point(17, 22);
            this.btnWaitPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWaitPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWaitPos.Name = "btnWaitPos";
            this.btnWaitPos.Size = new System.Drawing.Size(125, 28);
            this.btnWaitPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnWaitPos.TabIndex = 48;
            this.btnWaitPos.Text = "待机位置";
            this.btnWaitPos.Click += new System.EventHandler(this.btnWaitPos_Click);
            // 
            // htGroupBox3
            // 
            this.htGroupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox3.Controls.Add(this.btnResume);
            this.htGroupBox3.Controls.Add(this.btnAutoPickStop);
            this.htGroupBox3.Controls.Add(this.btnPause);
            this.htGroupBox3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox3.Location = new System.Drawing.Point(183, 20);
            this.htGroupBox3.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox3.Name = "htGroupBox3";
            this.htGroupBox3.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox3.Size = new System.Drawing.Size(335, 52);
            this.htGroupBox3.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox3.TabIndex = 47;
            this.htGroupBox3.Text = "流程控制";
            this.htGroupBox3.TitleTop = 8;
            // 
            // btnResume
            // 
            this.btnResume.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResume.FillColor = System.Drawing.Color.White;
            this.btnResume.FillColor2 = System.Drawing.Color.Empty;
            this.btnResume.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnResume.ForeColor = System.Drawing.Color.Black;
            this.btnResume.Location = new System.Drawing.Point(115, 16);
            this.btnResume.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnResume.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(100, 28);
            this.btnResume.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnResume.TabIndex = 48;
            this.btnResume.Text = "恢复";
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnAutoPickStop
            // 
            this.btnAutoPickStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAutoPickStop.FillColor = System.Drawing.Color.White;
            this.btnAutoPickStop.FillColor2 = System.Drawing.Color.Empty;
            this.btnAutoPickStop.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAutoPickStop.ForeColor = System.Drawing.Color.Black;
            this.btnAutoPickStop.Location = new System.Drawing.Point(221, 16);
            this.btnAutoPickStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoPickStop.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAutoPickStop.Name = "btnAutoPickStop";
            this.btnAutoPickStop.RectColor = System.Drawing.Color.Red;
            this.btnAutoPickStop.Size = new System.Drawing.Size(104, 28);
            this.btnAutoPickStop.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnAutoPickStop.TabIndex = 49;
            this.btnAutoPickStop.Text = "停止";
            this.btnAutoPickStop.Click += new System.EventHandler(this.btnAutoPickStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPause.FillColor = System.Drawing.Color.White;
            this.btnPause.FillColor2 = System.Drawing.Color.Empty;
            this.btnPause.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPause.ForeColor = System.Drawing.Color.Black;
            this.btnPause.Location = new System.Drawing.Point(9, 16);
            this.btnPause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPause.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 28);
            this.btnPause.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPause.TabIndex = 48;
            this.btnPause.Text = "暂停";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // htGroupBox7
            // 
            this.htGroupBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox7.Controls.Add(this.cmxFoupLayerList);
            this.htGroupBox7.Controls.Add(this.htLabel4);
            this.htGroupBox7.Controls.Add(this.btnPickWaferFromFoup);
            this.htGroupBox7.Controls.Add(this.btnPlaceWafer);
            this.htGroupBox7.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox7.Location = new System.Drawing.Point(5, 74);
            this.htGroupBox7.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox7.Name = "htGroupBox7";
            this.htGroupBox7.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox7.Size = new System.Drawing.Size(176, 271);
            this.htGroupBox7.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox7.TabIndex = 47;
            this.htGroupBox7.Text = "动作流程";
            this.htGroupBox7.TitleTop = 8;
            // 
            // cmxFoupLayerList
            // 
            this.cmxFoupLayerList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxFoupLayerList.FillColor = System.Drawing.Color.White;
            this.cmxFoupLayerList.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxFoupLayerList.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmxFoupLayerList.Location = new System.Drawing.Point(78, 16);
            this.cmxFoupLayerList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxFoupLayerList.Name = "cmxFoupLayerList";
            this.cmxFoupLayerList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxFoupLayerList.Size = new System.Drawing.Size(92, 29);
            this.cmxFoupLayerList.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.cmxFoupLayerList.TabIndex = 49;
            this.cmxFoupLayerList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel4.Location = new System.Drawing.Point(4, 20);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(74, 20);
            this.htLabel4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel4.TabIndex = 48;
            this.htLabel4.Text = "Foup层号:";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPickWaferFromFoup
            // 
            this.btnPickWaferFromFoup.BackColor = System.Drawing.Color.White;
            this.btnPickWaferFromFoup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickWaferFromFoup.FillColor = System.Drawing.Color.White;
            this.btnPickWaferFromFoup.FillColor2 = System.Drawing.Color.Empty;
            this.btnPickWaferFromFoup.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnPickWaferFromFoup.ForeColor = System.Drawing.Color.Black;
            this.btnPickWaferFromFoup.Location = new System.Drawing.Point(33, 54);
            this.btnPickWaferFromFoup.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPickWaferFromFoup.Name = "btnPickWaferFromFoup";
            this.btnPickWaferFromFoup.Size = new System.Drawing.Size(137, 28);
            this.btnPickWaferFromFoup.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPickWaferFromFoup.TabIndex = 46;
            this.btnPickWaferFromFoup.Text = "从Foup取Wafer";
            this.btnPickWaferFromFoup.Click += new System.EventHandler(this.btnPickWaferFromFoup_Click);
            // 
            // btnPlaceWafer
            // 
            this.btnPlaceWafer.BackColor = System.Drawing.Color.White;
            this.btnPlaceWafer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlaceWafer.FillColor = System.Drawing.Color.White;
            this.btnPlaceWafer.FillColor2 = System.Drawing.Color.Empty;
            this.btnPlaceWafer.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnPlaceWafer.ForeColor = System.Drawing.Color.Black;
            this.btnPlaceWafer.Location = new System.Drawing.Point(33, 88);
            this.btnPlaceWafer.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPlaceWafer.Name = "btnPlaceWafer";
            this.btnPlaceWafer.Size = new System.Drawing.Size(137, 28);
            this.btnPlaceWafer.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPlaceWafer.TabIndex = 46;
            this.btnPlaceWafer.Text = "放Wafer到Foup";
            this.btnPlaceWafer.Click += new System.EventHandler(this.btnPlaceWafer_Click);
            // 
            // htGroupBox2
            // 
            this.htGroupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox2.Controls.Add(this.rbtnRight);
            this.htGroupBox2.Controls.Add(this.rbtnLeft);
            this.htGroupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox2.Location = new System.Drawing.Point(5, 20);
            this.htGroupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox2.Name = "htGroupBox2";
            this.htGroupBox2.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox2.Size = new System.Drawing.Size(176, 52);
            this.htGroupBox2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox2.TabIndex = 47;
            this.htGroupBox2.Text = "Foup选择";
            this.htGroupBox2.TitleTop = 8;
            // 
            // rbtnRight
            // 
            this.rbtnRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtnRight.Location = new System.Drawing.Point(78, 16);
            this.rbtnRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbtnRight.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbtnRight.Name = "rbtnRight";
            this.rbtnRight.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbtnRight.Size = new System.Drawing.Size(65, 28);
            this.rbtnRight.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.rbtnRight.TabIndex = 0;
            this.rbtnRight.Text = "Foup2";
            // 
            // rbtnLeft
            // 
            this.rbtnLeft.Checked = true;
            this.rbtnLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbtnLeft.Location = new System.Drawing.Point(6, 16);
            this.rbtnLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbtnLeft.MinimumSize = new System.Drawing.Size(1, 1);
            this.rbtnLeft.Name = "rbtnLeft";
            this.rbtnLeft.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.rbtnLeft.Size = new System.Drawing.Size(65, 28);
            this.rbtnLeft.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.rbtnLeft.TabIndex = 0;
            this.rbtnLeft.Text = "Foup1";
            this.rbtnLeft.ValueChanged += new StdLib.WindowsControls.HTRadioButton.OnValueChanged(this.rbtnLeft_ValueChanged);
            // 
            // WaferBufferStnTeachCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WaferBufferStnTeachCtrl";
            this.Size = new System.Drawing.Size(1057, 820);
            this.Load += new System.EventHandler(this.MeasureHeightTeachCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox5.ResumeLayout(false);
            this.htGroupBox8.ResumeLayout(false);
            this.htGroupBox8.PerformLayout();
            this.htGroupBox4.ResumeLayout(false);
            this.htGroupBox3.ResumeLayout(false);
            this.htGroupBox7.ResumeLayout(false);
            this.htGroupBox7.PerformLayout();
            this.htGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTButton btnPlaceWafer;
        private StdLib.WindowsControls.HTButton btnPickWaferFromFoup;
        private StdLib.WindowsControls.HTGroupBox htGroupBox2;
        private StdLib.WindowsControls.HTComboBox cmxFoupLayerList;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTRadioButton rbtnRight;
        private StdLib.WindowsControls.HTRadioButton rbtnLeft;
        private StdLib.WindowsControls.HTButton btnWaitPos;
        private StdLib.WindowsControls.HTButton btnAutoPickStop;
        private StdLib.WindowsControls.HTButton btnResume;
        private StdLib.WindowsControls.HTButton btnPause;
        private StdLib.WindowsControls.HTGroupBox htGroupBox3;
        private StdLib.WindowsControls.HTGroupBox htGroupBox4;
        private StdLib.WindowsControls.HTGroupBox htGroupBox5;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl1;
        private StdLib.WindowsControls.HTButton btnMoveToDestLayer;
        private StdLib.PCBaseFrame.UserCtrl.VacuumCtrl vacuumCtrl1;
        private StdLib.WindowsControls.HTGroupBox htGroupBox7;
        private StdLib.WindowsControls.HTTextBox tbxBarcode;
        private StdLib.WindowsControls.HTLabel htLabel6;
        private StdLib.WindowsControls.HTButton btnSubBarcode;
        private StdLib.WindowsControls.HTGroupBox htGroupBox8;
        private StdLib.WindowsControls.HTTextBox tbxRFID;
        private StdLib.WindowsControls.HTButton htButton2;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl2;
        private StdLib.WindowsControls.HTButton btnWaferTableLoadPos;
        private StdLib.WindowsControls.HTButton btnWaferTableUnloadPos;
        private FoupCtrl foupCtrl1;
    }
}
