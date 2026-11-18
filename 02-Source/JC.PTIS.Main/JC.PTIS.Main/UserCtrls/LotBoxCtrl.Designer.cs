namespace JC.PTIS.Main.UserCtrls
{
    partial class LotBoxCtrl
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
            this.tlpLotBox = new System.Windows.Forms.TableLayoutPanel();
            this.htTitlePanel1 = new StdLib.WindowsControls.HTTitlePanel();
            this.htTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLotBox
            // 
            this.tlpLotBox.BackColor = System.Drawing.Color.Transparent;
            this.tlpLotBox.ColumnCount = 2;
            this.tlpLotBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLotBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLotBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLotBox.Location = new System.Drawing.Point(0, 22);
            this.tlpLotBox.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLotBox.Name = "tlpLotBox";
            this.tlpLotBox.RowCount = 2;
            this.tlpLotBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLotBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLotBox.Size = new System.Drawing.Size(303, 223);
            this.tlpLotBox.TabIndex = 0;
            // 
            // htTitlePanel1
            // 
            this.htTitlePanel1.Controls.Add(this.tlpLotBox);
            this.htTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htTitlePanel1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htTitlePanel1.ForeColor = System.Drawing.Color.Black;
            this.htTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.htTitlePanel1.Name = "htTitlePanel1";
            this.htTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
            this.htTitlePanel1.Size = new System.Drawing.Size(303, 245);
            this.htTitlePanel1.Style = StdLib.WindowsControls.HTStyle.Custom;
            this.htTitlePanel1.TabIndex = 2;
            this.htTitlePanel1.TitleColor = System.Drawing.Color.SkyBlue;
            this.htTitlePanel1.TitleHeight = 22;
            // 
            // LotBoxCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htTitlePanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LotBoxCtrl";
            this.Size = new System.Drawing.Size(303, 245);
            this.Load += new System.EventHandler(this.LotBoxCtrl_Load);
            this.htTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLotBox;
        private StdLib.WindowsControls.HTTitlePanel htTitlePanel1;
    }
}
