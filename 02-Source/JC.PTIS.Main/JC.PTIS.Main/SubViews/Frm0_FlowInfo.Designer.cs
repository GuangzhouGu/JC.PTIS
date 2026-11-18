namespace JC.PTIS.Main.SubViews
{
    partial class Frm0_FlowInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.irNgTrayCtrl1 = new JC.PTIS.Main.UserCtrls.IRNgTrayCtrl();
            this.packingStsCtrl1 = new JC.PTIS.Main.UserCtrls.PackingStsCtrl();
            this.labelMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.LabelMonitorCtrl();
            this.waferTableCtrl1 = new JC.PTIS.Main.UserCtrls.WaferBufferCtrl();
            this.packingBeltDetectResultCtrl1 = new JC.PTIS.Main.UserCtrls.VisionResultCtrl();
            this.unloadPortCtrl1 = new JC.PTIS.Main.UserCtrls.UnloadPortCtrl();
            this.lotMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.LotMonitorCtrl();
            this.trayMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.TrayMonitorCtrl();
            this.trayMonitorCtrl2 = new JC.PTIS.Main.UserCtrls.TrayMonitorCtrl();
            this.trayMonitorCtrl4 = new JC.PTIS.Main.UserCtrls.TrayMonitorCtrl();
            this.trayMonitorCtrl3 = new JC.PTIS.Main.UserCtrls.TrayMonitorCtrl();
            this.trayArmMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.TrayMonitorCtrl();
            this.pickPatchCtrl1 = new JC.PTIS.Main.UserCtrls.PickPatchStnMonitorCtrl();
            this.lotBoxMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.LotBoxMonitorCtrl();
            this.loadPortCtrl1 = new HYC.MicroLedATDetect.Controls.FoupMonitorCtrl();
            this.halfTrayBufferCtrl1 = new JC.PTIS.Main.UserCtrls.HalfTrayBufferCtrl();
            this.lotTrayStnMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.Stn8TurnTableMonitorCtrl();
            this.waferCtrl1 = new HYC.MicroLedATDetect.Controls.WaferMapCtrl();
            this.trayStnMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.Stn4TurnTableMonitorCtrl();
            this.loadDieStnMonitorCtrl1 = new JC.PTIS.Main.UserCtrls.LoadDieStnMonitorCtrl();
            this.buttonEx1 = new StdLib.WindowsControls.Buttons.ButtonEx(this.components);
            this.SuspendLayout();
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadMap.Location = new System.Drawing.Point(1369, 494);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(65, 22);
            this.btnLoadMap.TabIndex = 25;
            this.btnLoadMap.Text = "加载Map";
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
            // 
            // irNgTrayCtrl1
            // 
            this.irNgTrayCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.irNgTrayCtrl1.Desc = "IRNgTray";
            this.irNgTrayCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.irNgTrayCtrl1.Location = new System.Drawing.Point(880, 147);
            this.irNgTrayCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.irNgTrayCtrl1.Name = "irNgTrayCtrl1";
            this.irNgTrayCtrl1.Size = new System.Drawing.Size(120, 270);
            this.irNgTrayCtrl1.TabIndex = 28;
            this.irNgTrayCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // packingStsCtrl1
            // 
            this.packingStsCtrl1.Desc = "束带平台";
            this.packingStsCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.packingStsCtrl1.Location = new System.Drawing.Point(759, 2);
            this.packingStsCtrl1.LotPosEnum = JC.PTIS.Main.LotPosEnum.PackingStage;
            this.packingStsCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.packingStsCtrl1.Name = "packingStsCtrl1";
            this.packingStsCtrl1.Size = new System.Drawing.Size(118, 143);
            this.packingStsCtrl1.TabIndex = 26;
            this.packingStsCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // labelMonitorCtrl1
            // 
            this.labelMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMonitorCtrl1.Location = new System.Drawing.Point(880, 3);
            this.labelMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelMonitorCtrl1.Name = "labelMonitorCtrl1";
            this.labelMonitorCtrl1.Size = new System.Drawing.Size(120, 142);
            this.labelMonitorCtrl1.TabIndex = 23;
            // 
            // waferTableCtrl1
            // 
            this.waferTableCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.waferTableCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.waferTableCtrl1.Location = new System.Drawing.Point(1001, 419);
            this.waferTableCtrl1.Margin = new System.Windows.Forms.Padding(1);
            this.waferTableCtrl1.Name = "waferTableCtrl1";
            this.waferTableCtrl1.Size = new System.Drawing.Size(437, 73);
            this.waferTableCtrl1.TabIndex = 22;
            // 
            // packingBeltDetectResultCtrl1
            // 
            this.packingBeltDetectResultCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.packingBeltDetectResultCtrl1.Location = new System.Drawing.Point(638, 147);
            this.packingBeltDetectResultCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.packingBeltDetectResultCtrl1.Name = "packingBeltDetectResultCtrl1";
            this.packingBeltDetectResultCtrl1.Size = new System.Drawing.Size(239, 270);
            this.packingBeltDetectResultCtrl1.TabIndex = 20;
            // 
            // unloadPortCtrl1
            // 
            this.unloadPortCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.unloadPortCtrl1.Location = new System.Drawing.Point(1439, 3);
            this.unloadPortCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.unloadPortCtrl1.Name = "unloadPortCtrl1";
            this.unloadPortCtrl1.Size = new System.Drawing.Size(130, 411);
            this.unloadPortCtrl1.TabIndex = 19;
            // 
            // lotMonitorCtrl1
            // 
            this.lotMonitorCtrl1.Desc = "Lot夹爪";
            this.lotMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotMonitorCtrl1.Location = new System.Drawing.Point(639, 3);
            this.lotMonitorCtrl1.LotPosEnum = JC.PTIS.Main.LotPosEnum.LotArmClaw;
            this.lotMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.lotMonitorCtrl1.Name = "lotMonitorCtrl1";
            this.lotMonitorCtrl1.Size = new System.Drawing.Size(118, 143);
            this.lotMonitorCtrl1.TabIndex = 18;
            this.lotMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // trayMonitorCtrl1
            // 
            this.trayMonitorCtrl1.Desc = "空Tray缓存";
            this.trayMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayMonitorCtrl1.IsReverseRC = true;
            this.trayMonitorCtrl1.Location = new System.Drawing.Point(107, 206);
            this.trayMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.trayMonitorCtrl1.Name = "trayMonitorCtrl1";
            this.trayMonitorCtrl1.Size = new System.Drawing.Size(106, 210);
            this.trayMonitorCtrl1.TabIndex = 17;
            this.trayMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.trayMonitorCtrl1.TrayCount = 1;
            this.trayMonitorCtrl1.TrayPosEnum = JC.PTIS.Main.TrayPosEnum.EmptyTrayBufferStage;
            // 
            // trayMonitorCtrl2
            // 
            this.trayMonitorCtrl2.Desc = "满TrayArm";
            this.trayMonitorCtrl2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayMonitorCtrl2.IsReverseRC = false;
            this.trayMonitorCtrl2.Location = new System.Drawing.Point(1, 104);
            this.trayMonitorCtrl2.Margin = new System.Windows.Forms.Padding(4);
            this.trayMonitorCtrl2.Name = "trayMonitorCtrl2";
            this.trayMonitorCtrl2.Size = new System.Drawing.Size(212, 100);
            this.trayMonitorCtrl2.TabIndex = 16;
            this.trayMonitorCtrl2.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.trayMonitorCtrl2.TrayCount = 2;
            this.trayMonitorCtrl2.TrayPosEnum = JC.PTIS.Main.TrayPosEnum.FullTrayArm;
            // 
            // trayMonitorCtrl4
            // 
            this.trayMonitorCtrl4.Desc = "Ng半Tray";
            this.trayMonitorCtrl4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayMonitorCtrl4.IsReverseRC = false;
            this.trayMonitorCtrl4.Location = new System.Drawing.Point(1, 419);
            this.trayMonitorCtrl4.Margin = new System.Windows.Forms.Padding(4);
            this.trayMonitorCtrl4.Name = "trayMonitorCtrl4";
            this.trayMonitorCtrl4.Size = new System.Drawing.Size(212, 100);
            this.trayMonitorCtrl4.TabIndex = 16;
            this.trayMonitorCtrl4.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.trayMonitorCtrl4.TrayCount = 2;
            this.trayMonitorCtrl4.TrayPosEnum = JC.PTIS.Main.TrayPosEnum.NgHalfTrayStage;
            // 
            // trayMonitorCtrl3
            // 
            this.trayMonitorCtrl3.Desc = "Ok半Tray";
            this.trayMonitorCtrl3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayMonitorCtrl3.IsReverseRC = false;
            this.trayMonitorCtrl3.Location = new System.Drawing.Point(1, 521);
            this.trayMonitorCtrl3.Margin = new System.Windows.Forms.Padding(4);
            this.trayMonitorCtrl3.Name = "trayMonitorCtrl3";
            this.trayMonitorCtrl3.Size = new System.Drawing.Size(212, 100);
            this.trayMonitorCtrl3.TabIndex = 16;
            this.trayMonitorCtrl3.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.trayMonitorCtrl3.TrayCount = 2;
            this.trayMonitorCtrl3.TrayPosEnum = JC.PTIS.Main.TrayPosEnum.OkHalfTrayStage;
            // 
            // trayArmMonitorCtrl1
            // 
            this.trayArmMonitorCtrl1.Desc = "空TrayArm";
            this.trayArmMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayArmMonitorCtrl1.IsReverseRC = false;
            this.trayArmMonitorCtrl1.Location = new System.Drawing.Point(1, 3);
            this.trayArmMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.trayArmMonitorCtrl1.Name = "trayArmMonitorCtrl1";
            this.trayArmMonitorCtrl1.Size = new System.Drawing.Size(212, 100);
            this.trayArmMonitorCtrl1.TabIndex = 16;
            this.trayArmMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            this.trayArmMonitorCtrl1.TrayCount = 2;
            this.trayArmMonitorCtrl1.TrayPosEnum = JC.PTIS.Main.TrayPosEnum.EmptyTrayArm;
            // 
            // pickPatchCtrl1
            // 
            this.pickPatchCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pickPatchCtrl1.Location = new System.Drawing.Point(1, 624);
            this.pickPatchCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.pickPatchCtrl1.Name = "pickPatchCtrl1";
            this.pickPatchCtrl1.Size = new System.Drawing.Size(212, 207);
            this.pickPatchCtrl1.TabIndex = 15;
            this.pickPatchCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // lotBoxMonitorCtrl1
            // 
            this.lotBoxMonitorCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.lotBoxMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotBoxMonitorCtrl1.Location = new System.Drawing.Point(1001, 3);
            this.lotBoxMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.lotBoxMonitorCtrl1.Name = "lotBoxMonitorCtrl1";
            this.lotBoxMonitorCtrl1.Size = new System.Drawing.Size(437, 414);
            this.lotBoxMonitorCtrl1.TabIndex = 14;
            // 
            // loadPortCtrl1
            // 
            this.loadPortCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.loadPortCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loadPortCtrl1.Location = new System.Drawing.Point(1439, 415);
            this.loadPortCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.loadPortCtrl1.Name = "loadPortCtrl1";
            this.loadPortCtrl1.Padding = new System.Windows.Forms.Padding(1);
            this.loadPortCtrl1.Size = new System.Drawing.Size(130, 418);
            this.loadPortCtrl1.TabIndex = 10;
            this.loadPortCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // halfTrayBufferCtrl1
            // 
            this.halfTrayBufferCtrl1.Desc = "半Tray缓存";
            this.halfTrayBufferCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halfTrayBufferCtrl1.Location = new System.Drawing.Point(1, 206);
            this.halfTrayBufferCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.halfTrayBufferCtrl1.Name = "halfTrayBufferCtrl1";
            this.halfTrayBufferCtrl1.Size = new System.Drawing.Size(105, 210);
            this.halfTrayBufferCtrl1.TabIndex = 13;
            this.halfTrayBufferCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // lotTrayStnMonitorCtrl1
            // 
            this.lotTrayStnMonitorCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.lotTrayStnMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lotTrayStnMonitorCtrl1.Location = new System.Drawing.Point(215, 3);
            this.lotTrayStnMonitorCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.lotTrayStnMonitorCtrl1.Name = "lotTrayStnMonitorCtrl1";
            this.lotTrayStnMonitorCtrl1.Padding = new System.Windows.Forms.Padding(1);
            this.lotTrayStnMonitorCtrl1.Size = new System.Drawing.Size(422, 414);
            this.lotTrayStnMonitorCtrl1.TabIndex = 12;
            this.lotTrayStnMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // waferCtrl1
            // 
            this.waferCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waferCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.waferCtrl1.ForeColor = System.Drawing.Color.Transparent;
            this.waferCtrl1.HitDieColNo = -1;
            this.waferCtrl1.HitDieRowNo = -1;
            this.waferCtrl1.Location = new System.Drawing.Point(1000, 493);
            this.waferCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.waferCtrl1.Name = "waferCtrl1";
            this.waferCtrl1.Padding = new System.Windows.Forms.Padding(1);
            this.waferCtrl1.Size = new System.Drawing.Size(438, 337);
            this.waferCtrl1.TabIndex = 7;
            this.waferCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // trayStnMonitorCtrl1
            // 
            this.trayStnMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trayStnMonitorCtrl1.Location = new System.Drawing.Point(213, 417);
            this.trayStnMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.trayStnMonitorCtrl1.Name = "trayStnMonitorCtrl1";
            this.trayStnMonitorCtrl1.Padding = new System.Windows.Forms.Padding(2);
            this.trayStnMonitorCtrl1.Size = new System.Drawing.Size(391, 416);
            this.trayStnMonitorCtrl1.TabIndex = 3;
            this.trayStnMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // loadDieStnMonitorCtrl1
            // 
            this.loadDieStnMonitorCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loadDieStnMonitorCtrl1.Location = new System.Drawing.Point(603, 417);
            this.loadDieStnMonitorCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.loadDieStnMonitorCtrl1.Name = "loadDieStnMonitorCtrl1";
            this.loadDieStnMonitorCtrl1.Padding = new System.Windows.Forms.Padding(2);
            this.loadDieStnMonitorCtrl1.Size = new System.Drawing.Size(397, 416);
            this.loadDieStnMonitorCtrl1.TabIndex = 1;
            this.loadDieStnMonitorCtrl1.TitleBackColor = System.Drawing.Color.SkyBlue;
            // 
            // buttonEx1
            // 
            this.buttonEx1.CornerRadius = 10;
            this.buttonEx1.Location = new System.Drawing.Point(1253, 270);
            this.buttonEx1.Name = "buttonEx1";
            this.buttonEx1.Size = new System.Drawing.Size(116, 49);
            this.buttonEx1.TabIndex = 29;
            this.buttonEx1.Text = "buttonEx1";
            this.buttonEx1.UseVisualStyleBackColor = true;
            this.buttonEx1.Click += new System.EventHandler(this.buttonEx1_Click);
            // 
            // Frm0_FlowInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1570, 834);
            this.Controls.Add(this.buttonEx1);
            this.Controls.Add(this.irNgTrayCtrl1);
            this.Controls.Add(this.packingStsCtrl1);
            this.Controls.Add(this.btnLoadMap);
            this.Controls.Add(this.labelMonitorCtrl1);
            this.Controls.Add(this.waferTableCtrl1);
            this.Controls.Add(this.packingBeltDetectResultCtrl1);
            this.Controls.Add(this.unloadPortCtrl1);
            this.Controls.Add(this.lotMonitorCtrl1);
            this.Controls.Add(this.trayMonitorCtrl1);
            this.Controls.Add(this.trayMonitorCtrl2);
            this.Controls.Add(this.trayMonitorCtrl4);
            this.Controls.Add(this.trayMonitorCtrl3);
            this.Controls.Add(this.trayArmMonitorCtrl1);
            this.Controls.Add(this.pickPatchCtrl1);
            this.Controls.Add(this.lotBoxMonitorCtrl1);
            this.Controls.Add(this.loadPortCtrl1);
            this.Controls.Add(this.halfTrayBufferCtrl1);
            this.Controls.Add(this.lotTrayStnMonitorCtrl1);
            this.Controls.Add(this.waferCtrl1);
            this.Controls.Add(this.trayStnMonitorCtrl1);
            this.Controls.Add(this.loadDieStnMonitorCtrl1);
            this.Name = "Frm0_FlowInfo";
            this.Text = "FrmFlowInfo";
            this.Load += new System.EventHandler(this.Frm0_FlowInfo_Load);
            this.Controls.SetChildIndex(this.loadDieStnMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.trayStnMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.waferCtrl1, 0);
            this.Controls.SetChildIndex(this.lotTrayStnMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.halfTrayBufferCtrl1, 0);
            this.Controls.SetChildIndex(this.loadPortCtrl1, 0);
            this.Controls.SetChildIndex(this.lotBoxMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.pickPatchCtrl1, 0);
            this.Controls.SetChildIndex(this.trayArmMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.trayMonitorCtrl3, 0);
            this.Controls.SetChildIndex(this.trayMonitorCtrl4, 0);
            this.Controls.SetChildIndex(this.trayMonitorCtrl2, 0);
            this.Controls.SetChildIndex(this.trayMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.lotMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.unloadPortCtrl1, 0);
            this.Controls.SetChildIndex(this.packingBeltDetectResultCtrl1, 0);
            this.Controls.SetChildIndex(this.waferTableCtrl1, 0);
            this.Controls.SetChildIndex(this.labelMonitorCtrl1, 0);
            this.Controls.SetChildIndex(this.btnLoadMap, 0);
            this.Controls.SetChildIndex(this.packingStsCtrl1, 0);
            this.Controls.SetChildIndex(this.irNgTrayCtrl1, 0);
            this.Controls.SetChildIndex(this.buttonEx1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private UserCtrls.LoadDieStnMonitorCtrl loadDieStnMonitorCtrl1;
        private UserCtrls.Stn4TurnTableMonitorCtrl trayStnMonitorCtrl1;
        private HYC.MicroLedATDetect.Controls.WaferMapCtrl waferCtrl1;
        private HYC.MicroLedATDetect.Controls.FoupMonitorCtrl loadPortCtrl1;
        private UserCtrls.Stn8TurnTableMonitorCtrl lotTrayStnMonitorCtrl1;
        private UserCtrls.HalfTrayBufferCtrl halfTrayBufferCtrl1;
        private UserCtrls.LotBoxMonitorCtrl lotBoxMonitorCtrl1;
        private UserCtrls.PickPatchStnMonitorCtrl pickPatchCtrl1;
        private UserCtrls.TrayMonitorCtrl trayArmMonitorCtrl1;
        private UserCtrls.TrayMonitorCtrl trayMonitorCtrl1;
        private UserCtrls.LotMonitorCtrl lotMonitorCtrl1;
        private UserCtrls.UnloadPortCtrl unloadPortCtrl1;
        private UserCtrls.VisionResultCtrl packingBeltDetectResultCtrl1;
        private UserCtrls.WaferBufferCtrl waferTableCtrl1;
        private UserCtrls.TrayMonitorCtrl trayMonitorCtrl2;
        private UserCtrls.TrayMonitorCtrl trayMonitorCtrl3;
        private UserCtrls.TrayMonitorCtrl trayMonitorCtrl4;
        private UserCtrls.LabelMonitorCtrl labelMonitorCtrl1;
        private System.Windows.Forms.Button btnLoadMap;
        private UserCtrls.PackingStsCtrl packingStsCtrl1;
        private UserCtrls.IRNgTrayCtrl irNgTrayCtrl1;
        private StdLib.WindowsControls.Buttons.ButtonEx buttonEx1;
    }
}