
namespace HYC.MicroLedATDetect.Controls
{
    partial class FoupMonitorCtrl
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
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.foupCtrl1 = new JC.PTIS.Main.UserCtrls.FoupCtrl();
            this.foupCtrl2 = new JC.PTIS.Main.UserCtrls.FoupCtrl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightCyan;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(620, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.foupCtrl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.foupCtrl2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(619, 765);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // foupCtrl1
            // 
            this.foupCtrl1.Desc = "Foup1";
            this.foupCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.foupCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.foupCtrl1.FoupNo = 0;
            this.foupCtrl1.Location = new System.Drawing.Point(1, 1);
            this.foupCtrl1.Margin = new System.Windows.Forms.Padding(1);
            this.foupCtrl1.Name = "foupCtrl1";
            this.foupCtrl1.Size = new System.Drawing.Size(617, 380);
            this.foupCtrl1.TabIndex = 0;
            this.foupCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.foupCtrl1.TotalWaferCount = 13;
            // 
            // foupCtrl2
            // 
            this.foupCtrl2.Desc = "Foup2";
            this.foupCtrl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.foupCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.foupCtrl2.FoupNo = 1;
            this.foupCtrl2.Location = new System.Drawing.Point(1, 383);
            this.foupCtrl2.Margin = new System.Windows.Forms.Padding(1);
            this.foupCtrl2.Name = "foupCtrl2";
            this.foupCtrl2.Size = new System.Drawing.Size(617, 381);
            this.foupCtrl2.TabIndex = 0;
            this.foupCtrl2.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.foupCtrl2.TotalWaferCount = 13;
            // 
            // FoupMonitorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FoupMonitorCtrl";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(621, 767);
            this.Load += new System.EventHandler(this.WaferMapBase_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private JC.PTIS.Main.UserCtrls.FoupCtrl foupCtrl1;
        private JC.PTIS.Main.UserCtrls.FoupCtrl foupCtrl2;
    }
}
