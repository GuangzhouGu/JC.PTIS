using JC.LidAttach.Devices.Laser;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.LidDispenser.UserCtrl.Devices
{
    public partial class LaserCtrl : UserControl
    {
        public LaserCtrl()
        {
            InitializeComponent();
        }

        private void btnLaserSend_Click(object sender, EventArgs e)
        {
            int channel = int.Parse(cmxChannels.Text);
            int cmd = cmxCmds.SelectedIndex;
            switch (cmd)
            {
                case 0:
                    {
                        var value = LaserHelper.Instance.GetValue(channel);
                        lbxLaserMsg.Items.Add("高度值：" + value.ToString());
                        break;
                    }
                case 1:
                    {
                        LaserHelper.Instance.Zero(channel);
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnClearLaserMsg_Click(object sender, EventArgs e)
        {
            lbxLaserMsg.Items.Clear();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitCtrl()
        {
            if (LaserHelper.Instance != null)
            {
                try
                {
                    for (int i = 0; i < LaserHelper.Instance.Count; i++)
                    {
                        this.cmxLasers.Items.Add("位移传感器" + (i + 1));
                    }
                    this.cmxLasers.SelectedIndex = 0;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void LaserCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
            }
        }
    }
}
