namespace JC.Map.Devices.RFID
{
    partial class RfidCtrl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxASCIIEncoding = new System.Windows.Forms.CheckBox();
            this.labelReadResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxReadPages = new System.Windows.Forms.ComboBox();
            this.buttonReadPage = new System.Windows.Forms.Button();
            this.buttonReadCarrierID = new System.Windows.Forms.Button();
            this.buttonReadAllPages = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxWriteHexFormat = new System.Windows.Forms.CheckBox();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.textBoxWriteData = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxWritePages = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxDebugMessage = new System.Windows.Forms.TextBox();
            this.comboBoxRFIDs = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxASCIIEncoding);
            this.groupBox1.Controls.Add(this.labelReadResult);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxReadPages);
            this.groupBox1.Controls.Add(this.buttonReadPage);
            this.groupBox1.Controls.Add(this.buttonReadCarrierID);
            this.groupBox1.Controls.Add(this.buttonReadAllPages);
            this.groupBox1.Location = new System.Drawing.Point(3, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 383);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读取";
            // 
            // checkBoxASCIIEncoding
            // 
            this.checkBoxASCIIEncoding.AutoSize = true;
            this.checkBoxASCIIEncoding.Location = new System.Drawing.Point(97, 108);
            this.checkBoxASCIIEncoding.Name = "checkBoxASCIIEncoding";
            this.checkBoxASCIIEncoding.Size = new System.Drawing.Size(142, 22);
            this.checkBoxASCIIEncoding.TabIndex = 26;
            this.checkBoxASCIIEncoding.Text = "显示为字符串";
            this.checkBoxASCIIEncoding.UseVisualStyleBackColor = true;
            // 
            // labelReadResult
            // 
            this.labelReadResult.AutoSize = true;
            this.labelReadResult.Location = new System.Drawing.Point(6, 141);
            this.labelReadResult.Name = "labelReadResult";
            this.labelReadResult.Size = new System.Drawing.Size(125, 18);
            this.labelReadResult.TabIndex = 25;
            this.labelReadResult.Text = "             ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "读取结果：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 18);
            this.label1.TabIndex = 23;
            this.label1.Text = "页";
            // 
            // comboBoxReadPages
            // 
            this.comboBoxReadPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReadPages.FormattingEnabled = true;
            this.comboBoxReadPages.Location = new System.Drawing.Point(228, 68);
            this.comboBoxReadPages.Name = "comboBoxReadPages";
            this.comboBoxReadPages.Size = new System.Drawing.Size(85, 26);
            this.comboBoxReadPages.TabIndex = 22;
            // 
            // buttonReadPage
            // 
            this.buttonReadPage.Location = new System.Drawing.Point(5, 64);
            this.buttonReadPage.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadPage.Name = "buttonReadPage";
            this.buttonReadPage.Size = new System.Drawing.Size(218, 32);
            this.buttonReadPage.TabIndex = 21;
            this.buttonReadPage.Tag = "ABORG";
            this.buttonReadPage.Text = "读取指定页数据";
            this.buttonReadPage.UseVisualStyleBackColor = true;
            this.buttonReadPage.Click += new System.EventHandler(this.buttonReadPage_Click);
            // 
            // buttonReadCarrierID
            // 
            this.buttonReadCarrierID.Location = new System.Drawing.Point(227, 26);
            this.buttonReadCarrierID.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadCarrierID.Name = "buttonReadCarrierID";
            this.buttonReadCarrierID.Size = new System.Drawing.Size(208, 32);
            this.buttonReadCarrierID.TabIndex = 20;
            this.buttonReadCarrierID.Tag = "ABORG";
            this.buttonReadCarrierID.Text = "读取CarrierID";
            this.buttonReadCarrierID.UseVisualStyleBackColor = true;
            this.buttonReadCarrierID.Click += new System.EventHandler(this.buttonReadCarrierID_Click);
            // 
            // buttonReadAllPages
            // 
            this.buttonReadAllPages.Location = new System.Drawing.Point(5, 26);
            this.buttonReadAllPages.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReadAllPages.Name = "buttonReadAllPages";
            this.buttonReadAllPages.Size = new System.Drawing.Size(218, 32);
            this.buttonReadAllPages.TabIndex = 19;
            this.buttonReadAllPages.Tag = "";
            this.buttonReadAllPages.Text = "读取所有页数据";
            this.buttonReadAllPages.UseVisualStyleBackColor = true;
            this.buttonReadAllPages.Click += new System.EventHandler(this.buttonReadAllPages_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.checkBoxWriteHexFormat);
            this.groupBox2.Controls.Add(this.buttonWrite);
            this.groupBox2.Controls.Add(this.textBoxWriteData);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBoxWritePages);
            this.groupBox2.Location = new System.Drawing.Point(449, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(566, 157);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "写入";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(24, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(447, 18);
            this.label7.TabIndex = 31;
            this.label7.Text = "注意：16进制用空格分开，每页最多写入16字节数据";
            // 
            // checkBoxWriteHexFormat
            // 
            this.checkBoxWriteHexFormat.AutoSize = true;
            this.checkBoxWriteHexFormat.Location = new System.Drawing.Point(466, 70);
            this.checkBoxWriteHexFormat.Name = "checkBoxWriteHexFormat";
            this.checkBoxWriteHexFormat.Size = new System.Drawing.Size(88, 22);
            this.checkBoxWriteHexFormat.TabIndex = 30;
            this.checkBoxWriteHexFormat.Text = "16进制";
            this.checkBoxWriteHexFormat.UseVisualStyleBackColor = true;
            // 
            // buttonWrite
            // 
            this.buttonWrite.Location = new System.Drawing.Point(466, 102);
            this.buttonWrite.Margin = new System.Windows.Forms.Padding(2);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(88, 32);
            this.buttonWrite.TabIndex = 29;
            this.buttonWrite.Tag = "ABORG";
            this.buttonWrite.Text = "写入";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // textBoxWriteData
            // 
            this.textBoxWriteData.Location = new System.Drawing.Point(123, 66);
            this.textBoxWriteData.Name = "textBoxWriteData";
            this.textBoxWriteData.Size = new System.Drawing.Size(337, 28);
            this.textBoxWriteData.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 27;
            this.label5.Text = "输入数据：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 18);
            this.label4.TabIndex = 26;
            this.label4.Text = "选择页数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "页";
            // 
            // comboBoxWritePages
            // 
            this.comboBoxWritePages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWritePages.FormattingEnabled = true;
            this.comboBoxWritePages.Location = new System.Drawing.Point(123, 27);
            this.comboBoxWritePages.Name = "comboBoxWritePages";
            this.comboBoxWritePages.Size = new System.Drawing.Size(85, 26);
            this.comboBoxWritePages.TabIndex = 24;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.textBoxDebugMessage);
            this.groupBox3.Location = new System.Drawing.Point(449, 208);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(566, 220);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "调试日志";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 26);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 32);
            this.button1.TabIndex = 21;
            this.button1.Tag = "";
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxDebugMessage
            // 
            this.textBoxDebugMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDebugMessage.Location = new System.Drawing.Point(6, 63);
            this.textBoxDebugMessage.Multiline = true;
            this.textBoxDebugMessage.Name = "textBoxDebugMessage";
            this.textBoxDebugMessage.Size = new System.Drawing.Size(554, 151);
            this.textBoxDebugMessage.TabIndex = 2;
            // 
            // comboBoxRFIDs
            // 
            this.comboBoxRFIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRFIDs.FormattingEnabled = true;
            this.comboBoxRFIDs.Location = new System.Drawing.Point(111, 14);
            this.comboBoxRFIDs.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxRFIDs.Name = "comboBoxRFIDs";
            this.comboBoxRFIDs.Size = new System.Drawing.Size(332, 26);
            this.comboBoxRFIDs.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 17);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 3;
            this.label6.Text = "选择RFID：";
            // 
            // RfidCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxRFIDs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "RfidCtrl";
            this.Size = new System.Drawing.Size(1027, 442);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonReadAllPages;
        private System.Windows.Forms.Button buttonReadCarrierID;
        private System.Windows.Forms.Button buttonReadPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxReadPages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelReadResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxWritePages;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxWriteData;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxDebugMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxRFIDs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxASCIIEncoding;
        private System.Windows.Forms.CheckBox checkBoxWriteHexFormat;
        private System.Windows.Forms.Label label7;
    }
}
