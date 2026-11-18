using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class WaferBufferCtrl : UserControl
    {
        public WaferBufferCtrl()
        {
            InitializeComponent();
        }
        public void UpdateCode(WaferInfo waferInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<WaferInfo>(UpdateCode), waferInfo);
            }
            else
            {
                if (waferInfo!=null)
                {
                    lblCode.Text = waferInfo.Code;
                    lblFoupNo.Text = waferInfo.FoupNo.ToString();
                    lblLayerNo.Text = waferInfo.LayerNo.ToString();
                }
            }
        }
        public void UpdateSensor(IOInfo iOInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<IOInfo>(UpdateSensor), iOInfo);
            }
            else
            {
                try
                {
                    if (iOInfo != null)
                    {
                        var sensorName = $"pbx{iOInfo.IOName}";
                        PictureBox pictureBox = htTitlePanel1.Controls.Find(sensorName, true).FirstOrDefault() as PictureBox;
                        if (pictureBox != null)
                        {
                            pictureBox.Image = iOInfo.IsInputed ? Properties.Resources.Green2 : Properties.Resources.Gray2;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void WaferTableCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                ProcInfoHelper.Instance.WaferInfoUpdated += OnWaferInfoUpdated;
            }
        }

        private void OnWaferInfoUpdated(WaferModel model, WaferPosEnum @enum)
        {
            if (@enum==WaferPosEnum.WaferBufferUpLayer)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<WaferModel, WaferPosEnum>(OnWaferInfoUpdated), model, @enum);
                }
                else
                {
                    if (model!=null)
                    {
                        lblFoupNo.Text = model.FoupNo.ToString();
                        lblLayerNo.Text = model.LayerNo.ToString();
                        if (string.IsNullOrEmpty(model.Code)==false)
                        {
                            lblCode.Text = model.Code;
                        }
                    }
                    else
                    {
                        lblFoupNo.Text = "";
                        lblLayerNo.Text = "";
                        lblCode.Text = "";
                    }
             
                }
            }
         
        }
    }
}
