
namespace JC.AutoMating.UserCtrl
{
    partial class YieldQueryCtrl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.tbpnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnQuery = new StdLib.WindowsControls.HTButton();
            this.tbxLot = new System.Windows.Forms.TextBox();
            this.tbxPanelID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtPickerTime1 = new System.Windows.Forms.DateTimePicker();
            this.dtPickerDate2 = new System.Windows.Forms.DateTimePicker();
            this.cmxLegendList = new System.Windows.Forms.ComboBox();
            this.cmxCategory = new System.Windows.Forms.ComboBox();
            this.cmxResultType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ckbNow1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtPickerDate1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtPickerTime2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.htTabControl1 = new StdLib.WindowsControls.HTTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvData = new StdLib.WindowsControls.HTDataGridViewUnit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询DieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询选中TrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询LotBox中LotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnClearDataTable = new StdLib.WindowsControls.HTButton();
            this.tbpnlMain.SuspendLayout();
            this.panel4.SuspendLayout();
            this.htTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbpnlMain
            // 
            this.tbpnlMain.ColumnCount = 1;
            this.tbpnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlMain.Controls.Add(this.panel4, 0, 0);
            this.tbpnlMain.Controls.Add(this.htTabControl1, 0, 1);
            this.tbpnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbpnlMain.Location = new System.Drawing.Point(0, 0);
            this.tbpnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpnlMain.Name = "tbpnlMain";
            this.tbpnlMain.RowCount = 2;
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tbpnlMain.Size = new System.Drawing.Size(958, 1045);
            this.tbpnlMain.TabIndex = 21;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnClearDataTable);
            this.panel4.Controls.Add(this.btnQuery);
            this.panel4.Controls.Add(this.tbxLot);
            this.panel4.Controls.Add(this.tbxPanelID);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.dtPickerTime1);
            this.panel4.Controls.Add(this.dtPickerDate2);
            this.panel4.Controls.Add(this.cmxLegendList);
            this.panel4.Controls.Add(this.cmxCategory);
            this.panel4.Controls.Add(this.cmxResultType);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.ckbNow1);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.dtPickerDate1);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.dtPickerTime2);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(952, 74);
            this.panel4.TabIndex = 5;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuery.FillColor = System.Drawing.Color.White;
            this.btnQuery.FillColor2 = System.Drawing.Color.Empty;
            this.btnQuery.ForeColor = System.Drawing.Color.Black;
            this.btnQuery.Location = new System.Drawing.Point(847, 41);
            this.btnQuery.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.RectSize = 2;
            this.btnQuery.Size = new System.Drawing.Size(100, 28);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tbxLot
            // 
            this.tbxLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxLot.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxLot.Location = new System.Drawing.Point(531, 43);
            this.tbxLot.Name = "tbxLot";
            this.tbxLot.Size = new System.Drawing.Size(147, 23);
            this.tbxLot.TabIndex = 12;
            // 
            // tbxPanelID
            // 
            this.tbxPanelID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxPanelID.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxPanelID.Location = new System.Drawing.Point(531, 13);
            this.tbxPanelID.Name = "tbxPanelID";
            this.tbxPanelID.Size = new System.Drawing.Size(147, 23);
            this.tbxPanelID.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(9, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "起始:";
            // 
            // dtPickerTime1
            // 
            this.dtPickerTime1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtPickerTime1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtPickerTime1.Location = new System.Drawing.Point(183, 10);
            this.dtPickerTime1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtPickerTime1.Name = "dtPickerTime1";
            this.dtPickerTime1.ShowUpDown = true;
            this.dtPickerTime1.Size = new System.Drawing.Size(83, 23);
            this.dtPickerTime1.TabIndex = 7;
            // 
            // dtPickerDate2
            // 
            this.dtPickerDate2.Enabled = false;
            this.dtPickerDate2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtPickerDate2.Location = new System.Drawing.Point(48, 41);
            this.dtPickerDate2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtPickerDate2.Name = "dtPickerDate2";
            this.dtPickerDate2.Size = new System.Drawing.Size(132, 23);
            this.dtPickerDate2.TabIndex = 3;
            this.dtPickerDate2.ValueChanged += new System.EventHandler(this.dtPickerDate1_ValueChanged);
            // 
            // cmxLegendList
            // 
            this.cmxLegendList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmxLegendList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmxLegendList.FormattingEnabled = true;
            this.cmxLegendList.Items.AddRange(new object[] {
            "柱状图",
            "曲线图",
            "饼图"});
            this.cmxLegendList.Location = new System.Drawing.Point(720, 12);
            this.cmxLegendList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmxLegendList.Name = "cmxLegendList";
            this.cmxLegendList.Size = new System.Drawing.Size(121, 25);
            this.cmxLegendList.TabIndex = 11;
            this.cmxLegendList.Visible = false;
            this.cmxLegendList.SelectedIndexChanged += new System.EventHandler(this.cmxLegendList_SelectedIndexChanged);
            // 
            // cmxCategory
            // 
            this.cmxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmxCategory.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmxCategory.FormattingEnabled = true;
            this.cmxCategory.Items.AddRange(new object[] {
            "Die",
            "Tray",
            "Wafer",
            "Lot",
            "LotBox"});
            this.cmxCategory.Location = new System.Drawing.Point(395, 12);
            this.cmxCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmxCategory.Name = "cmxCategory";
            this.cmxCategory.Size = new System.Drawing.Size(87, 25);
            this.cmxCategory.TabIndex = 11;
            // 
            // cmxResultType
            // 
            this.cmxResultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmxResultType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmxResultType.FormattingEnabled = true;
            this.cmxResultType.Location = new System.Drawing.Point(395, 41);
            this.cmxResultType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmxResultType.Name = "cmxResultType";
            this.cmxResultType.Size = new System.Drawing.Size(87, 25);
            this.cmxResultType.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(683, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "图例：";
            this.label5.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(333, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "查询类别：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(493, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "批次：";
            // 
            // ckbNow1
            // 
            this.ckbNow1.AutoSize = true;
            this.ckbNow1.Checked = true;
            this.ckbNow1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNow1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbNow1.Location = new System.Drawing.Point(272, 44);
            this.ckbNow1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckbNow1.Name = "ckbNow1";
            this.ckbNow1.Size = new System.Drawing.Size(51, 21);
            this.ckbNow1.TabIndex = 9;
            this.ckbNow1.Text = "现在";
            this.ckbNow1.UseVisualStyleBackColor = true;
            this.ckbNow1.CheckedChanged += new System.EventHandler(this.ckbNow1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(493, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "条码：";
            // 
            // dtPickerDate1
            // 
            this.dtPickerDate1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtPickerDate1.Location = new System.Drawing.Point(48, 10);
            this.dtPickerDate1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtPickerDate1.Name = "dtPickerDate1";
            this.dtPickerDate1.Size = new System.Drawing.Size(132, 23);
            this.dtPickerDate1.TabIndex = 2;
            this.dtPickerDate1.ValueChanged += new System.EventHandler(this.dtPickerDate1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(357, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "结果：";
            // 
            // dtPickerTime2
            // 
            this.dtPickerTime2.Enabled = false;
            this.dtPickerTime2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtPickerTime2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtPickerTime2.Location = new System.Drawing.Point(183, 41);
            this.dtPickerTime2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtPickerTime2.Name = "dtPickerTime2";
            this.dtPickerTime2.ShowUpDown = true;
            this.dtPickerTime2.Size = new System.Drawing.Size(83, 23);
            this.dtPickerTime2.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "截止:";
            // 
            // htTabControl1
            // 
            this.htTabControl1.Controls.Add(this.tabPage1);
            this.htTabControl1.Controls.Add(this.tabPage2);
            this.htTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.htTabControl1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTabControl1.ItemSize = new System.Drawing.Size(150, 28);
            this.htTabControl1.Location = new System.Drawing.Point(0, 82);
            this.htTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.htTabControl1.MenuStyle = StdLib.WindowsControls.HTMenuStyle.Custom;
            this.htTabControl1.Name = "htTabControl1";
            this.htTabControl1.SelectedIndex = 0;
            this.htTabControl1.Size = new System.Drawing.Size(958, 963);
            this.htTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.htTabControl1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTabControl1.TabBackColor = System.Drawing.Color.White;
            this.htTabControl1.TabIndex = 6;
            this.htTabControl1.TabSelectedColor = System.Drawing.Color.LightCyan;
            this.htTabControl1.TabSelectedForeColor = System.Drawing.Color.Black;
            this.htTabControl1.TabSelectedHighColor = System.Drawing.Color.LightCyan;
            this.htTabControl1.TabUnSelectedForeColor = System.Drawing.Color.Gray;
            this.htTabControl1.SelectedIndexChanged += new System.EventHandler(this.htTabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvData);
            this.tabPage1.Location = new System.Drawing.Point(0, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(958, 935);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "表格";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.BackColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvData.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvData.CountPerPage = 50;
            this.dgvData.DataSource = null;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Margin = new System.Windows.Forms.Padding(0);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(958, 935);
            this.dgvData.TabIndex = 6;
            this.dgvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem,
            this.查询DieToolStripMenuItem,
            this.查询选中TrayToolStripMenuItem,
            this.查询LotBox中LotToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 92);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 查询DieToolStripMenuItem
            // 
            this.查询DieToolStripMenuItem.Name = "查询DieToolStripMenuItem";
            this.查询DieToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.查询DieToolStripMenuItem.Text = "查询Tray中Die";
            this.查询DieToolStripMenuItem.Click += new System.EventHandler(this.查询DieToolStripMenuItem_Click);
            // 
            // 查询选中TrayToolStripMenuItem
            // 
            this.查询选中TrayToolStripMenuItem.Name = "查询选中TrayToolStripMenuItem";
            this.查询选中TrayToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.查询选中TrayToolStripMenuItem.Text = "查询Lot中Tray";
            this.查询选中TrayToolStripMenuItem.Click += new System.EventHandler(this.查询DieToolStripMenuItem_Click);
            // 
            // 查询LotBox中LotToolStripMenuItem
            // 
            this.查询LotBox中LotToolStripMenuItem.Name = "查询LotBox中LotToolStripMenuItem";
            this.查询LotBox中LotToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.查询LotBox中LotToolStripMenuItem.Text = "查询LotBox中Lot";
            this.查询LotBox中LotToolStripMenuItem.Click += new System.EventHandler(this.查询DieToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(0, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(958, 935);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "图表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.chart1.BorderlineColor = System.Drawing.Color.Pink;
            this.chart1.BorderSkin.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            this.chart1.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Sunken;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(958, 935);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            title2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.Name = "MattingResultChart";
            title2.Position.Auto = false;
            title2.Position.Height = 3.560141F;
            title2.Position.Width = 94F;
            title2.Position.X = 3F;
            title2.Position.Y = 1F;
            this.chart1.Titles.Add(title2);
            // 
            // btnClearDataTable
            // 
            this.btnClearDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDataTable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearDataTable.FillColor = System.Drawing.Color.White;
            this.btnClearDataTable.FillColor2 = System.Drawing.Color.Empty;
            this.btnClearDataTable.ForeColor = System.Drawing.Color.Black;
            this.btnClearDataTable.Location = new System.Drawing.Point(847, 10);
            this.btnClearDataTable.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearDataTable.Name = "btnClearDataTable";
            this.btnClearDataTable.RectSize = 2;
            this.btnClearDataTable.Size = new System.Drawing.Size(100, 28);
            this.btnClearDataTable.TabIndex = 13;
            this.btnClearDataTable.Text = "清空数据表";
            this.btnClearDataTable.Click += new System.EventHandler(this.btnClearDataBase_Click);
            // 
            // YieldQueryCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbpnlMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "YieldQueryCtrl";
            this.Size = new System.Drawing.Size(958, 1045);
            this.Load += new System.EventHandler(this.YieldQueryCtrl_Load);
            this.VisibleChanged += new System.EventHandler(this.YieldQueryCtrl_VisibleChanged);
            this.tbpnlMain.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.htTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbpnlMain;
        private StdLib.WindowsControls.HTDataGridViewUnit dgvData;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DateTimePicker dtPickerTime1;
        private System.Windows.Forms.DateTimePicker dtPickerDate2;
        private System.Windows.Forms.CheckBox ckbNow1;
        private System.Windows.Forms.DateTimePicker dtPickerDate1;
        private System.Windows.Forms.DateTimePicker dtPickerTime2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmxResultType;
        private System.Windows.Forms.Label label1;
        private StdLib.WindowsControls.HTTabControl htTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox cmxLegendList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxPanelID;
        private System.Windows.Forms.Label label6;
        private StdLib.WindowsControls.HTButton btnQuery;
        private System.Windows.Forms.ComboBox cmxCategory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查询DieToolStripMenuItem;
        private System.Windows.Forms.TextBox tbxLot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem 查询选中TrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查询LotBox中LotToolStripMenuItem;
        private StdLib.WindowsControls.HTButton btnClearDataTable;
    }
}
