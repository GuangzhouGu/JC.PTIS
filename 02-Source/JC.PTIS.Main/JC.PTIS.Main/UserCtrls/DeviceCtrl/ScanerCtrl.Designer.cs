
using StdLib.WindowsControls;

namespace JC.LidDispenser.UserCtrl.Devices
{
    partial class ScanerCtrl
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
            this.btnLaserSend = new StdLib.WindowsControls.HTButton();
            this.lbxLaserMsg = new StdLib.WindowsControls.HCListBox();
            this.cmxLasers = new StdLib.WindowsControls.HTComboBox();
            this.btnClearLaserMsg = new StdLib.WindowsControls.HTButton();
            this.label46 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
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
            this.groupBox19.Text = "扫码枪调试";
            this.groupBox19.TitleTop = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnLaserSend, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxLaserMsg, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmxLasers, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClearLaserMsg, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label46, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label49, 0, 3);
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
            this.btnLaserSend.Text = "扫码";
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
            // cmxLasers
            // 
            this.cmxLasers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmxLasers.DropDownStyle = StdLib.WindowsControls.HTDropDownStyle.DropDownList;
            this.cmxLasers.FillColor = System.Drawing.Color.White;
            this.cmxLasers.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cmxLasers.FormattingEnabled = true;
            this.cmxLasers.Location = new System.Drawing.Point(103, 3);
            this.cmxLasers.MinimumSize = new System.Drawing.Size(63, 0);
            this.cmxLasers.Name = "cmxLasers";
            this.cmxLasers.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.cmxLasers.Size = new System.Drawing.Size(196, 24);
            this.cmxLasers.TabIndex = 40;
            this.cmxLasers.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label46.ForeColor = System.Drawing.Color.Black;
            this.label46.Location = new System.Drawing.Point(3, 3);
            this.label46.Margin = new System.Windows.Forms.Padding(3);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(94, 24);
            this.label46.TabIndex = 36;
            this.label46.Text = "Scaner";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // ScanerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox19);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ScanerCtrl";
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
        private StdLib.WindowsControls.HTComboBox cmxLasers;
        private System.Windows.Forms.Label label46;
        private StdLib.WindowsControls.HCListBox lbxLaserMsg;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
