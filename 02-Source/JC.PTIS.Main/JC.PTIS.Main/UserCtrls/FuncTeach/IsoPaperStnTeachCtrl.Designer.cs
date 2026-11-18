namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class IsoPaperStnTeachCtrl
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
            this.htGroupBox6 = new StdLib.WindowsControls.HTGroupBox();
            this.btnResume = new StdLib.WindowsControls.HTButton();
            this.btnAutoPickStop = new StdLib.WindowsControls.HTButton();
            this.btnPause = new StdLib.WindowsControls.HTButton();
            this.htGroupBox5 = new StdLib.WindowsControls.HTGroupBox();
            this.vacuumCtrl3 = new StdLib.PCBaseFrame.UserCtrl.VacuumCtrl();
            this.vacuumCtrl2 = new StdLib.PCBaseFrame.UserCtrl.VacuumCtrl();
            this.vacuumCtrl1 = new StdLib.PCBaseFrame.UserCtrl.VacuumCtrl();
            this.cylinderCtrl3 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.cylinderCtrl2 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.cylinderCtrl1 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.htGroupBox4 = new StdLib.WindowsControls.HTGroupBox();
            this.btnPutPaperToLotTray = new StdLib.WindowsControls.HTButton();
            this.btnAlignTray = new StdLib.WindowsControls.HTButton();
            this.btnAlignPaper = new StdLib.WindowsControls.HTButton();
            this.btnPickPaperFlow = new StdLib.WindowsControls.HTButton();
            this.htGroupBox2 = new StdLib.WindowsControls.HTGroupBox();
            this.btnPaperGrab = new StdLib.WindowsControls.HTButton();
            this.btnTrayGrab = new StdLib.WindowsControls.HTButton();
            this.btnPaperAlign = new StdLib.WindowsControls.HTButton();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.btnTrayAlign = new StdLib.WindowsControls.HTButton();
            this.tbxPaperXyt = new StdLib.WindowsControls.HTTextBox();
            this.htLabel14 = new StdLib.WindowsControls.HTLabel();
            this.tbxTrayXyt = new StdLib.WindowsControls.HTTextBox();
            this.htGroupBox3 = new StdLib.WindowsControls.HTGroupBox();
            this.btnWaitPos = new StdLib.WindowsControls.HTButton();
            this.btnPutPaperToTrayPos = new StdLib.WindowsControls.HTButton();
            this.btnPaperGrabPos = new StdLib.WindowsControls.HTButton();
            this.btnPickPaperFromStagePos = new StdLib.WindowsControls.HTButton();
            this.btnTrayGrabPos = new StdLib.WindowsControls.HTButton();
            this.htGroupBox1.SuspendLayout();
            this.htGroupBox6.SuspendLayout();
            this.htGroupBox5.SuspendLayout();
            this.htGroupBox4.SuspendLayout();
            this.htGroupBox2.SuspendLayout();
            this.htGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox1.Controls.Add(this.htGroupBox6);
            this.htGroupBox1.Controls.Add(this.htGroupBox5);
            this.htGroupBox1.Controls.Add(this.htGroupBox4);
            this.htGroupBox1.Controls.Add(this.htGroupBox2);
            this.htGroupBox1.Controls.Add(this.htGroupBox3);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox1.Size = new System.Drawing.Size(885, 495);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "隔离纸工位调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // htGroupBox6
            // 
            this.htGroupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox6.Controls.Add(this.btnResume);
            this.htGroupBox6.Controls.Add(this.btnAutoPickStop);
            this.htGroupBox6.Controls.Add(this.btnPause);
            this.htGroupBox6.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox6.Location = new System.Drawing.Point(4, 15);
            this.htGroupBox6.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox6.Name = "htGroupBox6";
            this.htGroupBox6.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox6.Size = new System.Drawing.Size(425, 52);
            this.htGroupBox6.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox6.TabIndex = 58;
            this.htGroupBox6.Text = "流程控制";
            this.htGroupBox6.TitleTop = 8;
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
            // htGroupBox5
            // 
            this.htGroupBox5.Controls.Add(this.vacuumCtrl3);
            this.htGroupBox5.Controls.Add(this.vacuumCtrl2);
            this.htGroupBox5.Controls.Add(this.vacuumCtrl1);
            this.htGroupBox5.Controls.Add(this.cylinderCtrl3);
            this.htGroupBox5.Controls.Add(this.cylinderCtrl2);
            this.htGroupBox5.Controls.Add(this.cylinderCtrl1);
            this.htGroupBox5.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox5.Location = new System.Drawing.Point(435, 72);
            this.htGroupBox5.Name = "htGroupBox5";
            this.htGroupBox5.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox5.Size = new System.Drawing.Size(423, 280);
            this.htGroupBox5.TabIndex = 57;
            this.htGroupBox5.Text = "气缸真空";
            this.htGroupBox5.TitleTop = 8;
            // 
            // vacuumCtrl3
            // 
            this.vacuumCtrl3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.vacuumCtrl3.Location = new System.Drawing.Point(272, 148);
            this.vacuumCtrl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.vacuumCtrl3.Name = "vacuumCtrl3";
            this.vacuumCtrl3.Size = new System.Drawing.Size(128, 124);
            this.vacuumCtrl3.StationName = null;
            this.vacuumCtrl3.TabIndex = 60;
            this.vacuumCtrl3.VacuumName = "PaperStageVac";
            this.vacuumCtrl3.VacuumNameChn = "对位平台真空";
            // 
            // vacuumCtrl2
            // 
            this.vacuumCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.vacuumCtrl2.Location = new System.Drawing.Point(139, 148);
            this.vacuumCtrl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.vacuumCtrl2.Name = "vacuumCtrl2";
            this.vacuumCtrl2.Size = new System.Drawing.Size(128, 124);
            this.vacuumCtrl2.StationName = null;
            this.vacuumCtrl2.TabIndex = 60;
            this.vacuumCtrl2.VacuumName = "PaperArmVac";
            this.vacuumCtrl2.VacuumNameChn = "对位Arm真空";
            // 
            // vacuumCtrl1
            // 
            this.vacuumCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.vacuumCtrl1.Location = new System.Drawing.Point(5, 148);
            this.vacuumCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.vacuumCtrl1.Name = "vacuumCtrl1";
            this.vacuumCtrl1.Size = new System.Drawing.Size(128, 124);
            this.vacuumCtrl1.StationName = null;
            this.vacuumCtrl1.TabIndex = 60;
            this.vacuumCtrl1.VacuumName = "PickPaperVac";
            this.vacuumCtrl1.VacuumNameChn = "搬运手臂真空";
            // 
            // cylinderCtrl3
            // 
            this.cylinderCtrl3.CylinderName = "PickPaperLiftCyl";
            this.cylinderCtrl3.CylinderNameChn = "隔离纸升降气缸";
            this.cylinderCtrl3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl3.Location = new System.Drawing.Point(272, 18);
            this.cylinderCtrl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl3.Name = "cylinderCtrl3";
            this.cylinderCtrl3.Size = new System.Drawing.Size(128, 123);
            this.cylinderCtrl3.StationName = null;
            this.cylinderCtrl3.TabIndex = 59;
            // 
            // cylinderCtrl2
            // 
            this.cylinderCtrl2.CylinderName = "PaperCabinetRotateCyl";
            this.cylinderCtrl2.CylinderNameChn = "料仓旋转气缸";
            this.cylinderCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl2.Location = new System.Drawing.Point(139, 18);
            this.cylinderCtrl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl2.Name = "cylinderCtrl2";
            this.cylinderCtrl2.Size = new System.Drawing.Size(128, 123);
            this.cylinderCtrl2.StationName = null;
            this.cylinderCtrl2.TabIndex = 59;
            // 
            // cylinderCtrl1
            // 
            this.cylinderCtrl1.CylinderName = "PickPaperTransferCyl";
            this.cylinderCtrl1.CylinderNameChn = "隔离纸平移气缸";
            this.cylinderCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl1.Location = new System.Drawing.Point(6, 18);
            this.cylinderCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl1.Name = "cylinderCtrl1";
            this.cylinderCtrl1.Size = new System.Drawing.Size(128, 123);
            this.cylinderCtrl1.StationName = null;
            this.cylinderCtrl1.TabIndex = 59;
            // 
            // htGroupBox4
            // 
            this.htGroupBox4.Controls.Add(this.btnPutPaperToLotTray);
            this.htGroupBox4.Controls.Add(this.btnAlignTray);
            this.htGroupBox4.Controls.Add(this.btnAlignPaper);
            this.htGroupBox4.Controls.Add(this.btnPickPaperFlow);
            this.htGroupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox4.Location = new System.Drawing.Point(6, 262);
            this.htGroupBox4.Name = "htGroupBox4";
            this.htGroupBox4.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox4.Size = new System.Drawing.Size(423, 86);
            this.htGroupBox4.TabIndex = 55;
            this.htGroupBox4.Text = "流程调试";
            this.htGroupBox4.TitleTop = 8;
            // 
            // btnPutPaperToLotTray
            // 
            this.btnPutPaperToLotTray.BackColor = System.Drawing.Color.White;
            this.btnPutPaperToLotTray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPutPaperToLotTray.FillColor = System.Drawing.Color.White;
            this.btnPutPaperToLotTray.FillColor2 = System.Drawing.Color.Empty;
            this.btnPutPaperToLotTray.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPutPaperToLotTray.ForeColor = System.Drawing.Color.Black;
            this.btnPutPaperToLotTray.Location = new System.Drawing.Point(15, 55);
            this.btnPutPaperToLotTray.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPutPaperToLotTray.Name = "btnPutPaperToLotTray";
            this.btnPutPaperToLotTray.Size = new System.Drawing.Size(105, 28);
            this.btnPutPaperToLotTray.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPutPaperToLotTray.TabIndex = 50;
            this.btnPutPaperToLotTray.Tag = "14";
            this.btnPutPaperToLotTray.Text = "放隔离纸到Tray";
            this.btnPutPaperToLotTray.Click += new System.EventHandler(this.btnPutPaperToLotTray_Click);
            // 
            // btnAlignTray
            // 
            this.btnAlignTray.BackColor = System.Drawing.Color.White;
            this.btnAlignTray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlignTray.FillColor = System.Drawing.Color.White;
            this.btnAlignTray.FillColor2 = System.Drawing.Color.Empty;
            this.btnAlignTray.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlignTray.ForeColor = System.Drawing.Color.Black;
            this.btnAlignTray.Location = new System.Drawing.Point(240, 23);
            this.btnAlignTray.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAlignTray.Name = "btnAlignTray";
            this.btnAlignTray.Size = new System.Drawing.Size(105, 28);
            this.btnAlignTray.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnAlignTray.TabIndex = 50;
            this.btnAlignTray.Tag = "14";
            this.btnAlignTray.Text = "Tray盘对位";
            this.btnAlignTray.Click += new System.EventHandler(this.btnAlignTray_Click);
            // 
            // btnAlignPaper
            // 
            this.btnAlignPaper.BackColor = System.Drawing.Color.White;
            this.btnAlignPaper.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlignPaper.FillColor = System.Drawing.Color.White;
            this.btnAlignPaper.FillColor2 = System.Drawing.Color.Empty;
            this.btnAlignPaper.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlignPaper.ForeColor = System.Drawing.Color.Black;
            this.btnAlignPaper.Location = new System.Drawing.Point(126, 23);
            this.btnAlignPaper.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnAlignPaper.Name = "btnAlignPaper";
            this.btnAlignPaper.Size = new System.Drawing.Size(105, 28);
            this.btnAlignPaper.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnAlignPaper.TabIndex = 50;
            this.btnAlignPaper.Tag = "14";
            this.btnAlignPaper.Text = "隔离纸对位";
            this.btnAlignPaper.Click += new System.EventHandler(this.btnAlignPaper_Click);
            // 
            // btnPickPaperFlow
            // 
            this.btnPickPaperFlow.BackColor = System.Drawing.Color.White;
            this.btnPickPaperFlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickPaperFlow.FillColor = System.Drawing.Color.White;
            this.btnPickPaperFlow.FillColor2 = System.Drawing.Color.Empty;
            this.btnPickPaperFlow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPickPaperFlow.ForeColor = System.Drawing.Color.Black;
            this.btnPickPaperFlow.Location = new System.Drawing.Point(15, 23);
            this.btnPickPaperFlow.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPickPaperFlow.Name = "btnPickPaperFlow";
            this.btnPickPaperFlow.Size = new System.Drawing.Size(105, 28);
            this.btnPickPaperFlow.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPickPaperFlow.TabIndex = 50;
            this.btnPickPaperFlow.Tag = "14";
            this.btnPickPaperFlow.Text = "从料仓取隔离纸";
            this.btnPickPaperFlow.Click += new System.EventHandler(this.btnPickPaperFlow_Click);
            // 
            // htGroupBox2
            // 
            this.htGroupBox2.Controls.Add(this.btnPaperGrab);
            this.htGroupBox2.Controls.Add(this.btnTrayGrab);
            this.htGroupBox2.Controls.Add(this.btnPaperAlign);
            this.htGroupBox2.Controls.Add(this.htLabel1);
            this.htGroupBox2.Controls.Add(this.btnTrayAlign);
            this.htGroupBox2.Controls.Add(this.tbxPaperXyt);
            this.htGroupBox2.Controls.Add(this.htLabel14);
            this.htGroupBox2.Controls.Add(this.tbxTrayXyt);
            this.htGroupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox2.Location = new System.Drawing.Point(6, 171);
            this.htGroupBox2.Name = "htGroupBox2";
            this.htGroupBox2.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox2.Size = new System.Drawing.Size(423, 89);
            this.htGroupBox2.TabIndex = 55;
            this.htGroupBox2.Text = "对位调试";
            this.htGroupBox2.TitleTop = 8;
            // 
            // btnPaperGrab
            // 
            this.btnPaperGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaperGrab.FillColor = System.Drawing.Color.White;
            this.btnPaperGrab.FillColor2 = System.Drawing.Color.Empty;
            this.btnPaperGrab.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPaperGrab.ForeColor = System.Drawing.Color.Black;
            this.btnPaperGrab.Location = new System.Drawing.Point(241, 49);
            this.btnPaperGrab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPaperGrab.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPaperGrab.Name = "btnPaperGrab";
            this.btnPaperGrab.Size = new System.Drawing.Size(78, 25);
            this.btnPaperGrab.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPaperGrab.TabIndex = 56;
            this.btnPaperGrab.Text = "隔离纸拍照";
            this.btnPaperGrab.Click += new System.EventHandler(this.btnPaperGrab_Click);
            // 
            // btnTrayGrab
            // 
            this.btnTrayGrab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrayGrab.FillColor = System.Drawing.Color.White;
            this.btnTrayGrab.FillColor2 = System.Drawing.Color.Empty;
            this.btnTrayGrab.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTrayGrab.ForeColor = System.Drawing.Color.Black;
            this.btnTrayGrab.Location = new System.Drawing.Point(241, 20);
            this.btnTrayGrab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTrayGrab.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnTrayGrab.Name = "btnTrayGrab";
            this.btnTrayGrab.Size = new System.Drawing.Size(78, 25);
            this.btnTrayGrab.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnTrayGrab.TabIndex = 56;
            this.btnTrayGrab.Text = "Tray拍照";
            this.btnTrayGrab.Click += new System.EventHandler(this.btnTrayGrab_Click);
            // 
            // btnPaperAlign
            // 
            this.btnPaperAlign.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaperAlign.FillColor = System.Drawing.Color.White;
            this.btnPaperAlign.FillColor2 = System.Drawing.Color.Empty;
            this.btnPaperAlign.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPaperAlign.ForeColor = System.Drawing.Color.Black;
            this.btnPaperAlign.Location = new System.Drawing.Point(325, 49);
            this.btnPaperAlign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPaperAlign.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPaperAlign.Name = "btnPaperAlign";
            this.btnPaperAlign.Size = new System.Drawing.Size(78, 25);
            this.btnPaperAlign.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPaperAlign.TabIndex = 59;
            this.btnPaperAlign.Text = "隔离纸补正";
            this.btnPaperAlign.Click += new System.EventHandler(this.btnPaperAlign_Click);
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.BackColor = System.Drawing.Color.Transparent;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel1.Location = new System.Drawing.Point(3, 52);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(83, 17);
            this.htLabel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel1.TabIndex = 58;
            this.htLabel1.Text = "隔离纸补正值:";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTrayAlign
            // 
            this.btnTrayAlign.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrayAlign.FillColor = System.Drawing.Color.White;
            this.btnTrayAlign.FillColor2 = System.Drawing.Color.Empty;
            this.btnTrayAlign.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTrayAlign.ForeColor = System.Drawing.Color.Black;
            this.btnTrayAlign.Location = new System.Drawing.Point(325, 20);
            this.btnTrayAlign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTrayAlign.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnTrayAlign.Name = "btnTrayAlign";
            this.btnTrayAlign.Size = new System.Drawing.Size(78, 25);
            this.btnTrayAlign.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnTrayAlign.TabIndex = 59;
            this.btnTrayAlign.Text = "Tray补正";
            this.btnTrayAlign.Click += new System.EventHandler(this.btnTrayAlign_Click);
            // 
            // tbxPaperXyt
            // 
            this.tbxPaperXyt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxPaperXyt.FillColor = System.Drawing.Color.White;
            this.tbxPaperXyt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxPaperXyt.Location = new System.Drawing.Point(90, 49);
            this.tbxPaperXyt.Maximum = 2147483647D;
            this.tbxPaperXyt.Minimum = -2147483648D;
            this.tbxPaperXyt.Name = "tbxPaperXyt";
            this.tbxPaperXyt.Padding = new System.Windows.Forms.Padding(5);
            this.tbxPaperXyt.Size = new System.Drawing.Size(145, 23);
            this.tbxPaperXyt.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxPaperXyt.TabIndex = 60;
            // 
            // htLabel14
            // 
            this.htLabel14.AutoSize = true;
            this.htLabel14.BackColor = System.Drawing.Color.Transparent;
            this.htLabel14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel14.Location = new System.Drawing.Point(14, 24);
            this.htLabel14.Name = "htLabel14";
            this.htLabel14.Size = new System.Drawing.Size(72, 17);
            this.htLabel14.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel14.TabIndex = 58;
            this.htLabel14.Text = "Tray补正值:";
            this.htLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxTrayXyt
            // 
            this.tbxTrayXyt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxTrayXyt.FillColor = System.Drawing.Color.White;
            this.tbxTrayXyt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxTrayXyt.Location = new System.Drawing.Point(90, 20);
            this.tbxTrayXyt.Maximum = 2147483647D;
            this.tbxTrayXyt.Minimum = -2147483648D;
            this.tbxTrayXyt.Name = "tbxTrayXyt";
            this.tbxTrayXyt.Padding = new System.Windows.Forms.Padding(5);
            this.tbxTrayXyt.Size = new System.Drawing.Size(145, 23);
            this.tbxTrayXyt.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxTrayXyt.TabIndex = 60;
            // 
            // htGroupBox3
            // 
            this.htGroupBox3.Controls.Add(this.btnWaitPos);
            this.htGroupBox3.Controls.Add(this.btnPutPaperToTrayPos);
            this.htGroupBox3.Controls.Add(this.btnPaperGrabPos);
            this.htGroupBox3.Controls.Add(this.btnPickPaperFromStagePos);
            this.htGroupBox3.Controls.Add(this.btnTrayGrabPos);
            this.htGroupBox3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox3.Location = new System.Drawing.Point(6, 72);
            this.htGroupBox3.Name = "htGroupBox3";
            this.htGroupBox3.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox3.Size = new System.Drawing.Size(423, 97);
            this.htGroupBox3.TabIndex = 56;
            this.htGroupBox3.Text = "快速定位";
            this.htGroupBox3.TitleTop = 8;
            // 
            // btnWaitPos
            // 
            this.btnWaitPos.BackColor = System.Drawing.Color.White;
            this.btnWaitPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWaitPos.FillColor = System.Drawing.Color.White;
            this.btnWaitPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnWaitPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWaitPos.ForeColor = System.Drawing.Color.Black;
            this.btnWaitPos.Location = new System.Drawing.Point(15, 23);
            this.btnWaitPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWaitPos.Name = "btnWaitPos";
            this.btnWaitPos.Size = new System.Drawing.Size(105, 28);
            this.btnWaitPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnWaitPos.TabIndex = 50;
            this.btnWaitPos.Tag = "14";
            this.btnWaitPos.Text = "待机位置";
            this.btnWaitPos.Click += new System.EventHandler(this.btnWaitPos_Click);
            // 
            // btnPutPaperToTrayPos
            // 
            this.btnPutPaperToTrayPos.BackColor = System.Drawing.Color.White;
            this.btnPutPaperToTrayPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPutPaperToTrayPos.FillColor = System.Drawing.Color.White;
            this.btnPutPaperToTrayPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnPutPaperToTrayPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPutPaperToTrayPos.ForeColor = System.Drawing.Color.Black;
            this.btnPutPaperToTrayPos.Location = new System.Drawing.Point(261, 23);
            this.btnPutPaperToTrayPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPutPaperToTrayPos.Name = "btnPutPaperToTrayPos";
            this.btnPutPaperToTrayPos.Size = new System.Drawing.Size(129, 28);
            this.btnPutPaperToTrayPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPutPaperToTrayPos.TabIndex = 50;
            this.btnPutPaperToTrayPos.Tag = "14";
            this.btnPutPaperToTrayPos.Text = "放隔离纸到Tray盘位置";
            this.btnPutPaperToTrayPos.Click += new System.EventHandler(this.btnPutPaperToTrayPos_Click);
            // 
            // btnPaperGrabPos
            // 
            this.btnPaperGrabPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaperGrabPos.FillColor = System.Drawing.Color.White;
            this.btnPaperGrabPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnPaperGrabPos.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPaperGrabPos.ForeColor = System.Drawing.Color.Black;
            this.btnPaperGrabPos.Location = new System.Drawing.Point(126, 58);
            this.btnPaperGrabPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPaperGrabPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPaperGrabPos.Name = "btnPaperGrabPos";
            this.btnPaperGrabPos.Size = new System.Drawing.Size(129, 25);
            this.btnPaperGrabPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPaperGrabPos.TabIndex = 57;
            this.btnPaperGrabPos.Text = "隔离纸拍照位";
            this.btnPaperGrabPos.Click += new System.EventHandler(this.btnPaperGrabPos_Click);
            // 
            // btnPickPaperFromStagePos
            // 
            this.btnPickPaperFromStagePos.BackColor = System.Drawing.Color.White;
            this.btnPickPaperFromStagePos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickPaperFromStagePos.FillColor = System.Drawing.Color.White;
            this.btnPickPaperFromStagePos.FillColor2 = System.Drawing.Color.Empty;
            this.btnPickPaperFromStagePos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPickPaperFromStagePos.ForeColor = System.Drawing.Color.Black;
            this.btnPickPaperFromStagePos.Location = new System.Drawing.Point(126, 23);
            this.btnPickPaperFromStagePos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnPickPaperFromStagePos.Name = "btnPickPaperFromStagePos";
            this.btnPickPaperFromStagePos.Size = new System.Drawing.Size(129, 28);
            this.btnPickPaperFromStagePos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnPickPaperFromStagePos.TabIndex = 50;
            this.btnPickPaperFromStagePos.Tag = "14";
            this.btnPickPaperFromStagePos.Text = "从对位平台取放纸位置";
            this.btnPickPaperFromStagePos.Click += new System.EventHandler(this.btnPickPaperFromStagePos_Click);
            // 
            // btnTrayGrabPos
            // 
            this.btnTrayGrabPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrayGrabPos.FillColor = System.Drawing.Color.White;
            this.btnTrayGrabPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnTrayGrabPos.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTrayGrabPos.ForeColor = System.Drawing.Color.Black;
            this.btnTrayGrabPos.Location = new System.Drawing.Point(15, 58);
            this.btnTrayGrabPos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTrayGrabPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnTrayGrabPos.Name = "btnTrayGrabPos";
            this.btnTrayGrabPos.Size = new System.Drawing.Size(105, 25);
            this.btnTrayGrabPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnTrayGrabPos.TabIndex = 57;
            this.btnTrayGrabPos.Text = "Tray拍照位";
            this.btnTrayGrabPos.Click += new System.EventHandler(this.btnTrayGrabPos_Click);
            // 
            // IsoPaperStnTeachCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IsoPaperStnTeachCtrl";
            this.Size = new System.Drawing.Size(885, 495);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox6.ResumeLayout(false);
            this.htGroupBox5.ResumeLayout(false);
            this.htGroupBox4.ResumeLayout(false);
            this.htGroupBox2.ResumeLayout(false);
            this.htGroupBox2.PerformLayout();
            this.htGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTGroupBox htGroupBox2;
        private StdLib.WindowsControls.HTGroupBox htGroupBox3;
        private StdLib.WindowsControls.HTButton btnPickPaperFromStagePos;
        private StdLib.WindowsControls.HTGroupBox htGroupBox4;
        private StdLib.WindowsControls.HTGroupBox htGroupBox5;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl3;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl2;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl1;
        private StdLib.PCBaseFrame.UserCtrl.VacuumCtrl vacuumCtrl1;
        private StdLib.PCBaseFrame.UserCtrl.VacuumCtrl vacuumCtrl2;
        private StdLib.PCBaseFrame.UserCtrl.VacuumCtrl vacuumCtrl3;
        private StdLib.WindowsControls.HTButton btnWaitPos;
        private StdLib.WindowsControls.HTButton btnPickPaperFlow;
        private StdLib.WindowsControls.HTButton btnPutPaperToLotTray;
        private StdLib.WindowsControls.HTButton btnAlignPaper;
        private StdLib.WindowsControls.HTButton btnPaperGrab;
        private StdLib.WindowsControls.HTButton btnTrayGrab;
        private StdLib.WindowsControls.HTButton btnPaperGrabPos;
        private StdLib.WindowsControls.HTButton btnPaperAlign;
        private StdLib.WindowsControls.HTButton btnTrayGrabPos;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.WindowsControls.HTButton btnTrayAlign;
        private StdLib.WindowsControls.HTTextBox tbxPaperXyt;
        private StdLib.WindowsControls.HTLabel htLabel14;
        private StdLib.WindowsControls.HTTextBox tbxTrayXyt;
        private StdLib.WindowsControls.HTButton btnPutPaperToTrayPos;
        private StdLib.WindowsControls.HTGroupBox htGroupBox6;
        private StdLib.WindowsControls.HTButton btnResume;
        private StdLib.WindowsControls.HTButton btnAutoPickStop;
        private StdLib.WindowsControls.HTButton btnPause;
        private StdLib.WindowsControls.HTButton btnAlignTray;
    }
}
