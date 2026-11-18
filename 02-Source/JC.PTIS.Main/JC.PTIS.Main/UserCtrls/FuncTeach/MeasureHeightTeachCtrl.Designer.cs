namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    partial class MeasureHeightTeachCtrl
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
            this.btnExeMesHeight = new StdLib.WindowsControls.HTButton();
            this.htGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // htGroupBox1
            // 
            this.htGroupBox1.BackColor = System.Drawing.Color.White;
            this.htGroupBox1.Controls.Add(this.btnExeMesHeight);
            this.htGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.htGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.htGroupBox1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.htGroupBox1.Name = "htGroupBox1";
            this.htGroupBox1.Padding = new System.Windows.Forms.Padding(4, 34, 4, 4);
            this.htGroupBox1.Size = new System.Drawing.Size(636, 376);
            this.htGroupBox1.TabIndex = 0;
            this.htGroupBox1.Text = "测高功能调试";
            this.htGroupBox1.TitleTop = 8;
            // 
            // btnExeMesHeight
            // 
            this.btnExeMesHeight.BackColor = System.Drawing.Color.White;
            this.btnExeMesHeight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExeMesHeight.FillColor2 = System.Drawing.Color.Empty;
            this.btnExeMesHeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExeMesHeight.Location = new System.Drawing.Point(8, 23);
            this.btnExeMesHeight.Margin = new System.Windows.Forms.Padding(4);
            this.btnExeMesHeight.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnExeMesHeight.Name = "btnExeMesHeight";
            this.btnExeMesHeight.Size = new System.Drawing.Size(111, 25);
            this.btnExeMesHeight.TabIndex = 0;
            this.btnExeMesHeight.Text = "测高流程执行";
            this.btnExeMesHeight.Click += new System.EventHandler(this.btnExeMesHeight_Click);
            // 
            // MeasureHeightTeachCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.htGroupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MeasureHeightTeachCtrl";
            this.Size = new System.Drawing.Size(636, 376);
            this.Load += new System.EventHandler(this.MeasureHeightTeachCtrl_Load);
            this.htGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StdLib.WindowsControls.HTGroupBox htGroupBox1;
        private StdLib.WindowsControls.HTButton btnExeMesHeight;
    }
}
