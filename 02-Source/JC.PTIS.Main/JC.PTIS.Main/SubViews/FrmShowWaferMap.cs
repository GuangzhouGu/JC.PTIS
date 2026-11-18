using HYC.MicroLedATDetect.Controls;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.UserCtrls;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsControls;
using StdLib.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class FrmShowWaferMap : FormPopBase
    {
        public FrmShowWaferMap()
        {
            InitializeComponent();
        }

        public void ShowWaferMap(WaferModel model)
        {
            if (model != null)
            {
                WaferMapCtrl ctrl = new WaferMapCtrl();
                ctrl.Dock = DockStyle.Fill;
                ctrl.Parent = this;
                ctrl.OnWaferInfoUpdated(model, WaferPosEnum.WaferTable);
            }
        }
    }
}
