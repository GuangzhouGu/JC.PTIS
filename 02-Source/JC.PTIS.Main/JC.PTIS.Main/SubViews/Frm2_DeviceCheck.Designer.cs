namespace JC.PTIS.Main.SubViews
{
    partial class Frm2_DeviceCheck
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
            this.detectPcCtrl1 = new JC.LidDispenser.UserCtrl.Devices.DetectPcCtrl();
            this.alignPcCtrl1 = new JC.LidDispenser.UserCtrl.Devices.AlignPcCtrl();
            this.scanerCtrl1 = new JC.LidDispenser.UserCtrl.Devices.ScanerCtrl();
            this.laserCtrl1 = new JC.LidDispenser.UserCtrl.Devices.LaserCtrl();
            this.motionControllerCtrl1 = new JC.PTIS.Main.UserCtrls.DeviceCtrl.MotionControllerCtrl();
            this.buttonEx1 = new StdLib.WindowsControls.Buttons.ButtonEx(this.components);
            this.SuspendLayout();
            // 
            // detectPcCtrl1
            // 
            this.detectPcCtrl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.detectPcCtrl1.Location = new System.Drawing.Point(1, 392);
            this.detectPcCtrl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detectPcCtrl1.Name = "detectPcCtrl1";
            this.detectPcCtrl1.Size = new System.Drawing.Size(385, 410);
            this.detectPcCtrl1.TabIndex = 5;
            // 
            // alignPcCtrl1
            // 
            this.alignPcCtrl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.alignPcCtrl1.Location = new System.Drawing.Point(1174, 2);
            this.alignPcCtrl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alignPcCtrl1.Name = "alignPcCtrl1";
            this.alignPcCtrl1.Size = new System.Drawing.Size(385, 390);
            this.alignPcCtrl1.StationName = JC.PTIS.Main.MotionInfo.StationName.None;
            this.alignPcCtrl1.TabIndex = 4;
            // 
            // scanerCtrl1
            // 
            this.scanerCtrl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scanerCtrl1.Location = new System.Drawing.Point(1, 2);
            this.scanerCtrl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.scanerCtrl1.Name = "scanerCtrl1";
            this.scanerCtrl1.Size = new System.Drawing.Size(385, 390);
            this.scanerCtrl1.TabIndex = 3;
          
            // 
            // laserCtrl1
            // 
            this.laserCtrl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.laserCtrl1.Location = new System.Drawing.Point(392, 2);
            this.laserCtrl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserCtrl1.Name = "laserCtrl1";
            this.laserCtrl1.Size = new System.Drawing.Size(385, 390);
            this.laserCtrl1.TabIndex = 1;
            // 
            // motionControllerCtrl1
            // 
            this.motionControllerCtrl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.motionControllerCtrl1.Location = new System.Drawing.Point(394, 392);
            this.motionControllerCtrl1.Margin = new System.Windows.Forms.Padding(4);
            this.motionControllerCtrl1.Name = "motionControllerCtrl1";
            this.motionControllerCtrl1.Size = new System.Drawing.Size(383, 410);
            this.motionControllerCtrl1.TabIndex = 6;
            // 
            // buttonEx1
            // 
            this.buttonEx1.CornerRadius = 10;
            this.buttonEx1.Location = new System.Drawing.Point(1181, 506);
            this.buttonEx1.Name = "buttonEx1";
            this.buttonEx1.Size = new System.Drawing.Size(75, 23);
            this.buttonEx1.TabIndex = 7;
            this.buttonEx1.Text = "buttonEx1";
            this.buttonEx1.UseVisualStyleBackColor = true;
            this.buttonEx1.Click += new System.EventHandler(this.buttonEx1_Click);
            // 
            // Frm2_DeviceCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1570, 834);
            this.Controls.Add(this.buttonEx1);
            this.Controls.Add(this.motionControllerCtrl1);
            this.Controls.Add(this.detectPcCtrl1);
            this.Controls.Add(this.alignPcCtrl1);
            this.Controls.Add(this.scanerCtrl1);
            this.Controls.Add(this.laserCtrl1);
            this.Name = "Frm2_DeviceCheck";
            this.Text = "FrmDeviceCheck";
            this.Controls.SetChildIndex(this.laserCtrl1, 0);
            this.Controls.SetChildIndex(this.scanerCtrl1, 0);
            this.Controls.SetChildIndex(this.alignPcCtrl1, 0);
            this.Controls.SetChildIndex(this.detectPcCtrl1, 0);
            this.Controls.SetChildIndex(this.motionControllerCtrl1, 0);
            this.Controls.SetChildIndex(this.buttonEx1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private LidDispenser.UserCtrl.Devices.LaserCtrl laserCtrl1;
        private LidDispenser.UserCtrl.Devices.ScanerCtrl scanerCtrl1;
        private LidDispenser.UserCtrl.Devices.AlignPcCtrl alignPcCtrl1;
        private LidDispenser.UserCtrl.Devices.DetectPcCtrl detectPcCtrl1;
        private UserCtrls.DeviceCtrl.MotionControllerCtrl motionControllerCtrl1;
        private StdLib.WindowsControls.Buttons.ButtonEx buttonEx1;
    }
}