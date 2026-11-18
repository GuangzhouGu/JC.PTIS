using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main.Devices.Scaner;
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
    public partial class ScanerCtrl : UserControl
    {
        public ScanerCtrl()
        {
            InitializeComponent();
        }

        private async void btnLaserSend_Click(object sender, EventArgs e)
        {
            try
            {
                string code = "";
                var index = this.cmxLasers.SelectedIndex;
                code = await Task.Run<string>(() =>
                {
                    var value = ScanerHelper.Instance.Scan(index);
                    return value;
                });
                lbxLaserMsg.Items.Add("条码：" + code);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            if (ScanerHelper.Instance != null)
            {
                try
                {
                    for (int i = 0; i < ScanerHelper.Instance.ScanerCount; i++)
                    {
                        this.cmxLasers.Items.Add("扫码枪" + (i + 1));
                    }
                    if (this.cmxLasers.Items.Count>0)
                    {
                        this.cmxLasers.SelectedIndex = 0;
                    }
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
