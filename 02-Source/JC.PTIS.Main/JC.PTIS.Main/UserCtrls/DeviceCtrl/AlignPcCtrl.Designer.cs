
using StdLib.WindowsControls;

namespace JC.LidDispenser.UserCtrl.Devices
{
    partial class AlignPcCtrl
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
            this.groupBox19 = new StdLib.WindowsControls.HTGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbxMsg = new StdLib.WindowsControls.HTTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLaserSend = new StdLib.WindowsControls.HTButton();
            this.lbxLaserMsg = new StdLib.WindowsControls.HCListBox();
            this.cmxCmds = new StdLib.WindowsControls.HTComboBox();
            this.btnClearLaserMsg = new StdLib.WindowsControls.HTButton();
            this.label48 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmxAlignStation = new StdLib.WindowsControls.HTComboBox();
            this.cmxStnNo = new StdLib.WindowsControls.HTComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCalibration = new StdLib.WindowsControls.HTButton();
            this.btnMotionRequest = new StdLib.WindowsControls.HTButton();
            this.groupBox19.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.tableLayoutPanel1);
            this.groupBox19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox19.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.groupBox19.Location = new System.Drawing.Point(0, 0);
            this.groupBox19.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Padding = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.groupBox19.Size = new System.Drawing.Size(408, 454);
            this.groupBox19.TabIndex = 14;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "对位PC调试";
            this.groupBox19.TitleTop = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbxMsg, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLaserSend, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxLaserMsg, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmxCmds, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClearLaserMsg, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label48, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmxAlignStation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmxStnNo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCalibration, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnMotionRequest, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 20);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(402, 431);
            this.tableLayoutPanel1.TabIndex = 42;
            // 
            // tbxMsg
            // 
            this.tbxMsg.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxMsg.FillColor = System.Drawing.Color.White;
            this.tbxMsg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxMsg.Location = new System.Drawing.Point(103, 93);
            this.tbxMsg.Maximum = 2147483647D;
            this.tbxMsg.Minimum = -2147483648D;
            this.tbxMsg.Name = "tbxMsg";
            this.tbxMsg.Padding = new System.Windows.Forms.Padding(5);
            this.tbxMsg.Size = new System.Drawing.Size(196, 23);
            this.tbxMsg.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.tbxMsg.TabIndex = 58;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(32, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 24);
            this.label1.TabIndex = 42;
            this.label1.Text = "对位序号";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLaserSend
            // 
            this.btnLaserSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLaserSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLaserSend.FillColor = System.Drawing.Color.White;
            this.btnLaserSend.FillColor2 = System.Drawing.Color.Empty;
            this.btnLaserSend.ForeColor = System.Drawing.Color.Black;
            this.btnLaserSend.Location = new System.Drawing.Point(305, 3);
            this.btnLaserSend.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLaserSend.Name = "btnLaserSend";
            this.btnLaserSend.Size = new System.Drawing.Size(94, 24);
            this.btnLaserSend.TabIndex = 41;
            this.btnLaserSend.Text = "发送";
            this.btnLaserSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lbxLaserMsg
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lbxLaserMsg, 3);
            this.lbxLaserMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLaserMsg.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxLaserMsg.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lbxLaserMsg.FormattingEnabled = true;
            this.lbxLaserMsg.HorizontalScrollbar = true;
            this.lbxLaserMsg.IsShowMillisecond = false;
            this.lbxLaserMsg.ItemHeight = 20;
            this.lbxLaserMsg.Location = new System.Drawing.Point(1, 151);
            this.lbxLaserMsg.Margin = new System.Windows.Forms.Padding(1);
            this.lbxLaserMsg.Name = "lbxLaserMsg";
            this.lbxLaserMsg.SelectionBackColor = System.Drawing.Color.SkyBlue;
            this.lbxLaserMsg.Size = new System.Drawing.Size(400, 279);
            this.lbxLaserMsg.TabIndex = 38;
            // 
            // cmxCmds
            // 
            this.cmxCmds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmxCmds.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxCmds.FillColor = System.Drawing.Color.White;
            this.cmxCmds.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxCmds.FormattingEnabled = true;
            this.cmxCmds.Location = new System.Drawing.Point(103, 3);
            this.cmxCmds.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxCmds.Name = "cmxCmds";
            this.cmxCmds.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxCmds.Size = new System.Drawing.Size(196, 24);
            this.cmxCmds.TabIndex = 39;
            this.cmxCmds.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmxCmds.SelectedIndexChanged += new System.EventHandler(this.cmxCmds_SelectedIndexChanged);
            // 
            // btnClearLaserMsg
            // 
            this.btnClearLaserMsg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearLaserMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearLaserMsg.FillColor = System.Drawing.Color.White;
            this.btnClearLaserMsg.FillColor2 = System.Drawing.Color.Empty;
            this.btnClearLaserMsg.ForeColor = System.Drawing.Color.Black;
            this.btnClearLaserMsg.Location = new System.Drawing.Point(305, 123);
            this.btnClearLaserMsg.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearLaserMsg.Name = "btnClearLaserMsg";
            this.btnClearLaserMsg.Size = new System.Drawing.Size(94, 24);
            this.btnClearLaserMsg.TabIndex = 41;
            this.btnClearLaserMsg.Text = "清除消息";
            this.btnClearLaserMsg.Click += new System.EventHandler(this.btnClearLaserMsg_Click);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.Location = new System.Drawing.Point(3, 3);
            this.label48.Margin = new System.Windows.Forms.Padding(3);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(94, 24);
            this.label48.TabIndex = 33;
            this.label48.Text = "指令";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 24);
            this.label2.TabIndex = 33;
            this.label2.Text = "对位工位";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmxAlignStation
            // 
            this.cmxAlignStation.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxAlignStation.FillColor = System.Drawing.Color.White;
            this.cmxAlignStation.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxAlignStation.FormattingEnabled = true;
            this.cmxAlignStation.Location = new System.Drawing.Point(103, 33);
            this.cmxAlignStation.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxAlignStation.Name = "cmxAlignStation";
            this.cmxAlignStation.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxAlignStation.Size = new System.Drawing.Size(196, 24);
            this.cmxAlignStation.TabIndex = 39;
            this.cmxAlignStation.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmxStnNo
            // 
            this.cmxStnNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmxStnNo.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxStnNo.FillColor = System.Drawing.Color.White;
            this.cmxStnNo.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxStnNo.FormattingEnabled = true;
            this.cmxStnNo.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cmxStnNo.Location = new System.Drawing.Point(103, 63);
            this.cmxStnNo.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxStnNo.Name = "cmxStnNo";
            this.cmxStnNo.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxStnNo.Size = new System.Drawing.Size(196, 24);
            this.cmxStnNo.TabIndex = 39;
            this.cmxStnNo.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(32, 93);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 24);
            this.label3.TabIndex = 42;
            this.label3.Text = "发送数据";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCalibration
            // 
            this.btnCalibration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalibration.FillColor2 = System.Drawing.Color.Empty;
            this.btnCalibration.Location = new System.Drawing.Point(3, 123);
            this.btnCalibration.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(94, 24);
            this.btnCalibration.TabIndex = 59;
            this.btnCalibration.Text = "标定";
            this.btnCalibration.Visible = false;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnMotionRequest
            // 
            this.btnMotionRequest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMotionRequest.FillColor2 = System.Drawing.Color.Empty;
            this.btnMotionRequest.Location = new System.Drawing.Point(103, 123);
            this.btnMotionRequest.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnMotionRequest.Name = "btnMotionRequest";
            this.btnMotionRequest.Size = new System.Drawing.Size(94, 24);
            this.btnMotionRequest.TabIndex = 59;
            this.btnMotionRequest.Text = "移动";
            this.btnMotionRequest.Visible = false;
            this.btnMotionRequest.Click += new System.EventHandler(this.btnMotionRequest_Click);
            // 
            // AlignPcCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox19);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AlignPcCtrl";
            this.Size = new System.Drawing.Size(408, 454);
            this.Load += new System.EventHandler(this.LaserCtrl_Load);
            this.groupBox19.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox groupBox19;
        private StdLib.WindowsControls.HTButton btnClearLaserMsg;
        private StdLib.WindowsControls.HTButton btnLaserSend;
        private StdLib.WindowsControls.HTComboBox cmxCmds;
        private StdLib.WindowsControls.HCListBox lbxLaserMsg;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private HTComboBox cmxAlignStation;
        private HTComboBox cmxStnNo;
        private System.Windows.Forms.Label label3;
        private HTTextBox tbxMsg;
        private HTButton btnCalibration;
        private HTButton btnMotionRequest;
    }
}
