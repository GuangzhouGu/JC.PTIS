namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class LotBoxTeachCtrl
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
            this.htGroupBox4 = new StdLib.WindowsControls.HTGroupBox();
            this.htLabel2 = new StdLib.WindowsControls.HTLabel();
            this.htLabel9 = new StdLib.WindowsControls.HTLabel();
            this.tbxRfid = new StdLib.WindowsControls.HTTextBox();
            this.tbxCode = new StdLib.WindowsControls.HTTextBox();
            this.btnReadRfid = new StdLib.WindowsControls.HTButton();
            this.btnScan = new StdLib.WindowsControls.HTButton();
            this.htGroupBox2 = new StdLib.WindowsControls.HTGroupBox();
            this.btnNgLotMoveToStage2 = new StdLib.WindowsControls.HTButton();
            this.btnEmptyLotMoveToStage2 = new StdLib.WindowsControls.HTButton();
            this.htGroupBox3 = new StdLib.WindowsControls.HTGroupBox();
            this.cmxExBoxStageList = new StdLib.WindowsControls.HTComboBox();
            this.htLabel1 = new StdLib.WindowsControls.HTLabel();
            this.cmxInnerBoxStageList = new StdLib.WindowsControls.HTComboBox();
            this.htLabel4 = new StdLib.WindowsControls.HTLabel();
            this.htButton6 = new StdLib.WindowsControls.HTButton();
            this.btnWaitPos = new StdLib.WindowsControls.HTButton();
            this.htButton5 = new StdLib.WindowsControls.HTButton();
            this.htButton4 = new StdLib.WindowsControls.HTButton();
            this.btnMoveToDestStage = new StdLib.WindowsControls.HTButton();
            this.htGroupBox6 = new StdLib.WindowsControls.HTGroupBox();
            this.btnResume = new StdLib.WindowsControls.HTButton();
            this.btnAutoPickStop = new StdLib.WindowsControls.HTButton();
            this.btnPause = new StdLib.WindowsControls.HTButton();
            this.htGroupBox5 = new StdLib.WindowsControls.HTGroupBox();
            this.cylinderCtrl1 = new StdLib.PCBaseFrame.UserCtrl.CylinderCtrl();
            this.htGroupBox1.SuspendLayout();
            this.htGroupBox4.SuspendLayout();
            this.htGroupBox2.SuspendLayout();
            this.htGroupBox3.SuspendLayout();
            this.htGroupBox6.SuspendLayout();
            this.htGroupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox1.Controls.Add(this.htGroupBox4);
            this.htGroupBox1.Controls.Add(this.htGroupBox2);
            this.htGroupBox1.Controls.Add(this.htGroupBox3);
            this.htGroupBox1.Controls.Add(this.htGroupBox6);
            this.htGroupBox1.Controls.Add(this.htGroupBox5);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(4, 34, 4, 4);
            this.htGroupBox1.Size = new System.Drawing.Size(960, 718);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "料盒工位调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // htGroupBox4
            // 
            this.htGroupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox4.Controls.Add(this.htLabel2);
            this.htGroupBox4.Controls.Add(this.htLabel9);
            this.htGroupBox4.Controls.Add(this.tbxRfid);
            this.htGroupBox4.Controls.Add(this.tbxCode);
            this.htGroupBox4.Controls.Add(this.btnReadRfid);
            this.htGroupBox4.Controls.Add(this.btnScan);
            this.htGroupBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox4.Location = new System.Drawing.Point(7, 225);
            this.htGroupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox4.Name = "htGroupBox4";
            this.htGroupBox4.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox4.Size = new System.Drawing.Size(428, 82);
            this.htGroupBox4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox4.TabIndex = 60;
            this.htGroupBox4.Text = "扫码";
            this.htGroupBox4.TitleTop = 8;
            // 
            // htLabel2
            // 
            this.htLabel2.AutoSize = true;
            this.htLabel2.BackColor = System.Drawing.Color.Transparent;
            this.htLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel2.Location = new System.Drawing.Point(43, 52);
            this.htLabel2.Name = "htLabel2";
            this.htLabel2.Size = new System.Drawing.Size(34, 17);
            this.htLabel2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel2.TabIndex = 56;
            this.htLabel2.Text = "Rfid:";
            this.htLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htLabel9
            // 
            this.htLabel9.AutoSize = true;
            this.htLabel9.BackColor = System.Drawing.Color.Transparent;
            this.htLabel9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htLabel9.Location = new System.Drawing.Point(43, 19);
            this.htLabel9.Name = "htLabel9";
            this.htLabel9.Size = new System.Drawing.Size(35, 17);
            this.htLabel9.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel9.TabIndex = 56;
            this.htLabel9.Text = "条码:";
            this.htLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxRfid
            // 
            this.tbxRfid.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxRfid.FillColor = System.Drawing.Color.White;
            this.tbxRfid.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxRfid.Location = new System.Drawing.Point(80, 50);
            this.tbxRfid.Maximum = 2147483647D;
            this.tbxRfid.Minimum = -2147483648D;
            this.tbxRfid.Name = "tbxRfid";
            this.tbxRfid.Padding = new System.Windows.Forms.Padding(5);
            this.tbxRfid.Size = new System.Drawing.Size(145, 23);
            this.tbxRfid.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxRfid.TabIndex = 57;
            // 
            // tbxCode
            // 
            this.tbxCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxCode.FillColor = System.Drawing.Color.White;
            this.tbxCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxCode.Location = new System.Drawing.Point(80, 17);
            this.tbxCode.Maximum = 2147483647D;
            this.tbxCode.Minimum = -2147483648D;
            this.tbxCode.Name = "tbxCode";
            this.tbxCode.Padding = new System.Windows.Forms.Padding(5);
            this.tbxCode.Size = new System.Drawing.Size(145, 23);
            this.tbxCode.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxCode.TabIndex = 57;
            // 
            // btnReadRfid
            // 
            this.btnReadRfid.BackColor = System.Drawing.Color.White;
            this.btnReadRfid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadRfid.FillColor = System.Drawing.Color.White;
            this.btnReadRfid.FillColor2 = System.Drawing.Color.Empty;
            this.btnReadRfid.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadRfid.ForeColor = System.Drawing.Color.Black;
            this.btnReadRfid.Location = new System.Drawing.Point(231, 48);
            this.btnReadRfid.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadRfid.Name = "btnReadRfid";
            this.btnReadRfid.Size = new System.Drawing.Size(100, 28);
            this.btnReadRfid.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnReadRfid.TabIndex = 50;
            this.btnReadRfid.Tag = "14";
            this.btnReadRfid.Text = "读取Rfid";
            this.btnReadRfid.Click += new System.EventHandler(this.btnReadRfid_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.White;
            this.btnScan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScan.FillColor = System.Drawing.Color.White;
            this.btnScan.FillColor2 = System.Drawing.Color.Empty;
            this.btnScan.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnScan.ForeColor = System.Drawing.Color.Black;
            this.btnScan.Location = new System.Drawing.Point(231, 14);
            this.btnScan.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(100, 28);
            this.btnScan.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnScan.TabIndex = 50;
            this.btnScan.Tag = "14";
            this.btnScan.Text = "扫码";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // htGroupBox2
            // 
            this.htGroupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox2.Controls.Add(this.btnNgLotMoveToStage2);
            this.htGroupBox2.Controls.Add(this.btnEmptyLotMoveToStage2);
            this.htGroupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox2.Location = new System.Drawing.Point(7, 309);
            this.htGroupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox2.Name = "htGroupBox2";
            this.htGroupBox2.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox2.Size = new System.Drawing.Size(428, 116);
            this.htGroupBox2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox2.TabIndex = 60;
            this.htGroupBox2.Text = "流程调试";
            this.htGroupBox2.TitleTop = 8;
            // 
            // btnNgLotMoveToStage2
            // 
            this.btnNgLotMoveToStage2.BackColor = System.Drawing.Color.White;
            this.btnNgLotMoveToStage2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNgLotMoveToStage2.FillColor = System.Drawing.Color.White;
            this.btnNgLotMoveToStage2.FillColor2 = System.Drawing.Color.Empty;
            this.btnNgLotMoveToStage2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNgLotMoveToStage2.ForeColor = System.Drawing.Color.Black;
            this.btnNgLotMoveToStage2.Location = new System.Drawing.Point(163, 28);
            this.btnNgLotMoveToStage2.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnNgLotMoveToStage2.Name = "btnNgLotMoveToStage2";
            this.btnNgLotMoveToStage2.Size = new System.Drawing.Size(143, 28);
            this.btnNgLotMoveToStage2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnNgLotMoveToStage2.TabIndex = 50;
            this.btnNgLotMoveToStage2.Tag = "14";
            this.btnNgLotMoveToStage2.Text = "NgTrayLot移动到平台2";
            this.btnNgLotMoveToStage2.Click += new System.EventHandler(this.btnNgLotMoveToStage2_Click);
            // 
            // btnEmptyLotMoveToStage2
            // 
            this.btnEmptyLotMoveToStage2.BackColor = System.Drawing.Color.White;
            this.btnEmptyLotMoveToStage2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmptyLotMoveToStage2.FillColor = System.Drawing.Color.White;
            this.btnEmptyLotMoveToStage2.FillColor2 = System.Drawing.Color.Empty;
            this.btnEmptyLotMoveToStage2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEmptyLotMoveToStage2.ForeColor = System.Drawing.Color.Black;
            this.btnEmptyLotMoveToStage2.Location = new System.Drawing.Point(14, 28);
            this.btnEmptyLotMoveToStage2.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnEmptyLotMoveToStage2.Name = "btnEmptyLotMoveToStage2";
            this.btnEmptyLotMoveToStage2.Size = new System.Drawing.Size(143, 28);
            this.btnEmptyLotMoveToStage2.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnEmptyLotMoveToStage2.TabIndex = 50;
            this.btnEmptyLotMoveToStage2.Tag = "14";
            this.btnEmptyLotMoveToStage2.Text = "空TrayLot移动到平台2";
            this.btnEmptyLotMoveToStage2.Click += new System.EventHandler(this.btnEmptyLotMoveToStage2_Click);
            // 
            // htGroupBox3
            // 
            this.htGroupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox3.Controls.Add(this.cmxExBoxStageList);
            this.htGroupBox3.Controls.Add(this.htLabel1);
            this.htGroupBox3.Controls.Add(this.cmxInnerBoxStageList);
            this.htGroupBox3.Controls.Add(this.htLabel4);
            this.htGroupBox3.Controls.Add(this.htButton6);
            this.htGroupBox3.Controls.Add(this.btnWaitPos);
            this.htGroupBox3.Controls.Add(this.htButton5);
            this.htGroupBox3.Controls.Add(this.htButton4);
            this.htGroupBox3.Controls.Add(this.btnMoveToDestStage);
            this.htGroupBox3.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox3.Location = new System.Drawing.Point(7, 73);
            this.htGroupBox3.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox3.Name = "htGroupBox3";
            this.htGroupBox3.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox3.Size = new System.Drawing.Size(428, 151);
            this.htGroupBox3.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox3.TabIndex = 60;
            this.htGroupBox3.Text = "快速定位";
            this.htGroupBox3.TitleTop = 8;
            // 
            // cmxExBoxStageList
            // 
            this.cmxExBoxStageList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxExBoxStageList.FillColor = System.Drawing.Color.White;
            this.cmxExBoxStageList.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxExBoxStageList.Items.AddRange(new object[] {
            "内盒平台1",
            "内盒平台2",
            "内盒平台3",
            "内盒平台4"});
            this.cmxExBoxStageList.Location = new System.Drawing.Point(80, 83);
            this.cmxExBoxStageList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxExBoxStageList.Name = "cmxExBoxStageList";
            this.cmxExBoxStageList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxExBoxStageList.Size = new System.Drawing.Size(144, 22);
            this.cmxExBoxStageList.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.cmxExBoxStageList.TabIndex = 54;
            this.cmxExBoxStageList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmxExBoxStageList.SelectedIndexChanged += new System.EventHandler(this.cmxStageList_SelectedIndexChanged);
            // 
            // htLabel1
            // 
            this.htLabel1.AutoSize = true;
            this.htLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel1.Location = new System.Drawing.Point(10, 82);
            this.htLabel1.Name = "htLabel1";
            this.htLabel1.Size = new System.Drawing.Size(68, 20);
            this.htLabel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel1.TabIndex = 53;
            this.htLabel1.Text = "外盒平台:";
            this.htLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmxInnerBoxStageList
            // 
            this.cmxInnerBoxStageList.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxInnerBoxStageList.FillColor = System.Drawing.Color.White;
            this.cmxInnerBoxStageList.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxInnerBoxStageList.Items.AddRange(new object[] {
            "内盒平台1",
            "内盒平台2",
            "内盒平台3",
            "内盒平台4"});
            this.cmxInnerBoxStageList.Location = new System.Drawing.Point(80, 21);
            this.cmxInnerBoxStageList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxInnerBoxStageList.Name = "cmxInnerBoxStageList";
            this.cmxInnerBoxStageList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxInnerBoxStageList.Size = new System.Drawing.Size(144, 22);
            this.cmxInnerBoxStageList.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.cmxInnerBoxStageList.TabIndex = 54;
            this.cmxInnerBoxStageList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmxInnerBoxStageList.SelectedIndexChanged += new System.EventHandler(this.cmxStageList_SelectedIndexChanged);
            // 
            // htLabel4
            // 
            this.htLabel4.AutoSize = true;
            this.htLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htLabel4.Location = new System.Drawing.Point(10, 20);
            this.htLabel4.Name = "htLabel4";
            this.htLabel4.Size = new System.Drawing.Size(68, 20);
            this.htLabel4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htLabel4.TabIndex = 53;
            this.htLabel4.Text = "内盒平台:";
            this.htLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // htButton6
            // 
            this.htButton6.BackColor = System.Drawing.Color.White;
            this.htButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.htButton6.FillColor = System.Drawing.Color.White;
            this.htButton6.FillColor2 = System.Drawing.Color.Empty;
            this.htButton6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htButton6.ForeColor = System.Drawing.Color.Black;
            this.htButton6.Location = new System.Drawing.Point(263, 111);
            this.htButton6.MinimumSize = new System.Drawing.Size(1, 1);
            this.htButton6.Name = "htButton6";
            this.htButton6.Size = new System.Drawing.Size(142, 28);
            this.htButton6.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htButton6.TabIndex = 50;
            this.htButton6.Tag = "14";
            this.htButton6.Text = "进入外盒平台前位置";
            this.htButton6.Click += new System.EventHandler(this.htButton6_Click);
            // 
            // btnWaitPos
            // 
            this.btnWaitPos.BackColor = System.Drawing.Color.White;
            this.btnWaitPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWaitPos.FillColor = System.Drawing.Color.White;
            this.btnWaitPos.FillColor2 = System.Drawing.Color.Empty;
            this.btnWaitPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWaitPos.ForeColor = System.Drawing.Color.Black;
            this.btnWaitPos.Location = new System.Drawing.Point(9, 49);
            this.btnWaitPos.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnWaitPos.Name = "btnWaitPos";
            this.btnWaitPos.Size = new System.Drawing.Size(100, 28);
            this.btnWaitPos.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnWaitPos.TabIndex = 50;
            this.btnWaitPos.Tag = "14";
            this.btnWaitPos.Text = "待机位置";
            this.btnWaitPos.Click += new System.EventHandler(this.btnWaitPos_Click);
            // 
            // htButton5
            // 
            this.htButton5.BackColor = System.Drawing.Color.White;
            this.htButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.htButton5.FillColor = System.Drawing.Color.White;
            this.htButton5.FillColor2 = System.Drawing.Color.Empty;
            this.htButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htButton5.ForeColor = System.Drawing.Color.Black;
            this.htButton5.Location = new System.Drawing.Point(115, 111);
            this.htButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.htButton5.Name = "htButton5";
            this.htButton5.Size = new System.Drawing.Size(142, 28);
            this.htButton5.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htButton5.TabIndex = 50;
            this.htButton5.Tag = "14";
            this.htButton5.Text = "外盒平台取放内盒位置";
            this.htButton5.Click += new System.EventHandler(this.htButton5_Click);
            // 
            // htButton4
            // 
            this.htButton4.BackColor = System.Drawing.Color.White;
            this.htButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.htButton4.FillColor = System.Drawing.Color.White;
            this.htButton4.FillColor2 = System.Drawing.Color.Empty;
            this.htButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.htButton4.ForeColor = System.Drawing.Color.Black;
            this.htButton4.Location = new System.Drawing.Point(263, 49);
            this.htButton4.MinimumSize = new System.Drawing.Size(1, 1);
            this.htButton4.Name = "htButton4";
            this.htButton4.Size = new System.Drawing.Size(142, 28);
            this.htButton4.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htButton4.TabIndex = 50;
            this.htButton4.Tag = "14";
            this.htButton4.Text = "内盒平台扫码位置";
            this.htButton4.Click += new System.EventHandler(this.htButton4_Click);
            // 
            // btnMoveToDestStage
            // 
            this.btnMoveToDestStage.BackColor = System.Drawing.Color.White;
            this.btnMoveToDestStage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoveToDestStage.FillColor = System.Drawing.Color.White;
            this.btnMoveToDestStage.FillColor2 = System.Drawing.Color.Empty;
            this.btnMoveToDestStage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveToDestStage.ForeColor = System.Drawing.Color.Black;
            this.btnMoveToDestStage.Location = new System.Drawing.Point(115, 49);
            this.btnMoveToDestStage.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMoveToDestStage.Name = "btnMoveToDestStage";
            this.btnMoveToDestStage.Size = new System.Drawing.Size(142, 28);
            this.btnMoveToDestStage.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.btnMoveToDestStage.TabIndex = 50;
            this.btnMoveToDestStage.Tag = "14";
            this.btnMoveToDestStage.Text = "内盒平台取放内盒位置";
            this.btnMoveToDestStage.Click += new System.EventHandler(this.btnMoveToDestStage_Click);
            // 
            // htGroupBox6
            // 
            this.htGroupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.htGroupBox6.Controls.Add(this.btnResume);
            this.htGroupBox6.Controls.Add(this.btnAutoPickStop);
            this.htGroupBox6.Controls.Add(this.btnPause);
            this.htGroupBox6.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox6.Location = new System.Drawing.Point(7, 17);
            this.htGroupBox6.Margin = new System.Windows.Forms.Padding(1);
            this.htGroupBox6.Name = "htGroupBox6";
            this.htGroupBox6.Padding = new System.Windows.Forms.Padding(3, 25, 3, 4);
            this.htGroupBox6.Size = new System.Drawing.Size(428, 52);
            this.htGroupBox6.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htGroupBox6.TabIndex = 60;
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
            this.htGroupBox5.Controls.Add(this.cylinderCtrl1);
            this.htGroupBox5.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox5.Location = new System.Drawing.Point(440, 73);
            this.htGroupBox5.Name = "htGroupBox5";
            this.htGroupBox5.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.htGroupBox5.Size = new System.Drawing.Size(423, 234);
            this.htGroupBox5.TabIndex = 59;
            this.htGroupBox5.Text = "气缸真空";
            this.htGroupBox5.TitleTop = 8;
            // 
            // cylinderCtrl1
            // 
            this.cylinderCtrl1.CylinderName = "LotBox1FixLotCyl";
            this.cylinderCtrl1.CylinderNameChn = "内盒平台1固定气缸";
            this.cylinderCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cylinderCtrl1.Location = new System.Drawing.Point(6, 18);
            this.cylinderCtrl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cylinderCtrl1.Name = "cylinderCtrl1";
            this.cylinderCtrl1.Size = new System.Drawing.Size(128, 123);
            this.cylinderCtrl1.StationName = null;
            this.cylinderCtrl1.TabIndex = 59;
            // 
            // LotBoxTeachCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LotBoxTeachCtrl";
            this.Size = new System.Drawing.Size(960, 718);
            this.Load += new System.EventHandler(this.MeasureHeightTeachCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.htGroupBox4.ResumeLayout(false);
            this.htGroupBox4.PerformLayout();
            this.htGroupBox2.ResumeLayout(false);
            this.htGroupBox3.ResumeLayout(false);
            this.htGroupBox3.PerformLayout();
            this.htGroupBox6.ResumeLayout(false);
            this.htGroupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTButton btnMoveToDestStage;
        private StdLib.WindowsControls.HTButton btnWaitPos;
        private StdLib.WindowsControls.HTGroupBox htGroupBox5;
        private StdLib.PCBaseFrame.UserCtrl.CylinderCtrl cylinderCtrl1;
        private StdLib.WindowsControls.HTGroupBox htGroupBox6;
        private StdLib.WindowsControls.HTButton btnResume;
        private StdLib.WindowsControls.HTButton btnAutoPickStop;
        private StdLib.WindowsControls.HTButton btnPause;
        private StdLib.WindowsControls.HTGroupBox htGroupBox3;
        private StdLib.WindowsControls.HTGroupBox htGroupBox2;
        private StdLib.WindowsControls.HTComboBox cmxInnerBoxStageList;
        private StdLib.WindowsControls.HTLabel htLabel4;
        private StdLib.WindowsControls.HTComboBox cmxExBoxStageList;
        private StdLib.WindowsControls.HTLabel htLabel1;
        private StdLib.WindowsControls.HTButton htButton6;
        private StdLib.WindowsControls.HTButton htButton5;
        private StdLib.WindowsControls.HTButton htButton4;
        private StdLib.WindowsControls.HTGroupBox htGroupBox4;
        private StdLib.WindowsControls.HTLabel htLabel2;
        private StdLib.WindowsControls.HTLabel htLabel9;
        private StdLib.WindowsControls.HTTextBox tbxRfid;
        private StdLib.WindowsControls.HTTextBox tbxCode;
        private StdLib.WindowsControls.HTButton btnReadRfid;
        private StdLib.WindowsControls.HTButton btnScan;
        private StdLib.WindowsControls.HTButton btnEmptyLotMoveToStage2;
        private StdLib.WindowsControls.HTButton btnNgLotMoveToStage2;
    }
}
