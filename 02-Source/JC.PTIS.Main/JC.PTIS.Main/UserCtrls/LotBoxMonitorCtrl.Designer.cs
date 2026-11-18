namespace JC.PTIS.Main.UserCtrls
{
    partial class LotBoxMonitorCtrl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lotBoxCtrl1 = new JC.PTIS.Main.UserCtrls.LotBoxCtrl();
            this.lotBoxCtrl2 = new JC.PTIS.Main.UserCtrls.LotBoxCtrl();
            this.lotBoxCtrl3 = new JC.PTIS.Main.UserCtrls.LotBoxCtrl();
            this.lotBoxCtrl4 = new JC.PTIS.Main.UserCtrls.LotBoxCtrl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lotBoxCtrl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lotBoxCtrl2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lotBoxCtrl3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lotBoxCtrl4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(627, 485);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lotBoxCtrl1
            // 
            this.lotBoxCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotBoxCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxCtrl1.Location = new System.Drawing.Point(2, 2);
            this.lotBoxCtrl1.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.OKBinStn;
            this.lotBoxCtrl1.LotBoxType = JC.PTIS.Main.LotBoxType.Bin135;
            this.lotBoxCtrl1.Margin = new System.Windows.Forms.Padding(2);
            this.lotBoxCtrl1.Name = "lotBoxCtrl1";
            this.lotBoxCtrl1.Size = new System.Drawing.Size(309, 238);
            this.lotBoxCtrl1.TabIndex = 0;
            this.lotBoxCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // lotBoxCtrl2
            // 
            this.lotBoxCtrl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotBoxCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxCtrl2.Location = new System.Drawing.Point(315, 2);
            this.lotBoxCtrl2.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.LulLotStn;
            this.lotBoxCtrl2.LotBoxType = JC.PTIS.Main.LotBoxType.EmptyTray;
            this.lotBoxCtrl2.Margin = new System.Windows.Forms.Padding(2);
            this.lotBoxCtrl2.Name = "lotBoxCtrl2";
            this.lotBoxCtrl2.Size = new System.Drawing.Size(310, 238);
            this.lotBoxCtrl2.TabIndex = 1;
            this.lotBoxCtrl2.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // lotBoxCtrl3
            // 
            this.lotBoxCtrl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotBoxCtrl3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxCtrl3.Location = new System.Drawing.Point(2, 244);
            this.lotBoxCtrl3.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.NGBinStn;
            this.lotBoxCtrl3.LotBoxType = JC.PTIS.Main.LotBoxType.Bin246;
            this.lotBoxCtrl3.Margin = new System.Windows.Forms.Padding(2);
            this.lotBoxCtrl3.Name = "lotBoxCtrl3";
            this.lotBoxCtrl3.Size = new System.Drawing.Size(309, 239);
            this.lotBoxCtrl3.TabIndex = 1;
            this.lotBoxCtrl3.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // lotBoxCtrl4
            // 
            this.lotBoxCtrl4.BackColor = System.Drawing.Color.White;
            this.lotBoxCtrl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotBoxCtrl4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxCtrl4.Location = new System.Drawing.Point(315, 244);
            this.lotBoxCtrl4.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.NullStn;
            this.lotBoxCtrl4.LotBoxType = JC.PTIS.Main.LotBoxType.Null;
            this.lotBoxCtrl4.Margin = new System.Windows.Forms.Padding(2);
            this.lotBoxCtrl4.Name = "lotBoxCtrl4";
            this.lotBoxCtrl4.Size = new System.Drawing.Size(310, 239);
            this.lotBoxCtrl4.TabIndex = 1;
            this.lotBoxCtrl4.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // LotBoxMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LotBoxMonitorCtrl";
            this.Size = new System.Drawing.Size(627, 485);
            this.Load += new System.EventHandler(this.LotBoxMonitorCtrl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LotBoxCtrl lotBoxCtrl1;
        private LotBoxCtrl lotBoxCtrl2;
        private LotBoxCtrl lotBoxCtrl3;
        private LotBoxCtrl lotBoxCtrl4;
    }
}
