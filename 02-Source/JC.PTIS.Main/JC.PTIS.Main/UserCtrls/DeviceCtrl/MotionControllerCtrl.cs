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
    public partial class MotionControllerCtrl : UserControl
    {
        public MotionControllerCtrl()
        {
            InitializeComponent();
        }
        private void InitCtrls()
        {

        }
        private void btnClearControllerFaults_Click(object sender, EventArgs e)
        {
            try
            {
                var cardIndex = cmxControllerList.SelectedIndex;
                MotionGlobal.MotionCards[cardIndex].ClearControllerFaults();
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, $"ClearControllerFaults指令执行异常：{ex.ToString()}");
            }
        }

        private void MotionControllerCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                cmxControllerList.Items.Clear();
                if (MotionGlobal.MotionCards!=null&& MotionGlobal.MotionCards.Length>0)
                {
                    for (int i = 0; i < MotionGlobal.MotionCards.Length; i++)
                    {
                        cmxControllerList.Items.Add(MotionGlobal.MotionCards[i].CardInfo.CardName);
                    }
                    if (cmxControllerList.Items.Count>0)
                    {
                        cmxControllerList.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}
