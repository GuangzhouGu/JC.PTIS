using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Entities;
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

namespace HYC.MicroLedATDetect.Controls
{
    public partial class FoupMonitorCtrl : UserControl
    {
        public FoupMonitorCtrl()
        {
            InitializeComponent();
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return foupCtrl1.TitleBackColor; } set { foupCtrl1.TitleBackColor = value; foupCtrl2.TitleBackColor = value; } }
        private void WaferMapBase_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                for (int i = 0; i < 2; i++)
                {
                   var model= FoupModelHelper.GetFoup(i);
                    if (model!=null)
                    {
                        UpdateWaferCount(i,model);
                    }
                }
            }
        }
        public void UpdateWaferCount(int foupNo, FoupModel model)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int, FoupModel>(UpdateWaferCount), foupNo, model);
            }
            else
            {
                if (foupNo==0)
                {
                    foupCtrl1.UpdateFoupInfo(model);
                }
                else
                {
                    foupCtrl2.UpdateFoupInfo(model);
                }
            }
        }
    }
  
}
