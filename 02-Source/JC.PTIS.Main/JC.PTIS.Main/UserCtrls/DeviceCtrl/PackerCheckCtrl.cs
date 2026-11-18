using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.DeviceCtrl
{
    public partial class PackerCheckCtrl : UserControl
    {
        public PackerCheckCtrl()
        {
            InitializeComponent();
        }
        Timer Timer = new Timer();
        MotionManager Motion = MotionManager.GetInstance();
        PictureBox[]Pbxs= null;
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = InputName.PackerReady.ToInt(); i <= InputName.PackerComp.ToInt(); i++)
                {
                    var signal = Motion.GetDigitalInput((InputName)i);
                    Pbxs[i].Image = signal ? Properties.Resources.Green2 : Properties.Resources.Gray2;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void PackerCheckCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
                Timer.Interval = 200;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }
        private void InitCtrl()
        {
            Pbxs=new PictureBox[4];
            for (int i = 0; i < 4; i++)
            {
                var ctrlName = $"pictureBox{i+1}";
                var ctrl= this.Controls.Find(ctrlName, true);
                if (ctrl!=null)
                {
                    Pbxs[i]= (PictureBox)ctrl[0];
                }
            }
        }

        private async void btnPackingStart_Click(object sender, EventArgs e)
        {
            var btn=sender as HTButton;
            Motion.SetDigitalOutput(btn.Tag.ToString(), true);
            await Task.Delay((int)(ParaHelper.Motion.TriggerTime*1000));
            Motion.SetDigitalOutput(btn.Tag.ToString(), false);
        }
    }
}
