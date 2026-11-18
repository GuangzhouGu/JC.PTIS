
using StdLib.WindowsControls;

namespace JC.LidDispenser.UserCtrl.Devices
{
    partial class DetectPcCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnLaserSend = new StdLib.WindowsControls.HTButton();
            this.lbxLaserMsg = new StdLib.WindowsControls.HCListBox();
            this.cmxCmds = new StdLib.WindowsControls.HTComboBox();
            this.btnClearLaserMsg = new StdLib.WindowsControls.HTButton();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.tbxStnNo = new StdLib.WindowsControls.HTTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmxDetectType = new StdLib.WindowsControls.HTComboBox();
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
            this.groupBox19.Text = "检测PC调试";
            this.groupBox19.TitleTop = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLaserSend, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxLaserMsg, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmxCmds, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClearLaserMsg, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label49, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label48, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbxStnNo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmxDetectType, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 20);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(402, 431);
            this.tableLayoutPanel1.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 67);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 42;
            this.label1.Text = "工位号";
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
            this.btnLaserSend.Click += new System.EventHandler(this.btnLaserSend_Click);
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
            this.lbxLaserMsg.Location = new System.Drawing.Point(1, 121);
            this.lbxLaserMsg.Margin = new System.Windows.Forms.Padding(1);
            this.lbxLaserMsg.Name = "lbxLaserMsg";
            this.lbxLaserMsg.SelectionBackColor = System.Drawing.Color.SkyBlue;
            this.lbxLaserMsg.Size = new System.Drawing.Size(400, 309);
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
            // 
            // btnClearLaserMsg
            // 
            this.btnClearLaserMsg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearLaserMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearLaserMsg.FillColor = System.Drawing.Color.White;
            this.btnClearLaserMsg.FillColor2 = System.Drawing.Color.Empty;
            this.btnClearLaserMsg.ForeColor = System.Drawing.Color.Black;
            this.btnClearLaserMsg.Location = new System.Drawing.Point(305, 93);
            this.btnClearLaserMsg.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnClearLaserMsg.Name = "btnClearLaserMsg";
            this.btnClearLaserMsg.Size = new System.Drawing.Size(94, 24);
            this.btnClearLaserMsg.TabIndex = 41;
            this.btnClearLaserMsg.Text = "清除消息";
            this.btnClearLaserMsg.Click += new System.EventHandler(this.btnClearLaserMsg_Click);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label49.ForeColor = System.Drawing.Color.Black;
            this.label49.Location = new System.Drawing.Point(3, 93);
            this.label49.Margin = new System.Windows.Forms.Padding(3);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(94, 24);
            this.label49.TabIndex = 35;
            this.label49.Text = "接收消息";
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
            // tbxStnNo
            // 
            this.tbxStnNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxStnNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxStnNo.FillColor = System.Drawing.Color.White;
            this.tbxStnNo.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tbxStnNo.Location = new System.Drawing.Point(103, 63);
            this.tbxStnNo.Maximum = 2147483647D;
            this.tbxStnNo.Minimum = -2147483648D;
            this.tbxStnNo.Name = "tbxStnNo";
            this.tbxStnNo.Padding = new System.Windows.Forms.Padding(5);
            this.tbxStnNo.Size = new System.Drawing.Size(196, 26);
            this.tbxStnNo.TabIndex = 43;
            this.tbxStnNo.Text = "0";
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
            this.label2.Text = "检测类型";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmxDetectType
            // 
            this.cmxDetectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmxDetectType.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxDetectType.FillColor = System.Drawing.Color.White;
            this.cmxDetectType.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxDetectType.FormattingEnabled = true;
            this.cmxDetectType.Location = new System.Drawing.Point(103, 33);
            this.cmxDetectType.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxDetectType.Name = "cmxDetectType";
            this.cmxDetectType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxDetectType.Size = new System.Drawing.Size(196, 24);
            this.cmxDetectType.TabIndex = 39;
            this.cmxDetectType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DetectPcCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox19);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DetectPcCtrl";
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
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private HTTextBox tbxStnNo;
        private System.Windows.Forms.Label label2;
        private HTComboBox cmxDetectType;
    }
}
