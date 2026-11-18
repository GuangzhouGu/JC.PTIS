namespace JC.PTIS.Main.UserCtrls
{
    partial class UnloadPortCtrl
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
            this.exLotBoxCtrl1 = new JC.PTIS.Main.UserCtrls.ExLotBoxCtrl();
            this.exLotBoxCtrl2 = new JC.PTIS.Main.UserCtrls.ExLotBoxCtrl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.exLotBoxCtrl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.exLotBoxCtrl2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(206, 370);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // exLotBoxCtrl1
            // 
            this.exLotBoxCtrl1.Desc = "外盒1";
            this.exLotBoxCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exLotBoxCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exLotBoxCtrl1.Location = new System.Drawing.Point(1, 1);
            this.exLotBoxCtrl1.LotBoxNo = 0;
            this.exLotBoxCtrl1.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.ExBox1Stn;
            this.exLotBoxCtrl1.Margin = new System.Windows.Forms.Padding(1);
            this.exLotBoxCtrl1.Name = "exLotBoxCtrl1";
            this.exLotBoxCtrl1.Size = new System.Drawing.Size(204, 183);
            this.exLotBoxCtrl1.TabIndex = 0;
            this.exLotBoxCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // exLotBoxCtrl2
            // 
            this.exLotBoxCtrl2.Desc = "外盒2";
            this.exLotBoxCtrl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exLotBoxCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exLotBoxCtrl2.Location = new System.Drawing.Point(1, 186);
            this.exLotBoxCtrl2.LotBoxNo = 1;
            this.exLotBoxCtrl2.LotBoxStnType = JC.PTIS.Main.LotBoxStnType.ExBox2Stn;
            this.exLotBoxCtrl2.Margin = new System.Windows.Forms.Padding(1);
            this.exLotBoxCtrl2.Name = "exLotBoxCtrl2";
            this.exLotBoxCtrl2.Size = new System.Drawing.Size(204, 183);
            this.exLotBoxCtrl2.TabIndex = 0;
            this.exLotBoxCtrl2.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // UnloadPortCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UnloadPortCtrl";
            this.Size = new System.Drawing.Size(206, 370);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExLotBoxCtrl exLotBoxCtrl1;
        private ExLotBoxCtrl exLotBoxCtrl2;
    }
}
