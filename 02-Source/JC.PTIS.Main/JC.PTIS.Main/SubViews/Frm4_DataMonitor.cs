using JC.AutoMating.UserCtrl;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Properties;
using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class Frm4_DataMonitor : SubViewBase
    {
        public Frm4_DataMonitor()
        {
            InitializeComponent();
            this.ButtonName = "信号监控";
            this.ButtonNameEn = "SignalMonitor";
            this.Index = 4;
            this.ButtonImage = Resources.Signal;
            this.IsNeedLogin = false;
        }
        StepTimeMonitorCtrl StepTimeMonitorCtrl = null;
        private void FrmCheck_Load(object sender, EventArgs e)
        {
            
        }

        private void Frm4_DataMonitor_Load(object sender, EventArgs e)
        {
            StepTimeMonitorCtrl = new StepTimeMonitorCtrl();
            StepTimeMonitorCtrl.Dock = DockStyle.Fill;
            StepTimeMonitorCtrl.Parent = StepTime;
        }
    }
}
